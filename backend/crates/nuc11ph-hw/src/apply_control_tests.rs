//! `apply_control` outcome contract against in-memory hardware (see `fake.rs`).
//!
//! - `Rejected`: no write reached either fake.
//! - `PartialFailure`: a write was attempted and its postcondition did not hold.
//! - `Verified` / `Acknowledged`: exact readback / firmware acknowledgement.
//!
//! Every test also pins the exact write log, which is the wire encoding of the branch.
use crate::fake::*;
use crate::PhantomHardwareController;
use nuc11ph_protocol::*;

fn apply(hw: &PhantomHardwareController, mutation: &ControlMutation) -> (OperationOutcome, String) {
    hw.apply_control(mutation)
}

fn custom_power(driver: &FakeDriver, limits: PowerLimitConfig) -> ControlMutation {
    ControlMutation::CustomPower {
        patch: CustomPowerPatch {
            expected: PowerReadback::from_raw(driver.state().msr),
            limits,
        },
    }
}

const CUSTOM_LIMITS: PowerLimitConfig = PowerLimitConfig {
    pl1_watts: 42,
    pl2_watts: 66,
    tau_seconds: 32,
};

fn awake_patch(hw: &PhantomHardwareController, zone: u8) -> LightingPatch {
    let expected = hw.awake_lighting(zone).unwrap();
    assert_eq!(expected.unavailable, None);
    LightingPatch {
        expected,
        brightness: Some(80),
        rgb: Some([1, 2, 3]),
    }
}

fn advanced_patch(
    hw: &PhantomHardwareController,
    zone: u8,
    mode: u8,
    index: u8,
    value: u8,
) -> AdvancedLightingPatch {
    let expected = hw.lighting_mode(zone, mode).unwrap();
    assert_eq!(expected.unavailable, None);
    AdvancedLightingPatch {
        expected,
        writes: vec![LightingFieldWrite { index, value }],
        activate: true,
    }
}

/// One valid request per `ControlMutation` variant, built from fresh fake readbacks.
fn valid_mutations(hw: &PhantomHardwareController, driver: &FakeDriver) -> Vec<ControlMutation> {
    vec![
        custom_power(driver, CUSTOM_LIMITS),
        ControlMutation::FixedFan70 {},
        ControlMutation::SaveLighting {
            expected: hw.lighting_save_state().unwrap(),
        },
        ControlMutation::AdvancedLighting {
            patch: advanced_patch(hw, 2, 1, 0, 80),
        },
        ControlMutation::LightingPreset {
            patches: vec![advanced_patch(hw, 4, 0, 12, 30)],
        },
        ControlMutation::Power {
            preset: PowerPresetKind::MaxPerformance,
        },
        ControlMutation::Fan {
            preset: StockFanPreset::Cool,
        },
        ControlMutation::Lighting {
            patch: awake_patch(hw, 0),
        },
    ]
}

fn assert_no_writes(driver: &FakeDriver, cisd: &FakeCisd) {
    assert_eq!(driver.writes(), vec![]);
    assert_eq!(cisd.writes(), vec![]);
}

fn set_value(zone: u8, mode: u8, index: u8, value: u8) -> (String, u32) {
    (
        "SetValueIndicatorOptionLedType".into(),
        pack_wmi_cmd(zone, mode, index, value),
    )
}
fn set_role(zone: u8, mode: u8) -> (String, u32) {
    (
        "SetIndicatorOptionLedType".into(),
        pack_wmi_cmd(zone, mode, 0, 0),
    )
}

// ---- Rejected: nothing written ----

#[test]
fn unreviewed_device_rejects_every_mutation_without_writes() {
    let (eligible, eligible_driver, _) = fake_controller(true);
    let mutations = valid_mutations(&eligible, &eligible_driver);
    assert_eq!(mutations.len(), 8);
    for mutation in &mutations {
        let (hw, driver, cisd) = fake_controller(false);
        let (outcome, message) = apply(&hw, mutation);
        assert_eq!(outcome, OperationOutcome::Rejected, "{mutation:?}");
        assert!(
            message.contains("Unreviewed device or firmware"),
            "{message}"
        );
        assert_no_writes(&driver, &cisd);
    }
}

