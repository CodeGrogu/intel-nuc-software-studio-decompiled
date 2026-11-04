//! HDMI CEC & Display Power methods exposed by `root\WMI:CISD_WMI` on NUC11PHi7.

use crate::led::pack_wmi_cmd;
use serde::{Deserialize, Serialize};

pub const WMI_METHOD_SET_DISPLAY_POWER: &str = "SetDisplayDevicePower";
pub const WMI_METHOD_GET_EDID_ON_HDMI: &str = "GetEdidOnHdmi";
pub const WMI_METHOD_CEC_PASSTHROUGH: &str = "CecMessagePassingThrough";

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum HdmiCecAction {
    WakeDisplay = 1,
    StandbyDisplay = 2,
    QueryDisplayEdid = 3,
}

impl HdmiCecAction {
    /// Returns `(wmi_method_name, cmd_u32)` for `CISD_WMI`.
    pub fn to_wmi_invocation(self) -> (&'static str, u32) {
        match self {
            Self::WakeDisplay => (WMI_METHOD_SET_DISPLAY_POWER, pack_wmi_cmd(1, 1, 0, 0)),
            Self::StandbyDisplay => (WMI_METHOD_SET_DISPLAY_POWER, pack_wmi_cmd(1, 0, 0, 0)),
            Self::QueryDisplayEdid => (WMI_METHOD_GET_EDID_ON_HDMI, pack_wmi_cmd(1, 0, 0, 0)),
        }
    }
}
