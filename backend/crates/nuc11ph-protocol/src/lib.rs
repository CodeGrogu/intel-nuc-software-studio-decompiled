//! Shared hardware protocol, binary structures, MSR bitmasks, ECNUC22 fan curve
//! layouts, CISD_WMI 7-zone RGB models, and Named Pipe IPC contracts exclusively
//! for the Intel NUC 11 Enthusiast / Phantom Canyon (`NUC11PHi7`).

pub mod advanced;
pub mod cec;
pub mod config;
pub mod control;
pub mod fan;
pub mod ipc;
pub mod led;
pub mod power;
pub mod stock_fixtures;

pub use advanced::*;
pub use cec::*;
pub use config::*;
pub use control::*;
pub use fan::*;
pub use ipc::*;
pub use led::*;
pub use power::*;
pub const NUC11PH_BACKEND_REVISION: u32 = 6;
pub const NUC11PH_BACKEND_VERSION: &str = "0.1.2";

/// Hardware constants locked exclusively to Intel NUC11PHi7 (Phantom Canyon).
pub const NUC11PH_BIOS_PREFIX: &str = "PHTGL579";
pub const NUC11PH_ACPI_HW_ID: &str = "ACPI\\INTC1036";
pub const NUC11PH_NAMED_PIPE: &str = r"\\.\pipe\Nuc11PhStudioV1";
pub const NUC11PH_MUTEX_NAME: &str = "Global\\Access_ISABUS.HTP.Method";

/// Standard CISD_WMI return status codes (`Data.Bytes[0]`).
#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum CisdWmiStatus {
    NoError = 0x00,
    FunctionNotSupported = 0xE1, // 225
    UndefinedDevice = 0xE2,      // 226
    EcNoResponse = 0xE3,         // 227
    InvalidParameter = 0xE4,     // 228
    UnexpectedError = 0xEF,      // 239
}

impl CisdWmiStatus {
    pub fn from_byte(b: u8) -> Result<(), &'static str> {
        match b {
            0x00 => Ok(()),
            0xE1 => Err("CISD_WMI: Function not supported (0xE1)"),
            0xE2 => Err("CISD_WMI: Undefined device (0xE2)"),
            0xE3 => Err("CISD_WMI: Embedded Controller no response (0xE3)"),
            0xE4 => Err("CISD_WMI: Invalid parameter (0xE4)"),
            0xEF => Err("CISD_WMI: Unexpected firmware error (0xEF)"),
            _ => Err("CISD_WMI: Unknown status code"),
        }
    }
}