#[test]
fn custom_power_above_firmware_maximum_is_rejected_without_writes() {
    let (hw, driver, cisd) = fake_controller(true);
    // Inside the device-wide 15–50 W envelope, but above the firmware MaxPerformance PL1.
    let limits = PowerLimitConfig {
        pl1_watts: MAX_PERFORMANCE_LIMITS.pl1_watts + 1,
        ..MAX_PERFORMANCE_LIMITS
    };
    assert!(validate_phantom_limits(limits).is_ok());
    let (outcome, message) = apply(&hw, &custom_power(&driver, limits));
    assert_eq!(outcome, OperationOutcome::Rejected);
    assert!(
        message.contains("outside the firmware-derived bounds"),
        "{message}"
    );
    assert_no_writes(&driver, &cisd);
}

#[test]
fn custom_power_stale_register_snapshot_is_rejected_without_writes() {
    let (hw, driver, cisd) = fake_controller(true);
    let mutation = custom_power(&driver, CUSTOM_LIMITS);
    driver.state().msr ^= 1 << 3; // PL1 changed by someone else after the UI read it.
    let (outcome, message) = apply(&hw, &mutation);
    assert_eq!(outcome, OperationOutcome::Rejected);
    assert!(message.contains("Power register changed"), "{message}");
    assert_no_writes(&driver, &cisd);
}

#[test]
fn power_preset_missing_from_firmware_is_rejected_without_writes() {
    let (hw, driver, cisd) = fake_controller(true);
    let mutation = ControlMutation::Power {
        preset: PowerPresetKind::Custom,
    };
    let (outcome, message) = apply(&hw, &mutation);
    assert_eq!(outcome, OperationOutcome::Rejected);
    assert!(message.contains("Firmware preset unavailable"), "{message}");
    assert_no_writes(&driver, &cisd);
}

#[test]
fn unrecognized_fan_baseline_is_rejected_without_writes() {
    for mutation in [
        ControlMutation::Fan {
            preset: StockFanPreset::Cool,
        },
        ControlMutation::Fan {
            preset: StockFanPreset::Balanced,
        },
        ControlMutation::FixedFan70 {},
    ] {
        let (hw, driver, cisd) = fake_controller(true);
        let mut arbitrary = project_reviewed_fixed70(&balanced_fan_profile());
        arbitrary[12] = 0x80 | 71; // An unreviewed fixed duty.
        driver.state().fan_profile = arbitrary;
        let (outcome, message) = apply(&hw, &mutation);
        assert_eq!(outcome, OperationOutcome::Rejected, "{mutation:?}");
        assert!(
            message.contains("not reviewed Balanced, Cool or fixed70")
                || message.contains("Select reviewed Balanced before fixed70"),
            "{message}"
        );
        assert_no_writes(&driver, &cisd);
    }
}

#[test]
fn fixed70_requires_balanced_baseline() {
    let (hw, driver, cisd) = fake_controller(true);
    driver.state().fan_profile = project_stock_fan(&stock_fixtures::COOL, TEST036_BASELINE);
    assert_eq!(
        hw.control_panel_state().fan_preset,
        Some(StockFanPreset::Cool)
    );
    let (outcome, message) = apply(&hw, &ControlMutation::FixedFan70 {});
    assert_eq!(outcome, OperationOutcome::Rejected);
    assert!(
        message.contains("Select reviewed Balanced before fixed70"),
        "{message}"
    );
    assert_no_writes(&driver, &cisd);
}

#[test]
fn fixed70_preflight_change_is_rejected_without_write() {
    let (hw, driver, cisd) = fake_controller(true);
    let cool = project_stock_fan(&stock_fixtures::COOL, TEST036_BASELINE);
    driver
        .state()
        .queued_fan_reads
        .extend([balanced_fan_profile(), cool]);
    let (outcome, message) = apply(&hw, &ControlMutation::FixedFan70 {});
    assert_eq!(outcome, OperationOutcome::Rejected);
    assert!(
        message.contains("Fan state changed during preflight"),
        "{message}"
    );
    assert_no_writes(&driver, &cisd);
}

#[test]
fn stale_lighting_snapshot_is_rejected_without_writes() {
    let (hw, driver, cisd) = fake_controller(true);
    let mut patch = awake_patch(&hw, 0);
    patch.expected.fields.items[0].value = Some(99);
    let (outcome, message) = apply(&hw, &ControlMutation::Lighting { patch });
    assert_eq!(outcome, OperationOutcome::Rejected);
    assert!(message.contains("refresh first"), "{message}");
    assert_no_writes(&driver, &cisd);
}

