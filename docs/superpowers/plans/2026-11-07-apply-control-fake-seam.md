# `apply_control` Fake-Hardware Seam Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Run every `apply_control` write branch against in-memory fakes, with no change to
behavior or wire format.

**Architecture:** Three injection points:

- a method-level `DriverOps` trait, which `PerformanceDriver` implements;
- a byte-level `CisdTransport` trait under an unchanged `CisdWmiClient`;
- a `Platform` table of `fn` pointers for identity and reconnect.

Test-only fakes live in `#[cfg(test)]` modules of `nuc11ph-hw`.

**Tech Stack:** Rust 2021 workspace (`backend/`), `windows` crate, `thiserror`.

**Spec:** `docs/superpowers/specs/2026-11-07-apply-control-fake-seam-design.md`

## Global Constraints

- Base branch `test/nuc-control`. Work branch `test/apply-control-fake-seam`. Do not push.
- No test touches real hardware, the ISA bus mutex, the registry or the installed service.
- No IPC, IOCTL or CISD byte encoding changes. No outcome or message string changes.
- Gates:
  - `cargo fmt --all -- --check`
  - `cargo test --locked --workspace --lib --tests`, which must be green
  - `cargo build --locked --release -p nuc11ph-service`, which must have 0 warnings
- Commits are Conventional Commits ending with `Co-Authored-By: Claude Opus 5.5 <noreply@anthropic.com>`.
- Committed text has no absolute user-profile paths.

## Review Focus

1. Reconnect paths: a test controller whose fake is missing must never call the real `open`/`connect`. The test platform's `open_driver`/`connect_wmi` return errors (Task 1).
2. Lock order: `CisdWmiClient` must still validate `validate_cisd_led_write` *before* taking `wmi_lock` (Task 1, `front3_blue_write_is_blocked_before_transport`).
3. A rejected mutation may still have *read* hardware. The tests assert only that no *write* was recorded (Task 2).
4. `Fan` originally set `attempted = true` before the in-closure preflight re-read, so a baseline that changed between the two reads was a `PartialFailure` with no write. A follow-up moved the flag to just before the write, so that race is now `Rejected`, matching `FixedFan70` (Task 2, `fan_preflight_change_is_rejected_without_write`).
5. `SaveLighting` success is `Acknowledged`, never `Verified` (Task 2).

---

### Task 1: Seam with no behavior change

**Files:**
- Modify: `backend/crates/nuc11ph-hw/src/driver.rs`: add `DriverOps`, move the inherent methods into `impl DriverOps for PerformanceDriver`.
- Modify: `backend/crates/nuc11ph-hw/src/wmi_cisd.rs`: add `CisdTransport`, `ComCisdTransport`, and `CisdWmiClient::from_transport`.
- Modify: `backend/crates/nuc11ph-hw/src/lib.rs`: add `Platform`, `from_backends`, `identity()`, and boxed driver storage.
- Modify: `backend/crates/nuc11ph-hw/src/control_panel.rs`: change `identity()` to `self.identity()`.
- Create: `backend/crates/nuc11ph-hw/src/fake.rs` (`#[cfg(test)]`).

