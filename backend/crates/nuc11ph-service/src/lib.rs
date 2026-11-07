//! Pure decisions shared by the Windows service and its event simulation tests.

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum ResumeNotification {
    Automatic,
    Suspend,
    Other,
}

/// Returns the requested HDMI wake behavior when this power event should trigger
/// hardware restoration. The caller owns the delayed hardware work.
pub fn resume_action(
    notification: ResumeNotification,
    reapply_on_resume: bool,
    wake_hdmi_cec: bool,
) -> Option<bool> {
    match (notification, reapply_on_resume) {
        (ResumeNotification::Automatic | ResumeNotification::Suspend, true) => Some(wake_hdmi_cec),
        _ => None,
    }
}
pub mod operations;

/// Requests service shutdown. Stores a permit, so a stop that arrives before the pipe
/// loop is waiting (hardware initialization, between connections) is still observed.
pub fn request_stop(signal: &tokio::sync::Notify) {
    signal.notify_one();
}

#[cfg(test)]
mod stop_signal {
    use super::request_stop;
    use std::time::Duration;
    use tokio::sync::Notify;

    #[tokio::test]
    async fn stop_requested_before_waiting_is_observed() {
        let signal = Notify::new();
        request_stop(&signal);
        assert!(
            tokio::time::timeout(Duration::from_millis(50), signal.notified())
                .await
                .is_ok(),
            "stop request was lost"
        );
    }
}

#[cfg(test)]
mod resume_defaults {
    use super::{resume_action, ResumeNotification};
    use nuc11ph_protocol::StudioConfig;

    #[test]
    fn default_service_config_takes_no_action_on_resume() {
        let config = StudioConfig::default();
        for notification in [ResumeNotification::Automatic, ResumeNotification::Suspend] {
            assert_eq!(
                resume_action(
                    notification,
                    config.reapply_on_standby_resume,
                    config.hdmi_cec.wake_tv_on_resume
                ),
                None
            );
        }
    }
}
