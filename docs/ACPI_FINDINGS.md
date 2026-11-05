# Captured ACPI firmware findings

Static analysis on 5 November 2026 explains two LED API inconsistencies and
narrows the remaining fan and automatic-indicator questions. No firmware method
was executed for this analysis and no firmware or hardware setting was changed.

## Evidence and scope

Windows' cached ACPI registry supplied one DSDT and twelve SSDTs, all with valid
ACPI length/checksum fields. The relevant DSDT is 390006 bytes, OEM INTEL /
NUC11PHB, OEM revision `0x50`, SHA-256:

```
d5946858487dd7adb70b52ca679105b98e01586e0e0b3f9ba06acd2c61b65e26
```

ACPICA iASL 20260408 successfully disassembled that table individually. A prior
combined-namespace attempt failed with duplicate-name errors and is not claimed
as successful analysis. The referenced single-table DSL output has SHA-256:

```
38f8f5fbf0e9dc4b26871359d4a52832990eb055964b673692df0a3ff838cf8b
```

The retained raw tables and disassembly are local investigation artifacts, not
part of the public repository. These findings concern the captured boot table,
not every BIOS revision or a complete SPI/UEFI image. Checksum validity is not
firmware authenticity verification.

## Software brightness is advertised but unimplemented

The WMI method chain is `WMAA -> DIPH -> M003/M004/M006` for capability query,
getter, and setter respectively. M003 advertises Software item mask `0x3f`
(DSL lines 79551–79554), claiming indices 0 through 5.

However, M004's Software branches implement only indices 1 through 5. Item 0
falls through to status `0xE4` (228). This occurs for all five present zones;
the skull branch is at lines 80185–80228. M006's corresponding setter likewise
implements only indices 1 through 5 and rejects brightness index 0; see the
skull branch at lines 81548–81572.

This explains the live getter failures without another write experiment.
Activating Software mode cannot create the missing handler. Legacy
`GetLedStatus` also supplies no alternative in this table: its WMAA route calls
an empty ECTM handler and returns `0xE2`.

Software brightness should therefore be unavailable on this implementation.
Do not manufacture a value from UI defaults. PowerState brightness remains a
separate, implemented control. The [per-field API](LED_FIELD_READBACK.md) preserves
the failure while retaining readable RGB values.

## Front3 Software blue targets the wrong zone

M006's Front3 Software blue setter (`zone=6, mode=4, item=5`) writes `R51A`
(DSL line 82082), while its matching getter reads `R55A` (line 80999).
`R51A` is Front1's shared blue field. Front3's PowerState and Power Limit blue
setters use `R55A` as expected.

This is a static cross-zone setter defect; it was not reproduced on hardware.
The backend rejects the affected Software-only RGB plan and exact restore
before issuing any writes, and guards the individual setter dispatch. The
normal PowerState RGB path remains available. It does not attempt to compensate
by writing another zone.

## Fan tail and automatic Power Limit indication

The device's `_CRS` resource maps `0xFE410400` for `0x100` bytes. Combining that
with the recovered driver write mapping places its unreadable tail at
`0xFE4104E0..0xFE4104E2`. No named reader or semantics for those locations were
established in the inspected DSDT routes.

The FPFM preset response populates one 14-byte record after the response buffer
has been cleared. This explains zero padding in the returned preset, including
its final three bytes. It does not prove the live tail was zero or that writing
zero preserves its original state.

The Power Limit LED methods directly read and write configuration fields in
the H2RA memory region. The inspected configuration path contains no automatic
power/throttle comparator. Its consumer and optical trigger remain unverified;
the green observations cannot establish that the trigger is broken or working.

The fan experiments remain unexecuted after the earlier automatic-review
rejection. Static analysis does not authorize retrying those writes.

Tool references: [ACPICA project](https://github.com/open-acpica/acpica),
[Intel's Windows tool release](https://www.intel.com/content/www/us/en/download/774881/acpi-component-architecture-downloads-windows-binary-tools.html),
and [Windows firmware-table API limitations](https://learn.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-getsystemfirmwaretable).
