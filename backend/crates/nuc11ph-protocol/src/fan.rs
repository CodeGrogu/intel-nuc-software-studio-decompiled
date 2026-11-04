//! `ECNUC22` Dual-Fan binary protocol, 94-byte EC profile reader, 90-byte WMI profile
//! builder, Zero-RPM (`FanOff`) bit manipulation, and manual duty cycle override
//! structures for Intel NUC11PHi7 (Phantom Canyon).

use serde::{Deserialize, Serialize};

/// NUC11PHi7 uses `NucEcSection::UseEcSection22` (2).
pub const ECNUC22_SECTION: u8 = 2;
/// NUC11PHi7 has 2 physical blower fans: Fan 0 (CPU i7-1165G7) and Fan 1 (RTX 2060 dGPU).
pub const PHANTOM_ACTIVE_FAN_COUNT: usize = 2;
/// Maximum fan slots in the firmware table (`FAN_MAX_NUMBER = 6`).
pub const FIRMWARE_MAX_FAN_SLOTS: usize = 6;
/// Driver ECNUC22 control numbers use Fan1..Fan6; telemetry slots are 0..5.
pub fn driver_fan_number(slot: u8) -> Result<u8, &'static str> {
    if slot < 6 {
        Ok(slot + 1)
    } else {
        Err("Invalid fan slot")
    }
}

/// KUI thermal presets, based on this Phantom Canyon's original EC curves.
/// These are local presets; the installed BIOS report omits the GPU curve.
pub fn phantom_cooling_preset(
    mode: NucFanMode,
) -> Result<(EcNuc22FanCurveDto, EcNuc22FanCurveDto), &'static str> {
    let duty = match mode {
        NucFanMode::Quiet => 25,
        NucFanMode::Balanced => 30,
        NucFanMode::Cool => 45,
        _ => return Err("Unsupported thermal preset"),
    };
    let cpu = EcNuc22FanCurveDto {
        primary_source: 1,
        primary_max_temp: 85,
        primary_min_temp: 40,
        primary_min_duty: duty,
        primary_duty_increment: 2,
        primary_fan_off_temp: 36,
        secondary_source: 6,
        secondary_max_temp: 85,
        secondary_min_temp: 45,
        secondary_min_duty: duty,
        secondary_duty_increment: 2,
        secondary_fan_off_temp: 40,
        config1: 0,
        config2: 100,
    };
    let gpu = EcNuc22FanCurveDto {
        primary_source: 6,
        primary_max_temp: 80,
        primary_min_temp: 45,
        primary_min_duty: duty,
        primary_duty_increment: 2,
        primary_fan_off_temp: 38,
        secondary_source: 1,
        secondary_max_temp: 85,
        secondary_min_temp: 45,
        secondary_min_duty: duty,
        secondary_duty_increment: 2,
        secondary_fan_off_temp: 40,
        config1: 0,
        config2: 100,
    };
    Ok((cpu, gpu))
}

/// Infer the active control mode from the EC curves that were actually read.
/// The BIOS profile label is unavailable on some firmware, so it must not gate
/// cooling when the EC provides a complete, valid profile.
pub fn infer_ec_fan_mode(curves: &[EcNuc22FanCurveDto; 2], _fan_off_enabled: bool) -> NucFanMode {
    if curves.iter().all(|curve| curve.config1 & 0x80 != 0) {
        return NucFanMode::Fixed;
    }
    if curves.iter().any(|curve| curve.config1 & 0x80 != 0) {
        return NucFanMode::Custom;
    }
    for mode in [NucFanMode::Quiet, NucFanMode::Balanced, NucFanMode::Cool] {
        if phantom_cooling_preset(mode).is_ok_and(|(cpu, gpu)| {
            curves.iter().zip([cpu, gpu]).all(|(actual, expected)| {
                let mut actual = *actual;
                let mut expected = expected;
                actual.config2 &= 0x7f;
                expected.config2 &= 0x7f;
                actual == expected
            })
        }) {
            return mode;
        }
    }
    NucFanMode::Custom
}
/// Byte offset where the 6 x 14-byte fan blocks begin in the 90-byte WMI packet.
pub const WMI_FAN_TABLE_START_BYTE: usize = 3;
/// Byte index in the 90-byte WMI packet whose Bit 7 (`0x80`) controls Zero-RPM `FanOff` on ECNUC22.
pub const ECNUC22_FAN_OFF_FLAG_BYTE_INDEX: usize = 16;

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum NucFanMode {
    Quiet = 0,
    Balanced = 1,
    Cool = 2,
    Custom = 3,
    Fixed = 4,
    Fanless = 5,
    Reserve = 7,
}

