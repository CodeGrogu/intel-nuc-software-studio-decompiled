//! Kernel driver interface for `PerformanceDriver.sys` (`ACPI\INTC1036`,
//! `{1ACD3A40-C4DB-4542-9C73-9898B0D8FF6E}`).
//!
//! Remediates BUG-01 from legacy `DriverControl.cs`: Never calls `DeleteService("ACPI\\INTC1036")`.
//! Device discovery never installs, starts or registers a driver.

use std::ffi::OsString;
use std::mem::{size_of, zeroed};
use std::os::windows::ffi::OsStringExt;
use thiserror::Error;
use windows::core::{GUID, PCWSTR};
use windows::Win32::Devices::DeviceAndDriverInstallation::{
    SetupDiDestroyDeviceInfoList, SetupDiEnumDeviceInterfaces, SetupDiGetClassDevsW,
    SetupDiGetDeviceInterfaceDetailW, DIGCF_DEVICEINTERFACE, DIGCF_PRESENT,
    SP_DEVICE_INTERFACE_DATA, SP_DEVICE_INTERFACE_DETAIL_DATA_W, SP_DEVINFO_DATA,
};
use windows::Win32::Foundation::{CloseHandle, GENERIC_READ, GENERIC_WRITE, HANDLE};
use windows::Win32::Storage::FileSystem::{
    CreateFileW, FILE_ATTRIBUTE_NORMAL, FILE_SHARE_READ, FILE_SHARE_WRITE, OPEN_EXISTING,
};
use windows::Win32::System::IO::DeviceIoControl;

use crate::mutex::IsaBusMutex;
use nuc11ph_protocol::{
    FanDutyPacket, FanProfileEcRaw, FanProfileWmiRaw, FanSpeedStructRaw, TemperatureProfileEcRaw,
    ECNUC22_SECTION,
};

// DriverControl.cs GUID constructor starts with decimal 449657408 (0x1ACD3A40).
// This matches the device interface registered by the installed 1.0.0.17 driver.
pub const GUID_DEVINTERFACE_NUCWDT: GUID = GUID::from_u128(0x1ACD3A40_C4DB_4542_9C73_9898B0D8FF6E);

#[repr(u32)]
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum NucIoctl {
    MsrReadPowerUnit = 0x9C40_600C, // 2621464588
    MsrReadPowerLimit = 0x9C40_6010,
    MsrWritePowerLimit = 0x9C40_A014,
    ReadFanProfile = 0x9C40_6020,
    WriteFanProfileNuc22 = 0x9C40_A01C,
    ReadFanSpeed = 0x9C40_6024,
    SetFanSpeed = 0x9C40_A040,
    ReadDriverStatus = 0x9C40_6044,
    ReadTemperatureProfile = 0x9C40_6048,
}

#[derive(Debug, Error)]
pub enum DriverError {
    #[error("ACPI\\INTC1036 (PerformanceDriver.sys) device interface not found via SetupAPI")]
    DeviceNotFound,
    #[error("Failed to open PerformanceDriver.sys handle: {0}")]
    OpenHandleFailed(String),
    #[error("IOCTL 0x{ioctl:08X} failed: {reason}")]
    IoctlFailed { ioctl: u32, reason: String },
    #[error("ISA Bus Mutex error: {0}")]
    Mutex(#[from] crate::mutex::MutexError),
}

/// The `PerformanceDriver.sys` operations the controller uses. `PerformanceDriver` is the
/// only production implementation; tests substitute an in-memory fake.
pub trait DriverOps: Send + Sync {
    fn check_driver_status(&self) -> Result<bool, DriverError>;
    fn read_power_limit_msr(&self) -> Result<u64, DriverError>;
    fn write_power_limit_msr(&self, msr_value: u64) -> Result<(), DriverError>;
    fn read_ec_fan_profile(&self) -> Result<FanProfileEcRaw, DriverError>;
    fn write_wmi_fan_profile_nuc22(&self, profile: &FanProfileWmiRaw) -> Result<(), DriverError>;
    fn read_fan_speeds_in_section(&self, ec_section: u8) -> Result<FanSpeedStructRaw, DriverError>;
    fn set_fixed_fan_duty(&self, fan_number: u8, duty_pct: u8) -> Result<(), DriverError>;
    fn read_ec_temperatures(&self) -> Result<TemperatureProfileEcRaw, DriverError>;

