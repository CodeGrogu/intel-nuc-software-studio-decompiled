# Verified vendor feature parity

Target: Intel NUC11PHi7 / NUC11PHBi7, BIOS `PHTGL579.0080.2025.1118.1734`. Other devices and firmware revisions fail closed. This matrix distinguishes implemented configuration paths from physical acceptance and unresolved firmware behavior.

| Feature | Maintained behavior and evidence boundary |
| --- | --- |
| Power presets | Firmware Low Power/Balanced/Performance, exact readback and no guessed tables. |
| Custom power | Explicit Apply within firmware MAX PERFORMANCE ceilings (34 W / 60 W / 32 s on the captured board), existing 15 W / 20 W minima, PL2 >= PL1 and supported discrete Tau. Full 64-bit hexadecimal precondition is checked at the final driver read; unrelated bits are preserved and full-register postread must match. |
| Balanced/Cool cooling | Exact reviewed stock packets and 94-byte readback. Three writer-tail bytes remain unreadable. |
| Fixed 70% cooling | Dedicated recorded command `[2,1,70]` through IOCTL `0x9c40a040`; only profile byte 12 changes to `0xC6`. Entry requires reviewed Balanced or the exact known fixed projection. Balanced/Cool can restore automatic control. It is not independent GPU/CPU duty control or arbitrary-duty support. Both stock and fixed70 state are excluded from legacy fan resume reconstruction, including after service restart. |
| Other cooling modes | Quiet, custom curves, arbitrary fixed values and zero-RPM remain unavailable. Advertisement is not validation; the rejected fan experiment batch is not retried. |
| Lighting zones | Power button, skull, front left/middle/right; exact zone mask `0x75`. |
| Power-state lighting | Awake fields 0-5 and Modern Standby (vendor Ready Mode) fields 12-17 only. Color, brightness, Solid/Breathing/Pulsing/Strobing and vendor frequency scale 0-10 are editable through Advanced lighting. S3/S5 fields are absent on this BIOS. |
| Other lighting functions | Disk activity, Software, Power Limit and Disabled with exact role-specific field layout. Unsupported/unreadable fields remain unavailable. Software brightness has no firmware getter/setter; front-right Software blue is blocked because it writes another zone. Same-zone awake RGB aliases other functions' RGB storage. |
| Power Limit indication | Configuration has readable setters/getters. Its automatic optical trigger remains unverified; saving a custom color is not a measurement of emitted color. |
| BIOS lighting save | Separate explicit all-five-zone action under Settings, after review of every readable persisted function/field, including existing Ethernet settings. Fresh global state must match before AppNotify; volatile state is compared afterward. Outcome is Acknowledged, never a claim of durable persistence. No automatic save or replay after uncertainty. |
| Ethernet | Firmware supports role 2; editing/testing remains deferred by user decision. Existing fields are inspected only for honest global-save scope. |
| Wi-Fi lighting | Role 3 is absent from `0x77` and unsupported by this BIOS. |
| CEC/display/USB/wake settings | Shared vendor/WMI declarations do not establish support. Captured firmware has no applicable command dispatch, exploratory getters returned 225, and the original app filters non-ODM DisplaySettings and NucWakeSleepSettings. No new controls or writes exposed. |
| Mobile/GPU/keyboard features | Not advertised for this board; shared DLL modules for other models are excluded. |
| Local lighting profiles | Confirmed in the vendor WMI SettingsManager; a manual, fresh-precondition implementation is the next bounded slice. No automatic startup/reconnect/resume application will be added. |
| Desktop behavior | Tray close, opt-in startup, isolated demo mode, and System/Light/Dark preference. Demo has its own single-instance objects and preferences so validation does not take over the user's normal app. |

Evidence: the captured DSDT and limitations are documented in [ACPI findings](ACPI_FINDINGS.md). `M003` advertises `0x03F03F` power-state items; `M005` selects roles and `M006` implements fields. `M007` sets `R5FF | 2` for AppNotify(1), with no durable-storage reader. Original `NucLedMgr.WMI.Models.WmiWrapper.SaveBIOSDefaults` has matching shipped IL (token `0x06000202` in NucLedMgr.dll SHA256 `d4a0e5fe80b3914da7460b6e366a39e114133bb83dc8373dfb8180643f04ad8f`). Vendor `NssPerformanceModeManager.SetMaxValues` derives custom limits from MAX PERFORMANCE. Fixed70 is corroborated by retained test017's executed script, before/applied/restore readbacks; its baseline equals the maintained stock baseline fixture. No new live experiment was used for this implementation.

Physical protocol-6 control/resume acceptance, BIOS persistence across restart, full theme/high-contrast/DPI validation and clean-machine runtime acceptance are separate from source/unit-test coverage. Installed service remains protocol 5 during development. External vendor clients are not serialized by this service's gate.
