//! SDDL-secured Named Pipe RPC Server (`\\.\pipe\Nuc11PhStudioV1`).
//!
//! Creates the local-only Named Pipe (`reject_remote_clients(true)`) with an explicit
//! security descriptor allowing unprivileged local interactive processes (WinUI 3 Studio,
//! Tray Flyout, Widget Provider, CLI) to connect to the `NT AUTHORITY\SYSTEM` service
//! without `ERROR_ACCESS_DENIED` or UAC prompts.

use std::ffi::c_void;
use std::mem::size_of;

use std::sync::Arc;
use tokio::io::{AsyncReadExt, AsyncWriteExt};
use tokio::net::windows::named_pipe::{NamedPipeServer, PipeMode, ServerOptions};
use tokio::sync::Notify;

use nuc11ph_hw::PhantomHardwareController;
use nuc11ph_protocol::{
    HardwareStateSnapshot, IpcRequest, IpcResponse, NucFanMode, StudioConfig, NUC11PH_NAMED_PIPE,
};
use nuc11ph_telemetry::PhantomTelemetryEngine;

#[repr(C)]
struct RawSecurityAttributes {
    n_length: u32,
    lp_security_descriptor: *mut c_void,
    b_inherit_handle: i32,
}

#[link(name = "advapi32")]
extern "system" {
    fn ConvertStringSecurityDescriptorToSecurityDescriptorW(
        text: *const u16,
        revision: u32,
        descriptor: *mut *mut c_void,
        size: *mut u32,
    ) -> i32;
}
#[link(name = "kernel32")]
extern "system" {
    fn LocalFree(memory: *mut c_void) -> *mut c_void;
}
struct PipeDescriptor(*mut c_void);
impl Drop for PipeDescriptor {
    fn drop(&mut self) {
        unsafe {
            LocalFree(self.0);
        }
    }
}
fn create_pipe_descriptor() -> std::io::Result<PipeDescriptor> {
    // SYSTEM/admins own the server; interactive clients cannot create pipe instances.
    let sddl: Vec<u16> = "D:P(A;;GA;;;SY)(A;;GA;;;BA)(A;;0x12019b;;;IU)"
        .encode_utf16()
        .chain(Some(0))
        .collect();
    let mut descriptor = std::ptr::null_mut();
    if unsafe {
        ConvertStringSecurityDescriptorToSecurityDescriptorW(
            sddl.as_ptr(),
            1,
            &mut descriptor,
            std::ptr::null_mut(),
        )
    } == 0
    {
        return Err(std::io::Error::last_os_error());
    }
    Ok(PipeDescriptor(descriptor))
}
fn create_permissive_pipe_instance(first_instance: bool) -> std::io::Result<NamedPipeServer> {
    let mut opts = ServerOptions::new();
    opts.first_pipe_instance(first_instance)
        .reject_remote_clients(true)
        .pipe_mode(PipeMode::Byte)
        .in_buffer_size(65536)
        .out_buffer_size(65536);
    let descriptor = create_pipe_descriptor()?;
    let mut attributes = RawSecurityAttributes {
        n_length: size_of::<RawSecurityAttributes>() as u32,
        lp_security_descriptor: descriptor.0,
        b_inherit_handle: 0,
    };
    unsafe {
        opts.create_with_security_attributes_raw(
            NUC11PH_NAMED_PIPE,
            &mut attributes as *mut RawSecurityAttributes as *mut c_void,
        )
    }
}
pub async fn run_named_pipe_server(
    hw: Arc<PhantomHardwareController>,
    telemetry: Arc<PhantomTelemetryEngine>,
    config: Arc<std::sync::Mutex<StudioConfig>>,
    shutdown: Arc<Notify>,
) -> std::io::Result<()> {
    let mut first_instance = true;

    loop {
        let server = create_permissive_pipe_instance(first_instance)?;
        first_instance = false;

        tokio::select! {
            _ = shutdown.notified() => {
                break;
            }
            conn_res = server.connect() => {
                if conn_res.is_ok() {
                    let hw_clone = Arc::clone(&hw);
                    let tel_clone = Arc::clone(&telemetry);
                    let cfg_clone = Arc::clone(&config);
                    tokio::spawn(async move {
                        let _ = handle_pipe_client(server, hw_clone, tel_clone, cfg_clone).await;
                    });
                }
            }
        }
    }
    Ok(())
}

