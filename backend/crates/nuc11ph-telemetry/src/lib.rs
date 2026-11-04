//! Zero-WMI, HVCI-Compatible Telemetry Engine for Intel NUC11PHi7 (`i7-1165G7` + `RTX 2060`).
//!
//! Reads EC temperatures (`IOCTL 0x9C402048`), Dual Blower Fan RPMs (`IOCTL 0x9C402024`),
//! NVIDIA NVML (`nvml.dll` for the RTX 2060), and Win32 kernel counters (`GetSystemTimes`,
//! `CallNtPowerInformation`, `GlobalMemoryStatusEx`), with automatic fallback to
//! `MSAcpi_ThermalZoneTemperature` if `PerformanceDriver.sys` is not yet loaded.

use std::mem::size_of;
use std::sync::Mutex;
use std::time::{SystemTime, UNIX_EPOCH};

use nvml_wrapper::enum_wrappers::device::{Clock, TemperatureSensor};
use nvml_wrapper::Nvml;
use windows::Win32::Foundation::FILETIME;
use windows::Win32::System::Power::{CallNtPowerInformation, ProcessorInformation};
use windows::Win32::System::SystemInformation::{GlobalMemoryStatusEx, MEMORYSTATUSEX};
use windows::Win32::System::Threading::GetSystemTimes;

use nuc11ph_hw::PhantomHardwareController;
use nuc11ph_protocol::{decode_dual_fan_rpms, decode_msr_power_limit, TelemetrySnapshot};

#[repr(C)]
#[derive(Clone, Copy, Default)]
struct ProcessorPowerInformation {
    number: u32,
    max_mhz: u32,
    current_mhz: u32,
    mhz_limit: u32,
    max_idle_state: u32,
    current_idle_state: u32,
}

#[derive(Default)]
struct CpuTicksState {
    prev_idle: u64,
    prev_kernel: u64,
    prev_user: u64,
}

pub struct PhantomTelemetryEngine {
    nvml: Option<Nvml>,
    cpu_ticks: Mutex<CpuTicksState>,
}

impl PhantomTelemetryEngine {
    pub fn new() -> Self {
        let nvml = Nvml::init().ok();
        let engine = Self {
            nvml,
            cpu_ticks: Mutex::new(CpuTicksState::default()),
        };
        // Prime the CPU tick delta on creation so the very first poll returns a real percentage
        let _ = engine.sample_cpu_usage_pct();
        engine
    }

    pub fn sample(&self, hw: &PhantomHardwareController) -> TelemetrySnapshot {
        let timestamp_ms = SystemTime::now()
            .duration_since(UNIX_EPOCH)
            .map(|d| d.as_millis() as u64)
            .unwrap_or(0);

        // 1. EC Temperatures (data[0] = dGPU °C, data[1] = CPU °C, data[2] = iGPU °C)
        let (ec_dgpu_temp, mut cpu_temp_c, mut igpu_temp_c) = hw
            .with_driver(|drv| drv.read_ec_temperatures())
            .map(|t| (t.data[0], t.data[1], t.data[2]))
            .unwrap_or((0, 0, 0));

        // Fallback to ACPI ThermalZone if PerformanceDriver.sys is not loaded yet
        if cpu_temp_c == 0 {
            if let Ok(Some(acpi_c)) = hw.with_wmi(|w| Ok(w.query_acpi_thermal_zone_temp_c())) {
                cpu_temp_c = acpi_c;
                if igpu_temp_c == 0 {
                    igpu_temp_c = acpi_c.saturating_sub(2);
                }
            }
        }

        // 2. Dual Blower Fan RPMs (Fan 0 = CPU, Fan 1 = RTX 2060)
        let (cpu_fan_rpm, gpu_fan_rpm) = hw
            .with_driver(|drv| drv.read_fan_speeds())
            .map(|s| decode_dual_fan_rpms(&s))
            .unwrap_or((0, 0));

        // 3. Active MSR 0x610 Power Limits
        let (_, cached_limits, fan_mode, fan_off_enabled, stealth_mode_active, _, _, _, _, _) =
            hw.current_state_metadata();

        let power_limits = hw
            .with_driver(|drv| drv.read_power_limit_msr())
            .map(decode_msr_power_limit)
            .ok()
            .filter(|p| p.pl1_watts > 0)
            .unwrap_or(cached_limits);

        // 4. NVIDIA GeForce RTX 2060 Telemetry via NVML
        let (
            nvml_temp_c,
            dgpu_usage_pct,
            dgpu_core_clock_mhz,
            dgpu_mem_clock_mhz,
            dgpu_power_watts,
            dgpu_vram_used_mb,
            dgpu_vram_total_mb,
        ) = self.query_rtx2060_nvml();

        let dgpu_temp_c = if nvml_temp_c > 0 {
            nvml_temp_c
        } else {
            ec_dgpu_temp
        };

        // 5. Win32 Kernel CPU Load, Clock Frequency, and Physical RAM
        let cpu_usage_pct = self.sample_cpu_usage_pct();
        let cpu_clock_mhz = Self::sample_cpu_clock_mhz();
        let (ram_used_mb, ram_total_mb) = Self::sample_memory_mb();

        TelemetrySnapshot {
            timestamp_ms,
            cpu_temp_c,
            dgpu_temp_c,
            igpu_temp_c,
            cpu_fan_rpm,
            gpu_fan_rpm,
            cpu_usage_pct,
            cpu_clock_mhz,
            dgpu_usage_pct,
            dgpu_core_clock_mhz,
            dgpu_mem_clock_mhz,
            dgpu_power_watts,
            dgpu_vram_used_mb,
            dgpu_vram_total_mb,
            ram_used_mb,
            ram_total_mb,
            active_pl1_watts: power_limits.pl1_watts,
            active_pl2_watts: power_limits.pl2_watts,
            active_tau_sec: power_limits.tau_seconds,
            active_fan_mode: fan_mode as u8,
            fan_off_enabled,
            stealth_mode_active,
            driver_connected: hw.is_driver_connected(),
            wmi_connected: hw.is_wmi_connected(),
        }
    }

