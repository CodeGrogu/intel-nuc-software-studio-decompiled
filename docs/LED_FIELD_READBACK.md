# Per-field LED readback

Backend protocol revision 5 adds a read-only request for one zone and one
indicator mode. Clients must inspect `GetBackendInfo.protocol_revision` before
using it. Revision 4 services do not implement this request.

```json
{"GetLedFieldState":{"zone_id":2,"indicator_mode":4}}
```

The response is `LedFieldState` with the selected zone/mode, an
`advertised_items_mask`, a separate `successfully_read_items_mask`, a `complete`
flag, and individual `items`. Each item contains `index`, `firmware_status`,
`value`, and `error`.

For the captured skull Software-mode response, the advertised mask is 63 and
the successful-read mask is 62. Item 0 returns firmware status 228 with a null
value. Items 1 through 5 remain readable; their observed values include real
zeroes, which must remain distinct from null. The full response is incomplete.
The reduced fixture records the capture's source digest without publishing the
raw machine capture.

## Client rules

- A support bit means firmware advertises an item. It does not mean its getter
  succeeded, its setter is safe, or its effect has been physically observed.
- Treat `value` as a reading only when status is zero, a value is present, and
  there is no transport/malformed-response error. Display null as unknown.
- Retain successful fields when a sibling fails. Do not replace unknown values
  with zero, 50, 100, or a cached UI default.
- `complete` means all advertised items were read successfully. It does not
  establish a complete-zone restore snapshot, unadvertised values, or emitted
  light. The existing full-state and restore validity checks remain separate.
  An empty advertised mask contains no values even if `complete` is true;
  clients must check the individual support and read bits before enabling controls.
- This request reads the selected mode's configuration without activating it.
  Do not assume the requested mode is currently active.

Software mode 4 uses brightness item 0, behavior 1, frequency 2, and RGB 3..5.
Power Limit mode 5 uses Indication 0, brightness 1, and custom RGB 2..4. In its
automatic green-to-red scheme, those custom RGB values are configuration, not
a measurement of the light currently emitted. Preserve mode-specific semantics.

## Bounds and effects

Zone and mode selectors are limited to 0..6. The backend verifies that the zone
is present and the mode is advertised, then reads only advertised item indices
0..23. Invalid selectors fail before WMI access; unsupported selections and
capability-query failures return an error. Individual firmware, transport, and
malformed-response failures are retained while other advertised items are read.

The operation invokes only `QueryLed` and `NewGetLedStatus`. It does not switch
indicator modes, modify the hardware-state cache, write settings, save to BIOS,
touch fan controls, or elevate an incomplete result into restore validity.
Existing request and response encodings remain unchanged.

A request has at most three capability queries and 24 item reads. This bounds
the number of calls, not their elapsed time: the existing COM transport remains
synchronous and does not provide a hard per-request wall-clock deadline.

This API makes the known read limitation usable by a client; it does not add a
firmware reader for Software brightness or the fan writer's EC tail. See the
[boundary report](BOUNDARY_FOLLOWUP.md) for the exact remaining evidence limits.

The [captured ACPI implementation](ACPI_FINDINGS.md) confirms that Software
brightness is advertised but unimplemented in both getter and setter. It also
identifies a Front3 Software-blue setter targeting Front1. Successful reads
must never be interpreted as permission to issue those writes; the backend
guards the affected setter and uses the normal PowerState alternative for RGB.
