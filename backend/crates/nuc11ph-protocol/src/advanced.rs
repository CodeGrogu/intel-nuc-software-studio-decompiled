//! Reviewed advanced lighting fields for NUC11PH BIOS 0080. No generic multi-model defaults.
use crate::LedFieldState;
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, PartialEq, Serialize, Deserialize)]
pub struct LightingModeState {
    pub zone_id: u8,
    pub indicator_mode: u8,
    pub active_role: u8,
    pub indicator_mask: u32,
    pub fields: Option<LedFieldState>,
    pub unavailable: Option<String>,
}
#[derive(Debug, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub struct LightingFieldWrite {
    pub index: u8,
    pub value: u8,
}
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct AdvancedLightingPatch {
    pub expected: LightingModeState,
    pub writes: Vec<LightingFieldWrite>,
    pub activate: bool,
}

pub fn reviewed_lighting_mask(mode: u8) -> Option<u32> {
    match mode {
        0 => Some(0x03f03f),
        1 | 5 => Some(0x1f),
        4 => Some(0x3f),
        6 => Some(0),
        _ => None,
    }
}
/// Range and known-defect checks are independent of firmware advertisements.
pub fn advanced_lighting_field_allowed(zone: u8, mode: u8, index: u8, value: u8) -> bool {
    if ![0, 2, 4, 5, 6].contains(&zone) {
        return false;
    }
    match mode {
        0 if index < 6 || (12..18).contains(&index) => match index % 6 {
            0 => value <= 100,
            1 => value <= 3,
            2 => value <= 10,
            _ => true,
        },
        1 => match index {
            0 => value <= 100,
            1..=3 => true,
            4 => value <= 1,
            _ => false,
        },
        4 => match index {
            1 => value <= 3,
            2 => value <= 10,
            3 | 4 => true,
            5 => zone != 6,
            _ => false,
        },
        5 => match index {
            0 => value <= 1,
            1 => value <= 100,
            2..=4 => true,
            _ => false,
        },
        _ => false,
    }
}
pub fn readable_lighting_item(fields: &LedFieldState, index: u8) -> Option<u8> {
    if index >= 24
        || fields.advertised_items_mask & (1 << index) == 0
        || fields.successfully_read_items_mask & (1 << index) == 0
    {
        return None;
    }
    fields
        .items
        .iter()
        .find(|i| i.index == index && i.firmware_status == Some(0) && i.error.is_none())
        .and_then(|i| i.value)
}
pub fn plan_advanced_lighting_patch(
    before: &LightingModeState,
    patch: &AdvancedLightingPatch,
) -> Result<(LightingModeState, Vec<(u8, u8)>), &'static str> {
    if before != &patch.expected {
        return Err("Lighting changed; refresh before applying.");
    }
    if before.unavailable.is_some() || ![0, 2, 4, 5, 6].contains(&before.zone_id) {
        return Err("Lighting configuration unavailable.");
    }
    let mask =
        reviewed_lighting_mask(before.indicator_mode).ok_or("Role is unsupported or deferred.")?;
    if before.indicator_mask != 0x77 || before.indicator_mask & (1 << before.indicator_mode) == 0 {
        return Err("Unreviewed indicator capabilities.");
    }
    if mask == 0 {
        if before.fields.is_some() || !patch.writes.is_empty() {
            return Err("Disabled role has no editable fields.");
        }
    } else {
        let fields = before.fields.as_ref().ok_or("Missing field readback.")?;
        if fields.zone_id != before.zone_id
            || fields.indicator_mode != before.indicator_mode
            || fields.advertised_items_mask != mask
        {
            return Err("Unreviewed field capabilities.");
        }
    }
    if patch.writes.len() > 24 {
        return Err("Too many fields.");
    }
    let mut seen = 0u32;
    let mut expected = before.clone();
    let mut commands = Vec::new();
    for write in &patch.writes {
        if write.index >= 24 || seen & (1 << write.index) != 0 {
            return Err("Duplicate or invalid field.");
        }
        seen |= 1 << write.index;
        if !advanced_lighting_field_allowed(
            before.zone_id,
            before.indicator_mode,
            write.index,
            write.value,
        ) {
            return Err("Field is unsupported, unsafe, or outside the reviewed range.");
        }
        let fields = expected.fields.as_mut().ok_or("Missing field readback.")?;
        let current = readable_lighting_item(fields, write.index)
            .ok_or("Cannot edit an unreadable field.")?;
        if current != write.value {
            commands.push((write.index, write.value));
            fields
                .items
                .iter_mut()
                .find(|i| i.index == write.index)
                .unwrap()
                .value = Some(write.value);
        }
    }
    if patch.activate {
        expected.active_role = before.indicator_mode;
    }
    Ok((expected, commands))
}

