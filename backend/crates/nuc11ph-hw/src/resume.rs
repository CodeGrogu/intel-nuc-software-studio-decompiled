//! Pure resume preflight. No I/O occurs until the complete preflight has passed.
use crate::HardwareError;
use nuc11ph_protocol::{EcNuc22FanCurveDto, LedZoneStateDto, NucFanMode, PowerLimitConfig};

#[derive(Clone, Copy)]
pub(crate) struct ResumeSnapshot {
    pub limits: PowerLimitConfig,
    pub power_verified: bool,
    pub fan_mode: NucFanMode,
    pub fan_verified: bool,
    pub curves: [EcNuc22FanCurveDto; 2],
    pub fixed_duties: [u8; 2],
    pub fan_off: bool,
    pub leds: [LedZoneStateDto; 7],
    pub led_verified: [bool; 7],
    pub stealth: bool,
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub(crate) enum FanAction {
    NamedPreset(NucFanMode),
    CheckedCustom,
    FixedDuties,
}

pub(crate) struct ResumePlan {
    pub snapshot: ResumeSnapshot,
    pub fan_action: FanAction,
    pub restore_leds: [bool; 7],
}

pub(crate) fn preflight(snapshot: ResumeSnapshot) -> Result<ResumePlan, &'static str> {
    if !snapshot.power_verified {
        return Err("Resume skipped: cached power values were never verified");
    }
    if !snapshot.fan_verified {
        return Err("Resume skipped: cached fan values were never verified");
    }
    nuc11ph_protocol::validate_phantom_limits(snapshot.limits)?;
    let fan_action = match snapshot.fan_mode {
        NucFanMode::Quiet | NucFanMode::Balanced | NucFanMode::Cool | NucFanMode::Custom => {
            for curve in &snapshot.curves {
                nuc11ph_protocol::validate_custom_fan_curve(curve)?;
            }
            if snapshot.fan_mode == NucFanMode::Custom {
                FanAction::CheckedCustom
            } else {
                FanAction::NamedPreset(snapshot.fan_mode)
            }
        }
        NucFanMode::Fixed => {
            if snapshot.fixed_duties.iter().any(|v| !(25..=99).contains(v)) {
                return Err("Resume skipped: fixed fan duties were not representable");
            }
            FanAction::FixedDuties
        }
        _ => return Err("Resume skipped: unsupported cached fan mode"),
    };
    let mut restore_leds = [false; 7];
    if !snapshot.stealth {
        for (index, zone) in snapshot.leds.iter().enumerate() {
            if !snapshot.led_verified[index] {
                continue;
            }
            if zone.zone_id as usize != index || zone.indicator_mode > 6 {
                return Err("Resume skipped: verified LED cache contains an invalid zone or mode");
            }
            restore_leds[index] = true;
        }
    }
    Ok(ResumePlan {
        snapshot,
        fan_action,
        restore_leds,
    })
}

pub(crate) fn execute_checked<R>(
    snapshot: ResumeSnapshot,
    execute: impl FnOnce(ResumePlan) -> Result<R, HardwareError>,
) -> Result<R, HardwareError> {
    let plan = preflight(snapshot).map_err(|e| HardwareError::Invalid(e.into()))?;
    execute(plan)
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::cell::Cell;
    fn valid() -> ResumeSnapshot {
        ResumeSnapshot {
            limits: PowerLimitConfig {
                pl1_watts: 30,
                pl2_watts: 60,
                tau_seconds: 32,
            },
            power_verified: true,
            fan_mode: NucFanMode::Custom,
            fan_verified: true,
            curves: [
                EcNuc22FanCurveDto::default_cpu_fan_curve(),
                EcNuc22FanCurveDto::default_gpu_fan_curve(),
            ],
            fixed_duties: [70, 70],
            fan_off: true,
            leds: nuc11ph_protocol::StudioProfile::default_leds(),
            led_verified: [false; 7],
            stealth: false,
        }
    }
    fn rejected_before_executor(snapshot: ResumeSnapshot) {
        let calls = Cell::new(0);
        let result = execute_checked(snapshot, |_| {
            calls.set(calls.get() + 1);
            Ok(())
        });
        assert!(result.is_err());
        assert_eq!(
            calls.get(),
            0,
            "No hardware writer/CEC/LED executor may be entered"
        );
    }
    #[test]
    fn captured_resume_fixture_rejects_before_every_write() {
        let fixture = nuc11ph_protocol::FanProfileEcRaw {
            data: *include_bytes!("../../nuc11ph-protocol/tests/fixtures/test036-fan-profile.bin"),
        };
        let mut snapshot = valid();
        snapshot.curves = nuc11ph_protocol::parse_ec_fan_profile(&fixture).0;
        rejected_before_executor(snapshot);
    }
    #[test]
    fn unverified_power_and_fan_fallbacks_are_never_replayed() {
        let mut snapshot = valid();
        snapshot.power_verified = false;
        rejected_before_executor(snapshot);
        let mut snapshot = valid();
        snapshot.fan_verified = false;
        rejected_before_executor(snapshot);
    }
    #[test]
    fn unverified_led_defaults_and_absent_zones_are_skipped() {
        let mut snapshot = valid();
        snapshot.led_verified[4] = true;
        snapshot.led_verified[6] = true;
        let plan = preflight(snapshot).unwrap();
        assert_eq!(
            plan.restore_leds,
            [false, false, false, false, true, false, true]
        );
        assert_eq!(
            plan.snapshot.leds[4].indicator_mode,
            snapshot.leds[4].indicator_mode
        );
    }
    #[test]
    fn no_default_led_is_selected_when_every_readback_failed() {
        assert_eq!(preflight(valid()).unwrap().restore_leds, [false; 7]);
    }
    #[test]
    fn invalid_verified_led_cache_rejects_before_every_write() {
        let mut snapshot = valid();
        snapshot.led_verified[4] = true;
        snapshot.leds[4].zone_id = 2;
        rejected_before_executor(snapshot);
    }
    #[test]
    fn stealth_does_not_replay_saved_visible_lighting() {
        let mut snapshot = valid();
        snapshot.led_verified.fill(true);
        snapshot.stealth = true;
        assert_eq!(preflight(snapshot).unwrap().restore_leds, [false; 7]);
    }
    #[test]
    fn fixed_mode_requires_verified_representable_duties() {
        let mut snapshot = valid();
        snapshot.fan_mode = NucFanMode::Fixed;
        snapshot.fixed_duties = [0, 70];
        rejected_before_executor(snapshot);
        snapshot.fixed_duties = [70, 70];
        assert!(preflight(snapshot).is_ok());
    }
    #[test]
    fn fan_mode_dispatch_preserves_named_custom_and_fixed_paths() {
        for mode in [NucFanMode::Quiet, NucFanMode::Balanced, NucFanMode::Cool] {
            let mut snapshot = valid();
            snapshot.fan_mode = mode;
            assert_eq!(
                preflight(snapshot).unwrap().fan_action,
                FanAction::NamedPreset(mode)
            );
        }
        assert_eq!(
            preflight(valid()).unwrap().fan_action,
            FanAction::CheckedCustom
        );
        let mut fixed = valid();
        fixed.fan_mode = NucFanMode::Fixed;
        assert_eq!(preflight(fixed).unwrap().fan_action, FanAction::FixedDuties);
    }
}