    fn read_fan_speeds(&self) -> Result<FanSpeedStructRaw, DriverError> {
        self.read_fan_speeds_in_section(ECNUC22_SECTION)
    }

    /// Dedicated reviewed test017 packet [2,1,70]. This emits one command, not independent fan control.
    fn set_reviewed_fixed70(&self) -> Result<(), DriverError> {
        self.set_fixed_fan_duty(0, nuc11ph_protocol::REVIEWED_FIXED70_COMMAND[2])
    }
}

pub struct PerformanceDriver {
    handle: HANDLE,
    bus_mutex: IsaBusMutex,
}

unsafe impl Send for PerformanceDriver {}
unsafe impl Sync for PerformanceDriver {}

impl PerformanceDriver {
    /// Discovers the symbolic link path for `GUID_DEVINTERFACE_NUCWDT` and opens a read/write handle.
    /// A privileged service owns access; clients never auto-install a driver.
    pub fn open() -> Result<Self, DriverError> {
        let device_path = Self::find_device_interface_path()?;

        let mut wide_path: Vec<u16> = device_path.encode_utf16().collect();
        wide_path.push(0);

        let handle = unsafe {
            CreateFileW(
                PCWSTR::from_raw(wide_path.as_ptr()),
                (GENERIC_READ | GENERIC_WRITE).0,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                None,
                OPEN_EXISTING,
                FILE_ATTRIBUTE_NORMAL,
                None,
            )
            .map_err(|e| DriverError::OpenHandleFailed(e.to_string()))?
        };

        if handle.is_invalid() {
            return Err(DriverError::OpenHandleFailed(
                "CreateFileW returned INVALID_HANDLE_VALUE".to_string(),
            ));
        }

        let bus_mutex = IsaBusMutex::new()?;
        Ok(Self { handle, bus_mutex })
    }

    fn find_device_interface_path() -> Result<String, DriverError> {
        unsafe {
            let flags = DIGCF_PRESENT | DIGCF_DEVICEINTERFACE;
            let dev_info =
                SetupDiGetClassDevsW(Some(&GUID_DEVINTERFACE_NUCWDT), PCWSTR::null(), None, flags)
                    .map_err(|_| DriverError::DeviceNotFound)?;

            if dev_info.is_invalid() {
                return Err(DriverError::DeviceNotFound);
            }

            let mut iface_data = SP_DEVICE_INTERFACE_DATA {
                cbSize: size_of::<SP_DEVICE_INTERFACE_DATA>() as u32,
                ..zeroed()
            };

            let enum_ok = SetupDiEnumDeviceInterfaces(
                dev_info,
                None,
                &GUID_DEVINTERFACE_NUCWDT,
                0,
                &mut iface_data,
            );
            if enum_ok.is_err() {
                let _ = SetupDiDestroyDeviceInfoList(dev_info);
                return Err(DriverError::DeviceNotFound);
            }

            let mut required_size: u32 = 0;
            let _ = SetupDiGetDeviceInterfaceDetailW(
                dev_info,
                &iface_data,
                None,
                0,
                Some(&mut required_size),
                None,
            );

            if required_size < 6 {
                let _ = SetupDiDestroyDeviceInfoList(dev_info);
                return Err(DriverError::DeviceNotFound);
            }

            // Allocate 8-byte aligned buffer to avoid unaligned pointer dereference
            let u64_len = (required_size as usize + 7) / 8;
            let mut aligned_buf = vec![0u64; u64_len];
            let detail_ptr = aligned_buf.as_mut_ptr() as *mut SP_DEVICE_INTERFACE_DETAIL_DATA_W;
            (*detail_ptr).cbSize = if size_of::<usize>() == 8 { 8 } else { 6 };

            let mut dev_info_data = SP_DEVINFO_DATA {
                cbSize: size_of::<SP_DEVINFO_DATA>() as u32,
                ..zeroed()
            };

            let detail_ok = SetupDiGetDeviceInterfaceDetailW(
                dev_info,
                &iface_data,
                Some(detail_ptr),
                required_size,
                Some(&mut required_size),
                Some(&mut dev_info_data),
            );
            let _ = SetupDiDestroyDeviceInfoList(dev_info);

            detail_ok.map_err(|_| DriverError::DeviceNotFound)?;

            // Extract null-terminated UTF-16 DevicePath starting at offset 4 (`DevicePath[0]`)
            let byte_ptr = aligned_buf.as_ptr() as *const u8;
            let path_offset = 4usize;
            let u16_len = (required_size as usize).saturating_sub(path_offset) / 2;
            let u16_slice =
                std::slice::from_raw_parts(byte_ptr.add(path_offset) as *const u16, u16_len);
            let nul_pos = u16_slice
                .iter()
                .position(|&c| c == 0)
                .unwrap_or(u16_slice.len());
            let os_str = OsString::from_wide(&u16_slice[..nul_pos]);
            let path = os_str.to_string_lossy().into_owned();
            if path.is_empty() {
                return Err(DriverError::DeviceNotFound);
            }
            Ok(path)
        }
    }
}

impl DriverOps for PerformanceDriver {
    fn check_driver_status(&self) -> Result<bool, DriverError> {
        let mut bytes_returned = 0u32;
        let res = unsafe {
            DeviceIoControl(
                self.handle,
                NucIoctl::ReadDriverStatus as u32,
                None,
                0,
                None,
                0,
                Some(&mut bytes_returned),
                None,
            )
        };
        Ok(res.is_ok())
    }