/// One role per physical zone prevents cross-role RGB alias conflicts in a batch.
pub fn plan_lighting_preset(
    before: &[LightingModeState],
    patches: &[AdvancedLightingPatch],
) -> Result<(), &'static str> {
    if patches.is_empty() || patches.len() > 5 || before.len() != patches.len() {
        return Err("A preset must contain one to five unique zones.");
    }
    let mut seen = 0u32;
    for (state, patch) in before.iter().zip(patches) {
        let zone = patch.expected.zone_id;
        if zone > 6 || seen & (1 << zone) != 0 {
            return Err("Duplicate or invalid preset zone.");
        }
        seen |= 1 << zone;
        plan_advanced_lighting_patch(state, patch)?;
    }
    Ok(())
}

/// Shared executor seam: all preconditions first, each target again immediately before
/// writes, then all postconditions. The caller tracks attempted writes across failures.
pub fn execute_lighting_preset<E>(
    patches: &[AdvancedLightingPatch],
    mut read: impl FnMut(u8, u8) -> Result<LightingModeState, E>,
    mut apply: impl FnMut(&LightingModeState, &[(u8, u8)]) -> Result<(), E>,
    invalid: impl Fn(&str) -> E,
) -> Result<(), E> {
    if patches.is_empty() || patches.len() > 5 {
        return Err(invalid("Invalid lighting preset size"));
    }
    let before = patches
        .iter()
        .map(|p| read(p.expected.zone_id, p.expected.indicator_mode))
        .collect::<Result<Vec<_>, _>>()?;
    plan_lighting_preset(&before, patches).map_err(&invalid)?;
    let mut final_states = Vec::new();
    for patch in patches {
        let current = read(patch.expected.zone_id, patch.expected.indicator_mode)?;
        let (expected, commands) =
            plan_advanced_lighting_patch(&current, patch).map_err(&invalid)?;
        apply(&expected, &commands)?;
        if read(expected.zone_id, expected.indicator_mode)? != expected {
            return Err(invalid(
                "Lighting postcondition mismatch; refresh actual state",
            ));
        }
        final_states.push(expected);
    }
    for expected in final_states {
        if read(expected.zone_id, expected.indicator_mode)? != expected {
            return Err(invalid(
                "Lighting changed during preset application; refresh actual state",
            ));
        }
    }
    Ok(())
}

