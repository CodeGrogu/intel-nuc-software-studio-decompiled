# Control panel protocol revision 6

Transport remains the local-only `Nuc11PhStudioV1` pipe, with four-byte little-endian length-prefixed JSON, maximum 256 KiB. Revision-5 and earlier request encodings and the legacy snapshot ABI remain compatible. The never-deployed revision-6 preview now requires admission metadata; an older preview client fails closed until updated with its backend.

| Request | Response | Purpose |
| --- | --- | --- |
| `GetControlSessionInfo` | `ControlSessionInfo` | Current backend instance and admission epoch; no hardware gate |
| `GetControlPanelState` | `ControlPanelState` | Reviewed identity, firmware power table, exact power readback, stock fan state, LED mask and eligibility reasons |
| `GetTelemetryStatus` | `TelemetryStatus` | Nullable readings, source, acquisition time and error; genuine zero stays zero |
| `GetAwakeLighting { zone_id }` | `AwakeLighting` | Independently read active role and per-field PowerState results |
| `ApplyControl { operation_id, backend_instance, admission_epoch, mutation }` | `OperationStatus` | Admit a Power, stock Fan, or selected-zone Lighting patch |
| `GetOperationStatus { operation_id }` | `OperationStatus` or error | Ungated lookup of retained history; an unknown ID alone is not a recovery fence |
| `ReconcileOperation { operation_id, backend_instance, admission_epoch }` | `OperationStatus` | Ungated lookup or fence of the original admission envelope before fresh readback |

The wire uses Rust's externally tagged enums and snake_case fields. Unit requests are JSON strings. Fetch `GetControlSessionInfo`, retain that response with the operation ID, and use its actual values in a request such as:

```json
{"ApplyControl":{"operation_id":"example-1","backend_instance":"instance-from-session-info","admission_epoch":1,"mutation":{"Fan":{"preset":"Cool"}}}}
```

Lighting submits the exact observed `AwakeLightingState` as `expected`, optional brightness, and optional three-element RGB array. Fresh state must match before writes. Only changed awake values and the selected zone's active role are written. Stored frequency and sleep fields are not supplied by the frontend. PowerState RGB aliases the same zone's other role colors in firmware; this is selected-zone scope, not independent color storage for each role.

`ControlPanelState.power_readback` preserves `pl1_eighth_watts`, `pl2_eighth_watts`, the seven-bit `tau_code`, and nullable `tau_seconds`. Unknown Tau codes are never guessed. The convenience `power_limits` triple is null if the raw values cannot be represented exactly. The desktop matches presets against exact readback, displays fractional watts and unknown time windows, and labels nonmatching values Another setting. Legacy decoding remains unchanged for existing clients. Enable/clamp bits are preserved by existing setters; preset selection describes the stored limit fields, not proof that all firmware enforcement bits are enabled.

## Admission, completion and recovery

Operation IDs are 1-64 ASCII alphanumeric/hyphen characters. Immediately after a complete request is decoded, the service atomically validates its instance/epoch and registers Pending **before** waiting for a worker or hardware gate. Retained duplicates with identical mutations return existing status; conflicting content or instance cannot admit a second execution. Capacity is bounded at 128 entries. Pending entries are never evicted. Saturation rejects before acceptance; terminal-history eviction and rejection at capacity close the current epoch under the same registry mutex.

Epochs advance monotonically without wrapping. New work requires the current epoch. Already accepted work continues even after that epoch closes; execution does not revalidate admission and silently drop queued writes. The terminal outcomes are Verified, Rejected (before writes), PartialFailure, or Uncertain. Cancellation/disconnection does not cancel accepted firmware execution.

`ReconcileOperation` always carries the **original** instance, epoch and ID. It returns a retained Pending/terminal status, or atomically fences a missing/expired ID by closing its original epoch. An already closed epoch needs no second rotation. A changed backend instance also fences old-instance frames. Such missing history returns Uncertain: it cannot establish whether an earlier operation executed, but the original envelope cannot be newly admitted later. Fresh actual state is still mandatory. Invalid/future epochs fail closed.

This is bounded deduplication for the original instance/epoch/ID envelope, not durable or global exactly-once delivery. Deliberately reusing a forgotten ID with a newly fetched epoch is a new admission attempt. The desktop never does so, never retries a mutation automatically, and never substitutes new metadata into an uncertain command.

The desktop distinguishes connection failure before any frame-write attempt (including access denied) from partial-write/lost-response failures. Proven pre-submission failure clears the pending ID but still requires refresh. An uncertain operation keeps its original metadata and blocks writes. Refresh reconciles **first**, before backend state or telemetry reads; Pending performs no gated reads. After a terminal/fenced result, the client validates response identity and obtains consistent fresh session/state before unlocking. A failed refresh keeps recovery pending. No routine service restart is required for transport uncertainty or history expiry. An actually stuck hardware operation remains Pending until it finishes; fencing does not cancel it.

One service operation gate spans hardware preflight, writes and readback, including service resume callbacks. Order is operation gate, then driver/WMI mutex, then cached state. Admission/session metadata, backend version, status and reconciliation bypass the hardware gate and worker queue. Blocking firmware work uses at most eight concurrent blocking workers; hardware access remains serialized. The gate covers this service, not external vendor clients. Precondition/readback conflicts remain possible and are reported without repeated writes or speculative rollback.

## Evidence and recovery boundary

Controls are eligible only on the reviewed NUC11PHi7 / NUC11PHBi7 BIOS PHTGL579.0080.2025.1118.1734. Firmware power discovery uses PlatformConfigurationAccessV1; invalid tables have no guessed fallback.

Stock Balanced/Cool packets must equal the reviewed numeric fixtures. Current readable state must match either reviewed projection, including preserved gap bytes. Success verifies the 94 readable bytes. Three writer-tail fields have no established reader; success and rollback cannot prove their previous contents. Stock selection invalidates legacy fan replay eligibility.

All existing Software brightness and Front3 Software-blue guards remain active. New lighting controls use PowerState only, require readable steady awake fields, never save to BIOS, and leave unknown states disabled. Unknown brightness/color remain unavailable in the UI; readable zero values remain visible. A selected-zone read invalidates its previous snapshot before awaiting, and stale completions cannot replace a newer selection.
