# Revision-6 deployment and rollback

Development does not replace the installed service. Run `tools/prepare_backend6.ps1` to build the release candidate, hash-check the installed revision-5 executable/PDB pair, and copy both to an ignored staging directory. It stops before service changes and emits a concrete deployment plan with candidate and backup hashes.

Before deployment, an administrator must run the staged candidate with `--inspect-controls` and review its JSON. This is read-only and does not bind the service pipe. Require the reviewed identity, three valid power presets, recognized Balanced/Cool state, and eligible selected-zone lighting. A normal unelevated process can fail to access the driver and CISD WMI; that is not evidence that the firmware lacks these controls.

For installation, keep the service stopped while replacing executable and symbols. Verify every source and installed hash against the staged plan. If the release has no PDB, preserve the old PDB in the backup and remove it from the installed pair rather than leaving misleading symbols. Never run a partially replaced pair. Start the service only after checking the complete installed pair and confirm protocol revision 6 through its pipe.

If installation or acceptance fails, stop the service, restore the exact revision-5 executable and PDB from the hash-checked backup, verify both, restart, and confirm revision 5. Do not guess a rollback source or overwrite an unknown installed binary.

Live acceptance proceeds one control at a time: capture native power8/fan94 and LED fields; select a power preset and restore the original register; apply Cool then Balanced and compare the readable projection; edit one eligible light then restore its original role/readable values. Check response byte counts. Test reconnect and resume through fresh reads. Never interpret these checks as recovery of the three unreadable fan writer-tail fields, and never retry the excluded fan experiments.

Keep all machine-local paths, logs, deployment JSON, and captures in ignored `runtime/`. The source repository contains no installed binaries or private deployment paths.


## Reviewed deployment entry point

`tools/deploy_backend6.ps1 -PlanPath <staged deployment-plan.json>` defaults to **Inspect**. It requires an already elevated administrator shell, the exact service path/LocalSystem account and all staged/installed/rollback hashes. It runs the candidate's read-only `--inspect-controls`, saves elevated-preflight.json, requires reviewed identity and eligible control discovery, and confirms the existing pipe reports protocol5. It does not self-elevate or mutate the service in Inspect mode.

Only after reviewing that concrete result and obtaining deployment authorization, run the same command with `-Action Deploy`. A named mutex excludes concurrent deployment; the service is stopped before copying, source hashes are checked again at the copy boundary, the installed executable/PDB pair is verified before start, and the restarted PID/path and bounded pipe readiness are checked. Failure attempts exact revision5 restoration and reports the observed service state if rollback also fails. `-Action Rollback` requires the installed candidate's exact hashes and restores the recorded revision5 pair. Never edit hashes to force an unknown installation through these gates.

This script has been authored and syntax-checked, not executed for deployment. Stop/start/copy and physical acceptance remain pending authorization. A changed backend build requires a newly staged plan and symbol validation; do not use an older candidate hash for new source.

The current real revision5 pipe responds and reports connected driver/WMI and readable native power/fan state under LocalSystem. Unelevated candidate inspection sees access restrictions. Those restrictions are not unsupported hardware evidence; the controller now preserves COM/WMI connection errors and power-discovery errors instead of substituting instance-not-found/invalid-table messages. The desktop offers revision5 connection details read-only, while withholding legacy telemetry/rounded power values and every mutation until protocol6.