    fn read_power_limit_msr(&self) -> Result<u64, DriverError> {
        let _guard = self.bus_mutex.lock(500)?;
        let mut out_val: u64 = 0;
        let mut bytes_returned = 0u32;
        unsafe {
            DeviceIoControl(
                self.handle,
                NucIoctl::MsrReadPowerLimit as u32,
                None,
                0,
                Some(&mut out_val as *mut u64 as *mut _),
                size_of::<u64>() as u32,
                Some(&mut bytes_returned),
                None,
            )
            .map_err(|e| DriverError::IoctlFailed {
                ioctl: NucIoctl::MsrReadPowerLimit as u32,
                reason: e.to_string(),
            })?;
        }
        if bytes_returned != size_of::<u64>() as u32 {
            return Err(DriverError::IoctlFailed {
                ioctl: NucIoctl::MsrReadPowerLimit as u32,
                reason: format!("Short driver response: {} bytes", bytes_returned),
            });
        }
        Ok(out_val)
    }

    fn write_power_limit_msr(&self, msr_value: u64) -> Result<(), DriverError> {
        let _guard = self.bus_mutex.lock(500)?;
        let mut bytes_returned = 0u32;
        unsafe {
            DeviceIoControl(
                self.handle,
                NucIoctl::MsrWritePowerLimit as u32,
                Some(&msr_value as *const u64 as *const _),
                size_of::<u64>() as u32,
                None,
                0,
                Some(&mut bytes_returned),
                None,
            )
            .map_err(|e| DriverError::IoctlFailed {
                ioctl: NucIoctl::MsrWritePowerLimit as u32,
                reason: e.to_string(),
            })?;
        }
        Ok(())
    }

    fn read_ec_fan_profile(&self) -> Result<FanProfileEcRaw, DriverError> {
        let _guard = self.bus_mutex.lock(500)?;
        let mut out_profile = FanProfileEcRaw::default();
        let mut bytes_returned = 0u32;
        unsafe {
            DeviceIoControl(
                self.handle,
                NucIoctl::ReadFanProfile as u32,
                None,
                0,
                Some(&mut out_profile as *mut FanProfileEcRaw as *mut _),
                size_of::<FanProfileEcRaw>() as u32,
                Some(&mut bytes_returned),
                None,
            )
            .map_err(|e| DriverError::IoctlFailed {
                ioctl: NucIoctl::ReadFanProfile as u32,
                reason: e.to_string(),
            })?;
        }
        if bytes_returned != size_of::<FanProfileEcRaw>() as u32 {
            return Err(DriverError::IoctlFailed {
                ioctl: NucIoctl::ReadFanProfile as u32,
                reason: format!("Short driver response: {} bytes", bytes_returned),
            });
        }
        Ok(out_profile)
    }

