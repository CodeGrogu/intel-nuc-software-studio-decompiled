//! Native COM / WMI dispatcher for `root\WMI:CISD_WMI` on Intel NUC11PHi7.
//!
//! Ensures per-thread `CoInitializeEx(COINIT_MULTITHREADED)` on Tokio worker threads,
//! handles `VT_I4` (`CIM_UINT32`) `Cmd` parameter marshaling required by `IWbemClassObject::Put`,
//! and provides an ACPI thermal zone temperature fallback (`MSAcpi_ThermalZoneTemperature`).

use std::sync::Mutex;
use thiserror::Error;
use windows::core::{IUnknown, Interface, BSTR, PCWSTR, VARIANT};
use windows::Win32::System::Com::{
    CoCreateInstance, CoInitializeEx, CoSetProxyBlanket, CLSCTX_INPROC_SERVER,
    COINIT_MULTITHREADED, EOAC_NONE, RPC_C_AUTHN_LEVEL_CALL, RPC_C_IMP_LEVEL_IMPERSONATE,
    SAFEARRAY,
};
use windows::Win32::System::Ole::{
    SafeArrayAccessData, SafeArrayGetLBound, SafeArrayGetUBound, SafeArrayUnaccessData,
};
use windows::Win32::System::Variant::{
    VT_ARRAY, VT_BSTR, VT_DISPATCH, VT_I4, VT_UI1, VT_UI4, VT_UNKNOWN,
};
use windows::Win32::System::Wmi::{
    IWbemClassObject, IWbemLocator, IWbemServices, WbemLocator, WBEM_FLAG_FORWARD_ONLY,
    WBEM_FLAG_RETURN_IMMEDIATELY, WBEM_INFINITE,
};

use nuc11ph_protocol::{
    pack_wmi_cmd, CisdWmiStatus, HdmiCecAction, LedZoneStateDto, NucFanMode, PhantomLedZone,
};

const RPC_C_AUTHN_WINNT: u32 = 10;
const RPC_C_AUTHZ_NONE: u32 = 0;

/// Standard 64-bit Windows COM `tagVARIANT` memory layout (`oaidl.h`, 24 bytes).
#[repr(C)]
struct RawVariant64 {
    vt: u16,
    w_reserved1: u16,
    w_reserved2: u16,
    w_reserved3: u16,
    ptr_val: *mut std::ffi::c_void,
    _pad: u64,
}

#[derive(Debug, Error)]
pub enum WmiError {
    #[error("COM/WMI error: {0}")]
    Com(String),
    #[error("CISD_WMI instance not found in root\\WMI")]
    InstanceNotFound,
    #[error("Firmware status error: {0}")]
    FirmwareStatus(&'static str),
}

pub struct CisdWmiClient {
    services: IWbemServices,
    instance_path: BSTR,
    cached_zones: Mutex<[Option<LedZoneStateDto>; 7]>,
    wmi_lock: Mutex<()>,
}

unsafe impl Send for CisdWmiClient {}
unsafe impl Sync for CisdWmiClient {}

impl CisdWmiClient {
    pub fn connect() -> Result<Self, WmiError> {
        unsafe {
            let _ = CoInitializeEx(None, COINIT_MULTITHREADED);

            let locator: IWbemLocator = CoCreateInstance(&WbemLocator, None, CLSCTX_INPROC_SERVER)
                .map_err(|e| WmiError::Com(e.to_string()))?;

            let services = locator
                .ConnectServer(
                    &BSTR::from("root\\WMI"),
                    &BSTR::new(),
                    &BSTR::new(),
                    &BSTR::new(),
                    0,
                    &BSTR::new(),
                    None,
                )
                .map_err(|e| WmiError::Com(e.to_string()))?;

            CoSetProxyBlanket(
                &services,
                RPC_C_AUTHN_WINNT,
                RPC_C_AUTHZ_NONE,
                PCWSTR::null(),
                RPC_C_AUTHN_LEVEL_CALL,
                RPC_C_IMP_LEVEL_IMPERSONATE,
                None,
                EOAC_NONE,
            )
            .map_err(|e| WmiError::Com(e.to_string()))?;

            let enumerator = services
                .CreateInstanceEnum(
                    &BSTR::from("CISD_WMI"),
                    WBEM_FLAG_FORWARD_ONLY | WBEM_FLAG_RETURN_IMMEDIATELY,
                    None,
                )
                .map_err(|e| WmiError::Com(e.to_string()))?;

            let mut objs = [None; 1];
            let mut returned = 0u32;
            enumerator
                .Next(WBEM_INFINITE, &mut objs, &mut returned)
                .ok()
                .map_err(|e| WmiError::Com(e.to_string()))?;

            let instance = objs[0].take().ok_or(WmiError::InstanceNotFound)?;
            let instance_path = Self::extract_string_property(&instance, "__PATH")
                .or_else(|| Self::extract_string_property(&instance, "__RELPATH"))
                .map(|s| BSTR::from(s.as_str()))
                .ok_or(WmiError::InstanceNotFound)?;

            Ok(Self {
                services,
                instance_path,
                cached_zones: Mutex::new([None; 7]),
                wmi_lock: Mutex::new(()),
            })
        }
    }

