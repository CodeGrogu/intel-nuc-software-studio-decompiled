# Audit Remediation Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Close every in-repository finding from the 2026-11-07 project audit of `test/nuc-control` @ `36b25ca9` before any protocol-6 deployment.

**Architecture:** The service rejects every legacy write request before worker admission, so `ApplyControl` (identity check, admission, preconditions, readback) is the only write path. Resume replay is off by default. Deployment refuses a service binary whose file or folder is writable by non-administrators, and a new elevated relocation script moves the reviewed binary pair under `%ProgramFiles%` with an admin-only ACL and SCM recovery actions. Desktop, telemetry and documentation fixes follow.

**Tech Stack:** Rust 2021 workspace (tokio, windows-service), C# .NET 10 / WinUI 3 with xUnit (Microsoft.Testing.Platform), PowerShell 7, Python 3.12 `unittest`.

**Spec:** the audit report, reproduced as **Findings** below. It is the requirement list; every finding maps to a task or to **Owner actions**.

## Findings (spec)

| ID | Severity | Finding | Owner |
| --- | --- | --- | --- |
| H1 | High | LocalSystem service binary and its folder (under the user profile) are writable by the interactive user; dozens of DLLs sit beside it (DLL planting). Deploy checks path/account, not ACLs. | Task 5 + owner runs relocation |
| H2 | High | Legacy pipe writes (`SetCustomPower`, `SetFanMode`, `SetFixedFanDuty`, `SetCustomFanCurve`, `CommitLedsToBiosNvram`, `RestoreLedSnapshot`, and the other legacy setters) bypass identity, admission and precondition guards; legacy power allows 50/75 W against the reviewed 34/60 W. | Task 2 |
| H3 | High | `OpenHardwareMonitor.Hardware.WinRing0.sys` is corrupted in the working tree (`MZ`→`mz`); both WinRing0 drivers are tracked in a public repo; legacy roots are skipped by the checker. | Task 1 |
| M1 | Medium | Required checks on `main` omit `Windows desktop`. | Owner actions |
| M2 | Medium | `reapply_on_standby_resume` defaults to true with no config loader; resume rewrites power/fans/LEDs and sends an unsupported CEC wake. | Task 3 |
| M3 | Medium | Release `panic = "abort"` and no SCM failure actions: a crash leaves the service down. | Task 5 |
| M4 | Medium | `notify_waiters()` stores no permit; a stop during hardware init or between pipe loops is lost. | Task 4 |
| M5 | Medium | `Exit()` is blocked while an operation is pending; if the service dies, refresh never clears it. | Task 6 |
| M6 | Medium | Dependabot alerts and private vulnerability reporting are off; `SECURITY.md` points to the latter. | Owner actions |
| M7 | Medium | Public repo carries Intel decompiled code with no license grant (rights risk, not a legal conclusion). | Owner actions |
| L1 | Low | Stale docs: `PRODUCT.md:7`, `desktop/README.md:16`, `README.md` layout table, `backend/README.md:32`, `docs/RELEASING.md:31`, `docs/DESKTOP_VALIDATION.md` (520 vs 540), `desktop/PORTABLE.md:24`. | Task 8 |
| L2 | Low | Dependency PRs #2–#8 open; #8 (`windows` 0.62) fails backend check, #7 cancelled. | Owner actions |
| L3 | Low | Four merged `test/*` branches remain locally and on origin. | Owner actions |
| L4 | Low | Telemetry crate has 0 tests; `apply_control` write paths have no fake-hardware tests. | Task 7 (telemetry); fake-hardware seam is a separate plan |
| L5 | Low | Modern Standby tooltip is appended even when the light is unavailable (`MainWindow.xaml.cs:239`). | Task 6 |
| U1 | — | Uncommitted UI compaction (`MainWindow.xaml`, `MainWindow.xaml.cs`, `DESIGN.md`) builds clean but is uncommitted. | Task 0 |
| X1 | — | The old service folder also holds a legacy `Nuc11Ph.Studio.exe` that uses legacy writes; it loses write controls after H2. | Task 8 (documented) |

