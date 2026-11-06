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
