//! Byte contracts grounded in the original decompiled Intel implementation.
use crate::WmiError;
use nuc11ph_protocol::{
    CisdWmiStatus, FanDutyPacket, FanSpeedStructRaw, LedZoneStateDto, ECNUC22_SECTION,
};

pub fn checked_reply(bytes: &[u8], minimum: usize) -> Result<&[u8], WmiError> {
    if bytes.len() < minimum.max(1) {
        return Err(WmiError::Com(format!(
            "Firmware response is truncated: {} bytes, expected at least {}",
            bytes.len(),
            minimum.max(1)
        )));
    }
    CisdWmiStatus::from_byte(bytes[0]).map_err(WmiError::FirmwareStatus)?;
    Ok(bytes)
}
pub struct LightingPlan {
    pub mode: u8,
    pub commands: Vec<(&'static str, u32)>,
}

/// Build writes for restoring the saved indicator mode exactly, rather than mapping it
/// through the normal "best editable mode" path used by interactive RGB controls.
pub fn plan_exact_led_restore(
    zone: &LedZoneStateDto,
    items: u32,
    current_mode: u8,
) -> Result<Vec<(&'static str, u32)>, WmiError> {
    if zone.zone_id > 6 || zone.indicator_mode > 6 {
        return Err(WmiError::Com("Invalid LED snapshot".into()));
    }
    if zone.indicator_mode == 6 {
        return Ok(if current_mode == 6 {
            Vec::new()
        } else {
            vec![(
                "SetIndicatorOptionLedType",
                nuc11ph_protocol::pack_wmi_cmd(zone.zone_id, 6, 0, 0),
            )]
        });
    }
    let required = match zone.indicator_mode {
        0 => 0x00ff_ffff,
        1 | 2 | 5 => 0x1f,
        3 => 0x0f,
        4 => 0x3f,
        _ => unreachable!(),
    };
    if items & required != required {
        return Err(WmiError::Com(format!(
            "LED zone {} cannot restore all items for mode {}",
            zone.zone_id, zone.indicator_mode
        )));
    }
    let mut writes = Vec::new();
    if current_mode != zone.indicator_mode {
        writes.push((
            "SetIndicatorOptionLedType",
            nuc11ph_protocol::pack_wmi_cmd(zone.zone_id, zone.indicator_mode, 0, 0),
        ));
    }
    for (index, value) in zone.diff_control_items(None) {
        if required & (1u32 << index) != 0 {
            writes.push((
                "SetValueIndicatorOptionLedType",
                nuc11ph_protocol::pack_wmi_cmd(zone.zone_id, zone.indicator_mode, index, value),
            ));
        }
    }
    Ok(writes)
}

pub fn led_snapshot_matches(expected: &LedZoneStateDto, actual: &LedZoneStateDto) -> bool {
    expected == actual
}
pub fn plan_awake_lighting(
    zone: &LedZoneStateDto,
    indicators: u32,
    items: u32,
    current_mode: u8,
) -> Result<LightingPlan, WmiError> {
    if zone.zone_id > 6 {
        return Err(WmiError::Com("Unknown lighting zone".into()));
    }
    let supported = |mode: u8| indicators & (1u32 << mode) != 0;
    // Software brightness is advertised but rejects item 0 on the live firmware.
    // PowerState provides editable awake brightness without changing sleep slots.
    let mode = if supported(0) {
        0
    } else if supported(4) {
        4
    } else {
        return Err(WmiError::Com(
            "This zone has no manual RGB indicator mode".into(),
        ));
    };
    if items & 0b111000 != 0b111000 {
        return Err(WmiError::Com(
            "This zone does not expose all three RGB channels".into(),
        ));
    }
    let mut commands = Vec::new();
    if current_mode != mode {
        commands.push((
            "SetIndicatorOptionLedType",
            nuc11ph_protocol::pack_wmi_cmd(zone.zone_id, mode, 0, 0),
        ));
    }
    let values = [
        zone.s0.brightness.min(100),
        zone.s0.behavior.min(3),
        zone.s0.frequency.min(10),
        zone.s0.r,
        zone.s0.g,
        zone.s0.b,
    ];
    for (index, value) in values.into_iter().enumerate() {
        // Software item 0 is advertised but its getter returns 228, so no
        // original brightness is available to preserve or restore. RGB edits
        // must leave that unknown field untouched, even when its mask bit is set.
        if mode == 4 && index == 0 {
            continue;
        }
        if items & (1 << index) != 0 {
            commands.push((
                "SetValueIndicatorOptionLedType",
                nuc11ph_protocol::pack_wmi_cmd(zone.zone_id, mode, index as u8, value),
            ));
        }
    }
    Ok(LightingPlan { mode, commands })
}

#[cfg(test)]
mod lighting_tests {
    use super::plan_awake_lighting;
    use nuc11ph_protocol::{LedZoneStateDto, PhantomLedZone};