## Global Constraints

- Desktop app uses only `GetVerifiedHardwareState`, `ReconcileOperation`, `GetBackendInfo`, `GetControlSessionInfo`, `GetControlPanelState`, `GetLightingSaveState`, `GetLightingMode`, `GetAwakeLighting`, `GetTelemetryStatus`, `GetOperationStatus`, `ApplyControl`. None of these may change behavior.
- `NUC11PH_BACKEND_REVISION` stays 6; the wire format of `IpcRequest`/`IpcResponse` does not change (legacy variants stay deserializable so callers get an `Error`, not a frame error).
- No script in this plan runs against the installed service during implementation. `relocate_service.ps1` and `deploy_backend6.ps1` default to read-only `Inspect`; only the owner runs `Relocate`/`Deploy`, elevated.
- Committed text must not contain absolute user-profile paths (drive letter + `Users` folder; checked by `tools/check_repository.py`) or the owner's account name.
- Commit messages are Conventional Commits and end with `Co-Authored-By: Claude Opus 5.5 <noreply@anthropic.com>`.
- Gates: `cargo fmt --all -- --check`, `cargo test --locked --workspace --lib --tests`, `dotnet test --project NucControl.Tests/NucControl.Tests.csproj`, `python -m unittest discover -s tools/tests -v`, `python tools/check_repository.py`, `./tools/check_powershell.ps1`, and the new `./tools/tests/ServiceSecurity.Tests.ps1`.

## Review Focus

- A legacy **read** request (`Ping`, `GetHardwareState`, `GetTelemetrySnapshot`, `GetLightingZones`, `GetHardwareCapabilities`, `GetLedFieldState`) must still reach the worker and answer; only writes are rejected. Test in Task 2.
- A **stop requested before the pipe loop first awaits** must still stop the service. Test in Task 4.
- An ACL entry that grants write via a **group the user belongs to** (`Users`, `Authenticated Users`, `Everyone`, `INTERACTIVE`) or via **inherited** rights must count as unsafe; a **Deny** entry must not count as a grant. Tests in Task 5.
- **Exit while busy** (an apply still awaiting its reply) stays blocked; only an unresolved pending id may exit after confirmation. Test in Task 6.
- **Unavailable light** with a non-null snapshot (`Unavailable` set) gets no standby note. Test in Task 6.

---

### Task 0: Preserve the owner's pending UI work

**Files:** `DESIGN.md`, `desktop/NucControl.App/MainWindow.xaml`, `desktop/NucControl.App/MainWindow.xaml.cs` (already modified, uncommitted).

- [ ] **Step 1:** Build: `dotnet build desktop/NucControl.slnx` → `0 Warning(s) 0 Error(s)`.
- [ ] **Step 2:** Commit only these three files, as-is, with the owner's approval: `feat(desktop): compact preset headings and 540-DIP window`. Do **not** stage the `.sys` file.

### Task 1: Remove WinRing0 drivers and stop allow-listing them (H3)

**Files:**
- Delete: `decompiled_service/OpenHardwareMonitorLib/OpenHardwareMonitor.Hardware.WinRing0.sys`, `decompiled_service/OpenHardwareMonitorLib/OpenHardwareMonitor.Hardware.WinRing0x64.sys`
- Modify: `tools/check_repository.py` (`LEGACY_BINARIES`)
- Test: `tools/tests/test_publication.py`

- [ ] **Step 1: Failing test** in `PublicationTests`:

```python
def test_kernel_drivers_are_never_publishable(self):
    for path in ["decompiled_service/OpenHardwareMonitorLib/OpenHardwareMonitor.Hardware.WinRing0.sys",
                 "decompiled_service/OpenHardwareMonitorLib/OpenHardwareMonitor.Hardware.WinRing0x64.sys"]:
        with self.subTest(path=path):
            self.assertIsNotNone(publication_error(path))
```