#[cfg(test)]
mod tests {
    use super::*;
    use crate::LedItemReadback;
    fn state(zone: u8, mode: u8) -> LightingModeState {
        let mask = reviewed_lighting_mask(mode).unwrap();
        LightingModeState {
            zone_id: zone,
            indicator_mode: mode,
            active_role: 0,
            indicator_mask: 0x77,
            unavailable: None,
            fields: (mode != 6).then(|| LedFieldState {
                zone_id: zone,
                indicator_mode: mode,
                advertised_items_mask: mask,
                successfully_read_items_mask: mask,
                complete: true,
                items: (0..24)
                    .filter(|i| mask & (1 << i) != 0)
                    .map(|index| LedItemReadback {
                        index,
                        firmware_status: Some(0),
                        value: Some(0),
                        error: None,
                    })
                    .collect(),
            }),
        }
    }
    #[test]
    fn batch_preflight_and_late_conflicts_do_not_drop_or_rollback_writes() {
        use std::cell::{Cell, RefCell};
        let original = vec![state(0, 0), state(2, 0)];
        let patches: Vec<_> = original
            .iter()
            .map(|s| AdvancedLightingPatch {
                expected: s.clone(),
                writes: vec![LightingFieldWrite {
                    index: 0,
                    value: 50,
                }],
                activate: true,
            })
            .collect();
        let writes = Cell::new(0);
        let actual = RefCell::new(original.clone());
        actual.borrow_mut()[1].active_role = 1;
        assert!(execute_lighting_preset(
            &patches,
            |z, _| Ok(actual
                .borrow()
                .iter()
                .find(|s| s.zone_id == z)
                .unwrap()
                .clone()),
            |_, _| {
                writes.set(writes.get() + 1);
                Ok(())
            },
            str::to_string
        )
        .is_err());
        assert_eq!(writes.get(), 0); // stale second zone rejected before first write
        *actual.borrow_mut() = original.clone();
        let result = execute_lighting_preset(
            &patches,
            |z, _| {
                Ok(actual
                    .borrow()
                    .iter()
                    .find(|s| s.zone_id == z)
                    .unwrap()
                    .clone())
            },
            |expected, _| {
                writes.set(writes.get() + 1);
                let mut states = actual.borrow_mut();
                *states
                    .iter_mut()
                    .find(|s| s.zone_id == expected.zone_id)
                    .unwrap() = expected.clone();
                if expected.zone_id == 0 {
                    states[1].active_role = 1;
                }
                Ok(())
            },
            str::to_string,
        );
        assert!(result.is_err());
        assert_eq!(writes.get(), 1);
        assert_eq!(
            readable_lighting_item(actual.borrow()[0].fields.as_ref().unwrap(), 0),
            Some(50)
        );
        *actual.borrow_mut() = original;
        writes.set(0);
        let result = execute_lighting_preset(
            &patches,
            |z, _| {
                Ok(actual
                    .borrow()
                    .iter()
                    .find(|s| s.zone_id == z)
                    .unwrap()
                    .clone())
            },
            |expected, _| {
                writes.set(writes.get() + 1);
                let mut states = actual.borrow_mut();
                *states
                    .iter_mut()
                    .find(|s| s.zone_id == expected.zone_id)
                    .unwrap() = expected.clone();
                if expected.zone_id == 2 {
                    states[0].active_role = 1;
                }
                Ok(())
            },
            str::to_string,
        );
        assert!(result.is_err());
        assert_eq!(writes.get(), 2); // final pass catches earlier-zone change
        assert!(plan_lighting_preset(
            &[patches[0].expected.clone(), patches[0].expected.clone()],
            &[patches[0].clone(), patches[0].clone()]
        )
        .is_err());
    }
    #[test]
    fn modern_standby_and_effects_change_only_requested_fields() {
        let before = state(2, 0);
        let patch = AdvancedLightingPatch {
            expected: before.clone(),
            writes: vec![
                LightingFieldWrite {
                    index: 13,
                    value: 3,
                },
                LightingFieldWrite {
                    index: 14,
                    value: 10,
                },
            ],
            activate: true,
        };
        let (after, commands) = plan_advanced_lighting_patch(&before, &patch).unwrap();
        assert_eq!(commands, vec![(13, 3), (14, 10)]);
        assert_eq!(
            readable_lighting_item(after.fields.as_ref().unwrap(), 1),
            Some(0)
        );
        assert!(!advanced_lighting_field_allowed(2, 0, 7, 1));
        assert!(!advanced_lighting_field_allowed(2, 0, 19, 1));
    }
    #[test]
    fn partial_software_reads_preserve_unknown_brightness_and_front3_guard() {
        let mut before = state(6, 4);
        let fields = before.fields.as_mut().unwrap();
        fields.successfully_read_items_mask &= !1;
        fields.complete = false;
        fields.items[0].firmware_status = Some(228);
        fields.items[0].value = None;
        let mut patch = AdvancedLightingPatch {
            expected: before.clone(),
            writes: vec![LightingFieldWrite {
                index: 3,
                value: 90,
            }],
            activate: true,
        };
        let (after, commands) = plan_advanced_lighting_patch(&before, &patch).unwrap();
        assert_eq!(commands, vec![(3, 90)]);
        assert_eq!(
            after.fields.as_ref().unwrap().items[0],
            before.fields.as_ref().unwrap().items[0]
        );
        patch.writes = vec![LightingFieldWrite {
            index: 0,
            value: 50,
        }];
        assert!(plan_advanced_lighting_patch(&before, &patch).is_err());
        patch.writes = vec![LightingFieldWrite { index: 5, value: 0 }];
        assert!(plan_advanced_lighting_patch(&before, &patch).is_err());
    }
    #[test]
    fn disabled_role_has_no_fake_fields_and_conflicts_fail_before_commands() {
        let before = state(4, 6);
        let mut patch = AdvancedLightingPatch {
            expected: before.clone(),
            writes: vec![],
            activate: true,
        };
        let (after, writes) = plan_advanced_lighting_patch(&before, &patch).unwrap();
        assert_eq!(after.active_role, 6);
        assert!(writes.is_empty());
        patch.expected.active_role = 1;
        assert!(plan_advanced_lighting_patch(&before, &patch).is_err());
        assert!(reviewed_lighting_mask(2).is_none());
        assert!(reviewed_lighting_mask(3).is_none());
    }
    #[test]
    fn ranges_duplicates_and_unreadable_fields_fail_closed() {
        let before = state(0, 1);
        for writes in [
            vec![LightingFieldWrite { index: 4, value: 2 }],
            vec![LightingFieldWrite {
                index: 0,
                value: 101,
            }],
            vec![
                LightingFieldWrite { index: 0, value: 1 },
                LightingFieldWrite { index: 0, value: 2 },
            ],
        ] {
            assert!(plan_advanced_lighting_patch(
                &before,
                &AdvancedLightingPatch {
                    expected: before.clone(),
                    writes,
                    activate: true
                }
            )
            .is_err());
        }
        let mut unknown = before;
        unknown.fields.as_mut().unwrap().items[0].value = None;
        assert!(plan_advanced_lighting_patch(
            &unknown,
            &AdvancedLightingPatch {
                expected: unknown.clone(),
                writes: vec![LightingFieldWrite { index: 0, value: 1 }],
                activate: false
            }
        )
        .is_err());
    }
}

