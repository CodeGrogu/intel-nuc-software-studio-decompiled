//! Persistent configuration & profile schema (`%AppData%\Nuc11PhStudio\config.toml`)
//! for the NUC11PHi7 Windows 11 Studio Suite.

use crate::fan::{EcNuc22FanCurveDto, NucFanMode};
use crate::led::{LedZoneStateDto, PhantomLedZone};
use crate::power::PowerPresetKind;
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct HdmiCecSettings {
    pub wake_tv_on_resume: bool,
    pub sleep_tv_on_display_off: bool,
}

impl Default for HdmiCecSettings {
    fn default() -> Self {
        Self {
            wake_tv_on_resume: false,
            sleep_tv_on_display_off: false,
        }
    }
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct AutoSwitchRule {
    pub process_name: String,
    pub target_profile: String,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct StudioProfile {
    pub name: String,
    pub power_preset: PowerPresetKind,
    pub pl1_watts: u8,
    pub pl2_watts: u8,
    pub tau_seconds: u16,
    pub fan_mode: NucFanMode,
    pub fan_off_zero_rpm: bool,
    pub fixed_cpu_duty: u8,
    pub fixed_gpu_duty: u8,
    pub custom_fan_cpu: EcNuc22FanCurveDto,
    pub custom_fan_gpu: EcNuc22FanCurveDto,
    pub led_zones: [LedZoneStateDto; 7],
}

impl StudioProfile {
    pub fn default_leds() -> [LedZoneStateDto; 7] {
        [
            LedZoneStateDto::default_for_zone(PhantomLedZone::PowerButton),
            LedZoneStateDto::default_for_zone(PhantomLedZone::Hdd),
            LedZoneStateDto::default_for_zone(PhantomLedZone::Skull),
            LedZoneStateDto::default_for_zone(PhantomLedZone::Eyes),
            LedZoneStateDto::default_for_zone(PhantomLedZone::Front1),
            LedZoneStateDto::default_for_zone(PhantomLedZone::Front2),
            LedZoneStateDto::default_for_zone(PhantomLedZone::Front3),
        ]
    }

    pub fn silent_office() -> Self {
        Self {
            name: "Silent Office".to_string(),
            power_preset: PowerPresetKind::LowPower,
            pl1_watts: 18,
            pl2_watts: 35,
            tau_seconds: 16,
            fan_mode: NucFanMode::Quiet,
            fan_off_zero_rpm: true,
            fixed_cpu_duty: 25,
            fixed_gpu_duty: 25,
            custom_fan_cpu: EcNuc22FanCurveDto::default_cpu_fan_curve(),
            custom_fan_gpu: EcNuc22FanCurveDto::default_gpu_fan_curve(),
            led_zones: Self::default_leds(),
        }
    }

    pub fn balanced_daily() -> Self {
        Self {
            name: "Balanced".to_string(),
            power_preset: PowerPresetKind::Balanced,
            pl1_watts: 28,
            pl2_watts: 60,
            tau_seconds: 28,
            fan_mode: NucFanMode::Balanced,
            fan_off_zero_rpm: true,
            fixed_cpu_duty: 40,
            fixed_gpu_duty: 40,
            custom_fan_cpu: EcNuc22FanCurveDto::default_cpu_fan_curve(),
            custom_fan_gpu: EcNuc22FanCurveDto::default_gpu_fan_curve(),
            led_zones: Self::default_leds(),
        }
    }

    pub fn phantom_gaming() -> Self {
        let mut leds = Self::default_leds();
        // Set Skull to Cyber Red and Eyes to Ice Cyan in Gaming profile
        leds[PhantomLedZone::Skull as usize].s0.r = 255;
        leds[PhantomLedZone::Skull as usize].s0.g = 20;
        leds[PhantomLedZone::Skull as usize].s0.b = 60;
        leds[PhantomLedZone::Eyes as usize].s0.r = 0;
        leds[PhantomLedZone::Eyes as usize].s0.g = 240;
        leds[PhantomLedZone::Eyes as usize].s0.b = 255;

        Self {
            name: "Phantom Gaming".to_string(),
            power_preset: PowerPresetKind::Custom,
            pl1_watts: 45,
            pl2_watts: 64,
            tau_seconds: 56,
            fan_mode: NucFanMode::Custom,
            fan_off_zero_rpm: false,
            fixed_cpu_duty: 75,
            fixed_gpu_duty: 75,
            custom_fan_cpu: EcNuc22FanCurveDto::default_cpu_fan_curve(),
            custom_fan_gpu: EcNuc22FanCurveDto::default_gpu_fan_curve(),
            led_zones: leds,
        }
    }
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct StudioConfig {
    pub schema_version: u32,
    pub active_profile: String,
    pub sync_windows_power_mode: bool,
    pub sync_accent_color_to_leds: bool,
    pub sync_dynamic_lighting_lamparray: bool,
    pub enable_focus_stealth_mode: bool,
    pub enable_eco_qos_in_tray: bool,
    pub reapply_on_standby_resume: bool,
    pub thermal_alert_cpu_c: u8,
    pub thermal_alert_gpu_c: u8,
    pub hdmi_cec: HdmiCecSettings,
    pub auto_switch_rules: Vec<AutoSwitchRule>,
    pub profiles: Vec<StudioProfile>,
}

impl Default for StudioConfig {
    fn default() -> Self {
        Self {
            schema_version: 1,
            active_profile: "Balanced".to_string(),
            sync_windows_power_mode: true,
            sync_accent_color_to_leds: false,
            sync_dynamic_lighting_lamparray: false,
            enable_focus_stealth_mode: true,
            enable_eco_qos_in_tray: true,
            reapply_on_standby_resume: false,
            thermal_alert_cpu_c: 92,
            thermal_alert_gpu_c: 86,
            hdmi_cec: HdmiCecSettings::default(),
            auto_switch_rules: vec![
                AutoSwitchRule {
                    process_name: "cyberpunk2077.exe".to_string(),
                    target_profile: "Phantom Gaming".to_string(),
                },
                AutoSwitchRule {
                    process_name: "helldivers2.exe".to_string(),
                    target_profile: "Phantom Gaming".to_string(),
                },
            ],
            profiles: vec![
                StudioProfile::silent_office(),
                StudioProfile::balanced_daily(),
                StudioProfile::phantom_gaming(),
            ],
        }
    }
}

#[cfg(test)]
mod resume_defaults {
    use super::StudioConfig;

    #[test]
    fn defaults_never_replay_hardware_or_wake_a_display_on_resume() {
        let config = StudioConfig::default();
        assert!(!config.reapply_on_standby_resume);
        assert!(!config.hdmi_cec.wake_tv_on_resume);
    }
}
