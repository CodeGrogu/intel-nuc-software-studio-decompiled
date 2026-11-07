//! In-memory stand-ins for `PerformanceDriver.sys` and `root\WMI:CISD_WMI` (tests only).
//!
//! Nothing here opens a device, a COM connection, the ISA bus mutex or the registry. The
//! controller built by [`fake_controller`] uses a [`Platform`] whose reconnect hooks always
//! fail, so a test can never fall through to native hardware.
use crate::driver::{DriverError, DriverOps};
use crate::wmi_cisd::{CisdTransport, CisdWmiClient, WmiError};
use crate::{PhantomHardwareController, Platform};
use nuc11ph_protocol::{
    encode_phantom_msr_power_limit, pack_wmi_cmd, project_stock_fan, stock_fixtures,
    FanProfileEcRaw, FanProfileWmiRaw, FanSpeedStructRaw, NucFanMode, PowerLimitConfig,
    TemperatureProfileEcRaw,
};
use std::collections::{BTreeMap, VecDeque};
use std::sync::{Arc, Mutex};

/// Captured BIOS 0080 Balanced EC readback.
pub const TEST036_BASELINE: &[u8; 94] =
    include_bytes!("../../nuc11ph-protocol/tests/fixtures/test036-fan-profile.bin");
/// Enable/clamp bits outside the PL1/PL2/tau fields; preserved by every encode.
pub const MSR_RESERVED_BITS: u64 = 0x0018_8000_0001_8000;
pub const BALANCED_LIMITS: PowerLimitConfig = PowerLimitConfig {
    pl1_watts: 40,
    pl2_watts: 64,
    tau_seconds: 28,
};
pub const MAX_PERFORMANCE_LIMITS: PowerLimitConfig = PowerLimitConfig {
    pl1_watts: 45,
    pl2_watts: 70,
    tau_seconds: 56,
};
pub const LOW_POWER_LIMITS: PowerLimitConfig = PowerLimitConfig {
    pl1_watts: 28,
    pl2_watts: 40,
    tau_seconds: 28,
};

pub fn balanced_fan_profile() -> [u8; 94] {
    project_stock_fan(&stock_fixtures::BALANCED, TEST036_BASELINE)
}

/// A 3-record `PlatformConfigurationAccessV1([1,0,0,0])` reply in the documented 35-byte layout.
pub fn turbo_table() -> Vec<u8> {
    let mut bytes = vec![0, 0, 0, 3];
    for (name, limits) in [
        ("LOW POWER", LOW_POWER_LIMITS),
        ("BALANCED", BALANCED_LIMITS),
        ("MAX PERFORMANCE", MAX_PERFORMANCE_LIMITS),
    ] {
        let mut record = [0u8; 35];
        record[..name.len()].copy_from_slice(name.as_bytes());
        record[32] = limits.pl1_watts;
        record[33] = limits.pl2_watts;
        record[34] = limits.tau_seconds as u8;
        bytes.extend_from_slice(&record);
    }
    bytes
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum DriverWrite {
    PowerLimitMsr(u64),
    FanProfile([u8; 90]),
    FixedFanDuty { fan: u8, duty: u8 },
}

pub struct FakeDriverState {
    pub msr: u64,
    /// Power writes are acknowledged but the register keeps its old value.
    pub ignore_power_writes: bool,
    pub fan_profile: [u8; 94],
    /// Readable projection the EC reports after any fan write; `None` leaves it unchanged.
    pub fan_after_write: Option<[u8; 94]>,
    /// Served (front first) before `fan_profile`, to model a concurrent change between reads.
    pub queued_fan_reads: VecDeque<[u8; 94]>,
    pub writes: Vec<DriverWrite>,
}

#[derive(Clone)]
pub struct FakeDriver(pub Arc<Mutex<FakeDriverState>>);

impl FakeDriver {
    pub fn new() -> Self {
        Self(Arc::new(Mutex::new(FakeDriverState {
            msr: encode_phantom_msr_power_limit(MSR_RESERVED_BITS, BALANCED_LIMITS).unwrap(),
            ignore_power_writes: false,
            fan_profile: balanced_fan_profile(),
            fan_after_write: None,
            queued_fan_reads: VecDeque::new(),
            writes: Vec::new(),
        })))
    }
    pub fn state(&self) -> std::sync::MutexGuard<'_, FakeDriverState> {
        self.0.lock().unwrap()
    }
    pub fn writes(&self) -> Vec<DriverWrite> {
        self.state().writes.clone()
    }
    fn fan_written(state: &mut FakeDriverState) {
        if let Some(after) = state.fan_after_write {
            state.fan_profile = after;
        }
    }
}

