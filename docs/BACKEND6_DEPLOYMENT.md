# Revision-6 deployment and rollback

## First: move the service to an admin-only folder

The service runs as LocalSystem, so its executable and folder must not be writable by a standard user. `prepare_backend6.ps1` and `deploy_backend6.ps1` refuse a service file or folder that grants write access to anyone but SYSTEM, Administrators or TrustedInstaller. From an administrator PowerShell, run `tools/relocate_service.ps1` (read-only **Inspect** by default) and review its report. Then run `tools/relocate_service.ps1 -Action Relocate`. It copies only the hash-checked revision-5 `nuc11ph-service.exe` and `nuc11ph_service.pdb` into `%ProgramFiles%\NucControl\Backend` with a protected DACL (SYSTEM/Administrators full control, Users read and execute), repoints the service, confirms protocol 5, and sets restart-on-failure recovery actions. If any step fails it restores the old path. It never modifies or deletes the old folder. Delete that folder by hand afterwards, since its DLLs are no longer needed. Then stage again with `prepare_backend6.ps1` so the plan names the new path.

The old folder may also hold a legacy `Nuc11Ph.Studio.exe`. It writes through the pre-revision-6 requests, so it loses all write controls once the revision-6 service is installed. NUC Control is unaffected.

Development does not replace the installed service. Run `tools/prepare_backend6.ps1` to build the release candidate, hash-check the installed revision-5 executable/PDB pair, and copy both to an ignored staging directory. It stops before service changes and emits a concrete deployment plan with candidate and backup hashes.

Before deployment, an administrator must run the staged candidate with `--inspect-controls` and review its JSON. This is read-only and does not bind the service pipe. Require the reviewed identity, three valid power presets, recognized Balanced/Cool state, and eligible selected-zone lighting. A normal unelevated process can fail to access the driver and CISD WMI; that is not evidence that the firmware lacks these controls.

For installation, keep the service stopped while replacing executable and symbols. Verify every source and installed hash against the staged plan. If the release has no PDB, preserve the old PDB in the backup and remove it from the installed pair rather than leaving misleading symbols. Never run a partially replaced pair. Start the service only after checking the complete installed pair and confirm protocol revision 6 through its pipe.

If installation or acceptance fails, stop the service, restore the exact revision-5 executable and PDB from the hash-checked backup, verify both, restart, and confirm revision 5. Do not guess a rollback source or overwrite an unknown installed binary.

Live acceptance proceeds one control at a time: capture native power8/fan94 and LED fields; select a power preset and restore the original register; apply Cool then Balanced and compare the readable projection; edit one eligible light then restore its original role/readable values. Check response byte counts. Test reconnect and resume through fresh reads. Never interpret these checks as recovery of the three unreadable fan writer-tail fields, and never retry the excluded fan experiments.

Keep all machine-local paths, logs, deployment JSON, and captures in ignored `runtime/`. The source repository contains no installed binaries or private deployment paths.


## Reviewed deployment entry point

`tools/deploy_backend6.ps1 -PlanPath <staged deployment-plan.json>` defaults to **Inspect**. It requires an already elevated administrator shell, the exact service path/LocalSystem account and all staged/installed/rollback hashes. It does not self-elevate or mutate the service in Inspect mode.

### Admin-only copy of the staged files

The staging directory under `runtime/completion/` is writable by the standard user who built the candidate. If the script hashed a staged file and then ran or copied it from there, a user-level process could swap the file in between. The swapped file would then run elevated or be installed as the LocalSystem service. So before any hash check, every action does the following:

1. Creates a new folder `%ProgramFiles%\NucControl\deployment-<random id>`. Its protected DACL comes from `New-AdminOnlyDirectorySecurity` in `tools/ServiceSecurity.psm1`: SYSTEM and Administrators get full control, Users get read and execute, and nothing is inherited.
2. Copies the staged candidate `nuc11ph-service.exe` and `nuc11ph_service.pdb` into its `candidate\` subfolder, and the revision-5 backup pair into `revision5\`. `Get-TrustedCopyPlan` maps the staged paths to these copies, and the script reads the staged paths only to make this copy.
3. Makes Administrators the owner of every copied file and subfolder. It stops unless each copy, each subfolder, the folder and its parent pass `Assert-AdminOnlyWritable`.
4. Checks the SHA-256 of the copies against the plan. A file swapped during the copy fails this check.

From then on the script uses only the copies. It runs `--inspect-controls` from the copied candidate and saves `elevated-preflight.json`. It requires the reviewed identity and eligible control discovery, and it confirms that the existing pipe reports protocol 5. Deploy installs from the copied candidate pair, and rollback restores from the copied revision-5 pair. The script removes the copy folder when it exits, whatever the outcome, and leaves the staging directory untouched. If removal fails, it prints a warning that names the folder, and you delete that folder by hand. `tools/tests/ServiceSecurity.Tests.ps1` checks the copy plan. It also parses the deploy script to check that the script reads staged paths only to plan the copy, and makes the copy before any hash check.

Only after reviewing that concrete result and obtaining deployment authorization, run the same command with `-Action Deploy`. A named mutex excludes concurrent deployment; the service is stopped before copying, the admin-only copies are hash-checked again just before they are installed, the installed executable/PDB pair is verified before start, and the restarted PID/path and bounded pipe readiness are checked. Failure attempts exact revision5 restoration and reports the observed service state if rollback also fails. `-Action Rollback` requires the installed candidate's exact hashes and restores the recorded revision5 pair. Never edit hashes to force an unknown installation through these gates.

This script has been authored and syntax-checked, not executed for deployment. Stop/start/copy and physical acceptance remain pending authorization. A changed backend build requires a newly staged plan and symbol validation; do not use an older candidate hash for new source.

The current real revision5 pipe responds and reports connected driver/WMI and readable native power/fan state under LocalSystem. Unelevated candidate inspection sees access restrictions. Those restrictions are not unsupported hardware evidence; the controller now preserves COM/WMI connection errors and power-discovery errors instead of substituting instance-not-found/invalid-table messages. The desktop offers revision5 connection details read-only, while withholding legacy telemetry/rounded power values and every mutation until protocol6.