- [ ] **Step 2:** `python -m unittest discover -s tools/tests -v` → FAIL (`None is not not None`).
- [ ] **Step 3:** Remove both `.sys` entries from `LEGACY_BINARIES`. `git rm` both driver files (the working copy of one is corrupted; history keeps the originals).
- [ ] **Step 4:** Unit tests PASS; `python tools/check_repository.py` → `Repository checks passed.`
- [ ] **Step 5:** Commit `fix(repo): stop publishing WinRing0 kernel drivers`.

### Task 2: Reject legacy write requests before admission (H2)

**Files:**
- Modify: `backend/crates/nuc11ph-service/src/pipe_server.rs` (`prepare_request`, `dispatch_request`)
- Test: same file, new `#[cfg(test)] mod legacy_write_regression`

**Interfaces:**
- Produces: `fn is_legacy_write(req: &IpcRequest) -> bool` (private, `pipe_server.rs`); rejection message constant `LEGACY_WRITE_REJECTED: &str = "Legacy write requests are disabled in protocol 6; use ApplyControl."`

- [ ] **Step 1: Failing tests.** `legacy_writes_are_rejected_before_worker_admission`: for each of `SetPowerPreset`, `SetCustomPower { limits: 50/75/28 }`, `SetFanMode`, `SetFixedFanDuty`, `SetCustomFanCurve`, `SetLedZoneVolatile`, `SetAllLedsVolatile`, `SetGlobalLedBrightnessVolatile`, `SetStealthModeVolatile`, `CommitLedsToBiosNvram`, `RestoreLedSnapshot`, `SendHdmiCecCommand`, `ApplyProfileByName`, assert `prepare_request(&req)` matches `Some(IpcResponse::Error { message })` with `message == LEGACY_WRITE_REJECTED`. `legacy_reads_still_reach_the_worker`: `Ping`, `GetHardwareState`, `GetTelemetrySnapshot`, `GetLightingZones`, `GetHardwareCapabilities`, `GetLedFieldState { 2, 4 }` → `prepare_request` returns `None`.
- [ ] **Step 2:** `cargo test --locked -p nuc11ph-service legacy_` → FAIL (writes return `None`).
- [ ] **Step 3:** Add the `is_legacy_write` arm to `prepare_request`; in `dispatch_request` replace those 13 arms with one `unreachable!("Legacy writes are rejected before worker admission")` arm, matching the existing pattern for session requests. Update the module doc comment to say interactive clients may read and may write only through `ApplyControl`.
- [ ] **Step 4:** Full backend gate PASS; `cargo fmt --all -- --check` clean.
- [ ] **Step 5:** Commit `fix(service)!: reject legacy write requests in protocol 6`.

### Task 3: Resume replay off by default (M2)

**Files:**
- Modify: `backend/crates/nuc11ph-protocol/src/config.rs` (`StudioConfig::default`, `HdmiCecSettings::default`); `backend/crates/nuc11ph-service/src/main.rs` header comment (item 3)
- Test: `config.rs`, new `#[cfg(test)] mod resume_defaults`

- [ ] **Step 1: Failing test** `defaults_never_replay_hardware_or_wake_a_display_on_resume`: `let c = StudioConfig::default(); assert!(!c.reapply_on_standby_resume); assert!(!c.hdmi_cec.wake_tv_on_resume);` and `assert_eq!(nuc11ph_service::resume_action(ResumeNotification::Automatic, c.reapply_on_standby_resume, c.hdmi_cec.wake_tv_on_resume), None)` — the last assertion goes in `backend/crates/nuc11ph-service/src/lib.rs` tests since protocol cannot depend on service.
- [ ] **Step 2:** `cargo test --locked -p nuc11ph-protocol resume_defaults` → FAIL.
- [ ] **Step 3:** Set both defaults to `false`. Rewrite main.rs header item 3: power events are observed; hardware replay only runs when explicitly enabled, which no shipped configuration does.
- [ ] **Step 4:** Backend gate PASS.
- [ ] **Step 5:** Commit `fix(service): disable standby resume replay and CEC wake by default`.

