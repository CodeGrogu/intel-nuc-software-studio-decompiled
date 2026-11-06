//! Revision 6 control-panel contracts. Unknown readings are never represented by zero.
use crate::{BiosPowerProfile, LedFieldState, PowerLimitConfig, PowerPresetKind};
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, PartialEq, Serialize, Deserialize)]
pub struct Reading {
    pub value: Option<f64>,
    pub source: String,
    pub timestamp_ms: u64,
    pub error: Option<String>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct TelemetryStatus {
    pub cpu_temperature: Reading,
    pub gpu_temperature: Reading,
    pub cpu_fan: Reading,
    pub gpu_fan: Reading,
}

#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum StockFanPreset {
    Balanced,
    Cool,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct ControlPanelState {
    pub backend_instance: String,
    pub model: String,
    pub bios: String,
    pub device_eligible: bool,
    pub power_presets: Vec<BiosPowerProfile>,
    pub custom_power_bounds: Option<crate::CustomPowerBounds>,
    pub fixed_fan_70: bool,
    pub fixed_fan_70_available: bool,
    pub advanced_lighting_available: bool,
    pub power_limits: Option<PowerLimitConfig>,
    pub power_readback: Option<PowerReadback>,
    pub power_unavailable: Option<String>,
    pub fan_preset: Option<StockFanPreset>,
    pub fan_unavailable: Option<String>,
    pub led_zone_mask: u32,
}

/// Exact revision-6 readback. The legacy integer snapshot remains unchanged.
#[derive(Debug, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub struct PowerReadback {
    /// Exact register precondition encoded as fixed-width hexadecimal for JSON clients.
    pub raw_register: String,
    pub pl1_eighth_watts: u16,
    pub pl2_eighth_watts: u16,
    pub tau_code: u8,
    pub tau_seconds: Option<u16>,
}
impl PowerReadback {
    pub fn from_raw(raw: u64) -> Self {
        let tau_code = ((raw & crate::TAU_MASK) >> 17) as u8;
        Self {
            raw_register: format!("{raw:016X}"),
            pl1_eighth_watts: (raw & crate::PL1_MASK) as u16,
            pl2_eighth_watts: ((raw & crate::PL2_MASK) >> 32) as u16,
            tau_code,
            tau_seconds: crate::TAU_LOOKUP_TABLE
                .iter()
                .find(|(_, code)| *code == tau_code)
                .map(|(seconds, _)| *seconds),
        }
    }
    pub fn exact_limits(&self) -> Option<PowerLimitConfig> {
        if self.pl1_eighth_watts % 8 != 0 || self.pl2_eighth_watts % 8 != 0 {
            return None;
        }
        Some(PowerLimitConfig {
            pl1_watts: u8::try_from(self.pl1_eighth_watts / 8).ok()?,
            pl2_watts: u8::try_from(self.pl2_eighth_watts / 8).ok()?,
            tau_seconds: self.tau_seconds?,
        })
    }
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct ControlSessionInfo {
    pub backend_instance: String,
    pub admission_epoch: u64,
}

#[derive(Debug, Clone, PartialEq, Serialize, Deserialize)]
pub struct AwakeLightingState {
    pub zone_id: u8,
    pub active_role: u8,
    pub fields: LedFieldState,
    pub unavailable: Option<String>,
}

#[derive(Debug, Clone, PartialEq, Serialize, Deserialize)]
pub struct LightingPatch {
    pub expected: AwakeLightingState,
    pub brightness: Option<u8>,
    pub rgb: Option<[u8; 3]>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub enum ControlMutation {
    CustomPower {
        patch: crate::CustomPowerPatch,
    },
    FixedFan70 {},
    SaveLighting {
        expected: crate::LightingSaveState,
    },
    AdvancedLighting {
        patch: crate::AdvancedLightingPatch,
    },
    LightingPreset {
        patches: Vec<crate::AdvancedLightingPatch>,
    },
    Power {
        preset: PowerPresetKind,
    },
    Fan {
        preset: StockFanPreset,
    },
    Lighting {
        patch: LightingPatch,
    },
}

#[derive(Debug, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub enum OperationOutcome {
    Acknowledged,
    Pending,
    Verified,
    Rejected,
    PartialFailure,
    Uncertain,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct OperationStatus {
    pub operation_id: String,
    pub backend_instance: String,
    pub outcome: OperationOutcome,
    pub message: String,
}

/// Project only bytes the driver can read; never invent the unreadable tail.
pub fn project_stock_fan(packet: &[u8; 90], baseline: &[u8; 94]) -> [u8; 94] {
    let mut projected = *baseline;
    let mut target = 0;
    for (i, value) in packet.iter().enumerate().skip(3) {
        if target < projected.len() {
            projected[target] = *value;
        }
        target += if i > 14 && i % 14 == 2 { 3 } else { 1 };
    }
    projected
}

pub fn stock_fan_matches(packet: &[u8; 90], actual: &[u8; 94]) -> bool {
    project_stock_fan(packet, actual) == *actual
}

pub fn lighting_editable(fields: &LedFieldState) -> bool {
    (0..6).all(|index| {
        fields.items.iter().any(|item| {
            item.index == index && item.firmware_status == Some(0) && item.value.is_some()
        })
    }) && fields
        .items
        .iter()
        .any(|item| item.index == 1 && item.value == Some(0))
}

/// Pure selected-zone plan, shared by the executor and regression tests.
/// Commands contain only changed awake brightness/RGB indexes; role is applied separately.
pub fn plan_lighting_patch(
    before: &AwakeLightingState,
    patch: &LightingPatch,
) -> Result<(AwakeLightingState, Vec<(u8, u8)>), &'static str> {
    if before != &patch.expected
        || before.unavailable.is_some()
        || !lighting_editable(&before.fields)
        || before.fields.zone_id != before.zone_id
        || before.fields.indicator_mode != 0
    {
        return Err("Lighting state changed or is not editable; refresh first");
    }
    if patch.brightness.is_some_and(|b| b > 100) {
        return Err("Brightness must be 0-100");
    }
    let mut expected = before.clone();
    let mut commands = Vec::new();
    for item in &mut expected.fields.items {
        let requested = match item.index {
            0 => patch.brightness,
            3..=5 => patch.rgb.map(|rgb| rgb[(item.index - 3) as usize]),
            _ => None,
        };
        if let Some(value) = requested {
            if item.value != Some(value) {
                commands.push((item.index, value));
                item.value = Some(value);
            }
        }
    }
    expected.active_role = 0;
    Ok((expected, commands))
}