async fn handle_pipe_client(
    mut pipe: NamedPipeServer,
    hw: Arc<PhantomHardwareController>,
    telemetry: Arc<PhantomTelemetryEngine>,
    config: Arc<std::sync::Mutex<StudioConfig>>,
) -> std::io::Result<()> {
    loop {
        let mut len_buf = [0u8; 4];
        if pipe.read_exact(&mut len_buf).await.is_err() {
            break;
        }
        let payload_len = u32::from_le_bytes(len_buf) as usize;
        if payload_len == 0 || payload_len > 256 * 1024 {
            break;
        }

        let mut payload = vec![0u8; payload_len];
        pipe.read_exact(&mut payload).await?;

        let response = match serde_json::from_slice::<IpcRequest>(&payload) {
            Ok(req) => dispatch_request(req, &hw, &telemetry, &config),
            Err(e) => IpcResponse::Error {
                message: format!("Invalid IPC request frame: {e}"),
            },
        };

        let resp_bytes = serde_json::to_vec(&response).unwrap_or_default();
        let resp_len = (resp_bytes.len() as u32).to_le_bytes();
        pipe.write_all(&resp_len).await?;
        pipe.write_all(&resp_bytes).await?;
        pipe.flush().await?;
    }
    Ok(())
}

fn dispatch_request(
    req: IpcRequest,
    hw: &PhantomHardwareController,
    telemetry: &PhantomTelemetryEngine,
    config: &std::sync::Mutex<StudioConfig>,
) -> IpcResponse {
    match req {
        IpcRequest::GetBackendInfo => IpcResponse::BackendInfo {
            version: env!("CARGO_PKG_VERSION").into(),
            protocol_revision: nuc11ph_protocol::NUC11PH_BACKEND_REVISION,
        },
        IpcRequest::GetHardwareCapabilities => {
            IpcResponse::HardwareCapabilities(hw.hardware_capabilities())
        }
        IpcRequest::GetVerifiedHardwareState => {
            let capabilities = hw.hardware_capabilities();
            let mut snap = telemetry.sample(hw);
            let (
                profile_name,
                _,
                _,
                _,
                _,
                mut cpu_curve,
                mut gpu_curve,
                mut led_zones,
                driver_status,
                wmi_status,
            ) = hw.current_state_metadata();
            for zone in nuc11ph_protocol::PhantomLedZone::ALL {
                let index = zone as usize;
                if capabilities.led_zone_readback_valid[index] {
                    if let Ok(actual) = hw.with_wmi(|w| w.read_led_zone_hardware_state(zone)) {
                        led_zones[index] = actual;
                    }
                }
            }
            if capabilities.fan_profile_readback_valid {
                if let Ok((curves, fan_off)) = hw.with_driver(|d| {
                    Ok(nuc11ph_protocol::parse_ec_fan_profile(
                        &d.read_ec_fan_profile()?,
                    ))
                }) {
                    cpu_curve = curves[0];
                    gpu_curve = curves[1];
                    snap.fan_off_enabled = fan_off;
                    snap.active_fan_mode =
                        nuc11ph_protocol::infer_ec_fan_mode(&curves, fan_off) as u8;
                }
            }
            let snapshot = HardwareStateSnapshot {
                telemetry: snap,
                cpu_fan_curve: cpu_curve,
                gpu_fan_curve: gpu_curve,
                led_zones,
                active_profile_name: profile_name,
                driver_status,
                wmi_status,
            };
            IpcResponse::VerifiedHardwareState(Box::new(nuc11ph_protocol::VerifiedHardwareState {
                snapshot: Box::new(snapshot),
                capabilities,
            }))
        }
        IpcRequest::RestoreLedSnapshot { zones, valid } => {
            match hw.restore_led_snapshot(zones, valid) {
                Ok(()) => IpcResponse::Ok,
                Err(e) => IpcResponse::Error {
                    message: e.to_string(),
                },
            }
        }
        IpcRequest::GetLightingZones => match hw.with_wmi(|w| w.zone_mask()) {
            Ok(mask) => IpcResponse::LightingZones { mask },
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::Ping => IpcResponse::Ok,
        IpcRequest::GetTelemetrySnapshot => {
            let snap = telemetry.sample(hw);
            IpcResponse::Telemetry(snap)
        }
        IpcRequest::GetHardwareState => {
            let snap = telemetry.sample(hw);
            let (
                profile_name,
                _,
                _,
                _,
                _,
                cpu_curve,
                gpu_curve,
                led_zones,
                driver_status,
                wmi_status,
            ) = hw.current_state_metadata();
            IpcResponse::HardwareState(Box::new(HardwareStateSnapshot {
                telemetry: snap,
                cpu_fan_curve: cpu_curve,
                gpu_fan_curve: gpu_curve,
                led_zones,
                active_profile_name: profile_name,
                driver_status,
                wmi_status,
            }))
        }
        IpcRequest::ApplyProfileByName { profile_name } => {
            let maybe_prof = {
                let guard = config.lock().unwrap();
                guard
                    .profiles
                    .iter()
                    .find(|p| p.name.eq_ignore_ascii_case(&profile_name))
                    .cloned()
            };
            match maybe_prof {
                Some(prof) => match hw.apply_studio_profile(&prof) {
                    Ok(()) => IpcResponse::Ok,
                    Err(e) => IpcResponse::Error {
                        message: e.to_string(),
                    },
                },
                None => IpcResponse::Error {
                    message: format!("Profile '{profile_name}' not found"),
                },
            }
        }
        IpcRequest::SetPowerPreset { preset } => match hw.set_power_preset(preset) {
            Ok(_) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::SetCustomPower { limits } => match hw.set_custom_power_limits(limits) {
            Ok(()) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::SetFanMode {
            mode,
            fan_off_enabled,
        } => match hw.set_fan_mode(NucFanMode::from_u8(mode), fan_off_enabled) {
            Ok(()) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::SetFixedFanDuty {
            cpu_fan_duty,
            gpu_fan_duty,
        } => match hw.set_fixed_fan_duty(cpu_fan_duty, gpu_fan_duty) {
            Ok(()) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::SetCustomFanCurve {
            cpu_fan,
            gpu_fan,
            fan_off_enabled,
        } => match hw.set_custom_fan_curves(cpu_fan, gpu_fan, fan_off_enabled) {
            Ok(()) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::SetLedZoneVolatile { zone } => match hw.set_led_zone_volatile(zone) {
            Ok(()) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::SetAllLedsVolatile { zones } => match hw.set_all_leds_volatile(zones) {
            Ok(()) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::SetGlobalLedBrightnessVolatile { brightness } => {
            match hw.set_global_brightness_volatile(brightness) {
                Ok(()) => IpcResponse::Ok,
                Err(e) => IpcResponse::Error {
                    message: e.to_string(),
                },
            }
        }
        IpcRequest::SetStealthModeVolatile { enable } => {
            match hw.set_stealth_mode_volatile(enable) {
                Ok(()) => IpcResponse::Ok,
                Err(e) => IpcResponse::Error {
                    message: e.to_string(),
                },
            }
        }
        IpcRequest::CommitLedsToBiosNvram => match hw.commit_leds_to_bios_nvram() {
            Ok(()) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
        IpcRequest::SendHdmiCecCommand { action } => match hw.execute_hdmi_cec(action) {
            Ok(_) => IpcResponse::Ok,
            Err(e) => IpcResponse::Error {
                message: e.to_string(),
            },
        },
    }
}

#[cfg(test)]
mod security_regression_011 {
    use super::*;
    #[link(name = "advapi32")]
    extern "system" {
        fn GetSecurityDescriptorDacl(
            descriptor: *const c_void,
            present: *mut i32,
            dacl: *mut *mut c_void,
            defaulted: *mut i32,
        ) -> i32;
    }
    #[test]
    fn pipe_descriptor_has_an_explicit_non_null_dacl() {
        let descriptor = create_pipe_descriptor().unwrap();
        let mut present = 0;
        let mut dacl = std::ptr::null_mut();
        let mut defaulted = 0;
        assert_ne!(
            unsafe {
                GetSecurityDescriptorDacl(descriptor.0, &mut present, &mut dacl, &mut defaulted)
            },
            0
        );
        assert_ne!(present, 0);
        assert!(
            !dacl.is_null(),
            "null DACL exposes privileged controls to everyone"
        );
    }
}

#[cfg(test)]
mod pipe_client_regression_011 {
    use super::*;
    use tokio::net::windows::named_pipe::ClientOptions;
    #[tokio::test]
    async fn interactive_client_can_open_the_helper_pipe() {
        let name = format!(r"\\.\pipe\KUI-acl-test-{}", std::process::id());
        let descriptor = create_pipe_descriptor().unwrap();
        let mut attributes = RawSecurityAttributes {
            n_length: size_of::<RawSecurityAttributes>() as u32,
            lp_security_descriptor: descriptor.0,
            b_inherit_handle: 0,
        };
        let server = unsafe {
            ServerOptions::new()
                .first_pipe_instance(true)
                .reject_remote_clients(true)
                .create_with_security_attributes_raw(
                    &name,
                    &mut attributes as *mut RawSecurityAttributes as *mut c_void,
                )
        }
        .unwrap();
        let client = ClientOptions::new()
            .open(&name)
            .expect("interactive GUI must retain read/write access");
        server.connect().await.unwrap();
        drop(client);
    }
}