    unsafe fn extract_string_property(obj: &IWbemClassObject, prop_name: &str) -> Option<String> {
        let wide: Vec<u16> = prop_name.encode_utf16().chain(std::iter::once(0)).collect();
        let mut var = VARIANT::default();
        if obj
            .Get(PCWSTR::from_raw(wide.as_ptr()), 0, &mut var, None, None)
            .is_err()
        {
            return None;
        }
        let raw = &*(&var as *const VARIANT as *const RawVariant64);
        let res = if raw.vt == VT_BSTR.0 && !raw.ptr_val.is_null() {
            PCWSTR::from_raw(raw.ptr_val as *const u16).to_string().ok()
        } else {
            let s = var.to_string();
            if s.is_empty() {
                None
            } else {
                Some(s)
            }
        };
        res.filter(|s| !s.is_empty())
    }

    /// Invokes a strictly allowlisted `CISD_WMI` method with `[in] uint32 Cmd` and extracts `Data.Bytes`.
    pub fn exec_cisd_method(&self, method_name: &str, cmd: u32) -> Result<Vec<u8>, WmiError> {
        let _guard = self.wmi_lock.lock().unwrap();
        unsafe {
            // Ensure COM MTA is initialized on whichever thread (e.g. Tokio worker) invokes WMI
            let _ = CoInitializeEx(None, COINIT_MULTITHREADED);

            let class_name = BSTR::from("CISD_WMI");
            let mut class_obj: Option<IWbemClassObject> = None;
            self.services
                .GetObject(
                    &class_name,
                    Default::default(),
                    None,
                    Some(&mut class_obj),
                    None,
                )
                .map_err(|e| WmiError::Com(e.to_string()))?;
            let class_obj =
                class_obj.ok_or_else(|| WmiError::Com("Missing CISD_WMI class".into()))?;

            let method_wide: Vec<u16> = method_name
                .encode_utf16()
                .chain(std::iter::once(0))
                .collect();
            let mut in_params_def: Option<IWbemClassObject> = None;
            class_obj
                .GetMethod(
                    PCWSTR::from_raw(method_wide.as_ptr()),
                    0,
                    &mut in_params_def,
                    std::ptr::null_mut(),
                )
                .map_err(|e| WmiError::Com(e.to_string()))?;

            let in_params_def = in_params_def
                .ok_or_else(|| WmiError::Com("Missing InParameters definition".into()))?;
            let in_instance = in_params_def
                .SpawnInstance(0)
                .map_err(|e| WmiError::Com(e.to_string()))?;

            let cmd_prop: Vec<u16> = "Cmd\0".encode_utf16().collect();
            // WMI IWbemClassObject::Put requires VT_I4 (i32) for CIM_UINT32 properties
            let cmd_var_i4 = VARIANT::from(cmd as i32);
            if in_instance
                .Put(PCWSTR::from_raw(cmd_prop.as_ptr()), 0, &cmd_var_i4, 0)
                .is_err()
            {
                let cmd_var_u4 = VARIANT::from(cmd);
                in_instance
                    .Put(PCWSTR::from_raw(cmd_prop.as_ptr()), 0, &cmd_var_u4, 0)
                    .map_err(|e| WmiError::Com(e.to_string()))?;
            }

            let mut out_params: Option<IWbemClassObject> = None;
            self.services
                .ExecMethod(
                    &self.instance_path,
                    &BSTR::from(method_name),
                    Default::default(),
                    None,
                    Some(&in_instance),
                    Some(&mut out_params),
                    None,
                )
                .map_err(|e| WmiError::Com(e.to_string()))?;

            let out_obj = out_params.ok_or_else(|| WmiError::Com("Empty OutParameters".into()))?;
            let bytes = Self::extract_data_bytes(&out_obj)?;
            crate::reference::checked_reply(&bytes, 1)?;
            Ok(bytes)
        }
    }