    fn query_rtx2060_nvml(&self) -> (u8, u8, u16, u16, f32, u16, u16) {
        let Some(ref nvml) = self.nvml else {
            return (0, 0, 0, 0, 0.0, 0, 6144);
        };
        let Ok(device) = nvml.device_by_index(0) else {
            return (0, 0, 0, 0, 0.0, 0, 6144);
        };

        let temp = device
            .temperature(TemperatureSensor::Gpu)
            .unwrap_or(0)
            .min(125) as u8;
        let usage = device
            .utilization_rates()
            .map(|u| u.gpu.min(100) as u8)
            .unwrap_or(0);
        let core_clock = device.clock_info(Clock::Graphics).unwrap_or(0) as u16;
        let mem_clock = device.clock_info(Clock::Memory).unwrap_or(0) as u16;
        let power_watts = device
            .power_usage()
            .map(|mw| (mw as f32) / 1000.0)
            .unwrap_or(0.0);
        let (vram_used, vram_total) = device
            .memory_info()
            .map(|m| {
                (
                    (m.used / (1024 * 1024)) as u16,
                    (m.total / (1024 * 1024)) as u16,
                )
            })
            .unwrap_or((0, 6144));

        (
            temp,
            usage,
            core_clock,
            mem_clock,
            power_watts,
            vram_used,
            vram_total,
        )
    }

    fn sample_cpu_usage_pct(&self) -> f32 {
        unsafe {
            let mut idle_ft = FILETIME::default();
            let mut kernel_ft = FILETIME::default();
            let mut user_ft = FILETIME::default();

            if GetSystemTimes(Some(&mut idle_ft), Some(&mut kernel_ft), Some(&mut user_ft)).is_err()
            {
                return 0.0;
            }

            let to_u64 = |ft: FILETIME| -> u64 {
                ((ft.dwHighDateTime as u64) << 32) | (ft.dwLowDateTime as u64)
            };

            let idle = to_u64(idle_ft);
            let kernel = to_u64(kernel_ft);
            let user = to_u64(user_ft);

            let mut guard = self.cpu_ticks.lock().unwrap();
            let d_idle = idle.saturating_sub(guard.prev_idle);
            let d_kernel = kernel.saturating_sub(guard.prev_kernel);
            let d_user = user.saturating_sub(guard.prev_user);

            guard.prev_idle = idle;
            guard.prev_kernel = kernel;
            guard.prev_user = user;

            let total_sys = d_kernel + d_user;
            if total_sys == 0 {
                return 0.0;
            }
            let busy = total_sys.saturating_sub(d_idle);
            (((busy as f64) / (total_sys as f64)) * 100.0).clamp(0.0, 100.0) as f32
        }
    }

    fn sample_cpu_clock_mhz() -> u16 {
        unsafe {
            let mut infos = [ProcessorPowerInformation::default(); 16];
            let status = CallNtPowerInformation(
                ProcessorInformation,
                None,
                0,
                Some(infos.as_mut_ptr() as *mut _),
                (infos.len() * size_of::<ProcessorPowerInformation>()) as u32,
            );
            if status.is_ok() {
                let active: Vec<u32> = infos
                    .iter()
                    .map(|i| i.current_mhz)
                    .filter(|&mhz| mhz > 0)
                    .collect();
                if !active.is_empty() {
                    let sum: u32 = active.iter().sum();
                    return (sum / (active.len() as u32)) as u16;
                }
            }
            2800
        }
    }

    fn sample_memory_mb() -> (u32, u32) {
        unsafe {
            let mut mem = MEMORYSTATUSEX {
                dwLength: size_of::<MEMORYSTATUSEX>() as u32,
                ..Default::default()
            };
            if GlobalMemoryStatusEx(&mut mem).is_ok() {
                let total_mb = (mem.ullTotalPhys / (1024 * 1024)) as u32;
                let avail_mb = (mem.ullAvailPhys / (1024 * 1024)) as u32;
                return (total_mb.saturating_sub(avail_mb), total_mb);
            }
            (0, 16384)
        }
    }
}
