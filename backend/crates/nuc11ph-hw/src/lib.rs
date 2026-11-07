//! Unified Fault-Tolerant Hardware Controller (`PhantomHardwareController`) for the Intel NUC11PHi7.
//!
//! Coordinates `PerformanceDriver.sys` IOCTLs and `root\WMI:CISD_WMI` lighting/CEC/preset calls
//! with independent subsystem fault tolerance: if `PerformanceDriver.sys` is not installed yet,
//! `CISD_WMI` 7-zone RGB, NVML RTX 2060 telemetry, and Win32 CPU/RAM telemetry continue to work
//! and the controller automatically reconnects to the driver as soon as it becomes available.

mod control_panel;
pub mod driver;
#[cfg(test)]
mod fake;
pub use control_panel::backend_instance;
pub mod mutex;
pub mod reference;
mod resume;
pub mod wmi_cisd;

use std::sync::Mutex;
use thiserror::Error;

pub use driver::{DriverError, DriverOps, PerformanceDriver};
pub use mutex::{IsaBusMutex, MutexError};
pub use wmi_cisd::{CisdWmiClient, WmiError};

use nuc11ph_protocol::{
    build_custom_wmi_fan_profile, decode_msr_power_limit, encode_phantom_msr_power_limit,
    infer_ec_fan_mode, parse_bios_turbo_profiles, parse_ec_fan_profile, BiosPowerProfile,
    EcNuc22FanCurveDto, HardwareCapabilities, HdmiCecAction, LedZoneStateDto, NucFanMode,
    PowerLimitConfig, PowerPresetKind, StudioProfile,
};