#[derive(Debug, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub struct CustomPowerBounds {
    pub minimum_pl1_watts: u8,
    pub maximum_pl1_watts: u8,
    pub minimum_pl2_watts: u8,
    pub maximum_pl2_watts: u8,
    pub tau_seconds: Vec<u16>,
}
pub fn custom_power_bounds(profiles: &[crate::BiosPowerProfile]) -> Option<CustomPowerBounds> {
    if profiles.len() != 3 {
        return None;
    }
    let maximum = profiles
        .iter()
        .find(|p| p.preset_kind == crate::PowerPresetKind::MaxPerformance)?;
    Some(CustomPowerBounds {
        minimum_pl1_watts: crate::MIN_SAFE_PL1_WATTS,
        maximum_pl1_watts: maximum.pl1_watts,
        minimum_pl2_watts: crate::MIN_SAFE_PL2_WATTS,
        maximum_pl2_watts: maximum.pl2_watts,
        tau_seconds: crate::TAU_LOOKUP_TABLE
            .iter()
            .map(|(seconds, _)| *seconds)
            .filter(|s| *s <= maximum.tau_seconds)
            .collect(),
    })
}
pub fn validate_custom_power(
    bounds: &CustomPowerBounds,
    limits: crate::PowerLimitConfig,
) -> Result<(), &'static str> {
    crate::validate_phantom_limits(limits)?;
    if !(bounds.minimum_pl1_watts..=bounds.maximum_pl1_watts).contains(&limits.pl1_watts)
        || !(bounds.minimum_pl2_watts..=bounds.maximum_pl2_watts).contains(&limits.pl2_watts)
        || !bounds.tau_seconds.contains(&limits.tau_seconds)
    {
        return Err("Custom power is outside the firmware-derived bounds.");
    }
    Ok(())
}
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct CustomPowerPatch {
    pub expected: crate::PowerReadback,
    pub limits: crate::PowerLimitConfig,
}

pub fn plan_custom_power_write(
    current: u64,
    patch: &CustomPowerPatch,
    bounds: &CustomPowerBounds,
) -> Result<u64, &'static str> {
    validate_custom_power(bounds, patch.limits)?;
    if crate::PowerReadback::from_raw(current) != patch.expected {
        return Err("Power register changed; refresh before applying");
    }
    crate::encode_phantom_msr_power_limit(current, patch.limits)
}

/// Test017's single shared-control command; never a two-slot/arbitrary-duty promise.
pub const REVIEWED_FIXED70_COMMAND: [u8; 3] = [2, 1, 70];
pub fn project_reviewed_fixed70(balanced: &[u8; 94]) -> [u8; 94] {
    let mut projected = *balanced;
    projected[12] = 0x80 | REVIEWED_FIXED70_COMMAND[2];
    projected
}

