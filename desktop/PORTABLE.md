# NUC Control for Windows 11 x64

Extract the entire folder, then run `NucControl.exe`. Keep the runtime files beside it.
Run `NucControl.exe --demo` for a synthetic demonstration that cannot access hardware.

The frontend contains .NET and Windows App SDK runtime files. It does not install a service, Intel driver, or startup entry automatically. A separately installed protocol-6 Nuc11PhStudioService is required for hardware controls. Older backends display an update notice.

Closing hides to the notification area. Use its Exit menu to quit. Optional launch at sign-in is off by default; disable it before moving or deleting this folder. Startup and reconnect never apply settings.

Color and brightness remain drafts until Apply. Advanced lighting exposes supported functions, effects, vendor frequency settings and Modern Standby fields. Named local presets capture active functions and apply only after explicit review; Ethernet-active zones and unsafe Software fields are omitted. Presets never run automatically. Manual awake lighting replaces an activity indicator with its stored PowerState configuration.

Power includes firmware presets and custom limits within the reviewed firmware envelope. Cooling includes stock Balanced/Cool and the separately recorded shared fixed-70% command, available only from reviewed Balanced/fixed-70% state. Other fixed duties, custom curves, Quiet and zero-RPM remain unavailable. Three firmware writer fields are unreadable, so full-state recovery cannot be guaranteed.

Settings offers System/Light/Dark themes and an explicit global lighting BIOS-save request with a full readable-state preview. Firmware acknowledgement does not prove durable persistence. Software brightness and front-right Software blue remain guarded firmware defects; Wi-Fi and CEC/display controls are unavailable, and Ethernet editing is deferred.

Demo uses separate single-instance and local settings identities, so it can run beside the hardware frontend. All demo values and operations are synthetic.

If a command loses its acknowledgement, refresh to reconcile. Do not repeatedly submit it. A service restart loses operation history and requires fresh discovery.

Packaging validation on this development PC is not a clean-machine certification. See the repository's desktop validation record for outstanding environment checks.

Startup diagnostics are kept locally in `%LOCALAPPDATA%/NucControl/Logs/startup.log` (under `NucControl/Demo/Logs` for demo). The log rotates with one previous file and records startup stages/exceptions; it is not uploaded.

The compact window keeps everyday controls together. Native caption buttons retain Windows Snap/window behavior. Functions/effects and local presets are under Lighting options; custom power and additional cooling sit in the Power and Cooling section headings. Click connection status for backend details. F5 refreshes and Ctrl+Q exits; Exit is also available in the window and tray. Revision5 exposes connection details only and requires a separately reviewed administrator upgrade for controls.