**Interfaces (produces):**
- `pub trait DriverOps: Send + Sync`, with these methods (signatures as in today's `PerformanceDriver`):
  - `check_driver_status`
  - `read_power_limit_msr` / `write_power_limit_msr`
  - `read_ec_fan_profile` / `write_wmi_fan_profile_nuc22`
  - `read_fan_speeds_in_section` / `set_fixed_fan_duty`
  - `read_ec_temperatures`
  - provided methods: `read_fan_speeds`, `set_reviewed_fixed70`
- `pub(crate) trait CisdTransport: Send + Sync`:
  - `fn exec(&self, method: &str, cmd: u32) -> Result<Vec<u8>, WmiError>`
  - `fn query_acpi_thermal_zone_temp_c(&self) -> Option<u8>`
- `pub(crate) fn CisdWmiClient::from_transport(Box<dyn CisdTransport>) -> Self`
- `#[derive(Clone, Copy)] pub(crate) struct Platform`, with fields:
  - `identity: fn() -> (String, String, bool)`
  - `open_driver: fn() -> Result<Box<dyn DriverOps>, DriverError>`
  - `connect_wmi: fn() -> Result<CisdWmiClient, WmiError>`
  - a `const NATIVE`
- `pub(crate) fn PhantomHardwareController::from_backends(Platform, Result<Box<dyn DriverOps>, DriverError>, Result<CisdWmiClient, WmiError>) -> Self`
- `with_driver<F: FnOnce(&dyn DriverOps) -> Result<R, DriverError>>`
- `fake.rs`:
  - `FakeDriver` (Clone, `Arc<Mutex<FakeDriverState>>`) with `DriverWrite::{PowerLimitMsr(u64), FanProfile([u8; 90]), FixedFanDuty { fan: u8, duty: u8 }}`;
  - `FakeCisd` (Clone) with `writes: Vec<(String, u32)>`;
  - `fake_controller(eligible: bool) -> (PhantomHardwareController, FakeDriver, FakeCisd)`.

- [ ] **Step 1: Write the failing smoke tests** in `fake.rs`:
  - `fake_controller_initializes_from_fakes_only`: `control_panel_state()` reports `device_eligible`, `custom_power_bounds.is_some()`, `fan_preset == Some(Balanced)`, `led_zone_mask == 0x75`. Both write logs are empty.
  - `front3_blue_write_is_blocked_before_transport`: `exec_cisd_method("SetValueIndicatorOptionLedType", pack_wmi_cmd(6,4,5,1))` is `Err`, and the fake's `exec` log is empty.
  - `missing_backends_never_reconnect_to_native`: a controller from `from_backends(test_platform, Err(DeviceNotFound), Err(InstanceNotFound))` returns `Err` from `with_driver`/`with_wmi`.
- [ ] **Step 2:** Run `cargo test -p nuc11ph-hw --lib fake`. Expected: compile error (no `from_backends`).
- [ ] **Step 3:** Implement the seam as listed in Interfaces. Move bodies verbatim. `wmi_lock` stays in `CisdWmiClient` around every `transport` call.
- [ ] **Step 4:** Run `cargo test -p nuc11ph-hw --lib`. Expected: PASS. Then `cargo build --locked --release -p nuc11ph-service`: 0 warnings.
- [ ] **Step 5:** Commit `test(hw): add driver and CISD transport seam with in-memory fakes`.

### Task 2: `apply_control` branch tests

**Files:**
- Create: `backend/crates/nuc11ph-hw/src/apply_control_tests.rs` (`#[cfg(test)] mod`, declared in `lib.rs`).

**Interfaces (consumes):** `fake_controller`, `FakeDriver`, `FakeCisd` and `DriverWrite` from Task 1.

Each test asserts `(OperationOutcome, message)` and the exact write log.

- [ ] **Step 1: Rejection tests.** For each of these, the outcome is `Rejected` and both write logs are empty:
  - `unreviewed_device_rejects_every_mutation_without_writes`: all 8 variants
  - `custom_power_above_firmware_maximum_is_rejected_without_writes`
  - `power_preset_missing_from_firmware_is_rejected_without_writes`
  - `unrecognized_fan_baseline_is_rejected_without_writes`: covers `Fan` and `FixedFan70`
  - `fixed70_requires_balanced_baseline`: from a Cool baseline
  - `stale_lighting_snapshot_is_rejected_without_writes`
  - `stale_save_snapshot_is_rejected_without_writes`
- [ ] **Step 2: Verified tests.**
  - `custom_power_exact_readback_is_verified`: the log is `[PowerLimitMsr(plan_custom_power_write(..))]`.
  - `power_preset_exact_readback_is_verified`
  - `stock_fan_projection_readback_is_verified`: the log is `[FanProfile(COOL)]`.
  - `fixed70_projection_readback_is_verified`: the log is `[FixedFanDuty { fan: 0, duty: 70 }]`.
  - `awake_lighting_readback_is_verified`: the log is the `SetValueIndicatorOptionLedType` words.
  - `advanced_lighting_field_readback_is_verified`
  - `lighting_preset_two_zones_are_verified`
  - `save_lighting_is_acknowledged_not_verified`: the log is exactly `[("AppNotify", pack_wmi_cmd(1,0,0,0))]`.
- [ ] **Step 3: PartialFailure tests.**
  - `custom_power_readback_mismatch_is_partial_failure`: `ignore_power_writes`
  - `power_preset_readback_mismatch_is_partial_failure`
  - `stock_fan_readback_mismatch_is_partial_failure`
  - `fixed70_readback_mismatch_is_partial_failure`
  - `awake_lighting_postcondition_mismatch_is_partial_failure`: `ignore_led_writes`
  - `advanced_lighting_postcondition_mismatch_is_partial_failure`
  - `fan_preflight_change_is_rejected_without_write`
- [ ] **Step 4:** Run `cargo test -p nuc11ph-hw --lib apply_control`. Expected: PASS.
- [ ] **Step 5:** Mutation check, then revert. Remove the MSR readback comparison in the `CustomPower` branch, and confirm that `custom_power_readback_mismatch_is_partial_failure` fails.
- [ ] **Step 6:** Run all gates (Global Constraints) and commit `test(hw): cover apply_control write branches against fake hardware`.