    unsafe fn extract_data_bytes(out_obj: &IWbemClassObject) -> Result<Vec<u8>, WmiError> {
        let data_prop: Vec<u16> = "Data\0".encode_utf16().collect();
        let mut data_var = VARIANT::default();
        out_obj
            .Get(
                PCWSTR::from_raw(data_prop.as_ptr()),
                0,
                &mut data_var,
                None,
                None,
            )
            .map_err(|e| WmiError::Com(e.to_string()))?;

        let raw_data = &*(&data_var as *const VARIANT as *const RawVariant64);
        if ![VT_UNKNOWN.0, VT_DISPATCH.0].contains(&raw_data.vt) || raw_data.ptr_val.is_null() {
            return Err(WmiError::Com(format!(
                "Unexpected embedded WMI Data type: 0x{:04x}",
                raw_data.vt
            )));
        }

        let unk = IUnknown::from_raw_borrowed(&raw_data.ptr_val)
            .ok_or_else(|| WmiError::Com("Data property punkVal is null".into()))?;
        let data_obj: IWbemClassObject = unk.cast().map_err(|e| WmiError::Com(e.to_string()))?;

        let bytes_prop: Vec<u16> = "Bytes\0".encode_utf16().collect();
        let mut bytes_var = VARIANT::default();
        data_obj
            .Get(
                PCWSTR::from_raw(bytes_prop.as_ptr()),
                0,
                &mut bytes_var,
                None,
                None,
            )
            .map_err(|e| WmiError::Com(e.to_string()))?;

        let mut result = Vec::new();
        let raw_bytes = &*(&bytes_var as *const VARIANT as *const RawVariant64);
        if raw_bytes.vt == (VT_ARRAY.0 | VT_UI1.0) {
            let psa = raw_bytes.ptr_val as *const SAFEARRAY;
            if !psa.is_null() {
                let lbound = SafeArrayGetLBound(psa, 1).unwrap_or(0);
                let ubound = SafeArrayGetUBound(psa, 1).unwrap_or(-1);
                if ubound >= lbound {
                    let len = (ubound - lbound + 1) as usize;
                    let mut raw_ptr: *mut std::ffi::c_void = std::ptr::null_mut();
                    if SafeArrayAccessData(psa, &mut raw_ptr).is_ok() {
                        let slice = std::slice::from_raw_parts(raw_ptr as *const u8, len);
                        result.extend_from_slice(slice);
                        let _ = SafeArrayUnaccessData(psa);
                    }
                }
            }
        }
        Ok(result)
    }

