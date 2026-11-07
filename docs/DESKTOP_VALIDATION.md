# NUC Control implementation validation

## Current candidate: 7fe0637c, 7 November 2026

All four clean Windows CI jobs passed in run `42276472283`. Independent checks passed 43 C# tests, 53 Rust tests, six publication-policy tests, repository checks, PowerShell syntax and Rust formatting. The frontend ZIP SHA256 is `f963313566d50e90c63994b63b4853b76d8ed6030eb84281a5561ce183655be3`. The required icon, compiled App/MainWindow XAML and app PRI were verified present and nonempty.

Fresh-artifact demo inspection confirmed:

- Successful startup, connected synthetic telemetry/preset state and scrollable controls.
- Explicit Light and Dark themes: readable main content, matching custom-power dialog and uniform full-window background when maximized beyond the content column.
- Custom power below the 15 W lower bound disables Apply after field validation; cancellation preserves 30/60/32.
- Software brightness is shown unavailable. Local preset capture and concrete review include Modern Standby and explicitly skip the Ethernet-active demo zone; review was canceled without applying.
- A brightness draft survived close-to-tray and second-instance reopening of the original window. On the final artifact, changing 50 to 40 and Discard restored 50 and cleared the unsaved indicator.

Theme preference was restored to System; demo preferences were verified with startup off. Only the test's own demo process was stopped during cleanup. This does not establish tray-menu Exit behavior. Host theme/scaling, hardware and installed service settings were unchanged.

The missing-icon and missing-XBF/PRI publication defects are closed by successful fresh-artifact startup at `3234bec9` and subsequent `7fe0637c` inspection. The light-theme background/dialog defect is closed by the final Light/Dark checks above. Earlier successful builds alone did not establish runtime acceptance.


## Original candidate validation (before the 6 November audit fixes)

- WinUI 3 Debug build: zero warnings and zero errors.
- Frontend Release publish and ZIP creation with locked dependencies and repository VERSION.
- 12 C# tests: framing, short reads, invalid lengths, truncation, cancellation, zero/unknown/stale readings, backend errors, protocol-5 gating, no startup writes, lost acknowledgements, demo power and selected-zone lighting.
- 37 Rust tests across protocol, hardware, service library, pipe security, and pure resume preflight. New cases cover stock projections in both directions, invalid power discovery, selected-zone lighting write scope/conflicts, missing fan records, and operation status/eviction.
- Six publication-policy tests, maintained PowerShell syntax checks, repository checks, and Rust formatting.
- Native demo inspection: compact layout and accessibility labels; Cool selection changed the selected control; brightness remained an unsaved draft until Apply, then refreshed to the applied value. Periodic refresh preserves preset controls instead of recreating focused elements.
- A read-only candidate inspection confirmed the reviewed model/board/BIOS identity. The unelevated process could not acquire driver/CISD WMI readings; control eligibility was correctly withheld.
- Release backend staging preserves hash-checked revision-5 rollback files and checks the candidate executable/PDB CodeView identity. Staging does not install the candidate or change hardware settings.

## Audit-fix validation, 6 November 2026

