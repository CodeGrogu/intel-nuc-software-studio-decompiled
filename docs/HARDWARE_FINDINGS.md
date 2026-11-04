# Curated hardware findings

These notes summarize source analysis and 39 recorded local investigation cases.
The local captures are not part of the public repository. Historical `NSS_*.md`
documents and decompiler names are leads, not stronger evidence than the corrected
observations below. Results concern the tested Phantom Canyon system; other
firmware and models require their own validation.

## Verified observations

- Five RGB zones were mapped: power button, skull, then front indicators from
  left to right. Color, brightness, disable mode, and software RGB effects were
  physically checked. Breathing, pulsing, and strobe required an apply/save call;
  frequency 10 produced visibly faster or repeated effects. Readback alone did
  not establish that the emitted light changed.
- Disk activity was physically visible on the far-left front indicator. The
  next indicators were configured for Ethernet activity and power-limit status.
- A temporary 25 W PL1 setting was restored to 30 W; the other power bytes were
  preserved. Cool preset fan RPM increased in telemetry; Balanced's 94 readable
  profile bytes were restored. The first populated fan record affected both fans;
  an independently controllable GPU curve was not established.
- A UI-initiated standby produced S0 sleep/resume events. An earlier API suspend
  attempt produced S4 instead. A user-confirmed full power cycle was followed by
  matching power bytes, readable fan bytes, and compared LED settings.

## Corrected protocol facts

The observed device interface GUID was
`{1ACD3A40-C4DB-4542-9C73-9898B0D8FF6E}`.

| Operation | IOCTL |
| --- | --- |
| Status read | `0x9c406044` |
| Power read | `0x9c406010` |
| Fan-profile read | `0x9c406020` |
| Fan-speed read | `0x9c406024` |
| Temperature read | `0x9c406048` |
| Power write | `0x9c40a014` |
| EC22 fan-profile write | `0x9c40a01c` |
| Fixed fan duty write | `0x9c40a040` |

Power access used PCI-derived MMIO. Managed method names containing `MSR` are not
evidence that the driver executes RDMSR. Intel service metadata was reachable on
HTTP ports 43564, 43565, 43566, and 43568; 23 typed WCF reads succeeded. HTTP.sys
ownership can appear under PID 4, so process-only listener filtering was misleading.

## Remaining boundaries

- Ethernet activity validation is deferred and outside the current project scope
  by user decision. It is not a completion requirement. Its physical response
  remains unverified; enumeration and configuration do not prove that effect.
- The fan writer includes three bytes outside the known 94-byte readable window.
  No accepted reader exposed those bytes; a complete byte-for-byte recovery claim
  would be unsupported. The mapped offsets are `0xE0..0xE2`; increasing the
  output buffer cannot extend the reader's fixed `0x80..0xDD` source window.
- Software mode advertises brightness item 0, but its getter returns status 228
  on all five present zones, including an earlier active-mode test. The other
  155 advertised item reads succeed. Historical UI defaults of 50/100 are not
  recovered values. Software RGB edits must preserve this unknown field; the
  maintained backend now omits brightness writes in its Software fallback.
- Additional Quiet/custom-minimum/fan-off write experiments were not executed
  after automatic tool review rejected that batch. Offline review establishes
  that CustomMinDuty raises minimum duties (30/35% to 35/40%) and FanOffDisabled
  disables zero-RPM behavior; neither requests that fans stop. Nevertheless,
  all three cases use the full profile writer, including the unreadable tail.
  They remain physically unvalidated, and the rejected batch was not retried.
- Tested IGCL initialization returned platform-not-supported; the working NVIDIA
  telemetry route does not establish support for those IGCL operations.
- Captured zero fan bounds reproduce a pure Rust validation/panic boundary. The
  repaired backend rejects that input before writes and skips unverified LED
  replay. This evidence does not conclusively identify the trigger of historical
  crashes whose original triggering stacks were not retained.

Offline fixtures and tests preserve these boundaries. CI does not expand them
into claims about live hardware, complete firmware restoration, or binary parity.

See the [5 November boundary follow-up](BOUNDARY_FOLLOWUP.md) for the latest
power-light observations, field-preservation safeguard, and fan-packet review.
