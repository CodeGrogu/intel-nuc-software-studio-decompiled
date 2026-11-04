//! Pure packet/validation regressions: no device, service, WMI or driver calls.
use nuc11ph_protocol::*;

fn valid_curves() -> (EcNuc22FanCurveDto, EcNuc22FanCurveDto) {
    (
        EcNuc22FanCurveDto::default_cpu_fan_curve(),
        EcNuc22FanCurveDto::default_gpu_fan_curve(),
    )
}

#[test]
fn captured_zero_maximum_and_absent_gpu_are_errors_without_rewriting_data() {
    let captured: serde_json::Value =
        serde_json::from_str(include_str!("fixtures/test036-captured-state.json")).unwrap();
    let cpu: EcNuc22FanCurveDto = serde_json::from_value(captured["cpu"].clone()).unwrap();
    let gpu: EcNuc22FanCurveDto = serde_json::from_value(captured["gpu"].clone()).unwrap();
    assert_eq!(cpu.primary_max_temp, 0);
    assert_eq!(gpu.primary_source, 0);
    assert_eq!(gpu.primary_max_temp, 0);
    assert!(build_custom_wmi_fan_profile(&FanProfileEcRaw::default(), &cpu, &gpu, true).is_err());
    // Error return must not replace unknown firmware values with invented maxima.
    assert_eq!(cpu.primary_max_temp, 0);
    assert_eq!(gpu.primary_source, 0);
}

#[test]
fn absent_gpu_alone_is_not_synthesized() {
    let (cpu, mut gpu) = valid_curves();
    gpu.primary_source = 0;
    gpu.primary_max_temp = 0;
    assert!(build_custom_wmi_fan_profile(&FanProfileEcRaw::default(), &cpu, &gpu, false).is_err());
}

#[test]
fn invalid_primary_and_secondary_bounds_are_rejected() {
    let (cpu, gpu) = valid_curves();
    for maximum in 0..40 {
        let mut invalid = cpu;
        invalid.primary_max_temp = maximum;
        assert!(
            build_custom_wmi_fan_profile(&FanProfileEcRaw::default(), &invalid, &gpu, true)
                .is_err()
        );
        let mut invalid = cpu;
        invalid.secondary_max_temp = maximum;
        assert!(
            build_custom_wmi_fan_profile(&FanProfileEcRaw::default(), &invalid, &gpu, true)
                .is_err()
        );
    }
    let mut reversed = cpu;
    reversed.primary_min_temp = reversed.primary_max_temp;
    assert!(validate_custom_fan_curve(&reversed).is_err());
}

#[test]
fn disabled_secondary_channel_stays_explicitly_zero() {
    let (mut cpu, gpu) = valid_curves();
    cpu.secondary_source = 0;
    cpu.secondary_max_temp = 0;
    cpu.secondary_min_temp = 0;
    cpu.secondary_min_duty = 0;
    cpu.secondary_duty_increment = 0;
    cpu.secondary_fan_off_temp = 0;
    let packet =
        build_custom_wmi_fan_profile(&FanProfileEcRaw::default(), &cpu, &gpu, true).unwrap();
    assert_eq!(&packet.data[9..15], &[0; 6]);
    cpu.secondary_min_temp = 1;
    assert!(validate_custom_fan_curve(&cpu).is_err());
}

#[test]
fn valid_supported_curves_preserve_the_legacy_golden_packet() {
    let (cpu, gpu) = valid_curves();
    let mut baseline = FanProfileEcRaw::default();
    for (index, value) in baseline.data.iter_mut().enumerate() {
        *value = index as u8;
    }
    let packet = build_custom_wmi_fan_profile(&baseline, &cpu, &gpu, true).unwrap();
    let expected: Vec<u8> =
        serde_json::from_str(include_str!("fixtures/legacy-valid-packet.json")).unwrap();
    assert_eq!(packet.data.as_slice(), expected.as_slice());
    let off = build_custom_wmi_fan_profile(&baseline, &cpu, &gpu, false).unwrap();
    let mut expected_off = expected;
    expected_off[ECNUC22_FAN_OFF_FLAG_BYTE_INDEX] &= !0x80;
    assert_eq!(off.data.as_slice(), expected_off.as_slice());
}