    /// Reads CPU/ThermalZone temperature in °C from `root\WMI:MSAcpi_ThermalZoneTemperature`
    /// as a fallback when `PerformanceDriver.sys` is unavailable.
    pub fn query_acpi_thermal_zone_temp_c(&self) -> Option<u8> {
        let _guard = self.wmi_lock.lock().unwrap();
        unsafe {
            let _ = CoInitializeEx(None, COINIT_MULTITHREADED);
            let enumerator = self
                .services
                .CreateInstanceEnum(
                    &BSTR::from("MSAcpi_ThermalZoneTemperature"),
                    WBEM_FLAG_FORWARD_ONLY | WBEM_FLAG_RETURN_IMMEDIATELY,
                    None,
                )
                .ok()?;

            let mut objs = [None; 1];
            let mut returned = 0u32;
            let _ = enumerator.Next(WBEM_INFINITE, &mut objs, &mut returned);
            let instance = objs[0].take()?;

            let prop: Vec<u16> = "CurrentTemperature\0".encode_utf16().collect();
            let mut var = VARIANT::default();
            instance
                .Get(PCWSTR::from_raw(prop.as_ptr()), 0, &mut var, None, None)
                .ok()?;
            let raw = &*(&var as *const VARIANT as *const RawVariant64);
            let tenths_kelvin = if raw.vt == VT_I4.0 || raw.vt == VT_UI4.0 {
                (raw.ptr_val as usize) as u32
            } else {
                0
            };
            if tenths_kelvin > 2732 {
                let c = ((tenths_kelvin - 2732) / 10).clamp(1, 115) as u8;
                Some(c)
            } else {
                None
            }
        }
    }

    pub fn query_bios_turbo_profile_bytes(&self) -> Result<Vec<u8>, WmiError> {
        let cmd = pack_wmi_cmd(1, 0, 0, 0);
        let bytes = self.exec_cisd_method("PlatformConfigurationProfileReport", cmd)?;
        if !bytes.is_empty() {
            CisdWmiStatus::from_byte(bytes[0]).map_err(WmiError::FirmwareStatus)?;
            return Ok(bytes);
        }
        let fallback = self.exec_cisd_method("PlatformConfigurationAccessV1", cmd)?;
        if !fallback.is_empty() {
            CisdWmiStatus::from_byte(fallback[0]).map_err(WmiError::FirmwareStatus)?;
        }
        Ok(fallback)
    }

    pub fn query_bios_fan_preset_bytes(&self, mode: NucFanMode) -> Result<[u8; 90], WmiError> {
        let cmd = pack_wmi_cmd(2, 2, mode as u8, 0);
        let mut bytes = self.exec_cisd_method("PlatformConfigurationAccessV1", cmd)?;
        if bytes.is_empty() {
            bytes = self.exec_cisd_method("PlatformConfigurationProfileReport", cmd)?;
        }
        if bytes.len() < 90 {
            return Err(WmiError::Com(
                "BIOS fan preset response shorter than the 90 bytes required for an ECNUC22 write"
                    .into(),
            ));
        }
        CisdWmiStatus::from_byte(bytes[0]).map_err(WmiError::FirmwareStatus)?;
        let mut out = [0u8; 90];
        let copy_len = bytes.len().min(90);
        out[..copy_len].copy_from_slice(&bytes[..copy_len]);
        Ok(out)
    }

    pub fn query_active_bios_fan_mode(&self) -> Result<NucFanMode, WmiError> {
        let cmd = pack_wmi_cmd(2, 1, 0, 0);
        let bytes = self.exec_cisd_method("PlatformConfigurationProfileReport", cmd)?;
        if bytes.len() >= 6 && bytes[0] == 0 {
            return Ok(NucFanMode::from_u8(bytes[5]));
        }
        Err(WmiError::Com(
            "BIOS fan-mode readback was unavailable or malformed".into(),
        ))
    }

