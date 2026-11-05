# NUC Control for Windows 11 x64

Extract the entire folder, then run `NucControl.exe`. Keep the runtime files beside it.
Run `NucControl.exe --demo` for a synthetic demonstration that cannot access hardware.

The frontend contains .NET and Windows App SDK runtime files. It does not install a service, Intel driver, or startup entry automatically. A separately installed protocol-6 Nuc11PhStudioService is required for hardware controls. Older backends display an update notice.

Closing hides to the notification area. Use its Exit menu to quit. Optional launch at sign-in is off by default; disable it before moving or deleting this folder. Startup and reconnect never apply settings.

Color and brightness remain drafts until Apply. Manual lighting replaces an activity indicator with its stored PowerState configuration. Cooling uses reviewed firmware Balanced/Cool packets; three firmware writer fields are unreadable, so full-state recovery cannot be guaranteed.

If a command loses its acknowledgement, refresh to reconcile. Do not repeatedly submit it. A service restart loses operation history and requires fresh discovery.

Packaging validation on this development PC is not a clean-machine certification. See the repository's desktop validation record for outstanding environment checks.
