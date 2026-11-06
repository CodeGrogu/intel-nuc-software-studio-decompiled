//! Lock order: service operation_gate -> driver/WMI -> cached state.
//! Callers hold operation_gate across this module's preflight/write/readback.
use super::*;
use nuc11ph_protocol::*;
use std::sync::OnceLock;

#[link(name = "advapi32")]
extern "system" {
    fn RegGetValueW(
        key: isize,
        subkey: *const u16,
        value: *const u16,
        flags: u32,
        kind: *mut u32,
        data: *mut std::ffi::c_void,
        size: *mut u32,
    ) -> i32;
}
fn bios_value(name: &str) -> String {
    let path: Vec<u16> = "HARDWARE\\DESCRIPTION\\System\\BIOS\0"
        .encode_utf16()
        .collect();
    let name: Vec<u16> = name.encode_utf16().chain(Some(0)).collect();
    let mut data = [0u16; 256];
    let mut size = std::mem::size_of_val(&data) as u32;
    let status = unsafe {
        RegGetValueW(
            0x80000002u32 as i32 as isize,
            path.as_ptr(),
            name.as_ptr(),
            2,
            std::ptr::null_mut(),
            data.as_mut_ptr().cast(),
            &mut size,
        )
    };
    if status != 0 {
        return String::new();
    }
    String::from_utf16_lossy(&data[..data.iter().position(|v| *v == 0).unwrap_or(data.len())])
}
fn identity() -> (String, String, bool) {
    let model = bios_value("SystemProductName");
    let bios = bios_value("BIOSVersion");
    let eligible = model == "NUC11PHi7"
        && bios_value("BaseBoardProduct") == "NUC11PHBi7"
        && bios == "PHTGL579.0080.2025.1118.1734";
    (model, bios, eligible)
}
pub fn backend_instance() -> String {
    static INSTANCE: OnceLock<String> = OnceLock::new();
    INSTANCE
        .get_or_init(|| {
            format!(
                "{}-{}",
                std::process::id(),
                std::time::SystemTime::now()
                    .duration_since(std::time::UNIX_EPOCH)
                    .unwrap_or_default()
                    .as_nanos()
            )
        })
        .clone()
}
fn checked_packet(packet: [u8; 90], preset: StockFanPreset) -> Result<[u8; 90], HardwareError> {
    let golden = match preset {
        StockFanPreset::Balanced => stock_fixtures::BALANCED,
        StockFanPreset::Cool => stock_fixtures::COOL,
    };
    if packet != golden {
        return Err(HardwareError::Invalid(
            "Firmware stock packet differs from reviewed fixture".into(),
        ));
    }
    Ok(packet)
}
fn recognized(actual: &[u8; 94], packets: &[[u8; 90]; 2]) -> Option<StockFanPreset> {
    let baseline = include_bytes!("../../nuc11ph-protocol/tests/fixtures/test036-fan-profile.bin");
    [StockFanPreset::Balanced, StockFanPreset::Cool]
        .into_iter()
        .zip(packets)
        .find(|(_, packet)| project_stock_fan(packet, baseline) == *actual)
        .map(|(p, _)| p)
}
pub(super) fn is_reviewed_stock(actual: &[u8; 94]) -> bool {
    recognized(actual, &[stock_fixtures::BALANCED, stock_fixtures::COOL]).is_some()
}
impl PhantomHardwareController {
    fn stock_packets(&self) -> Result<[[u8; 90]; 2], HardwareError> {
        let balanced = self.with_wmi(|w| w.query_bios_fan_preset_bytes(NucFanMode::Balanced))?;
        let cool = self.with_wmi(|w| w.query_bios_fan_preset_bytes(NucFanMode::Cool))?;
        Ok([
            checked_packet(balanced, StockFanPreset::Balanced)?,
            checked_packet(cool, StockFanPreset::Cool)?,
        ])
    }
    pub fn control_panel_state(&self) -> ControlPanelState {
        let (model, bios, eligible) = identity();
        let profiles = self
            .with_wmi(|w| w.query_bios_turbo_profile_bytes())
            .map(|b| parse_bios_turbo_profiles(&b))
            .unwrap_or_default();
        let power = self
            .with_driver(|d| d.read_power_limit_msr())
            .map(PowerReadback::from_raw);
        let fan = self.stock_packets().and_then(|packets| {
            let actual = self.with_driver(|d| d.read_ec_fan_profile())?;
            recognized(&actual.data, &packets).ok_or_else(|| {
                HardwareError::Invalid(
                    "Current fan baseline is not reviewed Balanced or Cool".into(),
                )
            })
        });
        ControlPanelState {
            backend_instance: backend_instance(),
            model,
            bios,
            device_eligible: eligible,
            power_unavailable: if !eligible {
                Some("Unreviewed device or firmware".into())
            } else if profiles.len() != 3 {
                Some("Firmware power table unavailable or invalid".into())
            } else {
                power.as_ref().err().map(ToString::to_string)
            },
            power_presets: profiles,
            power_limits: power.as_ref().ok().and_then(PowerReadback::exact_limits),
            power_readback: power.ok(),
            fan_unavailable: if !eligible {
                Some("Unreviewed device or firmware".into())
            } else {
                fan.as_ref().err().map(ToString::to_string)
            },
            fan_preset: fan.ok(),
            led_zone_mask: self.with_wmi(|w| w.zone_mask()).unwrap_or(0),
        }
    }
    pub fn awake_lighting(&self, zone_id: u8) -> Result<AwakeLightingState, HardwareError> {
        self.with_wmi(|w| {
            let role = w.current_indicator(zone_id)?;
            let fields = w.read_led_field_state(zone_id, 0)?;
            if w.current_indicator(zone_id)? != role {
                return Err(WmiError::Com(
                    "Active lighting role changed during read".into(),
                ));
            }
            let unavailable = if !identity().2 {
                Some("Unreviewed device or firmware".into())
            } else if !lighting_editable(&fields) {
                Some("Awake fields must be readable and steady to edit".into())
            } else {
                None
            };
            Ok(AwakeLightingState {
                zone_id,
                active_role: role,
                fields,
                unavailable,
            })
        })
    }
    pub fn lighting_mode(
        &self,
        zone_id: u8,
        indicator_mode: u8,
    ) -> Result<LightingModeState, HardwareError> {
        let expected_mask = reviewed_lighting_mask(indicator_mode)
            .ok_or_else(|| HardwareError::Invalid("Role is unsupported or deferred".into()))?;
        if ![0, 2, 4, 5, 6].contains(&zone_id) {
            return Err(HardwareError::Invalid("Unsupported lighting zone".into()));
        }
        self.with_wmi(|w| {
            if w.zone_mask()? != 0x75 {
                return Err(WmiError::Com("Unreviewed lighting zones".into()));
            }
            let indicator_mask = w.indicator_mask(zone_id)?;
            let active_role = w.current_indicator(zone_id)?;
            let fields = if indicator_mode == 6 {
                None
            } else {
                Some(w.read_led_field_state(zone_id, indicator_mode)?)
            };
            if w.current_indicator(zone_id)? != active_role {
                return Err(WmiError::Com("Role changed during read".into()));
            }
            let unavailable = if !identity().2
                || indicator_mask != 0x77
                || fields
                    .as_ref()
                    .is_some_and(|f| f.advertised_items_mask != expected_mask)
            {
                Some("Unreviewed device, firmware or lighting capabilities".into())
            } else {
                None
            };
            Ok(LightingModeState {
                zone_id,
                indicator_mode,
                active_role,
                indicator_mask,
                fields,
                unavailable,
            })
        })
    }
    /// Rejection is guaranteed before the first write. Failures after a write attempt are partial/uncertain.
    pub fn apply_control(&self, mutation: &ControlMutation) -> (OperationOutcome, String) {
        let mut attempted = false;
        let result = (|| -> Result<(), HardwareError> {
            if !identity().2 {
                return Err(HardwareError::Invalid(
                    "Unreviewed device or firmware".into(),
                ));
            }
            match mutation {
                ControlMutation::AdvancedLighting { patch } => {
                    let before =
                        self.lighting_mode(patch.expected.zone_id, patch.expected.indicator_mode)?;
                    let (expected, commands) = plan_advanced_lighting_patch(&before, patch)
                        .map_err(|e| HardwareError::Invalid(e.into()))?;
                    self.state.lock().unwrap().led_cache_verified[before.zone_id as usize] = false;
                    self.with_wmi(|w| {
                        for (index, value) in commands {
                            attempted = true;
                            w.exec_cisd_method(
                                "SetValueIndicatorOptionLedType",
                                pack_wmi_cmd(before.zone_id, before.indicator_mode, index, value),
                            )?;
                        }
                        if expected.active_role != before.active_role {
                            attempted = true;
                            w.exec_cisd_method(
                                "SetIndicatorOptionLedType",
                                pack_wmi_cmd(before.zone_id, expected.active_role, 0, 0),
                            )?;
                        }
                        Ok(())
                    })?;
                    if self.lighting_mode(before.zone_id, before.indicator_mode)? != expected {
                        return Err(HardwareError::Invalid(
                            "Lighting postcondition mismatch; refresh actual state".into(),
                        ));
                    }
                }
                ControlMutation::Power { preset } => {
                    let bytes = self.with_wmi(|w| w.query_bios_turbo_profile_bytes())?;
                    let profiles = parse_bios_turbo_profiles(&bytes);
                    let profile = profiles
                        .iter()
                        .find(|p| p.preset_kind == *preset)
                        .ok_or_else(|| {
                            HardwareError::Invalid("Firmware preset unavailable".into())
                        })?;
                    let limits = PowerLimitConfig {
                        pl1_watts: profile.pl1_watts,
                        pl2_watts: profile.pl2_watts,
                        tau_seconds: profile.tau_seconds,
                    };
                    self.with_driver(|d| {
                        d.read_power_limit_msr()?;
                        Ok(())
                    })?;
                    attempted = true;
                    self.set_custom_power_limits(limits)?;
                }
                ControlMutation::Fan { preset } => {
                    let packets = self.stock_packets()?;
                    let before = self.with_driver(|d| d.read_ec_fan_profile())?;
                    if recognized(&before.data, &packets).is_none() {
                        return Err(HardwareError::Invalid(
                            "Current fan baseline is not reviewed Balanced or Cool".into(),
                        ));
                    }
                    let packet = packets[if *preset == StockFanPreset::Balanced {
                        0
                    } else {
                        1
                    }];
                    let expected = project_stock_fan(&packet, &before.data);
                    // Invalidate before writing: resume must never reconstruct a stock packet as KUI curves.
                    {
                        let mut state = self.state.lock().unwrap();
                        state.fan_cache_verified = false;
                        state.stock_fan_selected = true;
                    }
                    attempted = true;
                    self.with_driver(|d| {
                        if d.read_ec_fan_profile()?.data != before.data {
                            return Err(DriverError::IoctlFailed { ioctl: 0, reason: "Fan state changed during preflight".into() });
                        }
                        d.write_wmi_fan_profile_nuc22(&FanProfileWmiRaw { data: packet })?;
                        if d.read_ec_fan_profile()?.data != expected {
                            return Err(DriverError::IoctlFailed { ioctl: 0, reason: "Stock fan readable projection mismatch; tail remains unverifiable".into() });
                        }
                        Ok(())
                    })?;
                }
                ControlMutation::Lighting { patch } => {
                    let before = self.awake_lighting(patch.expected.zone_id)?;
                    let (expected, commands) = plan_lighting_patch(&before, patch)
                        .map_err(|e| HardwareError::Invalid(e.into()))?;
                    self.state.lock().unwrap().led_cache_verified[before.zone_id as usize] = false;
                    self.with_wmi(|w| {
                        for (index, value) in commands {
                            attempted = true;
                            w.exec_cisd_method(
                                "SetValueIndicatorOptionLedType",
                                pack_wmi_cmd(before.zone_id, 0, index, value),
                            )?;
                        }
                        if before.active_role != 0 {
                            attempted = true;
                            w.exec_cisd_method(
                                "SetIndicatorOptionLedType",
                                pack_wmi_cmd(before.zone_id, 0, 0, 0),
                            )?;
                        }
                        Ok(())
                    })?;
                    if self.awake_lighting(before.zone_id)? != expected {
                        return Err(HardwareError::Invalid(
                            "Lighting postcondition mismatch; refresh actual state".into(),
                        ));
                    }
                }
            }
            Ok(())
        })();
        match result {
            Ok(()) => (
                OperationOutcome::Verified,
                match mutation {
                    ControlMutation::Power { .. } => "Power setting verified.",
                    ControlMutation::Fan { .. } => {
                        "Cooling setting verified. Three firmware writer fields remain unreadable."
                    }
                    ControlMutation::Lighting { .. } | ControlMutation::AdvancedLighting { .. } => {
                        "Lighting setting verified."
                    }
                }
                .into(),
            ),
            Err(e) => (
                if attempted {
                    OperationOutcome::PartialFailure
                } else {
                    OperationOutcome::Rejected
                },
                e.to_string(),
            ),
        }
    }
}
