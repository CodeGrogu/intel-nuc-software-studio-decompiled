# Moves the reviewed LocalSystem service binary pair to an admin-only folder and sets SCM recovery.
# Default action is read-only. Run Relocate only from an administrator PowerShell, deliberately.
# Only nuc11ph-service.exe and nuc11ph_service.pdb are copied; the old folder is never modified.
[CmdletBinding()]
param(
    [ValidateSet('Inspect', 'Relocate')][string]$Action = 'Inspect',
    [ValidatePattern('^[a-fA-F0-9]{64}$')][string]$ExpectedExecutableSha256 = 'ff43952f82aaa153bb4eb61022774ddfd0456d810cc80390d9fadd6fd6289725',
    [ValidatePattern('^[a-fA-F0-9]{64}$')][string]$ExpectedPdbSha256 = 'af96a4020e14467a6e5c659f5528d1235cbbfbcfc52cbfad4c0f04e204120cf1'
)
$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'ServiceSecurity.psm1') -Force
if (!(New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) { throw 'Run from an administrator PowerShell; no automatic elevation is performed' }
$deploymentMutex = [Threading.Mutex]::new($false, 'Global\NucControl.BackendDeployment')
$held = $false
try { $held = $deploymentMutex.WaitOne(0) } catch [Threading.AbandonedMutexException] { $held = $true }
if (!$held) { $deploymentMutex.Dispose(); throw 'Another deployment/inspection is active' }
try {
    $serviceName = 'Nuc11PhStudioService'
    $service = Get-CimInstance Win32_Service -Filter "Name='$serviceName'"
    if (!$service -or $service.StartName -ne 'LocalSystem' -or $service.PathName -notmatch '^"([^"]+\.exe)"(?:\s|$)') { throw 'Unexpected service identity/account/path' }
    $current = [IO.Path]::GetFullPath($Matches[1])
    $currentPdb = Join-Path (Split-Path -Parent $current) 'nuc11ph_service.pdb'
    $target = Join-Path $ReviewedServiceDirectory 'nuc11ph-service.exe'
    $targetPdb = Join-Path $ReviewedServiceDirectory 'nuc11ph_service.pdb'
    function Test-Hash([string]$Path, [string]$Expected) {
        (Test-Path -LiteralPath $Path -PathType Leaf) -and (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash -eq $Expected
    }
    function Get-UnsafeReason([string]$Path) { try { Assert-AdminOnlyWritable $Path; $null } catch { $_.Exception.Message } }
    function Set-ServicePath([string]$Executable) {
        $result = Invoke-CimMethod -InputObject $service -MethodName Change -Arguments @{ PathName = "`"$Executable`"" }
        if ($result.ReturnValue -ne 0) { throw "Win32_Service.Change failed with $($result.ReturnValue)" }
    }
    function Wait-ServiceStatus([string]$Status) {
        (Get-Service -Name $serviceName).WaitForStatus([ServiceProcess.ServiceControllerStatus]$Status, [TimeSpan]::FromSeconds(20))
    }
    function Confirm-RunningBackend([string]$Executable) {
        Start-Service -Name $serviceName; Wait-ServiceStatus 'Running'
        $running = Get-CimInstance Win32_Service -Filter "Name='$serviceName'"
        $process = Get-CimInstance Win32_Process -Filter "ProcessId=$($running.ProcessId)"
        if (!$process -or [IO.Path]::GetFullPath($process.ExecutablePath) -ne $Executable) { throw 'Running process path does not match the reviewed executable' }
        $deadline = [DateTime]::UtcNow.AddSeconds(20)
        do {
            try { if ((Read-BackendProtocolRevision) -eq 5) { return } } catch { $lastError = $_.Exception.Message }
            Start-Sleep -Milliseconds 200
        } while ([DateTime]::UtcNow -lt $deadline)
        throw "Backend did not report protocol 5. $lastError"
    }
    $report = [ordered]@{
        service_path = $current
        reviewed_pair = (Test-Hash $current $ExpectedExecutableSha256) -and (Test-Hash $currentPdb $ExpectedPdbSha256)
        unsafe_location = Get-UnsafeReason $current
        target_path = $target
        failure_actions = (sc.exe qfailure $serviceName) -join "`n"
    }
    if ($Action -eq 'Inspect') { [pscustomobject]$report | Format-List; Write-Output 'Read-only inspection; service unchanged'; return }

    if ($current -ne $target) {
        if (!$report.reviewed_pair) { throw 'Installed executable/symbols are not the reviewed pair; nothing changed' }
        $null = New-Item -ItemType Directory -Force -Path $ReviewedServiceDirectory
        Set-Acl -LiteralPath $ReviewedServiceDirectory -AclObject (New-AdminOnlyDirectorySecurity)
        if ((Get-Service -Name $serviceName).Status -ne 'Stopped') { Stop-Service -Name $serviceName }
        Wait-ServiceStatus 'Stopped'
        $pathChanged = $false
        try {
            Copy-Item -LiteralPath $current -Destination $target -Force
            Copy-Item -LiteralPath $currentPdb -Destination $targetPdb -Force
            foreach ($copied in @($target, $targetPdb)) {
                $fileAcl = Get-Acl -LiteralPath $copied
                $fileAcl.SetOwner([Security.Principal.SecurityIdentifier]::new('S-1-5-32-544'))
                Set-Acl -LiteralPath $copied -AclObject $fileAcl
            }
            if (!(Test-Hash $target $ExpectedExecutableSha256) -or !(Test-Hash $targetPdb $ExpectedPdbSha256)) { throw 'Copied pair failed hash verification' }
            Assert-AdminOnlyWritable $target
            Set-ServicePath $target; $pathChanged = $true
            Confirm-RunningBackend $target
        } catch {
            $failure = $_
            try {
                if ((Get-Service -Name $serviceName).Status -ne 'Stopped') { Stop-Service -Name $serviceName; Wait-ServiceStatus 'Stopped' }
                if ($pathChanged) { Set-ServicePath $current }
                Confirm-RunningBackend $current
            } catch { throw "Relocation and restore both failed; inspect the service path manually. Original: $failure. Restore: $_" }
            throw "Relocation failed; original service path restored and running. $failure"
        }
    } elseif ($report.unsafe_location) { throw "Service is at the reviewed path but its ACL is unsafe: $($report.unsafe_location)" }

    sc.exe failure $serviceName reset= 86400 actions= restart/5000/restart/30000/restart/60000 | Out-Null
    if ($LASTEXITCODE) { throw 'Setting service recovery actions failed' }
    sc.exe failureflag $serviceName 1 | Out-Null
    if ($LASTEXITCODE) { throw 'Setting the failure-actions flag failed' }
    Write-Output "Service runs from $target with an admin-only ACL and restart-on-failure recovery. Re-run prepare_backend6.ps1 before deploying protocol 6. The old folder was left untouched."
} finally { if ($held) { $deploymentMutex.ReleaseMutex() }; $deploymentMutex.Dispose() }