#[derive(Debug, Clone, PartialEq, Serialize, Deserialize)]
pub struct LightingSaveZone {
    pub zone_id: u8,
    pub active_role: u8,
    pub indicator_mask: u32,
    pub fields: Vec<LedFieldState>,
}
#[derive(Debug, Clone, PartialEq, Serialize, Deserialize)]
pub struct LightingSaveState {
    pub zone_mask: u32,
    pub zones: Vec<LightingSaveZone>,
}
/// Save is global: include even deferred Ethernet's existing readable settings, never edit them.
pub fn validate_lighting_save_state(state: &LightingSaveState) -> Result<(), &'static str> {
    if state.zone_mask != 0x75 || state.zones.len() != 5 {
        return Err("Incomplete global lighting snapshot.");
    }
    for (zone, id) in state.zones.iter().zip([0, 2, 4, 5, 6]) {
        if zone.zone_id != id
            || zone.indicator_mask != 0x77
            || zone.active_role > 6
            || 0x77 & (1 << zone.active_role) == 0
            || zone.fields.len() != 5
        {
            return Err("Unreviewed global lighting capabilities.");
        }
        for (fields, mode) in zone.fields.iter().zip([0, 1, 2, 4, 5]) {
            let mask = if mode == 2 {
                0x1e
            } else {
                reviewed_lighting_mask(mode).unwrap()
            };
            if fields.zone_id != id
                || fields.indicator_mode != mode
                || fields.advertised_items_mask != mask
                || fields.items.len() != mask.count_ones() as usize
            {
                return Err("Incomplete persisted field snapshot.");
            }
            for index in (0..24).filter(|i| mask & (1 << i) != 0) {
                if mode == 4 && index == 0 {
                    let unknown = fields
                        .items
                        .iter()
                        .find(|i| i.index == 0)
                        .ok_or("Missing Software brightness status.")?;
                    if unknown.value.is_some()
                        || unknown.firmware_status != Some(228)
                        || unknown.error.is_some()
                        || fields.successfully_read_items_mask & 1 != 0
                    {
                        return Err("Unexpected Software brightness result.");
                    }
                } else if readable_lighting_item(fields, index).is_none() {
                    return Err("A persisted lighting field is unreadable; save withheld.");
                }
            }
        }
    }
    Ok(())
}