    pub fn indicator_mask(&self, zone: u8) -> Result<u32, WmiError> {
        let bytes = self.exec_cisd_method("QueryLed", pack_wmi_cmd(2, zone, 0, 0))?;
        crate::reference::checked_reply(&bytes, 4)?;
        Ok(u32::from_le_bytes([bytes[1], bytes[2], bytes[3], 0]))
    }
    pub fn zone_mask(&self) -> Result<u32, WmiError> {
        let bytes = self.exec_cisd_method("QueryLed", 0)?;
        crate::reference::checked_reply(&bytes, 4)?;
        Ok(u32::from_le_bytes([bytes[1], bytes[2], bytes[3], 0]))
    }
    pub fn read_staged_leds(&self) -> Result<Vec<LedZoneStateDto>, WmiError> {
        let pending = *self.cached_zones.lock().unwrap();
        let mut verified = Vec::new();
        for (id, zone) in pending.iter().enumerate() {
            if zone.is_some() {
                verified.push(
                    self.read_led_zone_hardware_state(PhantomLedZone::from_u8(id as u8).unwrap())?,
                );
            }
        }
        self.cached_zones.lock().unwrap().fill(None);
        Ok(verified)
    }
    pub fn control_mask(&self, zone: u8, indicator: u8) -> Result<u32, WmiError> {
        let bytes = self.exec_cisd_method("QueryLed", pack_wmi_cmd(3, zone, indicator, 0))?;
        crate::reference::checked_reply(&bytes, 4)?;
        Ok(u32::from_le_bytes([bytes[1], bytes[2], bytes[3], 0]))
    }
    fn current_indicator(&self, zone: u8) -> Result<u8, WmiError> {
        let bytes = self.exec_cisd_method("NewGetLedStatus", pack_wmi_cmd(0, zone, 0, 0))?;
        crate::reference::checked_reply(&bytes, 2)?;
        Ok(bytes[1])
    }
    pub fn apply_led_zone_volatile(&self, state: &LedZoneStateDto) -> Result<(), WmiError> {
        let current = self.current_indicator(state.zone_id)?;
        let indicators = self.indicator_mask(state.zone_id)?;
        let desired =
            crate::reference::plan_awake_lighting(state, indicators, 0x00ff_ffff, current)?.mode;
        let items = self.control_mask(state.zone_id, desired)?;
        let plan = crate::reference::plan_awake_lighting(state, indicators, items, current)?;
        for (method, cmd) in plan.commands {
            self.exec_cisd_method(method, cmd)?;
        }
        let mut staged = *state;
        staged.indicator_mode = plan.mode;
        self.cached_zones.lock().unwrap()[state.zone_id as usize] = Some(staged);
        Ok(())
    }
    pub fn apply_all_leds_volatile(&self, zones: &[LedZoneStateDto; 7]) -> Result<(), WmiError> {
        for zone in zones {
            self.apply_led_zone_volatile(zone)?;
        }
        Ok(())
    }

    pub fn commit_leds_to_bios_nvram(&self) -> Result<(), WmiError> {
        let cmd = pack_wmi_cmd(1, 0, 0, 0);
        let res = self.exec_cisd_method("AppNotify", cmd)?;
        if !res.is_empty() {
            CisdWmiStatus::from_byte(res[0]).map_err(WmiError::FirmwareStatus)?;
        }
        Ok(())
    }