#[test]
fn stale_advanced_lighting_snapshot_is_rejected_without_writes() {
    let (hw, driver, cisd) = fake_controller(true);
    let mut patch = advanced_patch(&hw, 2, 1, 0, 80);
    patch.expected.active_role = 1;
    let (outcome, message) = apply(&hw, &ControlMutation::AdvancedLighting { patch });
    assert_eq!(outcome, OperationOutcome::Rejected);
    assert!(message.contains("refresh before applying"), "{message}");
    assert_no_writes(&driver, &cisd);
}

#[test]
fn stale_save_snapshot_is_rejected_without_writes() {
    let (hw, driver, cisd) = fake_controller(true);
    let mut expected = hw.lighting_save_state().unwrap();
    expected.zones[0].fields[0].items[0].value = Some(1);
    assert!(validate_lighting_save_state(&expected).is_ok());
    let (outcome, message) = apply(&hw, &ControlMutation::SaveLighting { expected });
    assert_eq!(outcome, OperationOutcome::Rejected);
    assert!(message.contains("Global lighting changed"), "{message}");
    assert_no_writes(&driver, &cisd);
}

// ---- Verified / Acknowledged: exact readback ----

#[test]
fn custom_power_exact_readback_is_verified() {
    let (hw, driver, cisd) = fake_controller(true);
    let before = driver.state().msr;
    let requested = encode_phantom_msr_power_limit(before, CUSTOM_LIMITS).unwrap();
    let (outcome, message) = apply(&hw, &custom_power(&driver, CUSTOM_LIMITS));
    assert_eq!(
        (outcome, message.as_str()),
        (OperationOutcome::Verified, "Power setting verified.")
    );
    assert_eq!(driver.writes(), vec![DriverWrite::PowerLimitMsr(requested)]);
    assert_eq!(requested & MSR_RESERVED_BITS, MSR_RESERVED_BITS);
    assert_eq!(hw.control_panel_state().power_limits, Some(CUSTOM_LIMITS));
    assert_eq!(cisd.writes(), vec![]);
}

#[test]
fn power_preset_exact_readback_is_verified() {
    let (hw, driver, cisd) = fake_controller(true);
    let requested =
        encode_phantom_msr_power_limit(driver.state().msr, MAX_PERFORMANCE_LIMITS).unwrap();
    let mutation = ControlMutation::Power {
        preset: PowerPresetKind::MaxPerformance,
    };
    let (outcome, message) = apply(&hw, &mutation);
    assert_eq!(
        (outcome, message.as_str()),
        (OperationOutcome::Verified, "Power setting verified.")
    );
    assert_eq!(driver.writes(), vec![DriverWrite::PowerLimitMsr(requested)]);
    assert_eq!(cisd.writes(), vec![]);
}

#[test]
fn stock_fan_projection_readback_is_verified() {
    let (hw, driver, cisd) = fake_controller(true);
    let cool = project_stock_fan(&stock_fixtures::COOL, &balanced_fan_profile());
    driver.state().fan_after_write = Some(cool);
    let mutation = ControlMutation::Fan {
        preset: StockFanPreset::Cool,
    };
    let (outcome, message) = apply(&hw, &mutation);
    assert_eq!(outcome, OperationOutcome::Verified, "{message}");
    assert!(message.starts_with("Cooling setting verified."));
    assert_eq!(
        driver.writes(),
        vec![DriverWrite::FanProfile(stock_fixtures::COOL)]
    );
    assert_eq!(
        hw.control_panel_state().fan_preset,
        Some(StockFanPreset::Cool)
    );
    assert_eq!(cisd.writes(), vec![]);
}

#[test]
fn fixed70_projection_readback_is_verified() {
    let (hw, driver, cisd) = fake_controller(true);
    driver.state().fan_after_write = Some(project_reviewed_fixed70(&balanced_fan_profile()));
    let (outcome, message) = apply(&hw, &ControlMutation::FixedFan70 {});
    assert_eq!(outcome, OperationOutcome::Verified, "{message}");
    assert!(message.starts_with("Fixed70 command"));
    assert_eq!(
        driver.writes(),
        vec![DriverWrite::FixedFanDuty { fan: 0, duty: 70 }]
    );
    assert!(hw.control_panel_state().fixed_fan_70);
    assert_eq!(cisd.writes(), vec![]);
}

