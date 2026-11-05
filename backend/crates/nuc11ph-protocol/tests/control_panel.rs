use nuc11ph_protocol::*;

#[test]
fn absent_fan_records_are_not_reported_as_stopped_fans() {
    let mut raw = FanSpeedStructRaw::default();
    assert_eq!(decode_fan_readings(&raw), (None, None));
    raw.data[0] = 1;
    assert_eq!(decode_fan_readings(&raw), (Some(0), None));
    raw.data[3..6].copy_from_slice(&[2, 0x09, 0x60]);
    assert_eq!(decode_fan_readings(&raw), (Some(0), Some(2400)));
}

#[test]
fn stock_projection_preserves_gaps_and_allows_both_directions() {
    let original = *include_bytes!("fixtures/test036-fan-profile.bin");
    let balanced = project_stock_fan(&stock_fixtures::BALANCED, &original);
    let cool = project_stock_fan(&stock_fixtures::COOL, &balanced);
    assert_ne!(balanced, cool);
    assert_eq!(
        project_stock_fan(&stock_fixtures::BALANCED, &cool),
        balanced
    );
    assert!(stock_fan_matches(&stock_fixtures::BALANCED, &balanced));
    assert!(!stock_fan_matches(&stock_fixtures::COOL, &balanced));
    let mut changed_tail = stock_fixtures::BALANCED;
    changed_tail[87..].fill(173);
    assert_eq!(project_stock_fan(&changed_tail, &original), balanced);
}

fn power_table() -> Vec<u8> {
    let mut bytes = vec![0; 256];
    bytes[3] = 3;
    for (i, (name, watts)) in [("LOW POWER", 25), ("BALANCED", 30), ("MAX PERFORMANCE", 34)]
        .into_iter()
        .enumerate()
    {
        let start = 4 + i * 35;
        bytes[start] = if i == 1 { 0x81 } else { 1 };
        bytes[start + 1..start + 1 + name.len()].copy_from_slice(name.as_bytes());
        bytes[start + 32] = watts;
        bytes[start + 33] = 60;
        bytes[start + 34] = 32;
    }
    bytes
}
#[test]
fn power_discovery_is_complete_validated_and_honors_default_high_bit() {
    let mut bytes = power_table();
    let profiles = parse_bios_turbo_profiles(&bytes);
    assert_eq!(profiles.len(), 3);
    assert!(profiles[1].is_bios_default);
    assert!(parse_bios_turbo_profiles(&bytes[..50]).is_empty());
    bytes[4 + 32] = 0;
    assert!(parse_bios_turbo_profiles(&bytes).is_empty());
    bytes = power_table();
    bytes[0] = 228;
    assert!(parse_bios_turbo_profiles(&bytes).is_empty());
    bytes = power_table();
    bytes[3] = 2;
    assert!(parse_bios_turbo_profiles(&bytes).is_empty());
    bytes = power_table();
    let duplicate = bytes[4..39].to_vec();
    bytes[39..74].copy_from_slice(&duplicate);
    assert!(parse_bios_turbo_profiles(&bytes).is_empty());
}
#[test]
fn lighting_requires_real_readable_awake_fields_and_steady_behavior() {
    let mut fields = LedFieldState {
        zone_id: 2,
        indicator_mode: 0,
        advertised_items_mask: 63,
        successfully_read_items_mask: 63,
        complete: true,
        items: (0..6)
            .map(|index| LedItemReadback {
                index,
                firmware_status: Some(0),
                value: Some(0),
                error: None,
            })
            .collect(),
    };
    assert!(lighting_editable(&fields));
    fields.items[0].value = None;
    assert!(!lighting_editable(&fields));
    fields.items[0].value = Some(0);
    fields.items[1].value = Some(1);
    assert!(!lighting_editable(&fields));
    fields.items.clear();
    assert!(!lighting_editable(&fields));
}
#[test]
fn new_requests_are_additive_and_preserve_unknown_readings() {
    assert_eq!(
        serde_json::to_string(&IpcRequest::Ping).unwrap(),
        "\"Ping\""
    );
    let request = IpcRequest::ApplyControl {
        operation_id: "test-id".into(),
        mutation: ControlMutation::Fan {
            preset: StockFanPreset::Cool,
        },
    };
    let wire = serde_json::to_string(&request).unwrap();
    assert!(wire.contains("ApplyControl"));
    assert!(wire.contains("Cool"));
    let reading = Reading {
        value: None,
        source: "EC".into(),
        timestamp_ms: 0,
        error: Some("short read".into()),
    };
    assert!(serde_json::to_value(reading).unwrap()["value"].is_null());
}

#[test]
fn selected_zone_patch_preserves_frequency_sleep_and_rejects_conflicts() {
    let before = AwakeLightingState {
        zone_id: 6,
        active_role: 5,
        unavailable: None,
        fields: LedFieldState {
            zone_id: 6,
            indicator_mode: 0,
            advertised_items_mask: 0xffffff,
            successfully_read_items_mask: 0xffffff,
            complete: true,
            items: (0..24)
                .map(|index| LedItemReadback {
                    index,
                    firmware_status: Some(0),
                    value: Some(if index == 1 { 0 } else { index }),
                    error: None,
                })
                .collect(),
        },
    };
    let patch = LightingPatch {
        expected: before.clone(),
        brightness: Some(50),
        rgb: Some([3, 4, 255]),
    };
    let (after, commands) = plan_lighting_patch(&before, &patch).unwrap();
    assert_eq!(commands, vec![(0, 50), (5, 255)]);
    assert_eq!(after.active_role, 0);
    assert_eq!(after.fields.items[2], before.fields.items[2]);
    assert_eq!(after.fields.items[6..], before.fields.items[6..]);
    let mut conflict = before.clone();
    conflict.active_role = 0;
    assert!(plan_lighting_patch(&conflict, &patch).is_err());
    let invalid = LightingPatch {
        brightness: Some(101),
        ..patch
    };
    assert!(plan_lighting_patch(&before, &invalid).is_err());
}