#[cfg(test)]
mod expanded_tests {
    use super::*;
    use crate::*;
    #[test]
    fn custom_limits_keep_existing_minima_and_firmware_ceiling_and_tau() {
        let profiles = [
            PowerPresetKind::LowPower,
            PowerPresetKind::Balanced,
            PowerPresetKind::MaxPerformance,
        ]
        .into_iter()
        .zip([25, 30, 34])
        .map(|(preset_kind, pl1_watts)| BiosPowerProfile {
            name: format!("{preset_kind:?}"),
            preset_kind,
            pl1_watts,
            pl2_watts: 60,
            tau_seconds: 32,
            is_bios_default: false,
        })
        .collect::<Vec<_>>();
        let bounds = custom_power_bounds(&profiles).unwrap();
        assert_eq!(
            (
                bounds.minimum_pl1_watts,
                bounds.maximum_pl1_watts,
                bounds.minimum_pl2_watts,
                bounds.maximum_pl2_watts
            ),
            (15, 34, 20, 60)
        );
        assert!(validate_custom_power(
            &bounds,
            PowerLimitConfig {
                pl1_watts: 27,
                pl2_watts: 50,
                tau_seconds: 24
            }
        )
        .is_ok());
        for (pl1, pl2, tau) in [
            (14, 60, 32),
            (35, 60, 32),
            (30, 61, 32),
            (30, 60, 33),
            (30, 60, 64),
            (30, 20, 32),
        ] {
            assert!(validate_custom_power(
                &bounds,
                PowerLimitConfig {
                    pl1_watts: pl1,
                    pl2_watts: pl2,
                    tau_seconds: tau
                }
            )
            .is_err());
        }
        assert!(custom_power_bounds(&profiles[..2]).is_none());
        let raw = 240 | (480u64 << 32) | (15 << 17) | (1 << 15) | (1 << 47) | (1u64 << 63);
        let patch = CustomPowerPatch {
            expected: PowerReadback::from_raw(raw),
            limits: PowerLimitConfig {
                pl1_watts: 27,
                pl2_watts: 50,
                tau_seconds: 24,
            },
        };
        let written = plan_custom_power_write(raw, &patch, &bounds).unwrap();
        assert_eq!(
            written & !(PL1_MASK | PL2_MASK | TAU_MASK),
            raw & !(PL1_MASK | PL2_MASK | TAU_MASK)
        );
        assert!(plan_custom_power_write(raw ^ (1 << 15), &patch, &bounds).is_err());
        assert!(plan_custom_power_write(raw + 1, &patch, &bounds).is_err());
    }
    #[test]
    fn fixed70_packet_and_projection_match_the_narrow_recorded_operation() {
        assert_eq!(
            REVIEWED_FIXED70_COMMAND,
            [ECNUC22_SECTION, driver_fan_number(0).unwrap(), 70]
        );
        let balanced = *include_bytes!("../tests/fixtures/test036-fan-profile.bin");
        let fixed = project_reviewed_fixed70(&balanced);
        let changes = balanced
            .iter()
            .zip(fixed)
            .enumerate()
            .filter(|(_, (a, b))| **a != *b)
            .map(|(i, (_, b))| (i, b))
            .collect::<Vec<_>>();
        assert_eq!(changes, vec![(12, 198)]);
        assert_eq!(
            project_stock_fan(&stock_fixtures::BALANCED, &fixed),
            balanced
        );
    }
    fn global() -> LightingSaveState {
        LightingSaveState {
            zone_mask: 0x75,
            zones: [0, 2, 4, 5, 6]
                .into_iter()
                .map(|zone_id| LightingSaveZone {
                    zone_id,
                    active_role: 0,
                    indicator_mask: 0x77,
                    fields: [0, 1, 2, 4, 5]
                        .into_iter()
                        .map(|mode| {
                            let mask = if mode == 2 {
                                0x1e
                            } else {
                                reviewed_lighting_mask(mode).unwrap()
                            };
                            LedFieldState {
                                zone_id,
                                indicator_mode: mode,
                                advertised_items_mask: mask,
                                successfully_read_items_mask: if mode == 4 {
                                    mask & !1
                                } else {
                                    mask
                                },
                                complete: mode != 4,
                                items: (0..24)
                                    .filter(|i| mask & (1 << i) != 0)
                                    .map(|index| LedItemReadback {
                                        index,
                                        firmware_status: Some(if mode == 4 && index == 0 {
                                            228
                                        } else {
                                            0
                                        }),
                                        value: if mode == 4 && index == 0 {
                                            None
                                        } else {
                                            Some(0)
                                        },
                                        error: None,
                                    })
                                    .collect(),
                            }
                        })
                        .collect(),
                })
                .collect(),
        }
    }
    #[test]
    fn save_requires_every_persisted_readable_field_and_explicit_known_unknown() {
        let valid = global();
        assert!(validate_lighting_save_state(&valid).is_ok());
        let mut missing = valid.clone();
        missing.zones[4].fields[0].items.pop();
        assert!(validate_lighting_save_state(&missing).is_err());
        missing = valid.clone();
        missing.zones[0].fields[3].items[0].value = Some(50);
        assert!(validate_lighting_save_state(&missing).is_err());
        missing = valid.clone();
        missing.zones[2].fields[2].items[0].firmware_status = Some(228);
        assert!(validate_lighting_save_state(&missing).is_err());
        missing = valid.clone();
        missing.zones.swap(0, 1);
        assert!(validate_lighting_save_state(&missing).is_err());
        let mut external = valid.clone();
        external.zones[4].fields[0].items.last_mut().unwrap().value = Some(1);
        assert!(validate_lighting_save_precondition(&external, &valid).is_err());
        // Inactive standby changes remain in the exact global precondition.
    }
}

pub fn validate_lighting_save_precondition(
    current: &LightingSaveState,
    expected: &LightingSaveState,
) -> Result<(), &'static str> {
    validate_lighting_save_state(current)?;
    validate_lighting_save_state(expected)?;
    if current != expected {
        return Err("Global lighting changed; review a fresh snapshot before saving");
    }
    Ok(())
}