#[test]
fn awake_lighting_readback_is_verified() {
    let (hw, driver, cisd) = fake_controller(true);
    let patch = awake_patch(&hw, 0);
    let (outcome, message) = apply(&hw, &ControlMutation::Lighting { patch });
    assert_eq!(
        (outcome, message.as_str()),
        (OperationOutcome::Verified, "Lighting setting verified.")
    );
    // Only changed awake brightness/RGB items; role 0 is already active.
    assert_eq!(
        cisd.writes(),
        vec![
            set_value(0, 0, 0, 80),
            set_value(0, 0, 3, 1),
            set_value(0, 0, 4, 2),
            set_value(0, 0, 5, 3),
        ]
    );
    assert_eq!(driver.writes(), vec![]);
}

#[test]
fn awake_lighting_reactivates_awake_role() {
    let (hw, _driver, cisd) = fake_controller(true);
    cisd.state().active_role.insert(0, 5);
    let patch = awake_patch(&hw, 0);
    let (outcome, message) = apply(&hw, &ControlMutation::Lighting { patch });
    assert_eq!(outcome, OperationOutcome::Verified, "{message}");
    assert_eq!(cisd.writes().last(), Some(&set_role(0, 0)));
}

#[test]
fn advanced_lighting_field_readback_is_verified() {
    let (hw, driver, cisd) = fake_controller(true);
    let patch = advanced_patch(&hw, 2, 1, 0, 80);
    let (outcome, message) = apply(&hw, &ControlMutation::AdvancedLighting { patch });
    assert_eq!(
        (outcome, message.as_str()),
        (OperationOutcome::Verified, "Lighting setting verified.")
    );
    assert_eq!(cisd.writes(), vec![set_value(2, 1, 0, 80), set_role(2, 1)]);
    assert_eq!(driver.writes(), vec![]);
}

#[test]
fn lighting_preset_two_zones_are_verified() {
    let (hw, driver, cisd) = fake_controller(true);
    let patches = vec![
        advanced_patch(&hw, 4, 0, 12, 30),
        advanced_patch(&hw, 5, 5, 1, 90),
    ];
    let (outcome, message) = apply(&hw, &ControlMutation::LightingPreset { patches });
    assert_eq!(
        (outcome, message.as_str()),
        (OperationOutcome::Verified, "Lighting setting verified.")
    );
    // Zone 4 mode 0 is already active, so only zone 5 needs a role selector write.
    assert_eq!(
        cisd.writes(),
        vec![
            set_value(4, 0, 12, 30),
            set_value(5, 5, 1, 90),
            set_role(5, 5),
        ]
    );
    assert_eq!(driver.writes(), vec![]);
}

#[test]
fn save_lighting_is_acknowledged_not_verified() {
    let (hw, driver, cisd) = fake_controller(true);
    let expected = hw.lighting_save_state().unwrap();
    let (outcome, message) = apply(&hw, &ControlMutation::SaveLighting { expected });
    assert_eq!(outcome, OperationOutcome::Acknowledged, "{message}");
    assert!(message.contains("not independently readable or verified"));
    assert_eq!(
        cisd.writes(),
        vec![("AppNotify".to_string(), pack_wmi_cmd(1, 0, 0, 0))]
    );
    assert_eq!(driver.writes(), vec![]);
}

// ---- PartialFailure: a write was attempted, the postcondition did not hold ----

#[test]
fn custom_power_readback_mismatch_is_partial_failure() {
    let (hw, driver, _cisd) = fake_controller(true);
    driver.state().ignore_power_writes = true;
    let before = driver.state().msr;
    let (outcome, message) = apply(&hw, &custom_power(&driver, CUSTOM_LIMITS));
    assert_eq!(outcome, OperationOutcome::PartialFailure);
    assert!(
        message.contains("Exact power register postcondition mismatch"),
        "{message}"
    );
    assert_eq!(
        driver.writes(),
        vec![DriverWrite::PowerLimitMsr(
            encode_phantom_msr_power_limit(before, CUSTOM_LIMITS).unwrap()
        )]
    );
}