    fn write_wmi_fan_profile_nuc22(&self, profile: &FanProfileWmiRaw) -> Result<(), DriverError> {
        let _guard = self.bus_mutex.lock(500)?;
        let mut bytes_returned = 0u32;
        unsafe {
            DeviceIoControl(
                self.handle,
                NucIoctl::WriteFanProfileNuc22 as u32,
                Some(profile as *const FanProfileWmiRaw as *const _),
                size_of::<FanProfileWmiRaw>() as u32,
                None,
                0,
                Some(&mut bytes_returned),
                None,
            )
            .map_err(|e| DriverError::IoctlFailed {
                ioctl: NucIoctl::WriteFanProfileNuc22 as u32,
                reason: e.to_string(),
            })?;
        }
        Ok(())
    }

    fn read_fan_speeds_in_section(&self, ec_section: u8) -> Result<FanSpeedStructRaw, DriverError> {
        let _guard = self.bus_mutex.lock(500)?;

        let mut out_speeds = FanSpeedStructRaw::default();
        let mut bytes_returned = 0u32;
        unsafe {
            DeviceIoControl(
                self.handle,
                NucIoctl::ReadFanSpeed as u32,
                Some(&ec_section as *const u8 as *const _),
                size_of::<u8>() as u32,
                Some(&mut out_speeds as *mut FanSpeedStructRaw as *mut _),
                size_of::<FanSpeedStructRaw>() as u32,
                Some(&mut bytes_returned),
                None,
            )
            .map_err(|e| DriverError::IoctlFailed {
                ioctl: NucIoctl::ReadFanSpeed as u32,
                reason: e.to_string(),
            })?;
        }
        if bytes_returned != size_of::<FanSpeedStructRaw>() as u32 {
            return Err(DriverError::IoctlFailed {
                ioctl: NucIoctl::ReadFanSpeed as u32,
                reason: format!("Short driver response: {} bytes", bytes_returned),
            });
        }
        Ok(out_speeds)
    }

    fn set_fixed_fan_duty(&self, fan_number: u8, duty_pct: u8) -> Result<(), DriverError> {
        let _guard = self.bus_mutex.lock(500)?;
        if fan_number >= 6 || !(25..=99).contains(&duty_pct) {
            return Err(DriverError::IoctlFailed {
                ioctl: NucIoctl::SetFanSpeed as u32,
                reason: "Invalid fan slot or duty".into(),
            });
        }
        let packet = FanDutyPacket {
            nuc_ec_section: ECNUC22_SECTION,
            fan_number: nuc11ph_protocol::driver_fan_number(fan_number).map_err(|reason| {
                DriverError::IoctlFailed {
                    ioctl: NucIoctl::SetFanSpeed as u32,
                    reason: reason.into(),
                }
            })?,
            fan_duty: duty_pct,
        };
        let mut bytes_returned = 0u32;
        unsafe {
            DeviceIoControl(
                self.handle,
                NucIoctl::SetFanSpeed as u32,
                Some(&packet as *const FanDutyPacket as *const _),
                size_of::<FanDutyPacket>() as u32,
                None,
                0,
                Some(&mut bytes_returned),
                None,
            )
            .map_err(|e| DriverError::IoctlFailed {
                ioctl: NucIoctl::SetFanSpeed as u32,
                reason: e.to_string(),
            })?;
        }
        Ok(())
    }

    fn read_ec_temperatures(&self) -> Result<TemperatureProfileEcRaw, DriverError> {
        let _guard = self.bus_mutex.lock(500)?;
        let mut out_temps = TemperatureProfileEcRaw::default();
        let mut bytes_returned = 0u32;
        unsafe {
            DeviceIoControl(
                self.handle,
                NucIoctl::ReadTemperatureProfile as u32,
                None,
                0,
                Some(&mut out_temps as *mut TemperatureProfileEcRaw as *mut _),
                size_of::<TemperatureProfileEcRaw>() as u32,
                Some(&mut bytes_returned),
                None,
            )
            .map_err(|e| DriverError::IoctlFailed {
                ioctl: NucIoctl::ReadTemperatureProfile as u32,
                reason: e.to_string(),
            })?;
        }
        if bytes_returned != size_of::<TemperatureProfileEcRaw>() as u32 {
            return Err(DriverError::IoctlFailed {
                ioctl: NucIoctl::ReadTemperatureProfile as u32,
                reason: format!("Short driver response: {} bytes", bytes_returned),
            });
        }
        Ok(out_temps)
    }
}

impl Drop for PerformanceDriver {
    fn drop(&mut self) {
        unsafe {
            if !self.handle.is_invalid() {
                let _ = CloseHandle(self.handle);
            }
        }
    }
}