    pub fn read_led_zone_hardware_state(
        &self,
        zone: PhantomLedZone,
    ) -> Result<LedZoneStateDto, WmiError> {
        let id = zone as u8;
        let mode = self.current_indicator(id)?;
        let mut dto = LedZoneStateDto::default_for_zone(zone);
        dto.indicator_mode = mode;
        if mode == 6 {
            dto.s0.brightness = 0;
            return Ok(dto);
        }
        let mask = self.control_mask(id, mode)?;
        let required = match mode {
            0 => 0x00ff_ffff,
            1 => 0x1f,
            2 | 5 => 0x1f,
            3 => 0x0f,
            4 => 0x3f,
            _ => 0,
        };
        if mask & required != required {
            return Err(WmiError::Com(format!(
                "LED zone {id} does not expose every readback item for mode {mode}"
            )));
        }
        let read = |idx: u8| -> Result<u8, WmiError> {
            if mask & (1u32 << idx) == 0 {
                return Ok(0);
            }
            let bytes = self.exec_cisd_method("NewGetLedStatus", pack_wmi_cmd(1, id, mode, idx))?;
            crate::reference::checked_reply(&bytes, 2)?;
            Ok(bytes[1])
        };
        match mode {
            0 | 4 => {
                dto.s0 = nuc11ph_protocol::PowerStateLightConfig {
                    brightness: read(0)?,
                    behavior: read(1)?,
                    frequency: read(2)?,
                    r: read(3)?,
                    g: read(4)?,
                    b: read(5)?,
                };
                if mode == 0 {
                    let mut slots = [dto.s3, dto.modern_standby, dto.s5];
                    for (i, slot) in slots.iter_mut().enumerate() {
                        let base = (6 + i * 6) as u8;
                        *slot = nuc11ph_protocol::PowerStateLightConfig {
                            brightness: read(base)?,
                            behavior: read(base + 1)?,
                            frequency: read(base + 2)?,
                            r: read(base + 3)?,
                            g: read(base + 4)?,
                            b: read(base + 5)?,
                        };
                    }
                    [dto.s3, dto.modern_standby, dto.s5] = slots;
                }
            }
            1 | 3 => {
                dto.s0.brightness = read(0)?;
                dto.s0.r = read(1)?;
                dto.s0.g = read(2)?;
                dto.s0.b = read(3)?;
                dto.s0.behavior = if mode == 1 { read(4)? } else { 0 };
            }
            2 | 5 => {
                dto.s0.behavior = read(0)?;
                dto.s0.brightness = read(1)?;
                dto.s0.r = read(2)?;
                dto.s0.g = read(3)?;
                dto.s0.b = read(4)?;
            }
            _ => {
                return Err(WmiError::Com(format!(
                    "Unsupported active LED indicator {mode}"
                )))
            }
        }
        Ok(dto)
    }

    /// Restore a previously read zone without converting its indicator mode or changing
    /// other zones. The readback must match the complete active configuration afterwards.
    pub fn restore_led_zone_snapshot(&self, zone: &LedZoneStateDto) -> Result<(), WmiError> {
        let id = zone.zone_id;
        if id > 6 || zone.indicator_mode > 6 {
            return Err(WmiError::Com("Invalid LED snapshot".into()));
        }
        let supported_modes = self.indicator_mask(id)?;
        if supported_modes & (1u32 << zone.indicator_mode) == 0 {
            return Err(WmiError::Com(format!(
                "LED zone {id} no longer supports saved mode {}",
                zone.indicator_mode
            )));
        }
        {
            let current = self.current_indicator(id)?;
            let items = if zone.indicator_mode == 6 {
                0
            } else {
                self.control_mask(id, zone.indicator_mode)?
            };
            let plan = crate::reference::plan_exact_led_restore(zone, items, current)?;
            for (method, cmd) in plan {
                self.exec_cisd_method(method, cmd)?;
            }
        }
        let actual = self.read_led_zone_hardware_state(PhantomLedZone::from_u8(id).unwrap())?;
        if !crate::reference::led_snapshot_matches(zone, &actual) {
            return Err(WmiError::Com(format!(
                "LED zone {id} restore did not match hardware readback"
            )));
        }
        Ok(())
    }
    pub fn execute_hdmi_cec(&self, action: HdmiCecAction) -> Result<Vec<u8>, WmiError> {
        let (method_name, cmd) = action.to_wmi_invocation();
        self.exec_cisd_method(method_name, cmd)
    }
}