impl DriverOps for FakeDriver {
    fn check_driver_status(&self) -> Result<bool, DriverError> {
        Ok(true)
    }
    fn read_power_limit_msr(&self) -> Result<u64, DriverError> {
        Ok(self.state().msr)
    }
    fn write_power_limit_msr(&self, msr_value: u64) -> Result<(), DriverError> {
        let mut state = self.state();
        state.writes.push(DriverWrite::PowerLimitMsr(msr_value));
        if !state.ignore_power_writes {
            state.msr = msr_value;
        }
        Ok(())
    }
    fn read_ec_fan_profile(&self) -> Result<FanProfileEcRaw, DriverError> {
        let mut state = self.state();
        let data = match state.queued_fan_reads.pop_front() {
            Some(data) => data,
            None => state.fan_profile,
        };
        Ok(FanProfileEcRaw { data })
    }
    fn write_wmi_fan_profile_nuc22(&self, profile: &FanProfileWmiRaw) -> Result<(), DriverError> {
        let mut state = self.state();
        state.writes.push(DriverWrite::FanProfile(profile.data));
        Self::fan_written(&mut state);
        Ok(())
    }
    fn read_fan_speeds_in_section(
        &self,
        _ec_section: u8,
    ) -> Result<FanSpeedStructRaw, DriverError> {
        Ok(FanSpeedStructRaw::default())
    }
    fn set_fixed_fan_duty(&self, fan_number: u8, duty_pct: u8) -> Result<(), DriverError> {
        let mut state = self.state();
        state.writes.push(DriverWrite::FixedFanDuty {
            fan: fan_number,
            duty: duty_pct,
        });
        Self::fan_written(&mut state);
        Ok(())
    }
    fn read_ec_temperatures(&self) -> Result<TemperatureProfileEcRaw, DriverError> {
        Ok(TemperatureProfileEcRaw::default())
    }
}

/// Reviewed BIOS 0080 field masks per indicator mode (mode 2 advertises 0x1e).
pub fn field_mask(mode: u8) -> u32 {
    match mode {
        0 => 0x03f03f,
        1 | 5 => 0x1f,
        2 => 0x1e,
        4 => 0x3f,
        _ => 0,
    }
}
pub const REVIEWED_ZONES: [u8; 5] = [0, 2, 4, 5, 6];
/// Software brightness (mode 4 item 0) is advertised but the getter fails with this status.
pub const SOFTWARE_BRIGHTNESS_STATUS: u8 = 228;

pub struct FakeCisdState {
    pub turbo_table: Vec<u8>,
    pub zone_mask: u32,
    pub indicator_mask: u32,
    pub active_role: BTreeMap<u8, u8>,
    /// `(zone, mode, index) -> value` for every advertised readable item.
    pub items: BTreeMap<(u8, u8, u8), u8>,
    /// Setter calls are acknowledged but leave the model unchanged.
    pub ignore_led_writes: bool,
    /// Every call that reached the transport, in order.
    pub calls: Vec<(String, u32)>,
    /// Setter and `AppNotify` calls only.
    pub writes: Vec<(String, u32)>,
}

#[derive(Clone)]
pub struct FakeCisd(pub Arc<Mutex<FakeCisdState>>);