#[test]
fn power_preset_readback_mismatch_is_partial_failure() {
    let (hw, driver, _cisd) = fake_controller(true);
    driver.state().ignore_power_writes = true;
    let mutation = ControlMutation::Power {
        preset: PowerPresetKind::LowPower,
    };
    let (outcome, message) = apply(&hw, &mutation);
    assert_eq!(outcome, OperationOutcome::PartialFailure);
    assert!(message.contains("readback does not match"), "{message}");
    assert_eq!(driver.writes().len(), 1);
}

#[test]
fn stock_fan_readback_mismatch_is_partial_failure() {
    let (hw, driver, _cisd) = fake_controller(true);
    // No scripted projection: the EC keeps reporting Balanced after the Cool write.
    let mutation = ControlMutation::Fan {
        preset: StockFanPreset::Cool,
    };
    let (outcome, message) = apply(&hw, &mutation);
    assert_eq!(outcome, OperationOutcome::PartialFailure);
    assert!(
        message.contains("Stock fan readable projection mismatch"),
        "{message}"
    );
    assert_eq!(
        driver.writes(),
        vec![DriverWrite::FanProfile(stock_fixtures::COOL)]
    );
}

#[test]
fn fixed70_readback_mismatch_is_partial_failure() {
    let (hw, driver, _cisd) = fake_controller(true);
    let (outcome, message) = apply(&hw, &ControlMutation::FixedFan70 {});
    assert_eq!(outcome, OperationOutcome::PartialFailure);
    assert!(
        message.contains("Fixed70 readable projection mismatch"),
        "{message}"
    );
    assert_eq!(
        driver.writes(),
        vec![DriverWrite::FixedFanDuty { fan: 0, duty: 70 }]
    );
}

/// Pins current, conservative behavior: `Fan` marks the operation attempted before its
/// in-closure preflight re-read, so a concurrent change reports `PartialFailure` even though
/// no write was issued. (`FixedFan70` reports `Rejected` for the same race.)
#[test]
fn fan_preflight_change_reports_partial_failure_without_write() {
    let (hw, driver, cisd) = fake_controller(true);
    let cool = project_stock_fan(&stock_fixtures::COOL, TEST036_BASELINE);
    driver
        .state()
        .queued_fan_reads
        .extend([balanced_fan_profile(), cool]);
    let mutation = ControlMutation::Fan {
        preset: StockFanPreset::Cool,
    };
    let (outcome, message) = apply(&hw, &mutation);
    assert_eq!(outcome, OperationOutcome::PartialFailure);
    assert!(
        message.contains("Fan state changed during preflight"),
        "{message}"
    );
    assert_no_writes(&driver, &cisd);
}

#[test]
fn awake_lighting_postcondition_mismatch_is_partial_failure() {
    let (hw, _driver, cisd) = fake_controller(true);
    let patch = awake_patch(&hw, 0);
    cisd.state().ignore_led_writes = true;
    let (outcome, message) = apply(&hw, &ControlMutation::Lighting { patch });
    assert_eq!(outcome, OperationOutcome::PartialFailure);
    assert!(
        message.contains("Lighting postcondition mismatch"),
        "{message}"
    );
    assert_eq!(cisd.writes().len(), 4);
}

#[test]
fn advanced_lighting_postcondition_mismatch_is_partial_failure() {
    let (hw, _driver, cisd) = fake_controller(true);
    let patch = advanced_patch(&hw, 2, 1, 0, 80);
    cisd.state().ignore_led_writes = true;
    let (outcome, message) = apply(&hw, &ControlMutation::AdvancedLighting { patch });
    assert_eq!(outcome, OperationOutcome::PartialFailure);
    assert!(
        message.contains("Lighting postcondition mismatch"),
        "{message}"
    );
    assert_eq!(cisd.writes(), vec![set_value(2, 1, 0, 80), set_role(2, 1)]);
}

#[test]
fn lighting_preset_stops_after_first_zone_mismatch() {
    let (hw, _driver, cisd) = fake_controller(true);
    let patches = vec![
        advanced_patch(&hw, 4, 0, 12, 30),
        advanced_patch(&hw, 5, 5, 1, 90),
    ];
    cisd.state().ignore_led_writes = true;
    let (outcome, message) = apply(&hw, &ControlMutation::LightingPreset { patches });
    assert_eq!(outcome, OperationOutcome::PartialFailure, "{message}");
    // The second zone is never written once the first postcondition fails.
    assert_eq!(cisd.writes(), vec![set_value(4, 0, 12, 30)]);
}
