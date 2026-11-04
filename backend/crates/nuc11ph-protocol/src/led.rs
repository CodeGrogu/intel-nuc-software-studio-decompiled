//! `root\WMI:CISD_WMI` 7-Zone RGB Lighting protocol, control item byte offsets
//! (`ConstBytes`), state diffing (for zero-NVRAM-wear volatile updates), and
//! command builders for Intel NUC11PHi7 (Phantom Canyon).

use serde::{Deserialize, Serialize};

/// Number of addressable WMI RGB zones on the NUC11PHi7 chassis.
pub const PHANTOM_LED_ZONE_COUNT: usize = 7;

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash, Serialize, Deserialize)]
pub enum PhantomLedZone {
    PowerButton = 0,
    Hdd = 1,
    Skull = 2,
    Eyes = 3,
    Front1 = 4,
    Front2 = 5,
    Front3 = 6,
}

impl PhantomLedZone {
    pub const ALL: [PhantomLedZone; 7] = [
        Self::PowerButton,
        Self::Hdd,
        Self::Skull,
        Self::Eyes,
        Self::Front1,
        Self::Front2,
        Self::Front3,
    ];

    pub fn from_u8(val: u8) -> Option<Self> {
        match val {
            0 => Some(Self::PowerButton),
            1 => Some(Self::Hdd),
            2 => Some(Self::Skull),
            3 => Some(Self::Eyes),
            4 => Some(Self::Front1),
            5 => Some(Self::Front2),
            6 => Some(Self::Front3),
            _ => None,
        }
    }

    pub fn display_name(self) -> &'static str {
        match self {
            Self::PowerButton => "Power Button",
            Self::Hdd => "Storage / HDD Indicator",
            Self::Skull => "Phantom Skull Emblem",
            Self::Eyes => "Skull Eyes",
            Self::Front1 => "Front Lightbar Left",
            Self::Front2 => "Front Lightbar Center",
            Self::Front3 => "Front Lightbar Right",
        }
    }
}

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum IndicatorMode {
    PowerState = 0,
    HddActivity = 1,
    Ethernet = 2,
    Wifi = 3,
    Software = 4,
    PowerLimit = 5,
    Disable = 6,
}

impl IndicatorMode {
    pub fn from_u8(val: u8) -> Self {
        match val {
            0 => Self::PowerState,
            1 => Self::HddActivity,
            2 => Self::Ethernet,
            3 => Self::Wifi,
            4 => Self::Software,
            5 => Self::PowerLimit,
            _ => Self::Disable,
        }
    }
}

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum LedBehavior {
    Solid = 0,
    Breathing = 1,
    Pulsing = 2,
    Strobing = 3,
}

impl LedBehavior {
    pub fn from_u8(val: u8) -> Self {
        match val {
            1 => Self::Breathing,
            2 => Self::Pulsing,
            3 => Self::Strobing,
            _ => Self::Solid,
        }
    }
}

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum PowerStateSlot {
    S0SystemOn = 0,
    S3Sleep = 1,
    ModernStandby = 2,
    S5SoftOff = 3,
}

impl PowerStateSlot {
    /// Base control item byte index (`0`, `6`, `12`, `18`) for `IndicatorMode::PowerState`.
    pub const fn base_index(self) -> u8 {
        match self {
            Self::S0SystemOn => 0,
            Self::S3Sleep => 6,
            Self::ModernStandby => 12,
            Self::S5SoftOff => 18,
        }
    }
}

#[repr(C)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub struct PowerStateLightConfig {
    pub brightness: u8, // 0..=100
    pub behavior: u8,   // LedBehavior (0..=3)
    pub frequency: u8,  // 1..=10
    pub r: u8,
    pub g: u8,
    pub b: u8,
}

impl Default for PowerStateLightConfig {
    fn default() -> Self {
        Self {
            brightness: 80,
            behavior: LedBehavior::Solid as u8,
            frequency: 1,
            r: 0,
            g: 180,
            b: 255,
        }
    }
}

/// Complete state of a single LED zone on the NUC11PHi7.
#[repr(C)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub struct LedZoneStateDto {
    pub zone_id: u8,        // PhantomLedZone (0..=6)
    pub indicator_mode: u8, // IndicatorMode (0..=6)
    pub s0: PowerStateLightConfig,
    pub s3: PowerStateLightConfig,
    pub modern_standby: PowerStateLightConfig,
    pub s5: PowerStateLightConfig,
}