impl FakeCisd {
    pub fn new() -> Self {
        let mut items = BTreeMap::new();
        for zone in REVIEWED_ZONES {
            for mode in [0, 1, 2, 4, 5] {
                for index in (0..24).filter(|i| field_mask(mode) & (1 << i) != 0) {
                    if mode == 4 && index == 0 {
                        continue;
                    }
                    // Awake behavior (mode 0 item 1) must be steady (0) to be editable.
                    let value = match (mode, index) {
                        (_, 0) => 50,
                        (0, 1) => 0,
                        (_, i) => 10 * i,
                    };
                    items.insert((zone, mode, index), value);
                }
            }
        }
        Self(Arc::new(Mutex::new(FakeCisdState {
            turbo_table: turbo_table(),
            zone_mask: 0x75,
            indicator_mask: 0x77,
            active_role: REVIEWED_ZONES.iter().map(|z| (*z, 0)).collect(),
            items,
            ignore_led_writes: false,
            calls: Vec::new(),
            writes: Vec::new(),
        })))
    }
    pub fn state(&self) -> std::sync::MutexGuard<'_, FakeCisdState> {
        self.0.lock().unwrap()
    }
    pub fn writes(&self) -> Vec<(String, u32)> {
        self.state().writes.clone()
    }
}

fn mask_reply(mask: u32) -> Vec<u8> {
    let [a, b, c, _] = mask.to_le_bytes();
    vec![0, a, b, c]
}
fn unsupported(method: &str, cmd: u32) -> WmiError {
    WmiError::Com(format!("fake CISD: unsupported {method}({cmd:#010x})"))
}

impl CisdTransport for FakeCisd {
    fn exec(&self, method: &str, cmd: u32) -> Result<Vec<u8>, WmiError> {
        let mut state = self.state();
        state.calls.push((method.to_string(), cmd));
        let [b0, b1, b2, b3] = cmd.to_le_bytes();
        let zone_known = |s: &FakeCisdState, zone: u8| zone < 7 && s.zone_mask & (1 << zone) != 0;
        match method {
            "PlatformConfigurationAccessV1" => match (b0, b1, b2) {
                (1, 0, 0) => Ok(state.turbo_table.clone()),
                (2, 2, mode) if mode == NucFanMode::Balanced as u8 => {
                    Ok(stock_fixtures::BALANCED.to_vec())
                }
                (2, 2, mode) if mode == NucFanMode::Cool as u8 => Ok(stock_fixtures::COOL.to_vec()),
                _ => Err(unsupported(method, cmd)),
            },
            "QueryLed" => match b0 {
                0 => Ok(mask_reply(state.zone_mask)),
                2 if zone_known(&state, b1) => Ok(mask_reply(state.indicator_mask)),
                3 if zone_known(&state, b1) && state.indicator_mask & (1 << b2) != 0 => {
                    Ok(mask_reply(field_mask(b2)))
                }
                _ => Err(unsupported(method, cmd)),
            },
            "NewGetLedStatus" => match b0 {
                0 if zone_known(&state, b1) => Ok(vec![0, state.active_role[&b1]]),
                1 if b2 == 4 && b3 == 0 && zone_known(&state, b1) => {
                    Ok(vec![SOFTWARE_BRIGHTNESS_STATUS])
                }
                1 => state
                    .items
                    .get(&(b1, b2, b3))
                    .map(|value| vec![0, *value])
                    .ok_or_else(|| unsupported(method, cmd)),
                _ => Err(unsupported(method, cmd)),
            },
            "SetValueIndicatorOptionLedType" => {
                if !state.items.contains_key(&(b0, b1, b2)) {
                    return Err(unsupported(method, cmd));
                }
                state.writes.push((method.to_string(), cmd));
                if !state.ignore_led_writes {
                    state.items.insert((b0, b1, b2), b3);
                }
                Ok(vec![0])
            }
            "SetIndicatorOptionLedType" => {
                if !zone_known(&state, b0) || state.indicator_mask & (1 << b1) == 0 {
                    return Err(unsupported(method, cmd));
                }
                state.writes.push((method.to_string(), cmd));
                if !state.ignore_led_writes {
                    state.active_role.insert(b0, b1);
                }
                Ok(vec![0])
            }
            "AppNotify" if cmd == pack_wmi_cmd(1, 0, 0, 0) => {
                state.writes.push((method.to_string(), cmd));
                Ok(vec![0])
            }
            _ => Err(unsupported(method, cmd)),
        }
    }
    fn query_acpi_thermal_zone_temp_c(&self) -> Option<u8> {
        None
    }
}

