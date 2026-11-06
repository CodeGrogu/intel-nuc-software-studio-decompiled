# NUC Control implementation validation

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
- Clean Windows runtime smoke test; a Windows Sandbox attempt is tracked locally, and absence of a completed result is not a pass.
- Keyboard navigation through every dialog, tray/single-instance/startup integration, and full light/dark/high-contrast plus 150/200% scaling coverage.

The installed service remains revision 5. This implementation is a reviewable candidate, not a claim that live hardware or clean-machine release acceptance has passed. The three unreadable fan writer-tail fields remain outside full verification and recovery.

## Expanded-control checkpoint, 6 November 2026

- Advanced-lighting commit `4bd8cf60` passed all four clean Windows CI jobs, including desktop publication. This provides successful build evidence despite the local XAML intermediate DLL access/sharing issue.
- The following software slice implements custom power with exact raw-register precondition, dedicated reviewed fixed70 cooling, explicit global BIOS-save acknowledgement, theme selection and isolated demo single-instance/preferences.
- 39 C# and 52 Rust tests pass locally for this slice. Coverage includes firmware-derived custom bounds, unrelated/raw register changes, fixed70 projection and startup replay exclusion, full-save field completeness, stale inactive fields, and lost save acknowledgement without replay.
- No installed service, hardware settings, host power state or security configuration changed. Expanded-controls commit 95353544 passed all four clean CI jobs (run 42228108551), including Windows desktop publish and backend release build. Native UI acceptance remains separate.


## Local preset checkpoint

43 C# tests and 53 Rust tests pass locally for this slice. The preset batch uses the same production executor in offline tests: stale later-zone preflight, conflicts after an earlier write, and final detection of earlier-zone changes. Core tests cover local storage without hardware calls, fresh explicit apply, duplicate zones/names, firmware binding, guarded fields, skipped Ethernet and unknown-field rejection. Profile-slice Windows CI and demo-only UI inspection must be checked against its final commit; earlier green artifacts do not certify new source. No live hardware/service/security changes are part of these checks.


## Packaged startup defect found during demo acceptance

The e398c7fb clean-CI ZIP passed builds/tests but crashed before opening its demo window (WER 9c7e08d0-ec73-46d5-9e2f-bc3ea9323fd6). Inspection found no physical `Assets/NucControl.ico`, although startup requires it for both AppWindow and the tray. `ApplicationIcon` only embeds the executable icon; WinUI's implicit Content item lacked output/publish-copy metadata. The project now updates that item to copy to both destinations, and Publish.ps1 fails before ZIP creation if the published asset is absent or differs from source. Bounded local startup logging records initialization stages and exceptions without suppressing failures. The corrected artifact requires a fresh clean-CI/demo run; the missing asset is a confirmed defect, while full causal closure awaits that run.


The corrected-icon commit 0d024f15 passed all four CI jobs, but its demo startup log identified an earlier MainWindow XAML load failure. Its ZIP also omitted `App.xbf`, `MainWindow.xbf` and `NucControl.pri`, although these are generated in build output. The SDK's XAML copy target prepares build/AppX output rather than this .NET publish payload. An explicit publish target now includes the SDK-generated XAML destinations and project PRI; publication fails if any required resource is missing/empty. Resource keys were checked against the shipped SDK theme source and were present. This is a second confirmed packaging defect; fresh corrected-artifact runtime validation is still required.