impl NucFanMode {
    pub fn from_u8(val: u8) -> Self {
        match val {
            0 => Self::Quiet,
            1 => Self::Balanced,
            2 => Self::Cool,
            3 => Self::Custom,
            4 => Self::Fixed,
            5 => Self::Fanless,
            _ => Self::Reserve,
        }
    }
}

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum PrimaryTempSource {
    Default = 0,
    CpuTemperature = 1,
    PchTemperature = 2,
    Sensor1Temperature = 3,
    Sensor2Temperature = 4,
    Sensor3Temperature = 5,
    DgpuTemperature = 6,
}

impl PrimaryTempSource {
    pub fn from_u8(val: u8) -> Self {
        match val {
            1 => Self::CpuTemperature,
            2 => Self::PchTemperature,
            3 => Self::Sensor1Temperature,
            4 => Self::Sensor2Temperature,
            5 => Self::Sensor3Temperature,
            6 => Self::DgpuTemperature,
            _ => Self::Default,
        }
    }
}

#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum SecondaryTempSource {
    NotControlled = 0,
    CpuTemperature = 1,
    PchTemperature = 2,
    Sensor1Temperature = 3,
    Sensor2Temperature = 4,
    Sensor3Temperature = 5,
    DgpuTemperature = 6,
}

impl SecondaryTempSource {
    pub fn from_u8(val: u8) -> Self {
        match val {
            1 => Self::CpuTemperature,
            2 => Self::PchTemperature,
            3 => Self::Sensor1Temperature,
            4 => Self::Sensor2Temperature,
            5 => Self::Sensor3Temperature,
            6 => Self::DgpuTemperature,
            _ => Self::NotControlled,
        }
    }
}

/// High-level DTO for a single fan's 14-byte thermal control block on `ECNUC22`.
#[repr(C)]
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub struct EcNuc22FanCurveDto {
    pub primary_source: u8,           // PrimaryTempSource
    pub primary_max_temp: u8,         // °C (e.g. 85)
    pub primary_min_temp: u8,         // °C (e.g. 45)
    pub primary_min_duty: u8,         // %  (e.g. 25)
    pub primary_duty_increment: u8,   // % per °C (e.g. 2)
    pub primary_fan_off_temp: u8,     // °C below which fan stops when FanOff is enabled (e.g. 36)
    pub secondary_source: u8,         // SecondaryTempSource
    pub secondary_max_temp: u8,       // °C
    pub secondary_min_temp: u8,       // °C
    pub secondary_min_duty: u8,       // %
    pub secondary_duty_increment: u8, // % per °C
    pub secondary_fan_off_temp: u8,   // °C
    pub config1: u8,                  // Byte 12 (Bit 7 = Fixed mode, Bits 0..6 = Fixed duty %)
    pub config2: u8,                  // Byte 13 (Bit 7 of Fan 0 = FanOff Zero-RPM flag)
}

impl EcNuc22FanCurveDto {
    pub fn default_cpu_fan_curve() -> Self {
        Self {
            primary_source: PrimaryTempSource::CpuTemperature as u8,
            primary_max_temp: 85,
            primary_min_temp: 45,
            primary_min_duty: 28,
            primary_duty_increment: 2,
            primary_fan_off_temp: 36,
            secondary_source: SecondaryTempSource::DgpuTemperature as u8,
            secondary_max_temp: 85,
            secondary_min_temp: 55,
            secondary_min_duty: 32,
            secondary_duty_increment: 2,
            secondary_fan_off_temp: 40,
            config1: 0,
            config2: 0x80,
        }
    }

    pub fn default_gpu_fan_curve() -> Self {
        Self {
            primary_source: PrimaryTempSource::DgpuTemperature as u8,
            primary_max_temp: 82,
            primary_min_temp: 45,
            primary_min_duty: 28,
            primary_duty_increment: 2,
            primary_fan_off_temp: 38,
            secondary_source: SecondaryTempSource::CpuTemperature as u8,
            secondary_max_temp: 90,
            secondary_min_temp: 65,
            secondary_min_duty: 30,
            secondary_duty_increment: 2,
            secondary_fan_off_temp: 45,
            config1: 0,
            config2: 0,
        }
    }

