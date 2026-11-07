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