### Task 4: Stop requests are never lost (M4)

**Files:**
- Modify: `backend/crates/nuc11ph-service/src/lib.rs`, `backend/crates/nuc11ph-service/src/main.rs:98`
- Test: `lib.rs`

**Interfaces:**
- Produces: `pub fn request_stop(signal: &tokio::sync::Notify)` — calls `notify_one()`, storing a permit for a not-yet-registered waiter.

- [ ] **Step 1: Failing test** `stop_requested_before_waiting_is_observed` (`#[tokio::test]`): `let n = Notify::new(); request_stop(&n); assert!(tokio::time::timeout(Duration::from_millis(50), n.notified()).await.is_ok());`
- [ ] **Step 2:** Test FAILS to compile (`request_stop` undefined). Temporarily implementing with `notify_waiters()` must make it fail on timeout — check once, then switch.
- [ ] **Step 3:** Implement with `notify_one()`; main.rs Stop/Shutdown arm calls `nuc11ph_service::request_stop(&shutdown_for_handler)`.
- [ ] **Step 4:** Backend gate PASS.
- [ ] **Step 5:** Commit `fix(service): keep stop requests that arrive before the pipe loop waits`.

### Task 5: Admin-only service location, deploy refusal and recovery (H1, M3)

**Files:**
- Create: `tools/ServiceSecurity.psm1`, `tools/relocate_service.ps1`, `tools/tests/ServiceSecurity.Tests.ps1`
- Modify: `tools/deploy_backend6.ps1`, `tools/prepare_backend6.ps1`, `.github/workflows/ci.yml` (`powershell` job runs the new test)

**Interfaces:**
- Produces (module `ServiceSecurity.psm1`):
  - `Get-UnsafeWriteGrant -Rules <AuthorizationRule[]>` → array of `"<SID> <rights>"` strings for every **Allow** rule granting any of `Write, Modify, FullControl, ChangePermissions, TakeOwnership, Delete, AppendData, CreateFiles` to a SID outside `S-1-5-18` (SYSTEM), `S-1-5-32-544` (Administrators), `S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464` (TrustedInstaller). Inherited rules count. Deny rules are ignored.
  - `Assert-AdminOnlyWritable -Path <string>` → throws `"User-writable service location: <path>: <grants>"` when `Get-UnsafeWriteGrant` is non-empty for the file **or** its parent directory (rules read with `Get-Acl` and `GetAccessRules($true,$true,[Security.Principal.SecurityIdentifier])`).
  - `$ReviewedServiceDirectory = Join-Path $env:ProgramFiles 'NucControl\Backend'`.

- [ ] **Step 1: Failing tests** in `ServiceSecurity.Tests.ps1` (plain asserts, exits 1 on failure; no Pester dependency). Build rules with `[Security.AccessControl.FileSystemAccessRule]::new([Security.Principal.SecurityIdentifier]'<sid>', '<rights>', 'Allow'|'Deny')`:
  - `S-1-5-32-545` (Users) `ReadAndExecute` Allow → 0 grants.
  - `S-1-5-32-545` `Modify` Allow → 1 grant.
  - `S-1-1-0` (Everyone) `Write` Allow → 1 grant.
  - `S-1-5-4` (INTERACTIVE) `FullControl` Deny → 0 grants.
  - SYSTEM + Administrators + TrustedInstaller `FullControl` Allow → 0 grants.
  - A user SID `S-1-5-21-1-2-3-1001` `FullControl` Allow → 1 grant.