    /// Evaluates the target fan duty percentage (0..=100) for a given primary & secondary temperature.
    pub fn evaluate_duty(
        &self,
        primary_temp_c: u8,
        secondary_temp_c: u8,
        fan_off_enabled: bool,
    ) -> u8 {
        if (self.config1 & 0x80) != 0 {
            return (self.config1 & 0x7F).clamp(0, 100);
        }
        let calc_branch = |temp: u8, min_t: u8, max_t: u8, min_d: u8, inc: u8, off_t: u8| -> u8 {
            if fan_off_enabled && temp <= off_t {
                return 0;
            }
            if temp <= min_t {
                return min_d;
            }
            if temp >= max_t {
                return 100;
            }
            let delta = (temp - min_t) as u16;
            let duty = (min_d as u16) + delta * (inc.max(1) as u16);
            duty.min(100) as u8
        };

        let p_duty = calc_branch(
            primary_temp_c,
            self.primary_min_temp,
            self.primary_max_temp,
            self.primary_min_duty,
            self.primary_duty_increment,
            self.primary_fan_off_temp,
        );

        let s_duty = if self.secondary_source != SecondaryTempSource::NotControlled as u8 {
            calc_branch(
                secondary_temp_c,
                self.secondary_min_temp,
                self.secondary_max_temp,
                self.secondary_min_duty,
                self.secondary_duty_increment,
                self.secondary_fan_off_temp,
            )
        } else {
            0
        };

        p_duty.max(s_duty)
    }
}

/// IOCTL `0x9C406020` (`ReadFanProfileCode`) 94-byte output buffer from EC.
#[repr(C, packed)]
#[derive(Clone, Copy)]
pub struct FanProfileEcRaw {
    pub data: [u8; 94],
}

impl Default for FanProfileEcRaw {
    fn default() -> Self {
        Self { data: [0u8; 94] }
    }
}

/// IOCTL `0x9C40A01C` (`WriteFanProfileNuc22Code`) 90-byte input buffer.
#[repr(C, packed)]
#[derive(Clone, Copy)]
pub struct FanProfileWmiRaw {
    pub data: [u8; 90],
}

impl Default for FanProfileWmiRaw {
    fn default() -> Self {
        Self { data: [0u8; 90] }
    }
}

/// IOCTL `0x9C406024` (`ReadFanSpeedCode`) 18-byte output buffer (6 fans * 3 bytes).
#[repr(C, packed)]
#[derive(Clone, Copy, Default)]
pub struct FanSpeedStructRaw {
    pub data: [u8; 18],
}

/// IOCTL `0x9C40A040` (`SetFanSpeedCode`) 3-byte input buffer for manual duty override.
#[repr(C, packed)]
#[derive(Debug, Clone, Copy)]
pub struct FanDutyPacket {
    pub nuc_ec_section: u8,
    pub fan_number: u8,
    pub fan_duty: u8,
}

/// IOCTL `0x9C406048` (`ReadTempratureProfileCode`) 15-byte output buffer from EC.
#[repr(C, packed)]
#[derive(Clone, Copy, Default)]
pub struct TemperatureProfileEcRaw {
    pub data: [u8; 15],
}

/// Extracts Fan 0 (CPU) and Fan 1 (RTX 2060) curves from the 94-byte EC read buffer.
pub fn ec_fan_record_stride(ec_raw: &FanProfileEcRaw) -> usize {
    // The installed driver's ECNUC22 writer advances two reserved bytes after
    // each 14-byte record; its fixed-duty path uses slot * 16 + 12.
    // Keep support for the packed records supplied by older protocol callers.
    if ec_raw.data[14] == 0 && ec_raw.data[15] == 0 && (1..=6).contains(&ec_raw.data[16]) {
        16
    } else {
        14
    }
}

