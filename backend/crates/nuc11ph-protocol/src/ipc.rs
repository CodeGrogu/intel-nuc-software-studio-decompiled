//! Named Pipe IPC protocol (`\\.\pipe\Nuc11PhStudioV1`) and FFI data transfer
//! structures shared between `nuc11ph-service`, `nuc11ph-ffi`, `Nuc11Ph.Studio`,
//! and `Nuc11Ph.WidgetProvider`.

use crate::cec::HdmiCecAction;
use crate::fan::EcNuc22FanCurveDto;
use crate::led::{LedFieldState, LedZoneStateDto};
use crate::power::{PowerLimitConfig, PowerPresetKind};
use serde::{Deserialize, Serialize};

/// Fixed-size C-ABI compatible telemetry snapshot for zero-overhead FFI and Named Pipe streaming.
#[repr(C)]
#[derive(Debug, Clone, Copy, Default, PartialEq, Serialize, Deserialize)]
pub struct TelemetrySnapshot {
    pub timestamp_ms: u64,
    pub cpu_temp_c: u8,
    pub dgpu_temp_c: u8,
    pub igpu_temp_c: u8,
    pub cpu_fan_rpm: u16,
    pub gpu_fan_rpm: u16,
    pub cpu_usage_pct: f32,
    pub cpu_clock_mhz: u16,
    pub dgpu_usage_pct: u8,
    pub dgpu_core_clock_mhz: u16,
    pub dgpu_mem_clock_mhz: u16,
    pub dgpu_power_watts: f32,
    pub dgpu_vram_used_mb: u16,
    pub dgpu_vram_total_mb: u16,
    pub ram_used_mb: u32,
    pub ram_total_mb: u32,
    pub active_pl1_watts: u8,
    pub active_pl2_watts: u8,
    pub active_tau_sec: u16,
    pub active_fan_mode: u8,
    pub fan_off_enabled: bool,
    pub stealth_mode_active: bool,
    pub driver_connected: bool,
    pub wmi_connected: bool,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct HardwareStateSnapshot {
    pub telemetry: TelemetrySnapshot,
    pub cpu_fan_curve: EcNuc22FanCurveDto,
    pub gpu_fan_curve: EcNuc22FanCurveDto,
    pub led_zones: [LedZoneStateDto; 7],
    pub active_profile_name: String,
    pub driver_status: String,
    pub wmi_status: String,
}

/// Live capability and readback report. False validity flags mean the value could not
/// be read from this device during the current query; callers must not treat defaults as readings.
#[derive(Debug, Clone, Default, Serialize, Deserialize)]
#[serde(default)]
pub struct HardwareCapabilities {
    pub driver_connected: bool,
    /// Bit 0: CPU blower discovered; bit 1: GPU blower discovered.
    pub fan_slots: u32,
    /// True only when the current power limits were read successfully from the MSR.
    pub power_limit_readback_valid: bool,
    pub wmi_connected: bool,
    pub fan_profile_readback_valid: bool,
    pub fan_mode_readback_valid: bool,
    pub led_zone_mask: u32,
    pub led_zone_readback_valid: [bool; 7],
    /// Per-zone bit masks of supported IndicatorMode values.
    pub led_modes_by_zone: [u32; 7],
    /// Per-zone bit masks of advertised control-item indexes. Advertisement is
    /// not evidence that an item getter succeeded; use LedFieldState for that.
    pub led_items_by_zone: [u32; 7],
    /// Per-zone, per-mode bit masks of advertised control-item indexes.
    /// Indexed as `[zone][indicator_mode]`.
    pub led_items_by_mode: [[u32; 7]; 7],
}

impl HardwareCapabilities {
    pub fn unavailable() -> Self {
        Self::default()
    }
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct VerifiedHardwareState {
    pub snapshot: Box<HardwareStateSnapshot>,
    pub capabilities: HardwareCapabilities,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub enum IpcRequest {
    GetLightingSaveState,
    GetLightingMode {
        zone_id: u8,
        indicator_mode: u8,
    },
    GetControlSessionInfo,
    GetControlPanelState,
    GetTelemetryStatus,
    GetAwakeLighting {
        zone_id: u8,
    },
    ApplyControl {
        operation_id: String,
        backend_instance: String,
        admission_epoch: u64,
        mutation: crate::ControlMutation,
    },
    ReconcileOperation {
        operation_id: String,
        backend_instance: String,
        admission_epoch: u64,
    },
    GetOperationStatus {
        operation_id: String,
    },
    GetBackendInfo,
    GetHardwareCapabilities,
    GetVerifiedHardwareState,
    GetLedFieldState {
        zone_id: u8,
        indicator_mode: u8,
    },
    RestoreLedSnapshot {
        zones: [LedZoneStateDto; 7],
        valid: [bool; 7],
    },
    GetLightingZones,
    Ping,
    GetTelemetrySnapshot,
    GetHardwareState,
    ApplyProfileByName {
        profile_name: String,
    },
    SetPowerPreset {
        preset: PowerPresetKind,
    },
    SetCustomPower {
        limits: PowerLimitConfig,
    },
    SetFanMode {
        mode: u8,
        fan_off_enabled: bool,
    },
    SetFixedFanDuty {
        cpu_fan_duty: u8,
        gpu_fan_duty: u8,
    },
    SetCustomFanCurve {
        cpu_fan: EcNuc22FanCurveDto,
        gpu_fan: EcNuc22FanCurveDto,
        fan_off_enabled: bool,
    },
    SetLedZoneVolatile {
        zone: LedZoneStateDto,
    },
    SetAllLedsVolatile {
        zones: [LedZoneStateDto; 7],
    },
    SetGlobalLedBrightnessVolatile {
        brightness: u8,
    },
    SetStealthModeVolatile {
        enable: bool,
    },
    CommitLedsToBiosNvram,
    SendHdmiCecCommand {
        action: HdmiCecAction,
    },
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub enum IpcResponse {
    ControlSessionInfo(crate::ControlSessionInfo),
    ControlPanelState(crate::ControlPanelState),
    TelemetryStatus(crate::TelemetryStatus),
    AwakeLighting(crate::AwakeLightingState),
    LightingMode(crate::LightingModeState),
    LightingSaveState(crate::LightingSaveState),
    OperationStatus(crate::OperationStatus),
    LightingZones {
        mask: u32,
    },
    BackendInfo {
        version: String,
        protocol_revision: u32,
    },
    HardwareCapabilities(HardwareCapabilities),
    VerifiedHardwareState(Box<VerifiedHardwareState>),
    LedFieldState(LedFieldState),
    Ok,
    Telemetry(TelemetrySnapshot),
    HardwareState(Box<HardwareStateSnapshot>),
    Error {
        message: String,
    },
}

#[cfg(test)]
mod led_field_wire_tests {
    use super::{IpcRequest, IpcResponse};
    use crate::led::{LedFieldState, LedItemReadback};

    #[test]
    fn additive_led_field_request_keeps_legacy_unit_requests_unchanged() {
        let request = r#"{"GetLedFieldState":{"zone_id":2,"indicator_mode":4}}"#;
        assert!(matches!(
            serde_json::from_str::<IpcRequest>(request).unwrap(),
            IpcRequest::GetLedFieldState {
                zone_id: 2,
                indicator_mode: 4
            }
        ));
        assert_eq!(
            serde_json::to_string(&IpcRequest::GetBackendInfo).unwrap(),
            r#""GetBackendInfo""#
        );
        assert!(matches!(
            serde_json::from_str::<IpcRequest>(r#""Ping""#).unwrap(),
            IpcRequest::Ping
        ));
    }

    #[test]
    fn partial_field_wire_response_preserves_null_unknown_and_successful_zero() {
        let state = LedFieldState {
            zone_id: 2,
            indicator_mode: 4,
            advertised_items_mask: 3,
            successfully_read_items_mask: 2,
            complete: false,
            items: vec![
                LedItemReadback {
                    index: 0,
                    firmware_status: Some(228),
                    value: None,
                    error: None,
                },
                LedItemReadback {
                    index: 1,
                    firmware_status: Some(0),
                    value: Some(0),
                    error: None,
                },
            ],
        };
        let wire = serde_json::to_string(&IpcResponse::LedFieldState(state.clone())).unwrap();
        let value: serde_json::Value = serde_json::from_str(&wire).unwrap();
        assert!(value["LedFieldState"]["items"][0]["value"].is_null());
        assert_eq!(value["LedFieldState"]["items"][1]["value"], 0);
        match serde_json::from_str::<IpcResponse>(&wire).unwrap() {
            IpcResponse::LedFieldState(actual) => assert_eq!(actual, state),
            _ => panic!("Wrong response variant"),
        }
    }
}