fn eligible_identity() -> (String, String, bool) {
    (
        "NUC11PHi7".into(),
        "PHTGL579.0080.2025.1118.1734".into(),
        true,
    )
}
fn unreviewed_identity() -> (String, String, bool) {
    ("NUC11PHi7".into(), "PHTGL579.0079".into(), false)
}
fn no_driver() -> Result<Box<dyn DriverOps>, DriverError> {
    Err(DriverError::DeviceNotFound)
}
fn no_wmi() -> Result<CisdWmiClient, WmiError> {
    Err(WmiError::InstanceNotFound)
}

pub fn test_platform(eligible: bool) -> Platform {
    Platform {
        identity: if eligible {
            eligible_identity
        } else {
            unreviewed_identity
        },
        open_driver: no_driver,
        connect_wmi: no_wmi,
    }
}

/// A controller wired only to fresh fakes. Returned fakes share state with the controller.
pub fn fake_controller(eligible: bool) -> (PhantomHardwareController, FakeDriver, FakeCisd) {
    let driver = FakeDriver::new();
    let cisd = FakeCisd::new();
    let hw = PhantomHardwareController::from_backends(
        test_platform(eligible),
        Ok(Box::new(driver.clone())),
        Ok(CisdWmiClient::from_transport(Box::new(cisd.clone()))),
    );
    // Initialization only reads; start every test from an empty log.
    cisd.state().calls.clear();
    assert!(driver.writes().is_empty() && cisd.writes().is_empty());
    (hw, driver, cisd)
}

#[cfg(test)]
mod tests {
    use super::*;
    use nuc11ph_protocol::StockFanPreset;

    #[test]
    fn fake_controller_initializes_from_fakes_only() {
        let (hw, driver, cisd) = fake_controller(true);
        let state = hw.control_panel_state();
        assert!(state.device_eligible);
        assert!(state.custom_power_bounds.is_some());
        assert_eq!(state.power_presets.len(), 3);
        assert_eq!(state.power_limits, Some(BALANCED_LIMITS));
        assert_eq!(state.fan_preset, Some(StockFanPreset::Balanced));
        assert_eq!(state.led_zone_mask, 0x75);
        assert!(state.advanced_lighting_available);
        assert!(driver.writes().is_empty());
        assert!(cisd.writes().is_empty());
    }

    #[test]
    fn front3_blue_write_is_blocked_before_transport() {
        let (hw, _driver, cisd) = fake_controller(true);
        let result = hw.with_wmi(|w| {
            w.exec_cisd_method("SetValueIndicatorOptionLedType", pack_wmi_cmd(6, 4, 5, 1))
        });
        assert!(result.is_err());
        assert!(cisd.state().calls.is_empty());
    }

    #[test]
    fn missing_backends_never_reconnect_to_native() {
        let hw = PhantomHardwareController::from_backends(
            test_platform(true),
            Err(DriverError::DeviceNotFound),
            Err(WmiError::InstanceNotFound),
        );
        assert!(hw.with_driver(|d| d.read_power_limit_msr()).is_err());
        assert!(hw.with_wmi(|w| w.zone_mask()).is_err());
        let state = hw.control_panel_state();
        assert!(state.power_unavailable.is_some() && state.fan_unavailable.is_some());
    }
}
