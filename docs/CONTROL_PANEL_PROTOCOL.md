# Control panel protocol revision 6

Transport remains the local-only `Nuc11PhStudioV1` pipe, with four-byte little-endian length-prefixed JSON, maximum 256 KiB. Existing request encodings remain compatible.

| Request | Response | Purpose |
| --- | --- | --- |
| `GetControlPanelState` | `ControlPanelState` | Reviewed identity, backend instance, firmware power table, direct power readback, recognized stock fan state, LED mask and eligibility reasons |
| `GetTelemetryStatus` | `TelemetryStatus` | Nullable readings, source, acquisition time and error; genuine zero stays zero |
| `GetAwakeLighting { zone_id }` | `AwakeLighting` | Independently read active role and per-field PowerState results |
| `ApplyControl { operation_id, mutation }` | `OperationStatus` | Power, stock Fan, or selected-zone Lighting patch |
| `GetOperationStatus { operation_id }` | `OperationStatus` | Reconcile an acknowledgement lost while a command executes |

The wire uses Rust's externally tagged enums and snake_case fields. Unit requests are JSON strings. For example:

```json
{"ApplyControl":{"operation_id":"example-1","mutation":{"Fan":{"preset":"Cool"}}}}
```

Lighting submits the exact observed `AwakeLightingState` as `expected`, optional brightness, and optional three-element RGB array. Fresh state must match before writes. Only changed awake values and the selected zone's active role are written. Stored frequency and sleep fields are not supplied by the frontend.

## Completion and lock order

One service operation gate spans preflight, writes and readback for every pipe dispatch and service resume callback. Order is operation gate, then driver/WMI mutex, then cached state. Status lookup bypasses the hardware gate. Blocking firmware work uses at most eight concurrent blocking workers; hardware access remains serialized.

Operation IDs are 1–64 ASCII alphanumeric/hyphen characters. The service remembers the latest 128 operations in memory. Reusing a retained ID with identical content returns its existing status; different content is rejected. Outcomes are Pending, Verified, Rejected (before writes), PartialFailure, or Uncertain. Client cancellation does not cancel firmware execution. Unknown/evicted IDs and service restarts require fresh state, not automatic replay. This is not durable exactly-once delivery.

The gate covers this service, not external vendor clients. Precondition/readback conflicts remain possible and are reported without repeated writes or speculative rollback.

## Evidence and recovery boundary

Controls are eligible only on the reviewed NUC11PHi7 / NUC11PHBi7 BIOS PHTGL579.0080.2025.1118.1734. Firmware power discovery uses PlatformConfigurationAccessV1; invalid tables have no guessed fallback.

Stock Balanced/Cool packets must equal the reviewed numeric fixtures. Current readable state must match either reviewed projection, including preserved gap bytes. Success verifies the 94 readable bytes. Three writer-tail fields have no established reader; success and rollback cannot prove their previous contents. Stock selection invalidates legacy fan replay eligibility.

All existing Software brightness and Front3 Software-blue guards remain active. New lighting controls use PowerState only, require readable steady awake fields, never save to BIOS, and leave unknown states disabled.