pub fn parse_ec_fan_profile(ec_raw: &FanProfileEcRaw) -> ([EcNuc22FanCurveDto; 2], bool) {
    let parse_one = |slot: usize| -> EcNuc22FanCurveDto {
        let base = slot * ec_fan_record_stride(ec_raw);
        EcNuc22FanCurveDto {
            primary_source: ec_raw.data[base],
            primary_max_temp: ec_raw.data[base + 1],
            primary_min_temp: ec_raw.data[base + 2],
            primary_min_duty: ec_raw.data[base + 3],
            primary_duty_increment: ec_raw.data[base + 4],
            primary_fan_off_temp: ec_raw.data[base + 5],
            secondary_source: ec_raw.data[base + 6],
            secondary_max_temp: ec_raw.data[base + 7],
            secondary_min_temp: ec_raw.data[base + 8],
            secondary_min_duty: ec_raw.data[base + 9],
            secondary_duty_increment: ec_raw.data[base + 10],
            secondary_fan_off_temp: ec_raw.data[base + 11],
            config1: ec_raw.data[base + 12],
            config2: ec_raw.data[base + 13],
        }
    };
    let cpu = parse_one(0);
    let gpu = parse_one(1);
    let fan_off_enabled = (cpu.config2 & 0x80) != 0;
    ([cpu, gpu], fan_off_enabled)
}

/// Validate an explicitly editable curve. Zero firmware maxima/default/absent
/// records are unknown, not temperatures to replace with invented limits.
pub fn validate_custom_fan_curve(curve: &EcNuc22FanCurveDto) -> Result<(), &'static str> {
    fn channel(
        source: u8,
        max: u8,
        min: u8,
        duty: u8,
        step: u8,
        off: u8,
        secondary: bool,
    ) -> Result<(), &'static str> {
        if secondary && source == 0 {
            return if [max, min, duty, step, off].iter().all(|v| *v == 0) {
                Ok(())
            } else {
                Err("Disabled secondary channel contains unverified control values")
            };
        }
        if !(1..=6).contains(&source) {
            return Err(
                "Fan temperature source is absent/default or unsupported; no editable curve",
            );
        }
        if !(40..=100).contains(&max) {
            return Err(
                "Fan maximum temperature is unknown or outside editable bounds; no value inferred",
            );
        }
        if min < 20 || min > max.saturating_sub(5) {
            return Err(
                "Fan minimum temperature is outside the representable maximum-minus-five window",
            );
        }
        if !(10..=100).contains(&duty) || !(1..=20).contains(&step) || off > min.saturating_sub(1) {
            return Err("Fan duty, increment or fan-off temperature is outside editable bounds");
        }
        Ok(())
    }
    channel(
        curve.primary_source,
        curve.primary_max_temp,
        curve.primary_min_temp,
        curve.primary_min_duty,
        curve.primary_duty_increment,
        curve.primary_fan_off_temp,
        false,
    )?;
    channel(
        curve.secondary_source,
        curve.secondary_max_temp,
        curve.secondary_min_temp,
        curve.secondary_min_duty,
        curve.secondary_duty_increment,
        curve.secondary_fan_off_temp,
        true,
    )
}

/// Builds a 90-byte `FanProfileWmiRaw` packet for `WriteFanProfileNuc22Code` (`0x9C40A01C`),
/// exactly matching the EC-to-WMI stride alignment in `FanControl.cs:546–588` while unlocking
/// custom fan curves on the NUC11PHi7.
pub fn build_custom_wmi_fan_profile(
    ec_baseline: &FanProfileEcRaw,
    cpu_fan: &EcNuc22FanCurveDto,
    gpu_fan: &EcNuc22FanCurveDto,
    fan_off_enabled: bool,
) -> Result<FanProfileWmiRaw, &'static str> {
    validate_custom_fan_curve(cpu_fan)?;
    validate_custom_fan_curve(gpu_fan)?;
    let mut out = [0u8; 90];
    // Header bytes required by PerformanceDriver.sys WriteFanProfileNUC22
    out[0] = 0x00;
    out[1] = 0x01;
    out[2] = 0x1B; // 27

    // Translate 94-byte EC profile into 90-byte WMI layout (skipping 2 bytes every 14-byte block)
    let mut wmi_idx = WMI_FAN_TABLE_START_BYTE;
    let mut ec_idx = 0usize;
    while wmi_idx < 90 && ec_idx < ec_baseline.data.len() {
        out[wmi_idx] = ec_baseline.data[ec_idx];
        if wmi_idx % 14 == 2 {
            ec_idx += 2;
        }
        wmi_idx += 1;
        ec_idx += 1;
    }

    let write_block = |buf: &mut [u8; 90], slot: usize, cfg: &EcNuc22FanCurveDto| {
        let base = WMI_FAN_TABLE_START_BYTE + slot * 14;
        buf[base] = cfg.primary_source;
        buf[base + 1] = cfg.primary_max_temp;
        buf[base + 2] = cfg.primary_min_temp;
        buf[base + 3] = cfg.primary_min_duty;
        buf[base + 4] = cfg.primary_duty_increment;
        buf[base + 5] = cfg.primary_fan_off_temp;
        buf[base + 6] = cfg.secondary_source;
        buf[base + 7] = cfg.secondary_max_temp;
        buf[base + 8] = cfg.secondary_min_temp;
        buf[base + 9] = cfg.secondary_min_duty;
        buf[base + 10] = cfg.secondary_duty_increment;
        buf[base + 11] = cfg.secondary_fan_off_temp;
        buf[base + 12] = cfg.config1;
        buf[base + 13] = cfg.config2;
    };

    write_block(&mut out, 0, cpu_fan);
    write_block(&mut out, 1, gpu_fan);

    // Bit 7 of byte[16] (Fan 0 byte 13) controls Zero-RPM FanOff mode on ECNUC22
    if fan_off_enabled {
        out[ECNUC22_FAN_OFF_FLAG_BYTE_INDEX] |= 0x80;
    } else {
        out[ECNUC22_FAN_OFF_FLAG_BYTE_INDEX] &= !0x80;
    }

    Ok(FanProfileWmiRaw { data: out })
}