    #[test]
    fn software_rgb_fallback_preserves_unreadable_brightness_despite_advertisement() {
        // The captured Software capability mask is 0x3f, but item 0 returns
        // firmware status 228 on every present zone. Its DTO value must never
        // become a write, whether the mode is already active or newly selected.
        for zone in [
            PhantomLedZone::PowerButton,
            PhantomLedZone::Skull,
            PhantomLedZone::Front1,
            PhantomLedZone::Front2,
            PhantomLedZone::Front3,
        ] {
            for current_mode in [0, 4] {
                for brightness in [0, 50, 100, 255] {
                    let mut desired = LedZoneStateDto::default_for_zone(zone);
                    desired.s0.brightness = brightness;
                    desired.s0.r = 255;
                    desired.s0.g = 0;
                    desired.s0.b = 255;
                    let plan = plan_awake_lighting(&desired, 1 << 4, 0x3f, current_mode)
                        .expect("RGB-only Software fallback must remain usable");
                    assert_eq!(plan.mode, 4);
                    let item_commands: Vec<_> = plan
                        .commands
                        .iter()
                        .filter(|(method, _)| *method == "SetValueIndicatorOptionLedType")
                        .map(|(_, command)| command.to_le_bytes())
                        .collect();
                    assert!(item_commands.iter().all(|bytes| bytes[2] != 0));
                    for (index, value) in [(3, 255), (4, 0), (5, 255)] {
                        assert!(item_commands.contains(&[zone as u8, 4, index, value]));
                    }
                }
            }
        }
    }

    #[test]
    fn power_state_remains_preferred_and_keeps_readable_awake_brightness() {
        let mut desired = LedZoneStateDto::default_for_zone(PhantomLedZone::Skull);
        desired.s0.brightness = 10;
        let plan = plan_awake_lighting(&desired, (1 << 0) | (1 << 4), 0x00ff_ffff, 4)
            .expect("PowerState brightness was physically verified");
        assert_eq!(plan.mode, 0);
        let item_commands: Vec<_> = plan
            .commands
            .iter()
            .filter(|(method, _)| *method == "SetValueIndicatorOptionLedType")
            .map(|(_, command)| command.to_le_bytes())
            .collect();
        assert!(item_commands.contains(&[2, 0, 0, 10]));
        assert!(item_commands.iter().all(|bytes| bytes[2] < 6));
    }
}

pub fn fan_duty_packets(
    raw: &FanSpeedStructRaw,
    cpu: u8,
    gpu: u8,
) -> Result<[FanDutyPacket; 2], String> {
    if !(25..=99).contains(&cpu) || !(25..=99).contains(&gpu) {
        return Err("Fixed fan duties must be 25–99%".into());
    }
    let slot = |kind: u8| raw.data.chunks_exact(3).position(|r| r[0] == kind);
    let cpu_slot = slot(1)
        .or_else(|| slot(7))
        .ok_or("CPU fan record not found")?;
    let gpu_slot = slot(2).ok_or("GPU fan record not found")?;
    Ok([
        FanDutyPacket {
            nuc_ec_section: ECNUC22_SECTION,
            fan_number: cpu_slot as u8,
            fan_duty: cpu,
        },
        FanDutyPacket {
            nuc_ec_section: ECNUC22_SECTION,
            fan_number: gpu_slot as u8,
            fan_duty: gpu,
        },
    ])
}

pub fn verify_fan_profile(
    expected: &nuc11ph_protocol::FanProfileWmiRaw,
    actual: &nuc11ph_protocol::FanProfileEcRaw,
) -> Result<(), crate::DriverError> {
    for fan in 0..2 {
        let source = 3 + fan * 14;
        let target = fan * nuc11ph_protocol::ec_fan_record_stride(actual);
        if expected.data[source..source + 14] != actual.data[target..target + 14] {
            return Err(crate::DriverError::IoctlFailed {
                ioctl: crate::driver::NucIoctl::WriteFanProfileNuc22 as u32,
                reason: format!(
                    "Fan {fan} profile readback does not match: expected {:?}, actual {:?}",
                    &expected.data[source..source + 14],
                    &actual.data[target..target + 14]
                ),
            });
        }
    }
    Ok(())
}
pub fn verify_fixed_duties(
    packets: &[FanDutyPacket; 2],
    actual: &nuc11ph_protocol::FanProfileEcRaw,
) -> Result<(), crate::DriverError> {
    for packet in packets {
        let index =
            packet.fan_number as usize * nuc11ph_protocol::ec_fan_record_stride(actual) + 12;
        if index >= actual.data.len() || actual.data[index] != (0x80 | packet.fan_duty) {
            return Err(crate::DriverError::IoctlFailed {
                ioctl: crate::driver::NucIoctl::SetFanSpeed as u32,
                reason: format!(
                    "Fan {} duty readback does not match (index {index}, wanted {}, raw {:?})",
                    packet.fan_number,
                    0x80 | packet.fan_duty,
                    &actual.data[..40]
                ),
            });
        }
    }
    Ok(())
}
