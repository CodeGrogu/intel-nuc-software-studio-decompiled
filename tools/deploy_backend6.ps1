# Reviewable deployment tool. Default action is read-only; do not run Deploy/Rollback without explicit authorization.
[CmdletBinding()]
param([Parameter(Mandatory)][string]$PlanPath, [ValidateSet('Inspect','Deploy','Rollback')][string]$Action='Inspect')
$ErrorActionPreference='Stop'
$plan=Get-Content -LiteralPath $PlanPath -Raw | ConvertFrom-Json
if ($plan.schema -ne 1 -or $plan.protocol_revision -ne 6) { throw 'Unsupported deployment plan' }
if (!(New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) { throw 'Run the reviewed action from an administrator PowerShell; no automatic elevation is performed' }
$deploymentMutex=[Threading.Mutex]::new($false,'Global\NucControl.BackendDeployment')
$held=$false
$trustedRoot=$null
try { $held=$deploymentMutex.WaitOne(0) } catch [Threading.AbandonedMutexException] { $held=$true }
if (!$held) { $deploymentMutex.Dispose(); throw 'Another deployment/inspection is active' }
try {
$serviceName='Nuc11PhStudioService'
$service=Get-CimInstance Win32_Service -Filter "Name='$serviceName'"
if (!$service -or $service.StartName -ne 'LocalSystem' -or $service.PathName -notmatch '^"([^"]+\.exe)"(?:\s|$)') { throw 'Unexpected service identity/account/path' }
$installed=[IO.Path]::GetFullPath($Matches[1])
if ($installed -ne [IO.Path]::GetFullPath($plan.installed_executable)) { throw 'Service path differs from staged plan' }
Import-Module (Join-Path $PSScriptRoot 'ServiceSecurity.psm1') -Force
Assert-AdminOnlyWritable $installed  # Run relocate_service.ps1 first if this refuses.
$installedPdb=Join-Path (Split-Path -Parent $installed) 'nuc11ph_service.pdb'
# Staging under runtime/ is user-writable, so a user process could swap a staged file between a hash check and
# its elevated run or copy. Copy both pairs once into a fresh admin-only folder, hash the copies, and from then on
# run and deploy only those copies. Nothing below reads the staging paths again.
$trustedRoot=Join-Path (Split-Path -Parent $ReviewedServiceDirectory) ('deployment-'+[guid]::NewGuid().ToString('N'))
$trusted=Get-TrustedCopyPlan -CandidateExecutable $plan.candidate_executable -BackupDirectory $plan.backup_directory -TrustedRoot $trustedRoot
Copy-ToAdminOnlyDirectory -TrustedRoot $trustedRoot -CopyPlan $trusted
$candidateExe=$trusted.CandidateExecutable.Destination
$candidatePdb=$trusted.CandidatePdb.Destination
$backupExe=$trusted.BackupExecutable.Destination
$backupPdb=$trusted.BackupPdb.Destination
function Check-Hash([string]$Path,[string]$Expected) {
    if ($Expected -notmatch '^[a-fA-F0-9]{64}$' -or !(Test-Path -LiteralPath $Path -PathType Leaf) -or (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash -ne $Expected) { throw "Hash mismatch: $Path" }
}
Check-Hash $candidateExe $plan.candidate_executable_sha256
Check-Hash $candidatePdb $plan.candidate_pdb_sha256
Check-Hash $backupExe $plan.original_executable_sha256
Check-Hash $backupPdb $plan.original_pdb_sha256
$expectedInstalled=if ($Action -eq 'Rollback') {$plan.candidate_executable_sha256} else {$plan.original_executable_sha256}
$expectedSymbols=if ($Action -eq 'Rollback') {$plan.candidate_pdb_sha256} else {$plan.original_pdb_sha256}
Check-Hash $installed $expectedInstalled
Check-Hash $installedPdb $expectedSymbols
function Read-Protocol {
    $pipe=[IO.Pipes.NamedPipeClientStream]::new('.', 'Nuc11PhStudioV1', [IO.Pipes.PipeDirection]::InOut, [IO.Pipes.PipeOptions]::Asynchronous)
    try {
        $pipe.Connect(1000)
        $body=[Text.Encoding]::UTF8.GetBytes('"GetBackendInfo"')
        $frame=[BitConverter]::GetBytes([int]$body.Length)+$body
        $write=$pipe.WriteAsync($frame,0,$frame.Length)
        if (!$write.Wait(1000)) { throw 'Pipe write timeout' }; $write.GetAwaiter().GetResult()
        function Read-Bytes([int]$Count) {
            $buffer=New-Object byte[] $Count; $offset=0
            while ($offset -lt $Count) {
                $read=$pipe.ReadAsync($buffer,$offset,$Count-$offset)
                if (!$read.Wait(1000)) { throw 'Pipe read timeout' }
                $n=$read.GetAwaiter().GetResult(); if ($n -eq 0) { throw 'Truncated pipe response' }; $offset+=$n
            }
            return ,$buffer
        }
        $length=[BitConverter]::ToInt32((Read-Bytes 4),0)
        if ($length -le 0 -or $length -gt 262144) { throw 'Invalid pipe frame' }
        $reply=[Text.Encoding]::UTF8.GetString((Read-Bytes $length)) | ConvertFrom-Json
        return $reply.BackendInfo.protocol_revision
    } finally { $pipe.Dispose() }
}
if ($Action -ne 'Rollback') {
    $inspection=& $candidateExe --inspect-controls
    if ($LASTEXITCODE) { throw 'Read-only candidate inspection failed' }
    $inspection | Set-Content -LiteralPath (Join-Path (Split-Path -Parent $PlanPath) 'elevated-preflight.json') -Encoding UTF8
    $state=($inspection | Out-String | ConvertFrom-Json).control_panel
    if (!$state.device_eligible -or $state.model -ne 'NUC11PHi7' -or $state.bios -ne 'PHTGL579.0080.2025.1118.1734' -or $state.power_unavailable -or $state.power_presets.Count -ne 3 -or $state.fan_unavailable -or $state.led_zone_mask -ne 117 -or !$state.advanced_lighting_available) { throw 'Read-only eligibility failed; inspect elevated-preflight.json. No service change performed' }
    if ((Read-Protocol) -ne 5) { throw 'Installed backend did not report revision 5' }
    if ($Action -eq 'Inspect') { Write-Output 'Read-only deployment preflight passed; service unchanged'; return }
}
function Stop-ReviewedService {
    if ((Get-Service -Name $serviceName).Status -ne 'Stopped') { Stop-Service -Name $serviceName }
    (Get-Service -Name $serviceName).WaitForStatus([ServiceProcess.ServiceControllerStatus]::Stopped,[TimeSpan]::FromSeconds(20))
}
function Start-ReviewedService([int]$Revision) {
    Start-Service -Name $serviceName
    (Get-Service -Name $serviceName).WaitForStatus([ServiceProcess.ServiceControllerStatus]::Running,[TimeSpan]::FromSeconds(20))
    $running=Get-CimInstance Win32_Service -Filter "Name='$serviceName'"
    $process=Get-CimInstance Win32_Process -Filter "ProcessId=$($running.ProcessId)"
    if (!$process -or [IO.Path]::GetFullPath($process.ExecutablePath) -ne $installed) { throw 'Restarted process path does not match the reviewed executable' }
    $deadline=[DateTime]::UtcNow.AddSeconds(20)
    do {
        try { if ((Read-Protocol) -eq $Revision) { return } } catch { $lastError=$_.Exception.Message }
        Start-Sleep -Milliseconds 200
    } while ([DateTime]::UtcNow -lt $deadline)
    throw "Backend did not become ready at protocol $Revision. $lastError"
}
function Restore-ReviewedBackup {
    Check-Hash $backupExe $plan.original_executable_sha256
    Check-Hash $backupPdb $plan.original_pdb_sha256
    Copy-Item -LiteralPath $backupExe -Destination $installed -Force
    Copy-Item -LiteralPath $backupPdb -Destination $installedPdb -Force
    Check-Hash $installed $plan.original_executable_sha256
    Check-Hash $installedPdb $plan.original_pdb_sha256
}
try {
    Stop-ReviewedService
    if ($Action -eq 'Rollback') { Restore-ReviewedBackup; Start-ReviewedService 5 }
    else {
        Check-Hash $candidateExe $plan.candidate_executable_sha256
        Check-Hash $candidatePdb $plan.candidate_pdb_sha256
        Copy-Item -LiteralPath $candidateExe -Destination $installed -Force
        Copy-Item -LiteralPath $candidatePdb -Destination $installedPdb -Force
        Check-Hash $installed $plan.candidate_executable_sha256
        Check-Hash $installedPdb $plan.candidate_pdb_sha256
        Start-ReviewedService 6
    }
} catch {
    $failure=$_
    try { Stop-ReviewedService; Restore-ReviewedBackup; Start-ReviewedService 5 }
    catch {
        $rollbackFailure=$_
        $actual=(Get-Service -Name $serviceName).Status
        throw "Deployment and rollback failed. Observed service state: $actual. Do not start an unverified pair; inspect staged hashes. Original: $failure. Rollback: $rollbackFailure"
    }
    throw "Action failed; exact revision-5 pair restored and confirmed. $failure"
}
Write-Output "$Action completed and protocol confirmed; hardware acceptance remains separate"

} finally {
    if ($trustedRoot -and (Test-Path -LiteralPath $trustedRoot)) {
        try { Remove-Item -LiteralPath $trustedRoot -Recurse -Force } catch { Write-Warning "Could not remove the admin-only copy folder $trustedRoot. $_" }
    }
    if ($held) { $deploymentMutex.ReleaseMutex() }; $deploymentMutex.Dispose()
}