/// Applies the `FanOff` Zero-RPM bit (`byte[16] bit 7`) to a 90-byte BIOS preset profile
/// (`Quiet`, `Balanced`, or `Cool`) retrieved from `PlatformConfigurationAccessV1`.
pub fn apply_fan_off_bit_to_preset(
    mut preset_90_bytes: [u8; 90],
    fan_off_enabled: bool,
) -> FanProfileWmiRaw {
    if fan_off_enabled {
        preset_90_bytes[ECNUC22_FAN_OFF_FLAG_BYTE_INDEX] |= 0x80;
    } else {
        preset_90_bytes[ECNUC22_FAN_OFF_FLAG_BYTE_INDEX] &= !0x80;
    }
    FanProfileWmiRaw {
        data: preset_90_bytes,
    }
}

/// Decodes Fan 0 (CPU) and Fan 1 (RTX 2060) RPMs from the 18-byte `FanSpeedStructRaw`.
pub fn decode_dual_fan_rpms(raw: &FanSpeedStructRaw) -> (u16, u16) {
    // ReadWriteControl.cs identifies each record by FanType, not by its slot.
    let mut cpu = None;
    let mut gpu = None;
    for record in raw.data.chunks_exact(3) {
        let rpm = u16::from_be_bytes([record[1], record[2]]);
        match record[0] {
            1 => cpu = Some(rpm),
            2 => gpu = Some(rpm),
            7 if cpu.is_none() => cpu = Some(rpm),
            _ => {}
        }
    }
    (cpu.unwrap_or(0), gpu.unwrap_or(0))
}

/// Bit 0 denotes a discovered CPU fan record (type 1 or alternate type 7);
/// bit 1 denotes a discovered GPU fan record (type 2).
pub fn fan_slot_mask(raw: &FanSpeedStructRaw) -> u32 {
    let mut mask = 0;
    for record in raw.data.chunks_exact(3) {
        match record[0] {
            1 | 7 => mask |= 1,
            2 => mask |= 2,
            _ => {}
        }
    }
    mask
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_custom_wmi_profile_builder_and_fan_off_bit() {
        let ec = FanProfileEcRaw::default();
        let cpu = EcNuc22FanCurveDto::default_cpu_fan_curve();
        let gpu = EcNuc22FanCurveDto::default_gpu_fan_curve();

        let wmi_on = build_custom_wmi_fan_profile(&ec, &cpu, &gpu, true).unwrap();
        assert_eq!(&wmi_on.data[0..3], &[0x00, 0x01, 0x1B]);
        assert_eq!(wmi_on.data[3], PrimaryTempSource::CpuTemperature as u8);
        assert_eq!(wmi_on.data[17], PrimaryTempSource::DgpuTemperature as u8);
        assert_eq!(wmi_on.data[ECNUC22_FAN_OFF_FLAG_BYTE_INDEX] & 0x80, 0x80);

        let wmi_off = build_custom_wmi_fan_profile(&ec, &cpu, &gpu, false).unwrap();
        assert_eq!(wmi_off.data[ECNUC22_FAN_OFF_FLAG_BYTE_INDEX] & 0x80, 0x00);
    }
}