- All six vendor-backed audit findings are implemented in maintained source: asynchronous lighting selection, transport submission tracking, status-first recovery, early bounded admission/fencing, exact power readback, and unknown LED presentation.
- 28 C# tests pass in Release, including actual Core transport injection, delayed/stale zone reads, partial LED fields, pre-submission failure, original-token reconciliation, pending status without gated reads, expired history, delayed frames after fencing, response identity/service changes, and exact preset classification.
- 44 Rust tests pass, including the production decoded-request admission seam without a hardware controller, concurrent pending/fence/finish ordering, bounded saturation/eviction, stale/future epochs, and fractional/unknown-Tau decoding. Existing firmware guards and resume regressions remain green.
- The exact wire/recovery guarantees and bounded deduplication limits are documented in [the protocol contract](CONTROL_PANEL_PROTOCOL.md). No mutation is automatically replayed; an unknown history entry is fenced before fresh state can unlock the client.
- Local WinUI verification encountered sharing/access errors on the generated intermediate assembly, including fresh-output and short-path builds. A baseline checkout built on the same machine. These failed attempts are not build acceptance for the fixes; use [PR #12's clean Windows CI](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/pull/12/checks) for candidate compilation and package verification. No security settings were changed.
- No hardware mutation, service installation/change, vendor application execution, or protected driver access was used to implement or test the fixes. Historical candidate packaging and visual checks above do not validate the new binaries.

## Release acceptance still required

- Elevated protocol-6 inspection and controlled power, stock cooling, lighting, reconnect, and resume acceptance on the physical device.
- Clean Windows runtime smoke acceptance remains unverified. The completed isolated Sandbox harness was blocked by Application Control before app launch; see the exact result below. No policy bypass or alternate execution was attempted.
- Complete keyboard navigation, tray-menu Exit, opt-in sign-in startup and all dialog combinations. Close-to-tray/second-instance draft retention and Light/Dark main/custom-dialog rendering passed as recorded above; high-contrast and 150/200% scaling remain untested.

The installed service remains revision 5. This implementation is a reviewable candidate, not a claim that live hardware or clean-machine release acceptance has passed. The three unreadable fan writer-tail fields remain outside full verification and recovery.

## Expanded-control checkpoint, 6 November 2026

- Advanced-lighting commit `4bd8cf60` passed all four clean Windows CI jobs, including desktop publication. This provides successful build evidence despite the local XAML intermediate DLL access/sharing issue.
- The following software slice implements custom power with exact raw-register precondition, dedicated reviewed fixed70 cooling, explicit global BIOS-save acknowledgement, theme selection and isolated demo single-instance/preferences.
- 39 C# and 52 Rust tests pass locally for this slice. Coverage includes firmware-derived custom bounds, unrelated/raw register changes, fixed70 projection and startup replay exclusion, full-save field completeness, stale inactive fields, and lost save acknowledgement without replay.
- No installed service, hardware settings, host power state or security configuration changed. Expanded-controls commit 95353544 passed all four clean CI jobs (run 42228108551), including Windows desktop publish and backend release build. Native UI acceptance remains separate.


## Local preset checkpoint

43 C# tests and 53 Rust tests pass locally for this slice. The preset batch uses the same production executor in offline tests: stale later-zone preflight, conflicts after an earlier write, and final detection of earlier-zone changes. Core tests cover local storage without hardware calls, fresh explicit apply, duplicate zones/names, firmware binding, guarded fields, skipped Ethernet and unknown-field rejection. Profile commit e398c7fb passed all four clean CI jobs; subsequent packaged runtime defects and their fixes are recorded below. Final-artifact demo acceptance is summarized above. No live hardware/service/security changes are part of these checks.


## Packaged startup defect found during demo acceptance

The e398c7fb clean-CI ZIP passed builds/tests but crashed before opening its demo window (WER 9c7e08d0-ec73-46d5-9e2f-bc3ea9323fd6). Inspection found no physical `Assets/NucControl.ico`, although startup requires it for both AppWindow and the tray. `ApplicationIcon` only embeds the executable icon; WinUI's implicit Content item lacked output/publish-copy metadata. The project now updates that item to copy to both destinations, and Publish.ps1 fails before ZIP creation if the published asset is absent or differs from source. Bounded local startup logging records initialization stages and exceptions without suppressing failures. The icon omission was corrected at 0d024f15; the subsequent run exposed the separate missing-XAML-resource fault described below.


The corrected-icon commit 0d024f15 passed all four CI jobs, but its demo startup log identified an earlier MainWindow XAML load failure. Its ZIP also omitted `App.xbf`, `MainWindow.xbf` and `NucControl.pri`, although these are generated in build output. The SDK's XAML copy target prepares build/AppX output rather than this .NET publish payload. An explicit publish target now includes the SDK-generated XAML destinations and project PRI; publication fails if any required resource is missing/empty. Resource keys were checked against the shipped SDK theme source and were present. Fresh 3234bec9 startup succeeded with all required assets, and final 7fe0637c demo inspection confirmed continued startup and corrected themes.

## Isolated runtime smoke result, 7 November 2026

A fresh runtime-only Windows Sandbox harness used the exact 7fe0637c extracted artifact, mapping the app read-only and a separate results directory writable. Networking, audio/video input, clipboard and printer redirection were disabled. The retained clear PowerShell payload was supplied through EncodedCommand without changing execution policy. Bootstrap, prerequisite inventory and result JSON were produced.

Application Control blocked `Start-Process` before the app launched: `An Application Control policy has blocked this file.` The result has `process_id: null`, `has_exited: null`, and `window_activated: false`. Executable SHA256 was `8c1890c7ff7ad18aa303ff088a8ba288594fbad2f9e9c8e75680d2a490f3fa12`. This is an environment policy block, not an observed application startup crash, dependency failure or runtime pass. There was no retry through another path/executor, policy change, trust change or security bypass.

The inventory found no dotnet command, Program Files shared .NET frameworks or .NET SDK folders, but did find inbox `Microsoft.WindowsAppRuntime.CBS.1.6` version `6000.900.156.100` and `Microsoft.WindowsAppRuntime.CBS.2` version `2.5.1.100`. Therefore the environment is not described as having no preinstalled Windows App Runtime. The old incomplete Sandbox attempt is not counted. Local evidence is under `runtime/completion/sandbox-7fe0637c-20261107/results/`.

## Backend staging and remaining boundaries

Protocol 6 is staged only; the installed revision-5 service remains unchanged. The stage `runtime/completion/backend6-20261106-203731/deployment-plan.json` records `deployed: false`, candidate executable SHA256 `cabe132470e6ba0824832d9acfc187ed5c934f58e921b3d605f9cc6b4518fea0`, and candidate PDB SHA256 `08b90ad2948a7c0ebd6092e126c17d78b0529181d61e82505f5d1a1de2e221f0`, with matching CodeView identity. Revision-5 executable/PDB rollback files were hash-checked; the original executable SHA256 is `ff43952f82aaa153bb4eb61022774ddfd0456d810cc80390d9fadd6fd6289725`.

The implemented software scope is complete and reviewable; release acceptance remains separate:

- Deployment and elevated read-only protocol-6 inspection have not occurred. Physical power/cooling/lighting application and restore, reconnect/resume behavior, expanded effects/frequency/role behavior and multi-zone application still require controlled device acceptance.
- BIOS-save acknowledgement does not establish durable persistence. Automatic PowerLimit optical behavior and the three unreadable fan writer-tail fields remain unverified.
- Clean-runtime startup remains unverified because of the Sandbox policy block. Full keyboard coverage, tray-menu Exit, sign-in startup, high-contrast and 150/200% scaling remain untested.
- Ethernet editing remains deferred by request. Unsupported CEC/display/Wi-Fi/device-specific features and unvalidated Quiet/custom-curve/other-duty/zero-RPM paths remain unavailable. Known Software brightness and Front3 Software-blue guards remain in force; the previously rejected runtime fan batch was not retried.

No installed-service, hardware, host power/scaling/theme, security, trust or protected-driver changes were made during this implementation/acceptance stage. Demo preference changes were restored. No packaging or offline test result is presented as live-device certification.


## Compact native UI refinement, 7 November 2026

The next source checkpoint replaces the large duplicate heading and vertically separated preset rows with a 440×520-DIP native TitleBar window, combined power/cooling rows, compact telemetry and explicit lighting actions. Advanced features remain in labeled menus; native caption buttons/drag/Snap behavior and Mica backdrop use supported SDK controls. Outer-shell theme and explicit dialog theme inheritance retain the previously corrected behavior. A visible Exit, Ctrl+Q, F5 and connection-details action improve lifecycle/recovery access. Normal status stays compact; actionable warnings expand with scrolling available at constrained sizes.

45 C# and 53 Rust tests pass locally. Added tests prove protocol5 connection inspection reads only supported connection/capability fields, never promotes legacy telemetry/rounded power, and never enables or submits mutations. Real installed-service read-only inspection independently confirmed protocol5 and driver/WMI connectivity under LocalSystem; unelevated candidate errors reflect access restrictions. Controller acquisition/discovery errors now preserve their original cause.

The new deployment tool defaults to read-only inspection, with explicit administrator, identity, hash, mutex and eligibility checks. Deploy/Rollback paths are authored and syntax-checked but not executed. The compact UI requires its own clean Windows publish and bounded fresh-artifact GUI checks; prior screenshots do not certify this new layout or native title bar. No service/hardware/security changes occurred.
