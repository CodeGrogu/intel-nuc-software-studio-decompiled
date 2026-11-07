//! MSR `0x610` (`MSR_PKG_POWER_LIMIT`) bitfield manipulation, BIOS Turbo Profile
//! parsing (`PlatformConfigurationProfileReport`), and the 27-entry Tiger Lake
//! `Tau` lookup table for Intel Core i7-1165G7 on NUC11PHi7.

use serde::{Deserialize, Serialize};

/// Bitmask for PL1 (bits 0..=13, 1/8th Watt units).
pub const PL1_MASK: u64 = 0x0000_0000_0000_3FFF;
/// Bitmask for PL2 (bits 32..=45, 1/8th Watt units).
pub const PL2_MASK: u64 = 0x0000_3FFF_0000_0000;
/// Bitmask for Tau time window (bits 17..=23).
pub const TAU_MASK: u64 = 0x0000_0000_00FE_0000;

/// Safe minimum and maximum sustained/burst package power limits on NUC11PHi7 (Watts).
pub const MIN_SAFE_PL1_WATTS: u8 = 15;
pub const MAX_SAFE_PL1_WATTS: u8 = 64;
pub const MIN_SAFE_PL2_WATTS: u8 = 20;
pub const MAX_SAFE_PL2_WATTS: u8 = 64;

/// Exhaustive 27-entry lookup table mapping `(seconds, msr_encoded_code)`.
/// Remediates BUG-04 in legacy Intel NSS where the UI listed `58` instead of `56` (`56 -> 111`).
pub const TAU_LOOKUP_TABLE: [(u16, u8); 27] = [
    (1, 10),
    (2, 11),
    (3, 75),
    (4, 12),
    (5, 43),
    (6, 76),
    (7, 108),
    (8, 13),
    (10, 45),
    (12, 77),
    (14, 109),
    (16, 14),
    (20, 46),
    (24, 78),
    (28, 110),
    (32, 15),
    (40, 47),
    (48, 79),
    (56, 111),
    (64, 16),
    (80, 48),
    (96, 80),
    (112, 112),
    (128, 17),
    (160, 49),
    (192, 81),
    (224, 113),
];

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum PowerPresetKind {
    LowPower = 0,
    Balanced = 1,
    MaxPerformance = 2,
    Custom = 3,
}

impl PowerPresetKind {
    pub fn default_limits(self) -> PowerLimitConfig {
        match self {
            Self::LowPower => PowerLimitConfig {
                pl1_watts: 15,
                pl2_watts: 30,
                tau_seconds: 16,
            },
            Self::Balanced => PowerLimitConfig {
                pl1_watts: 28,
                pl2_watts: 60,
                tau_seconds: 28,
            },
            Self::MaxPerformance => PowerLimitConfig {
                pl1_watts: 45,
                pl2_watts: 64,
                tau_seconds: 56,
            },
            Self::Custom => PowerLimitConfig {
                pl1_watts: 40,
                pl2_watts: 64,
                tau_seconds: 28,
            },
        }
    }
}

#[repr(C)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub struct PowerLimitConfig {
    pub pl1_watts: u8,
    pub pl2_watts: u8,
    pub tau_seconds: u16,
}

#[derive(Debug, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub struct BiosPowerProfile {
    pub name: String,
    pub preset_kind: PowerPresetKind,
    pub pl1_watts: u8,
    pub pl2_watts: u8,
    pub tau_seconds: u16,
    pub is_bios_default: bool,
}

/// Converts a user-selected `Tau` duration in seconds to the nearest valid MSR code.
pub fn tau_seconds_to_msr_code(seconds: u16) -> u8 {
    TAU_LOOKUP_TABLE
        .iter()
        .min_by_key(|(s, _)| (*s as i32 - seconds as i32).abs())
        .map(|(_, code)| *code)
        .unwrap_or(110) // 28s default
}

/// Converts an MSR-encoded `Tau` code (`bits 17..=23`) back to seconds.
pub fn msr_code_to_tau_seconds(code: u8) -> u16 {
    TAU_LOOKUP_TABLE
        .iter()
        .find(|(_, c)| *c == code)
        .map(|(s, _)| *s)
        .unwrap_or(28)
}

/// Decodes `PL1`, `PL2`, and `Tau` from the raw 64-bit `MSR 0x610` register value.
pub fn decode_msr_power_limit(msr_raw: u64) -> PowerLimitConfig {
    let pl1_watts = (((msr_raw & PL1_MASK) as u32) >> 3) as u8;
    let pl2_watts = ((((msr_raw & PL2_MASK) >> 32) as u32) >> 3) as u8;
    let tau_code = (((msr_raw & TAU_MASK) >> 17) as u32) as u8;
    let tau_seconds = msr_code_to_tau_seconds(tau_code);

    PowerLimitConfig {
        pl1_watts,
        pl2_watts,
        tau_seconds,
    }
}

