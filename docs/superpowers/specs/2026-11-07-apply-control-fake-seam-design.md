# `apply_control` fake-hardware seam — design

Audit finding L4 (remainder), from `docs/superpowers/plans/2026-11-07-audit-remediation.md`.

## Intent

`PhantomHardwareController::apply_control` (`backend/crates/nuc11ph-hw/src/control_panel.rs`)
is the service's only write path in protocol 6. None of its eight write branches has a test,
because the controller holds the concrete `PerformanceDriver` and `CisdWmiClient` types and reads
the device identity from the registry. The goal is the smallest seam that lets every branch run
against in-memory fakes, so the outcome contract can be tested:

- `Rejected`: nothing was written. The fake records no write.
- `PartialFailure`: a write was attempted and its postcondition did not hold.
- `Verified` / `Acknowledged`: an exact readback, or a firmware acknowledgement (save).

Constraints:

- No change to behavior, the IPC wire format, or the IOCTL / CISD byte encodings.
- No test may reach real hardware, the ISA bus mutex, the registry or the installed service. That
  includes the controller's lazy reconnect paths.
- `cargo build --locked --release -p nuc11ph-service` stays at 0 warnings, so production code has
  no test-only items.

## Chosen approach

There are three injection points. Each one sits at the lowest level where the code above it is
pure logic.

### 1. `DriverOps` trait (driver MSR/EC reads and writes)

`pub trait DriverOps: Send + Sync` lists the `PerformanceDriver` operations exactly as they are
today:

- `check_driver_status`
- `read_power_limit_msr` / `write_power_limit_msr`
- `read_ec_fan_profile` / `write_wmi_fan_profile_nuc22`
- `read_fan_speeds_in_section` / `set_fixed_fan_duty`
- `read_ec_temperatures`

`read_fan_speeds` and `set_reviewed_fixed70` become provided methods with their current bodies.
`PerformanceDriver` implements the trait by moving its existing method bodies, unchanged, from
the inherent `impl` into `impl DriverOps for PerformanceDriver`. `open()` stays inherent.

The controller stores `Mutex<Option<Box<dyn DriverOps>>>`, and `with_driver` hands closures a
`&dyn DriverOps`. Existing call sites (`|d| d.read_ec_temperatures()`) compile unchanged, because
a trait object's own methods resolve without an import.

The seam is at method level rather than raw IOCTL level, for two reasons. The methods are already
thin typed wrappers over a single IOCTL each. A byte-level seam would also force the `unsafe`
struct-pointer marshalling to be rewritten, which risks changing behavior.

### 2. `CisdTransport` trait (CISD WMI exec/query)

`pub(crate) trait CisdTransport: Send + Sync` has two methods:
`exec(method, cmd) -> Result<Vec<u8>, WmiError>` (the COM `ExecMethod` + `Data.Bytes` extraction,
today's body of `exec_cisd_method_raw` after validation and locking) and
`query_acpi_thermal_zone_temp_c() -> Option<u8>`.

The COM code moves into `ComCisdTransport { services, instance_path }`. `CisdWmiClient` keeps its
allowlist check (`validate_cisd_led_write`, still run before the lock), its `wmi_lock`, the staged
LED cache and all decoding logic. It holds a `Box<dyn CisdTransport>`.

The seam is at byte level because `CisdWmiClient` itself has real logic (reply status checks, mask
decoding, field collection, the Front3 write block). The tests should exercise that logic, and the
fake should see the real CISD command words.

### 3. `Platform` function table (identity and reconnect)

`pub(crate) struct Platform { identity, open_driver, connect_wmi }` holds plain `fn` pointers.
`Platform::NATIVE` points at today's `identity()` registry read, `PerformanceDriver::open` and
`CisdWmiClient::connect`.

- `initialize()` becomes `from_backends(Platform::NATIVE, (open_driver)(), (connect_wmi)())`, with
  the rest of its body unchanged.
- `with_driver` / `with_wmi` reconnect through `self.platform`.
- Every `identity()` call in `control_panel.rs` becomes `self.identity()`.

Tests build a controller with `from_backends(test_platform, Ok(fake_driver), Ok(fake_wmi))`. In the
test platform, `open_driver` and `connect_wmi` always fail, so no test path can open the real
device.

## Fakes (test-only, `#[cfg(test)] mod fake`)

The fakes share state through `Arc<Mutex<…>>`, so a test can keep a handle after boxing them into
the controller.

**`FakeDriver`** holds:

- an MSR register: a write sticks unless `ignore_power_writes` is set.
- a 94-byte EC fan profile: after any fan write, it becomes `fan_after_write`, a readback the test
  scripts, if one is set. Otherwise it is unchanged.
- an ordered `writes: Vec<DriverWrite>` log: `PowerLimitMsr(u64)`, `FanProfile([u8; 90])` and
  `FixedFanDuty { fan, duty }`.

**`FakeCisd`** emulates the firmware replies that the controller decodes:

- `PlatformConfigurationAccessV1`: the turbo table, and the Balanced and Cool stock packets.
- `QueryLed`: zone, indicator and control masks.
- `NewGetLedStatus`: the active role, and per-item `(status, value)`.
- the two LED setters, which update the model unless `ignore_led_writes` is set.
- `AppNotify`.

Setter and `AppNotify` calls go into `writes: Vec<(String, u32)>`. An unknown method or command
returns an error. The default model is the reviewed BIOS 0080 lighting layout:

- zone mask `0x75`, and indicator mask `0x77` on zones 0, 2, 4, 5 and 6.
- reviewed field masks per mode.
- Software item 0 returns status 228.

The fixtures are data that already exists in the repo: `stock_fixtures::{BALANCED, COOL}`,
`tests/fixtures/test036-fan-profile.bin`, and a turbo table built from the documented 35-byte
record layout.

## Tests (`#[cfg(test)] mod apply_control_tests` in `nuc11ph-hw`)

| Branch | Verified / Acknowledged | Rejected, no write | PartialFailure |
|---|---|---|---|
| any (unreviewed device) | — | all 8 variants | — |
| CustomPower | exact MSR readback | PL1 above firmware maximum | MSR write ignored |
| Power | preset applied, exact readback | preset missing from firmware table | MSR write ignored |
| Fan | stock projection read back | unrecognized fan baseline | readback differs |
| FixedFan70 | fixed70 projection read back | baseline not Balanced | readback differs |
| Lighting | awake RGB read back | stale `expected` snapshot | LED writes ignored |
| AdvancedLighting | field write read back | — | LED writes ignored |
| LightingPreset | two zones read back | — | — |
| SaveLighting | `Acknowledged`, one `AppNotify` | stale snapshot | — |

Each test also asserts the exact write log (method or IOCTL, plus value), which pins the wire
encoding the branch produces.

## Out of scope

- Legacy setters (`set_custom_fan_curves`, `set_fixed_fan_duty`, the volatile LED setters, resume
  replay). The seam makes them testable later.
- Telemetry tests.
- Any change to outcomes or messages. If a test exposes a defect, it is recorded as a follow-up
  and not fixed here.