- [ ] **Step 2:** `pwsh -NoProfile -File tools/tests/ServiceSecurity.Tests.ps1` → FAIL (module missing).
- [ ] **Step 3:** Implement the module. In `deploy_backend6.ps1` (all actions) and `prepare_backend6.ps1`, import it and call `Assert-AdminOnlyWritable $installed` right after the service path is resolved.
- [ ] **Step 4:** Implement `relocate_service.ps1 -Action Inspect|Relocate` (default `Inspect`, read-only; requires administrator; same deployment mutex and service-identity checks as deploy). It accepts `-ExpectedExecutableSha256`/`-ExpectedPdbSha256` defaulting to the reviewed revision-5 hashes in `prepare_backend6.ps1`. `Relocate`: create `$ReviewedServiceDirectory`, set a protected DACL (SYSTEM/Administrators FullControl, Users ReadAndExecute, no inheritance), stop service, copy **only** `nuc11ph-service.exe` and `nuc11ph_service.pdb`, verify hashes, `sc.exe config Nuc11PhStudioService binPath= "\"<new exe>\""`, `sc.exe failure Nuc11PhStudioService reset= 86400 actions= restart/5000/restart/30000//`, `sc.exe failureflag Nuc11PhStudioService 1`, `Assert-AdminOnlyWritable` on the new path, start, confirm process path and protocol 5 over the pipe. On any failure after `config`, restore the old `binPath` and start; never delete the old folder. `Inspect` reports current path, unsafe grants, and failure-action config.
- [ ] **Step 5:** Add `- run: ./tools/tests/ServiceSecurity.Tests.ps1` (shell `pwsh`) to the `powershell` CI job. Run the test (PASS), `./tools/check_powershell.ps1` (PASS), `python tools/check_repository.py` (PASS).
- [ ] **Step 6:** Commit `fix(tools): refuse user-writable service paths and add admin-only relocation`.

### Task 6: Desktop exit and standby tooltip (M5, L5)

**Files:**
- Modify: `desktop/NucControl.Core/ControlSession.cs`, `desktop/NucControl.Core/LightingDraft.cs`, `desktop/NucControl.App/MainWindow.xaml.cs` (`Exit`, `RenderDraft`)
- Test: `desktop/NucControl.Tests/RecoveryTests.cs`, `desktop/NucControl.Tests/ControlTests.cs`

**Interfaces:**
- Produces: `public enum ExitBlock { None, Busy, UnresolvedOperation }` and `public static ExitBlock ControlSession.ExitBlockFor(bool busy, string? pendingId)` (busy wins); `public static string LightingDraft.DetailToolTip(LightingState? state, string detail)`.
- Standby note text, unchanged: `" Stored Modern Standby fields are preserved; selecting power-state lighting uses those stored fields."`