impl LedZoneStateDto {
    pub fn default_for_zone(zone: PhantomLedZone) -> Self {
        let (r, g, b, mode) = match zone {
            PhantomLedZone::Skull => (0, 210, 255, IndicatorMode::Software),
            PhantomLedZone::Eyes => (255, 0, 85, IndicatorMode::Software),
            PhantomLedZone::PowerButton => (0, 180, 255, IndicatorMode::PowerState),
            PhantomLedZone::Hdd => (0, 255, 160, IndicatorMode::HddActivity),
            PhantomLedZone::Front1 | PhantomLedZone::Front2 | PhantomLedZone::Front3 => {
                (0, 140, 255, IndicatorMode::Software)
            }
        };
        let s0 = PowerStateLightConfig {
            brightness: 80,
            behavior: LedBehavior::Solid as u8,
            frequency: 2,
            r,
            g,
            b,
        };
        let s3 = PowerStateLightConfig {
            brightness: 25,
            behavior: LedBehavior::Breathing as u8,
            frequency: 1,
            r,
            g,
            b,
        };
        let off = PowerStateLightConfig {
            brightness: 0,
            behavior: LedBehavior::Solid as u8,
            frequency: 1,
            r: 0,
            g: 0,
            b: 0,
        };
        Self {
            zone_id: zone as u8,
            indicator_mode: mode as u8,
            s0,
            s3,
            modern_standby: s3,
            s5: off,
        }
    }

    /// Generates the list of `(control_item_index, byte_value)` pairs for `SetValueIndicatorOptionLedType`.
    /// When `previous` is provided, only changed byte indices are emitted, cutting WMI roundtrips
    /// down to the exact changed bytes (typically 1–4 calls instead of 24).
    pub fn diff_control_items(&self, previous: Option<&Self>) -> Vec<(u8, u8)> {
        let mut writes = Vec::with_capacity(24);
        let mode = IndicatorMode::from_u8(self.indicator_mode);
        let mode_changed = previous.map_or(true, |p| p.indicator_mode != self.indicator_mode);

        match mode {
            IndicatorMode::Disable => {}
            IndicatorMode::Software => {
                let mut push_if_changed = |idx: u8, val: u8, prev_val: Option<u8>| {
                    if mode_changed || prev_val != Some(val) {
                        writes.push((idx, val));
                    }
                };
                let p = previous.map(|prev| &prev.s0);
                push_if_changed(0, self.s0.brightness.min(100), p.map(|x| x.brightness));
                push_if_changed(1, self.s0.behavior.min(3), p.map(|x| x.behavior));
                push_if_changed(2, self.s0.frequency.clamp(1, 10), p.map(|x| x.frequency));
                push_if_changed(3, self.s0.r, p.map(|x| x.r));
                push_if_changed(4, self.s0.g, p.map(|x| x.g));
                push_if_changed(5, self.s0.b, p.map(|x| x.b));
            }
            IndicatorMode::PowerState => {
                let slots = [
                    (
                        PowerStateSlot::S0SystemOn,
                        &self.s0,
                        previous.map(|p| &p.s0),
                    ),
                    (PowerStateSlot::S3Sleep, &self.s3, previous.map(|p| &p.s3)),
                    (
                        PowerStateSlot::ModernStandby,
                        &self.modern_standby,
                        previous.map(|p| &p.modern_standby),
                    ),
                    (PowerStateSlot::S5SoftOff, &self.s5, previous.map(|p| &p.s5)),
                ];
                for (slot, curr, prev) in slots {
                    let base = slot.base_index();
                    let mut push_if = |offset: u8, val: u8, prev_val: Option<u8>| {
                        if mode_changed || prev_val != Some(val) {
                            writes.push((base + offset, val));
                        }
                    };
                    push_if(0, curr.brightness.min(100), prev.map(|x| x.brightness));
                    push_if(1, curr.behavior.min(3), prev.map(|x| x.behavior));
                    push_if(2, curr.frequency.clamp(1, 10), prev.map(|x| x.frequency));
                    push_if(3, curr.r, prev.map(|x| x.r));
                    push_if(4, curr.g, prev.map(|x| x.g));
                    push_if(5, curr.b, prev.map(|x| x.b));
                }
            }
            IndicatorMode::HddActivity => {
                writes.push((0, self.s0.brightness.min(100)));
                writes.push((1, self.s0.r));
                writes.push((2, self.s0.g));
                writes.push((3, self.s0.b));
                writes.push((4, self.s0.behavior.min(1)));
            }
            IndicatorMode::Ethernet | IndicatorMode::PowerLimit => {
                writes.push((0, self.s0.behavior.min(2)));
                writes.push((1, self.s0.brightness.min(100)));
                writes.push((2, self.s0.r));
                writes.push((3, self.s0.g));
                writes.push((4, self.s0.b));
            }
            IndicatorMode::Wifi => {
                writes.push((0, self.s0.brightness.min(100)));
                writes.push((1, self.s0.r));
                writes.push((2, self.s0.g));
                writes.push((3, self.s0.b));
            }
        }
        writes
    }
}

/// Packs 4 bytes into the little-endian `uint32 Cmd` parameter expected by `CISD_WMI` methods.
#[inline]
pub const fn pack_wmi_cmd(b0: u8, b1: u8, b2: u8, b3: u8) -> u32 {
    u32::from_le_bytes([b0, b1, b2, b3])
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_led_diff_minimizes_wmi_writes() {
        let base = LedZoneStateDto::default_for_zone(PhantomLedZone::Skull);
        let mut modified = base;
        modified.s0.r = 255;

        let diff = modified.diff_control_items(Some(&base));
        // Only index 3 (Red) changed!
        assert_eq!(diff, vec![(3, 255)]);
    }
}
