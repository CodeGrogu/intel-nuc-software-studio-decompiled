# Revision 5 deployment validation

On 5 November 2026, [PR #10](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/pull/10)
was merged and the updated backend was installed on the tested NUC11PHi7 with
explicit user authorization. Source commit `12af07c4` became merge commit
`45a5e889`. The service package version remains 0.1.2; its protocol revision is 5.

The [source CI run](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/actions/runs/41987749106)
passed all three jobs, including 28 Rust regression tests, six publication-policy
tests, and the Windows release build. The executable's CodeView identity matches
the deployed PDB. The prior repaired revision-4 executable and matching symbols
were copied to a separate, hash-verified rollback backup. The older original
executable backup was preserved.

## Live read-only checks

| Check | Observed result |
| --- | --- |
| Backend metadata | Version 0.1.2, protocol revision 5 |
| Software field reports on zones 0, 2, 4, 5, 6 | Advertised mask 63, successful-read mask 62, complete false |
| Software brightness | Status 228 and null value on each zone; readable sibling fields retained |
| Skull PowerState report | All advertised items read successfully; mask 258111 |
| Front3 Power Limit report | All advertised items read successfully; mask 31 |
| Invalid zone selector 255 | Rejected with an IPC error |
| Legacy hardware read requests | Driver/WMI connection and power/fan readback remained available |
| Ordinary client after final restart | All five Software reports succeeded without administrator rights |
| Native power-register comparison | All 8 bytes unchanged |
| Native fan-profile comparison | All 94 readable bytes unchanged |
| LED configuration comparison | Full advertised-getter matrices unchanged |
| Final services | Custom backend, Intel service, and PerformanceDriver running |
| Final power fields | PL1 30 W, PL2 60 W, Tau 32 seconds |

The byte comparisons also checked actual returned lengths, exact read IOCTLs,
selectors, and unique response names. Array allocation length alone was not
accepted as evidence of a complete read.

An initial file-replacement attempt failed on PowerShell 5.1's null-string
argument handling. Recovery restarted the original revision 4 successfully.
The corrected file operation was tested on disposable files before the separate
successful deployment attempt. An intervening launcher preparation failure
stopped before any service change. All attempt records remain local and distinct.
The manual rollback of an already replaced executable was not fault-injected
after the healthy installation; its backups and identities were verified.

## Scope

Deployment verification invoked read-only metadata, capability, field, and native
read handlers. It did not write fan, power-limit, LED, or BIOS settings, load a new
driver, trigger sleep, or execute the rejected fan experiments. Stopping and
restarting the backend did not change the compared readable configuration.

This validates the new report and legacy read behavior on the tested PC. It does
not make unsupported firmware brightness available, establish the automatic
power-light optical trigger, or recover the fan writer's three unreadable tail
bytes. Those limits remain as documented in [ACPI findings](ACPI_FINDINGS.md).

The deployment manifest, matching hashes, attempt logs, native captures, and
rollback command are retained under the ignored local directory
`runtime/completion/revision5-deployment-verified/`. Raw machine evidence and
installed binaries are not redistributed by this documentation.