- [ ] **Step 1: Failing tests.** `ExitBlockFor(true, null) == Busy`, `ExitBlockFor(true, "id") == Busy`, `ExitBlockFor(false, "id") == UnresolvedOperation`, `ExitBlockFor(false, null) == None`. `DetailToolTip(null, "x") == "x"`; `DetailToolTip(state with Unavailable = "gone", "x") == "x"`; `DetailToolTip(available state, "x") == "x" + note`.
- [ ] **Step 2:** `dotnet test --project NucControl.Tests/NucControl.Tests.csproj` → FAIL (compile).
- [ ] **Step 3:** Implement both. `Exit()` becomes async: `Busy` keeps today's notice; `UnresolvedOperation` shows a `ContentDialog` (via `ShowDialog`) titled `Exit with an unresolved operation?`, content `The last command's outcome is unknown. NUC Control never retries it; the next launch reads actual hardware state.`, primary `Exit anyway`, close `Cancel`, default Close. `RenderDraft` uses `LightingDraft.DetailToolTip(draft.Snapshot, LightDetail.Text)`.
- [ ] **Step 4:** Desktop tests PASS; `dotnet build desktop/NucControl.slnx` 0 warnings.
- [ ] **Step 5:** Commit `fix(desktop): allow exit with an unresolved operation and gate the standby tooltip`.

### Task 7: Telemetry CPU-usage tests (L4, partial)

**Files:** Modify + test `backend/crates/nuc11ph-telemetry/src/lib.rs`.

**Interfaces:** Produces `fn cpu_usage_pct(d_idle: u64, d_kernel: u64, d_user: u64) -> f32` (private); `sample_cpu_usage_pct` calls it after computing deltas.

- [ ] **Step 1: Failing tests:** `(0,0,0) → 0.0`; `(50, 100, 0) → 50.0`; `(0, 60, 40) → 100.0`; idle larger than kernel+user `(200, 100, 0) → 0.0`.
- [ ] **Step 2:** `cargo test --locked -p nuc11ph-telemetry` → FAIL (undefined).
- [ ] **Step 3:** Extract the existing arithmetic unchanged into `cpu_usage_pct`.
- [ ] **Step 4:** Backend gate PASS.
- [ ] **Step 5:** Commit `test(telemetry): cover CPU usage arithmetic`.

### Task 8: Documentation truth pass (L1, X1, H1/H2 docs)

**Files:** `PRODUCT.md:7`, `desktop/README.md:16`, `README.md` layout table, `backend/README.md` (after line 32), `docs/RELEASING.md:31`, `docs/DESKTOP_VALIDATION.md:92`, `desktop/PORTABLE.md:24`, `docs/VENDOR_FEATURE_PARITY.md:3`, `docs/BACKEND6_DEPLOYMENT.md`, `CHANGELOG.md` is release-please owned — do not edit.

- [ ] **Step 1:** Apply: PRODUCT excludes only custom fan curves, Ethernet and automated profiles (lighting functions/effects and the all-lighting BIOS save request ship behind `ApplyControl`). desktop/README: the UI uses only revision-6 `ApplyControl` writes, including the acknowledged-not-verified BIOS save; never legacy profile/fan/save requests. README: add `desktop/` row (`WinUI 3 portable frontend, NucControl.Core and tests`). backend/README: add revision 6 (control-panel reads, admitted `ApplyControl`, legacy writes rejected). RELEASING: Dependabot also covers NuGet. DESKTOP_VALIDATION: 440×540. PORTABLE: custom power and additional cooling sit in the section headings. VENDOR_FEATURE_PARITY: other devices fail closed for every write because legacy writes are rejected. BACKEND6_DEPLOYMENT: new first step "Relocate the service" (`relocate_service.ps1 -Action Inspect`, then `-Action Relocate` elevated, re-stage with `prepare_backend6.ps1`), and a note that the legacy `Nuc11Ph.Studio.exe` loses write controls under protocol 6.
- [ ] **Step 2:** `python tools/check_repository.py` PASS.
- [ ] **Step 3:** Commit `docs: align product, deployment and validation docs with protocol 6`.

### Task 9: Whole-branch verification

- [ ] Run every gate in **Global Constraints**; record counts (Rust tests, C# tests, Python tests) against the audit baseline 53 / 45 / 6, which must only grow.
- [ ] `git status --short` shows nothing but `.claude/`.
- [ ] Fresh reviewer pass over the branch diff (superpowers:requesting-code-review).

## Owner actions (outside the repository; not executed by this plan)

1. **Run relocation elevated** (H1): `tools/relocate_service.ps1 -Action Inspect`, then `-Action Relocate`; re-run `prepare_backend6.ps1` so the staged plan names the new path. Afterwards the old profile folder (legacy Studio app + DLLs) can be deleted by hand.
2. **Branch protection** (M1): add `Windows desktop` to required checks on `main`.
3. **Security reporting** (M6): enable Dependabot alerts and private vulnerability reporting.
4. **Rights** (M7): decide between keeping `decompiled_*` public, removing it, or making the repo private.
5. **Dependency PRs** (L2) and **merged `test/*` branches** (L3): merge/close and delete.
6. **Separate plan** (L4 remainder): a driver/WMI trait seam so `apply_control` write paths run against fake hardware.
