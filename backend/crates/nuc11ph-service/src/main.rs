//! Privileged Rust Windows Service (`nuc11ph-service.exe`) for Intel NUC11PHi7 Studio.
//!
//! Follows strict Windows SCM best practices:
//! 1. Immediately registers with the Service Control Manager and reports `ServiceState::Running`
//!    before performing hardware/WMI initialization so the service never hangs at `StartPending`.
//! 2. Hosts the SDDL-secured Named Pipe server (`\\.\pipe\Nuc11PhStudioV1`) accessible to standard
//!    interactive users without UAC prompts.
//! 3. Observes `ServiceControl::PowerEvent` (`PBT_APMRESUMEAUTOMATIC` / `PBT_APMRESUMESUSPEND`).
//!    Hardware replay and HDMI-CEC wake run only when explicitly enabled in configuration,
//!    which no shipped default does.

mod panic_diagnostic;
mod pipe_server;

use std::ffi::OsString;
use std::sync::{Arc, Mutex, OnceLock};
use std::time::Duration;
use tokio::sync::Notify;
use windows_service::service::{
    PowerEventParam, ServiceControl, ServiceControlAccept, ServiceExitCode, ServiceState,
    ServiceStatus, ServiceType,
};
use windows_service::service_control_handler::{self, ServiceControlHandlerResult};
use windows_service::{define_windows_service, service_dispatcher};

use nuc11ph_hw::PhantomHardwareController;
use nuc11ph_protocol::StudioConfig;
use nuc11ph_service::{resume_action, ResumeNotification};
use nuc11ph_telemetry::PhantomTelemetryEngine;

const SERVICE_NAME: &str = "Nuc11PhStudioService";
const SERVICE_TYPE: ServiceType = ServiceType::OWN_PROCESS;

static GLOBAL_HW: OnceLock<Arc<PhantomHardwareController>> = OnceLock::new();

define_windows_service!(ffi_service_main, service_main);

fn main() -> Result<(), Box<dyn std::error::Error>> {
    panic_diagnostic::install();
    env_logger::init();
    let args: Vec<String> = std::env::args().collect();

    if args.iter().any(|a| a == "--inspect-controls") {
        // Read-only candidate validation without binding the installed service's pipe.
        let hw = PhantomHardwareController::initialize();
        let telemetry = PhantomTelemetryEngine::new();
        let lights: Vec<_> = [0, 2, 4, 5, 6]
            .into_iter()
            .map(|id| serde_json::json!({ "zone_id": id, "state": hw.awake_lighting(id).ok() }))
            .collect();
        println!(
            "{}",
            serde_json::to_string_pretty(&serde_json::json!({
                "control_panel": hw.control_panel_state(), "telemetry": telemetry.sample_status(&hw), "lighting": lights
            }))?
        );
        return Ok(());
    }

    if args.iter().any(|a| a == "--console") {
        println!("[Nuc11PhStudioService] Starting in interactive console mode...");
        let rt = tokio::runtime::Builder::new_multi_thread()
            .enable_all()
            .build()?;
        rt.block_on(async {
            let hw = Arc::new(PhantomHardwareController::initialize());
            let (_, _, _, _, _, _, _, _, drv_msg, wmi_msg) = hw.current_state_metadata();
            println!("[Nuc11PhStudioService] Driver: {drv_msg}");
            println!("[Nuc11PhStudioService] WMI   : {wmi_msg}");
            let telemetry = Arc::new(PhantomTelemetryEngine::new());
            let shutdown = Arc::new(Notify::new());

            pipe_server::run_named_pipe_server(hw, telemetry, shutdown).await
        })?;
        return Ok(());
    }

    service_dispatcher::start(SERVICE_NAME, ffi_service_main)?;
    Ok(())
}

fn service_main(_arguments: Vec<OsString>) {
    let _ = run_service();
}

fn run_service() -> Result<(), Box<dyn std::error::Error>> {
    let shutdown_notify = Arc::new(Notify::new());
    let shutdown_for_handler = Arc::clone(&shutdown_notify);

    let config = Arc::new(Mutex::new(StudioConfig::default()));
    let config_for_power_event = Arc::clone(&config);

    // 1. Register service control handler FIRST before any hardware/WMI I/O
    let event_handler = move |control_event| -> ServiceControlHandlerResult {
        match control_event {
            ServiceControl::Interrogate => ServiceControlHandlerResult::NoError,
            ServiceControl::Stop | ServiceControl::Shutdown => {
                shutdown_for_handler.notify_waiters();
                ServiceControlHandlerResult::NoError
            }
            ServiceControl::PowerEvent(power_param) => {
                let notification = match power_param {
                    PowerEventParam::ResumeAutomatic => ResumeNotification::Automatic,
                    PowerEventParam::ResumeSuspend => ResumeNotification::Suspend,
                    _ => ResumeNotification::Other,
                };
                let (reapply, wake_tv) = {
                    let guard = config_for_power_event.lock().unwrap();
                    (
                        guard.reapply_on_standby_resume,
                        guard.hdmi_cec.wake_tv_on_resume,
                    )
                };
                if let Some(wake_tv) = resume_action(notification, reapply, wake_tv) {
                    if let Some(hw_resume) = GLOBAL_HW.get().cloned() {
                        std::thread::spawn(move || {
                            std::thread::sleep(Duration::from_millis(1500));
                            if let Err(error) = hw_resume.restore_after_standby_resume(wake_tv) {
                                panic_diagnostic::record_resume_failure(&error.to_string());
                            }
                        });
                    }
                }
                ServiceControlHandlerResult::NoError
            }
            _ => ServiceControlHandlerResult::NotImplemented,
        }
    };

    let status_handle = service_control_handler::register(SERVICE_NAME, event_handler)?;

    // 2. Immediately report ServiceState::Running to Windows SCM so sc start never hangs
    status_handle.set_service_status(ServiceStatus {
        service_type: SERVICE_TYPE,
        current_state: ServiceState::Running,
        controls_accepted: ServiceControlAccept::STOP
            | ServiceControlAccept::SHUTDOWN
            | ServiceControlAccept::POWER_EVENT,
        exit_code: ServiceExitCode::Win32(0),
        checkpoint: 0,
        wait_hint: Duration::default(),
        process_id: None,
    })?;

    // 3. Initialize fault-tolerant hardware & telemetry engines
    let hw = Arc::new(PhantomHardwareController::initialize());
    let _ = GLOBAL_HW.set(Arc::clone(&hw));
    let telemetry = Arc::new(PhantomTelemetryEngine::new());

    let rt = tokio::runtime::Builder::new_multi_thread()
        .worker_threads(2)
        .enable_all()
        .build()?;

    let _ = rt.block_on(pipe_server::run_named_pipe_server(
        hw,
        telemetry,
        shutdown_notify,
    ));

    status_handle.set_service_status(ServiceStatus {
        service_type: SERVICE_TYPE,
        current_state: ServiceState::Stopped,
        controls_accepted: ServiceControlAccept::empty(),
        exit_code: ServiceExitCode::Win32(0),
        checkpoint: 0,
        wait_hint: Duration::default(),
        process_id: None,
    })?;

    Ok(())
}