/// Encodes `PL1`, `PL2`, and `Tau` into the 64-bit `MSR 0x610` register value while
/// preserving all other enable/lock/clamp bits in `current_msr` and enforcing safe hardware bounds.
pub fn encode_msr_power_limit(current_msr: u64, requested: PowerLimitConfig) -> u64 {
    let safe_pl1 = requested
        .pl1_watts
        .clamp(MIN_SAFE_PL1_WATTS, MAX_SAFE_PL1_WATTS);
    let safe_pl2 = requested
        .pl2_watts
        .clamp(safe_pl1.max(MIN_SAFE_PL2_WATTS), MAX_SAFE_PL2_WATTS);
    let tau_code = tau_seconds_to_msr_code(requested.tau_seconds);

    let cleared = current_msr & !(PL1_MASK | PL2_MASK | TAU_MASK);
    cleared | ((safe_pl1 as u64) << 3) | ((safe_pl2 as u64) << 35) | ((tau_code as u64) << 17)
}

/// Limits for the Phantom Canyon device, whose factory readback is 50 / 75 W.
pub const PHANTOM_MAX_PL1_WATTS: u8 = 50;
pub const PHANTOM_MAX_PL2_WATTS: u8 = 75;

pub fn validate_phantom_limits(limits: PowerLimitConfig) -> Result<(), &'static str> {
    if !(MIN_SAFE_PL1_WATTS..=PHANTOM_MAX_PL1_WATTS).contains(&limits.pl1_watts)
        || !(MIN_SAFE_PL2_WATTS..=PHANTOM_MAX_PL2_WATTS).contains(&limits.pl2_watts)
        || limits.pl2_watts < limits.pl1_watts
    {
        return Err("Use PL1 15–50 W and PL2 20–75 W, with PL2 at least PL1.");
    }
    if !TAU_LOOKUP_TABLE
        .iter()
        .any(|(s, _)| *s == limits.tau_seconds)
    {
        return Err("Select a supported turbo time window.");
    }
    Ok(())
}

pub fn encode_phantom_msr_power_limit(
    current: u64,
    limits: PowerLimitConfig,
) -> Result<u64, &'static str> {
    validate_phantom_limits(limits)?;
    Ok((current & !(PL1_MASK | PL2_MASK | TAU_MASK))
        | ((limits.pl1_watts as u64) << 3)
        | ((limits.pl2_watts as u64) << 35)
        | ((tau_seconds_to_msr_code(limits.tau_seconds) as u64) << 17))
}

/// Parses the 256-byte WMI buffer returned by `CISD_WMI::PlatformConfigurationProfileReport([1, 0, 0, 0])`.
pub fn parse_bios_turbo_profiles(wmi_bytes: &[u8]) -> Vec<BiosPowerProfile> {
    if wmi_bytes.len() < 4 || wmi_bytes[0] != 0 {
        return Vec::new();
    }
    let count = wmi_bytes[3] as usize;
    if count != 3 || 4 + count * 35 > wmi_bytes.len() {
        return Vec::new();
    }
    let mut profiles: Vec<BiosPowerProfile> = Vec::with_capacity(count);

    for idx in 0..count {
        let offset = 4 + idx * 35;
        if offset + 35 > wmi_bytes.len() {
            break;
        }
        let is_bios_default = wmi_bytes[offset] & 0x80 != 0;
        let raw_name = &wmi_bytes[offset..offset + 31];
        let cleaned_name: String = raw_name
            .iter()
            .copied()
            .filter(|b| *b >= 0x20 && *b <= 0x7E && *b != b'?')
            .map(|b| b as char)
            .collect::<String>()
            .replace("ENABLED", "")
            .trim()
            .to_string();

        if cleaned_name.is_empty() {
            continue;
        }

        let pl1_watts = wmi_bytes[offset + 32];
        let pl2_watts = wmi_bytes[offset + 33];
        let tau_seconds = wmi_bytes[offset + 34] as u16;
        let preset_kind = match cleaned_name.to_uppercase().as_str() {
            "MAX PERFORMANCE" => PowerPresetKind::MaxPerformance,
            "BALANCED" => PowerPresetKind::Balanced,
            "LOW POWER" => PowerPresetKind::LowPower,
            _ => PowerPresetKind::Custom,
        };

        if preset_kind == PowerPresetKind::Custom
            || profiles.iter().any(|p| p.preset_kind == preset_kind)
            || validate_phantom_limits(PowerLimitConfig {
                pl1_watts,
                pl2_watts,
                tau_seconds,
            })
            .is_err()
        {
            return Vec::new();
        }
        profiles.push(BiosPowerProfile {
            name: cleaned_name,
            preset_kind,
            pl1_watts,
            pl2_watts,
            tau_seconds,
            is_bios_default,
        });
    }
    if profiles.len() != count {
        return Vec::new();
    }
    profiles
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_msr_roundtrip_and_56s_tau_fix() {
        let baseline_msr: u64 = 0x0018_8000_0001_8000; // Enable/clamp bits set
        let req = PowerLimitConfig {
            pl1_watts: 45,
            pl2_watts: 64,
            tau_seconds: 56, // Was broken (58) in legacy NSS
        };
        let encoded = encode_msr_power_limit(baseline_msr, req);
        // Verify enable bits preserved
        assert_eq!(encoded & 0x0018_8000_0001_8000, 0x0018_8000_0001_8000);
        // Verify Tau code is 111
        assert_eq!(((encoded & TAU_MASK) >> 17) as u8, 111);

        let decoded = decode_msr_power_limit(encoded);
        assert_eq!(decoded, req);
    }
}
