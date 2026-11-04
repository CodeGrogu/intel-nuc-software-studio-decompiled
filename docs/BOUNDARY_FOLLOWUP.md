# Boundary follow-up: 5 November 2026

Ethernet is deferred by user decision and is not a current completion requirement.
This follow-up separates observable behavior, source-supported limits, and changes
to the maintained backend. Local raw captures remain outside the public repository.

## Automatic Power Limit indicator

The fresh advertised-getter matrix confirms Front3 is active in mode 5 with
Indication 0 (green-to-red) and brightness 50. Its saved custom color is not a
measurement of the color currently emitted by the automatic scheme.

The first observation used a ten-second wait-loop workload with no settings
changes. The second used the same workload for at most 35 seconds at the board's
own Low Power setting, PL1 25 W. The user observed green throughout both runs.
Both had Windows Performance Limit Flags equal to zero in every sample. High
CPU utilization by itself does not establish package power limiting, especially
when the workload uses wait instructions.

A third run replaces the wait loop with observable scalar arithmetic under the
same 25 W setting, bounded duration, and temperature checks. The user missed its
optical observation and requested a repeat. In that repeat the user confirmed
the light stayed green; all sampled Windows limit flags were again zero.
The automatic power-limit trigger therefore remains unverified. These
experiments do not read an Intel
package-specific PL1/PL2 throttle-status register. Windows' generic counter can
provide corroboration but its zero value does not rule out firmware-level limiting.

The low-power observations leave PL2 60 W and Tau 32 s unchanged. A separate
recovery process verifies restoration to PL1 30 W. There are no fan, LED-field,
BIOS-save, or service-deployment operations in these observations. The raw
eight-byte register is not exposed by this IPC path: the recovery claim is for
the three verified power fields, not an independent full-register comparison.

## Unreadable firmware fields

Software indicator 4 advertises brightness item 0 but returns status 228 for it
on all five present zones. The earlier active-mode experiment also returned 228,
so simply activating Software mode is not a missing test. The fresh matrix again
records 155 successful advertised item reads and these five failures.

The historical application constructs brightness defaults of 50 or 100. These
are not alternate reads and cannot establish original firmware values. The
replacement must preserve unknown brightness and distinguish it from readable
RGB. In maintained source, `plan_awake_lighting` now omits Software brightness
writes even when advertised. PowerState brightness remains available. Two pure
regressions cover that distinction; this patch has not been deployed to the PC.

The profile writer's last three bytes target mapped offsets `0xE0..0xE2`, beyond
the reader's fixed `0x80..0xDD` window. The recovered complete IOCTL dispatcher
provides no selectable-address reader for them. Buffer enlargement, a different
EC generation, and values from a stock preset do not recover their original live
contents. Their semantics remain unknown; no additional firmware writes were
used to infer them.

## Blocked fan experiments

Offline packet review establishes the intended changes precisely:

| Experiment | Changes relative to captured Balanced |
| --- | --- |
| Quiet | Primary minimum/off thresholds 50 to 55 degrees C; secondary minimum duty 35 to 30% |
| CustomMinDuty | Minimum duties 30/35% to 35/40%; increases airflow minima |
| FanOffDisabled | Configuration bit 7 cleared; disables zero-RPM behavior |

Despite their small intended changes, all use the full-profile writer, including
the unreadable tail. The old script also lacks GPU/stale-sensor checks and
process-independent recovery, and profile readback alone does not establish a
causal RPM response. These are technical findings, not a claim about why the
automatic reviewer rejected the batch.

The earlier rejection was reported only as `blocked by policy`. It remains in
effect. No rejected experiment was retried through another service, executor,
or modified request. Future live work requires a legitimately reviewed narrower
procedure; no complete recovery guarantee or new approval is asserted here.