#[derive(Debug, Error)]
pub enum HardwareError {
    #[error("Hardware validation failed: {0}")]
    Invalid(String),
    #[error("Kernel Driver Error: {0}")]
    Driver(#[from] DriverError),
    #[error("ACPI WMI Error: {0}")]
    Wmi(#[from] WmiError),
}

struct ActiveHardwareState {
    pub active_profile_name: String,
    pub power_limits: PowerLimitConfig,
    pub fan_mode: NucFanMode,
    pub fan_off_enabled: bool,
    pub fixed_cpu_duty: u8,
    pub fixed_gpu_duty: u8,
    pub cpu_fan_curve: EcNuc22FanCurveDto,
    pub gpu_fan_curve: EcNuc22FanCurveDto,
    pub user_led_zones: [LedZoneStateDto; 7],
    pub power_cache_verified: bool,
    pub fan_cache_verified: bool,
    pub stock_fan_selected: bool,
    pub led_cache_verified: [bool; 7],
    pub stealth_mode_active: bool,
    pub driver_status_msg: String,
    pub wmi_status_msg: String,
}

/// Process-level hooks: device identity and how a missing backend is (re)opened.
/// Production uses [`Platform::NATIVE`]; tests inject hooks that never reach hardware.
#[derive(Clone, Copy)]
pub(crate) struct Platform {
    pub identity: fn() -> (String, String, bool),
    pub open_driver: fn() -> Result<Box<dyn DriverOps>, DriverError>,
    pub connect_wmi: fn() -> Result<CisdWmiClient, WmiError>,
}

impl Platform {
    pub const NATIVE: Self = Self {
        identity: control_panel::identity,
        open_driver: || PerformanceDriver::open().map(|d| Box::new(d) as Box<dyn DriverOps>),
        connect_wmi: CisdWmiClient::connect,
    };
}

pub struct PhantomHardwareController {
    pub operation_gate: Mutex<()>,
    platform: Platform,
    driver: Mutex<Option<Box<dyn DriverOps>>>,
    wmi: Mutex<Option<CisdWmiClient>>,
    bios_power_profiles: Mutex<Vec<BiosPowerProfile>>,
    state: Mutex<ActiveHardwareState>,
}

impl PhantomHardwareController {
    /// Initializes the hardware controller without failing if one subsystem is temporarily unavailable.
    pub fn initialize() -> Self {
        let platform = Platform::NATIVE;
        Self::from_backends(platform, (platform.open_driver)(), (platform.connect_wmi)())
    }

    /// Builds the controller from already opened (or failed) backends. Never fails.
    pub(crate) fn from_backends(
        platform: Platform,
        driver: Result<Box<dyn DriverOps>, DriverError>,
        wmi: Result<CisdWmiClient, WmiError>,
    ) -> Self {
        let (driver_opt, driver_status_msg) = match driver {
            Ok(d) => (
                Some(d),
                "ACPI\\INTC1036 (PerformanceDriver.sys) Connected".to_string(),
            ),
            Err(e) => (None, format!("PerformanceDriver unavailable: {e}")),
        };

        let (wmi_opt, mut wmi_status_msg) = match wmi {
            Ok(w) => (Some(w), "root\\WMI:CISD_WMI Connected".to_string()),
            Err(e) => (None, format!("CISD_WMI unavailable: {e}")),
        };

        let bios_power_profiles = wmi_opt
            .as_ref()
            .and_then(|w| w.query_bios_turbo_profile_bytes().ok())
            .map(|b| parse_bios_turbo_profiles(&b))
            .unwrap_or_default();

        let measured_power_limits = driver_opt
            .as_ref()
            .and_then(|d| d.read_power_limit_msr().ok())
            .map(decode_msr_power_limit)
            .filter(|p| p.pl1_watts > 0);
        let power_cache_verified = measured_power_limits.is_some();
        let power_limits =
            measured_power_limits.unwrap_or_else(|| PowerPresetKind::Balanced.default_limits());

        let stock_fan_selected = driver_opt
            .as_ref()
            .and_then(|d| d.read_ec_fan_profile().ok())
            .is_some_and(|p| control_panel::is_reviewed_control_fan(&p.data));
        let measured_fan_state = driver_opt
            .as_ref()
            .and_then(|d| d.read_ec_fan_profile().ok())
            .map(|ec| parse_ec_fan_profile(&ec));
        let fan_readback_verified = measured_fan_state.is_some();
        let (curves, fan_off_enabled) = measured_fan_state.unwrap_or_else(|| {
            (
                [
                    EcNuc22FanCurveDto::default_cpu_fan_curve(),
                    EcNuc22FanCurveDto::default_gpu_fan_curve(),
                ],
                true,
            )
        });

        let fan_mode = infer_ec_fan_mode(&curves, fan_off_enabled);
        // Fixed-duty cache starts as UI defaults, not a measured duty snapshot.
        let fan_cache_verified = fan_readback_verified && fan_mode != NucFanMode::Fixed;

        let mut user_led_zones = StudioProfile::default_leds();
        let mut led_cache_verified = [false; 7];
        if let Some(wmi) = &wmi_opt {
            match wmi.zone_mask() {
                Ok(mask) => {
                    for zone in nuc11ph_protocol::PhantomLedZone::ALL {
                        let idx = zone as usize;
                        if mask & (1 << idx) != 0 {
                            match wmi.read_led_zone_hardware_state(zone) {
                                Ok(actual) => {
                                    user_led_zones[idx] = actual;
                                    led_cache_verified[idx] = true;
                                }
                                Err(e) => {
                                    wmi_status_msg =
                                        format!("CISD_WMI connected; LED readback failed: {e}")
                                }
                            }
                        } else {
                            user_led_zones[idx].indicator_mode = 6;
                            user_led_zones[idx].s0.brightness = 0;
                        }
                    }
                }
                Err(e) => {
                    wmi_status_msg = format!("CISD_WMI connected; capability query failed: {e}")
                }
            }
        }

        Self {
            operation_gate: Mutex::new(()),
            platform,
            driver: Mutex::new(driver_opt),
            wmi: Mutex::new(wmi_opt),
            bios_power_profiles: Mutex::new(bios_power_profiles),
            state: Mutex::new(ActiveHardwareState {
                active_profile_name: "Balanced".to_string(),
                power_limits,
                fan_mode,
                fan_off_enabled,
                fixed_cpu_duty: 50,
                fixed_gpu_duty: 50,
                cpu_fan_curve: curves[0],
                gpu_fan_curve: curves[1],
                user_led_zones,
                power_cache_verified,
                fan_cache_verified,
                stock_fan_selected,
                led_cache_verified,
                stealth_mode_active: false,
                driver_status_msg,
                wmi_status_msg,
            }),
        }
    }

    pub fn with_driver<F, R>(&self, f: F) -> Result<R, HardwareError>
    where
        F: FnOnce(&dyn DriverOps) -> Result<R, DriverError>,
    {
        let mut guard = self.driver.lock().unwrap();
        if guard.is_none() {
            if let Ok(d) = (self.platform.open_driver)() {
                *guard = Some(d);
                self.state.lock().unwrap().driver_status_msg =
                    "ACPI\\INTC1036 (PerformanceDriver.sys) Connected".to_string();
            }
        }
        match guard.as_ref() {
            Some(drv) => Ok(f(drv.as_ref())?),
            None => Err(HardwareError::Driver(DriverError::DeviceNotFound)),
        }
    }

    pub fn with_wmi<F, R>(&self, f: F) -> Result<R, HardwareError>
    where
        F: FnOnce(&CisdWmiClient) -> Result<R, WmiError>,
    {
        let mut guard = self.wmi.lock().unwrap();
        if guard.is_none() {
            match (self.platform.connect_wmi)() {
                Ok(w) => {
                    *guard = Some(w);
                    self.state.lock().unwrap().wmi_status_msg =
                        "root\\WMI:CISD_WMI Connected".to_string();
                }
                Err(error) => {
                    self.state.lock().unwrap().wmi_status_msg = error.to_string();
                    return Err(HardwareError::Wmi(error));
                }
            }
        }
        match guard.as_ref() {
            Some(wmi) => Ok(f(wmi)?),
            None => Err(HardwareError::Wmi(WmiError::InstanceNotFound)),
        }
    }

    pub fn is_driver_connected(&self) -> bool {
        self.driver
            .lock()
            .unwrap()
            .as_ref()
            .is_some_and(|d| d.check_driver_status().unwrap_or(false))
    }

    /// Device model, BIOS version and whether both are the reviewed NUC11PHi7 / BIOS 0080.
    pub(crate) fn identity(&self) -> (String, String, bool) {
        (self.platform.identity)()
    }

    pub fn is_wmi_connected(&self) -> bool {
        self.wmi.lock().unwrap().is_some()
    }

    pub fn set_power_preset(
        &self,
        preset: PowerPresetKind,
    ) -> Result<PowerLimitConfig, HardwareError> {
        let profiles = self.with_wmi(|w| w.query_bios_turbo_profile_bytes())?;
        *self.bios_power_profiles.lock().unwrap() = parse_bios_turbo_profiles(&profiles);
        let limits = self
            .bios_power_profiles
            .lock()
            .unwrap()
            .iter()
            .find(|p| p.preset_kind == preset)
            .map(|p| PowerLimitConfig {
                pl1_watts: p.pl1_watts,
                pl2_watts: p.pl2_watts,
                tau_seconds: p.tau_seconds,
            })
            .ok_or_else(|| {
                HardwareError::Invalid(
                    "Firmware power presets unavailable; no guessed fallback".into(),
                )
            })?;

        self.set_custom_power_limits(limits)?;
        Ok(limits)
    }

    pub fn set_custom_power_limits(&self, limits: PowerLimitConfig) -> Result<(), HardwareError> {
        nuc11ph_protocol::validate_phantom_limits(limits)
            .map_err(|e| HardwareError::Invalid(e.into()))?;
        self.state.lock().unwrap().power_cache_verified = false;
        self.with_driver(|drv| {
            let current_msr = drv.read_power_limit_msr()?;
            let new_msr =
                encode_phantom_msr_power_limit(current_msr, limits).map_err(|reason| {
                    DriverError::IoctlFailed {
                        ioctl: driver::NucIoctl::MsrWritePowerLimit as u32,
                        reason: reason.into(),
                    }
                })?;
            drv.write_power_limit_msr(new_msr)?;
            let readback = drv.read_power_limit_msr()?;
            if readback != new_msr {
                return Err(DriverError::IoctlFailed {
                    ioctl: driver::NucIoctl::MsrWritePowerLimit as u32,
                    reason: "Power-limit readback does not match the requested values".into(),
                });
            }
            Ok(())
        })?;
        let mut state = self.state.lock().unwrap();
        state.power_limits = limits;
        state.power_cache_verified = true;
        Ok(())
    }

    pub fn set_fan_mode(
        &self,
        mode: NucFanMode,
        fan_off_enabled: bool,
    ) -> Result<(), HardwareError> {
        match mode {
            NucFanMode::Quiet | NucFanMode::Balanced | NucFanMode::Cool => {
                // These are KUI software presets for both physical fans. The BIOS
                // preset report is incomplete on this firmware (CPU max temp is zero
                // and the GPU curve is absent), so applying it would clear valid control.
                let (cpu, gpu) = nuc11ph_protocol::phantom_cooling_preset(mode)
                    .map_err(|e| HardwareError::Invalid(e.into()))?;
                self.set_custom_fan_curves(cpu, gpu, fan_off_enabled)?;
            }
            NucFanMode::Custom => {
                let (cpu_curve, gpu_curve) = {
                    let guard = self.state.lock().unwrap();
                    (guard.cpu_fan_curve, guard.gpu_fan_curve)
                };
                self.set_custom_fan_curves(cpu_curve, gpu_curve, fan_off_enabled)?;
            }
            NucFanMode::Fixed => {
                let (c_duty, g_duty) = {
                    let guard = self.state.lock().unwrap();
                    (guard.fixed_cpu_duty, guard.fixed_gpu_duty)
                };
                self.set_fixed_fan_duty(c_duty, g_duty)?;
            }
            _ => return Err(HardwareError::Invalid("Unsupported fan mode".into())),
        }
        let mut guard = self.state.lock().unwrap();
        guard.fan_mode = mode;
        guard.fan_off_enabled = fan_off_enabled;
        Ok(())
    }

    pub fn set_fixed_fan_duty(&self, cpu_duty: u8, gpu_duty: u8) -> Result<(), HardwareError> {
        if !(25..=99).contains(&cpu_duty) || !(25..=99).contains(&gpu_duty) {
            return Err(HardwareError::Invalid(
                "Fixed fan duties must be25–99%".into(),
            ));
        }
        let speeds = self.with_driver(|d| d.read_fan_speeds())?;
        let packets = reference::fan_duty_packets(&speeds, cpu_duty, gpu_duty)
            .map_err(HardwareError::Invalid)?;
        self.state.lock().unwrap().fan_cache_verified = false;
        self.with_driver(|d| {
            for packet in packets {
                d.set_fixed_fan_duty(packet.fan_number, packet.fan_duty)?;
            }
            reference::verify_fixed_duties(&packets, &d.read_ec_fan_profile()?)?;
            Ok(())
        })?;
        let mut guard = self.state.lock().unwrap();
        guard.stock_fan_selected = false;
        guard.fan_mode = NucFanMode::Fixed;
        guard.fixed_cpu_duty = cpu_duty;
        guard.fixed_gpu_duty = gpu_duty;
        guard.fan_cache_verified = true;
        Ok(())
    }

    pub fn set_custom_fan_curves(
        &self,
        cpu_fan: EcNuc22FanCurveDto,
        gpu_fan: EcNuc22FanCurveDto,
        fan_off_enabled: bool,
    ) -> Result<(), HardwareError> {
        nuc11ph_protocol::validate_custom_fan_curve(&cpu_fan)
            .and_then(|_| nuc11ph_protocol::validate_custom_fan_curve(&gpu_fan))
            .map_err(|e| HardwareError::Invalid(e.into()))?;
        self.state.lock().unwrap().fan_cache_verified = false;
        self.with_driver(|d| {
            let ec_baseline = d.read_ec_fan_profile()?;
            let packet =
                build_custom_wmi_fan_profile(&ec_baseline, &cpu_fan, &gpu_fan, fan_off_enabled)
                    .map_err(|reason| DriverError::IoctlFailed {
                        ioctl: driver::NucIoctl::WriteFanProfileNuc22 as u32,
                        reason: reason.into(),
                    })?;
            d.write_wmi_fan_profile_nuc22(&packet)?;
            reference::verify_fan_profile(&packet, &d.read_ec_fan_profile()?)
        })?;
        let mut guard = self.state.lock().unwrap();
        guard.stock_fan_selected = false;
        guard.fan_mode = NucFanMode::Custom;
        guard.cpu_fan_curve = cpu_fan;
        guard.gpu_fan_curve = gpu_fan;
        guard.fan_off_enabled = fan_off_enabled;
        guard.fan_cache_verified = true;
        Ok(())
    }

    pub fn set_led_zone_volatile(&self, zone: LedZoneStateDto) -> Result<(), HardwareError> {
        if zone.zone_id > 6 {
            return Err(HardwareError::Invalid("Unknown LED zone".into()));
        }
        self.state.lock().unwrap().led_cache_verified[zone.zone_id as usize] = false;
        self.with_wmi(|w| w.apply_led_zone_volatile(&zone))
    }

    pub fn set_all_leds_volatile(&self, zones: [LedZoneStateDto; 7]) -> Result<(), HardwareError> {
        self.state.lock().unwrap().led_cache_verified.fill(false);
        self.with_wmi(|w| w.apply_all_leds_volatile(&zones))
    }

    pub fn set_global_brightness_volatile(&self, brightness: u8) -> Result<(), HardwareError> {
        let clamped = brightness.min(100);
        let updated_zones = {
            let mut guard = self.state.lock().unwrap();
            guard.led_cache_verified.fill(false);
            for z in &mut guard.user_led_zones {
                z.s0.brightness = clamped;
            }
            guard.user_led_zones
        };
        self.with_wmi(|w| w.apply_all_leds_volatile(&updated_zones))
    }

    pub fn set_stealth_mode_volatile(&self, enable: bool) -> Result<(), HardwareError> {
        let zones_to_apply = {
            let mut guard = self.state.lock().unwrap();
            guard.stealth_mode_active = enable;
            guard.led_cache_verified.fill(false);
            if enable {
                let mut blanked = guard.user_led_zones;
                for z in &mut blanked {
                    z.s0.brightness = 0;
                }
                blanked
            } else {
                guard.user_led_zones
            }
        };
        self.with_wmi(|w| w.apply_all_leds_volatile(&zones_to_apply))
    }

    pub fn commit_leds_to_bios_nvram(&self) -> Result<(), HardwareError> {
        let verified = self.with_wmi(|w| {
            w.commit_leds_to_bios_nvram()?;
            w.read_staged_leds()
        })?;
        let mut state = self.state.lock().unwrap();
        for zone in verified {
            state.user_led_zones[zone.zone_id as usize] = zone;
            state.led_cache_verified[zone.zone_id as usize] = true;
        }
        state.wmi_status_msg =
            "CISD_WMI connected; last lighting save read back successfully".into();
        Ok(())
    }

    pub fn execute_hdmi_cec(&self, action: HdmiCecAction) -> Result<Vec<u8>, HardwareError> {
        self.with_wmi(|w| w.execute_hdmi_cec(action))
    }

    pub fn apply_studio_profile(&self, profile: &StudioProfile) -> Result<(), HardwareError> {
        if profile.power_preset == PowerPresetKind::Custom {
            let _ = self.set_custom_power_limits(PowerLimitConfig {
                pl1_watts: profile.pl1_watts,
                pl2_watts: profile.pl2_watts,
                tau_seconds: profile.tau_seconds,
            });
        } else {
            let _ = self.set_power_preset(profile.power_preset);
        }

        {
            let mut guard = self.state.lock().unwrap();
            guard.active_profile_name = profile.name.clone();
            guard.cpu_fan_curve = profile.custom_fan_cpu;
            guard.gpu_fan_curve = profile.custom_fan_gpu;
            guard.fixed_cpu_duty = profile.fixed_cpu_duty;
            guard.fixed_gpu_duty = profile.fixed_gpu_duty;
            guard.fan_cache_verified = false;
        }

        let _ = self.set_fan_mode(profile.fan_mode, profile.fan_off_zero_rpm);
        let _ = self.set_all_leds_volatile(profile.led_zones);
        Ok(())
    }

    pub fn restore_after_standby_resume(&self, wake_hdmi_cec: bool) -> Result<(), HardwareError> {
        let _operation = self.operation_gate.lock().unwrap();
        let snapshot = {
            let guard = self.state.lock().unwrap();
            resume::ResumeSnapshot {
                limits: guard.power_limits,
                power_verified: guard.power_cache_verified,
                fan_mode: guard.fan_mode,
                fan_verified: guard.fan_cache_verified && !guard.stock_fan_selected,
                curves: [guard.cpu_fan_curve, guard.gpu_fan_curve],
                fixed_duties: [guard.fixed_cpu_duty, guard.fixed_gpu_duty],
                fan_off: guard.fan_off_enabled,
                leds: guard.user_led_zones,
                led_verified: guard.led_cache_verified,
                stealth: guard.stealth_mode_active,
            }
        };
        resume::execute_checked(snapshot, |plan| {
            let saved = plan.snapshot;
            self.set_custom_power_limits(saved.limits)?;
            match plan.fan_action {
                resume::FanAction::NamedPreset(mode) => self.set_fan_mode(mode, saved.fan_off)?,
                resume::FanAction::FixedDuties => {
                    self.set_fixed_fan_duty(saved.fixed_duties[0], saved.fixed_duties[1])?
                }
                resume::FanAction::CheckedCustom => {
                    self.set_custom_fan_curves(saved.curves[0], saved.curves[1], saved.fan_off)?
                }
            }
            if plan.restore_leds.iter().any(|v| *v) {
                self.with_wmi(|w| {
                    for (index, zone) in saved.leds.iter().enumerate() {
                        if plan.restore_leds[index] {
                            w.restore_led_zone_snapshot(zone)?;
                        }
                    }
                    Ok(())
                })?;
            }
            if wake_hdmi_cec {
                self.execute_hdmi_cec(HdmiCecAction::WakeDisplay)?;
            }
            Ok(())
        })
    }

    pub fn current_state_metadata(
        &self,
    ) -> (
        String,
        PowerLimitConfig,
        NucFanMode,
        bool,
        bool,
        EcNuc22FanCurveDto,
        EcNuc22FanCurveDto,
        [LedZoneStateDto; 7],
        String,
        String,
    ) {
        let guard = self.state.lock().unwrap();
        (
            guard.active_profile_name.clone(),
            guard.power_limits,
            guard.fan_mode,
            guard.fan_off_enabled,
            guard.stealth_mode_active,
            guard.cpu_fan_curve,
            guard.gpu_fan_curve,
            guard.user_led_zones,
            guard.driver_status_msg.clone(),
            guard.wmi_status_msg.clone(),
        )
    }

    /// Probe current hardware capabilities and readback validity. A failed read remains
    /// explicitly invalid; this method never substitutes a default for a measured value.
    pub fn hardware_capabilities(&self) -> HardwareCapabilities {
        let mut caps = HardwareCapabilities::unavailable();
        caps.driver_connected = self
            .with_driver(|d| {
                let profile = d.read_ec_fan_profile()?;
                let (curves, fan_off) = parse_ec_fan_profile(&profile);
                let _ = infer_ec_fan_mode(&curves, fan_off);
                caps.fan_profile_readback_valid = true;
                caps.fan_mode_readback_valid = true;
                Ok(())
            })
            .is_ok();
        caps.fan_slots = self
            .with_driver(|d| {
                let raw = d.read_fan_speeds()?;
                Ok(nuc11ph_protocol::fan_slot_mask(&raw))
            })
            .unwrap_or(0);
        caps.power_limit_readback_valid = self
            .with_driver(|d| {
                d.read_power_limit_msr()?;
                Ok(())
            })
            .is_ok();
        caps.wmi_connected = self
            .with_wmi(|w| {
                let mask = w.zone_mask()?;
                caps.led_zone_mask = mask;
                for zone in nuc11ph_protocol::PhantomLedZone::ALL {
                    let id = zone as usize;
                    if mask & (1 << id) == 0 {
                        continue;
                    }
                    let Ok(modes) = w.indicator_mask(id as u8) else {
                        continue;
                    };
                    caps.led_modes_by_zone[id] = modes;
                    let current_mode = w.read_led_zone_hardware_state(zone);
                    if current_mode.is_ok() {
                        caps.led_zone_readback_valid[id] = true;
                    }
                    let mut item_mask = 0;
                    for mode in 0..=6 {
                        if modes & (1 << mode) != 0 {
                            if let Ok(items) = w.control_mask(id as u8, mode) {
                                item_mask |= items;
                                caps.led_items_by_mode[id][mode as usize] = items;
                            }
                        }
                    }
                    caps.led_items_by_zone[id] = item_mask;
                }
                Ok(())
            })
            .is_ok();
        caps
    }

    /// Restore only explicitly valid LED zones. WMI verifies each zone after writing.
    pub fn restore_led_snapshot(
        &self,
        zones: [LedZoneStateDto; 7],
        valid: [bool; 7],
    ) -> Result<(), HardwareError> {
        self.with_wmi(|w| {
            let mask = w.zone_mask()?;
            for id in 0..7 {
                if !valid[id] {
                    continue;
                }
                if mask & (1 << id) == 0 {
                    return Err(WmiError::Com(format!(
                        "Saved LED zone {id} is no longer available"
                    )));
                }
                w.restore_led_zone_snapshot(&zones[id])?;
            }
            if valid.iter().any(|is_valid| *is_valid) {
                w.commit_leds_to_bios_nvram()?;
                for id in 0..7 {
                    if valid[id] {
                        let zone = nuc11ph_protocol::PhantomLedZone::from_u8(id as u8).unwrap();
                        let actual = w.read_led_zone_hardware_state(zone)?;
                        if !reference::led_snapshot_matches(&zones[id], &actual) {
                            return Err(WmiError::Com(format!(
                                "LED zone {id} did not persist after commit"
                            )));
                        }
                    }
                }
            }
            Ok(())
        })
    }
}
