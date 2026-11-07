# Stage only. This script never stops a service or replaces an installed executable.
[CmdletBinding()]
param([string]$OutputDirectory)
$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent $PSScriptRoot
if (-not $OutputDirectory) {
    $OutputDirectory = Join-Path $repo ('runtime/completion/backend6-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
}
if (Test-Path -LiteralPath $OutputDirectory) { throw 'Choose a new staging directory' }
$service = Get-CimInstance Win32_Service -Filter "Name='Nuc11PhStudioService'"
if (-not $service) { throw 'Existing service was not found' }
if ($service.PathName -notmatch '^"([^"]+\.exe)"(?:\s|$)') { throw 'Expected a quoted service executable path' }
$installed = $Matches[1]
Import-Module (Join-Path $PSScriptRoot 'ServiceSecurity.psm1') -Force
Assert-AdminOnlyWritable $installed  # Run relocate_service.ps1 first if this refuses.
$expectedExe = 'ff43952f82aaa153bb4eb61022774ddfd0456d810cc80390d9fadd6fd6289725'
$expectedPdb = 'af96a4020e14467a6e5c659f5528d1235cbbfbcfc52cbfad4c0f04e204120cf1'
if ((Get-FileHash -LiteralPath $installed).Hash.ToLowerInvariant() -ne $expectedExe) { throw 'Installed executable is not the reviewed revision 5' }
$installedPdb = Join-Path (Split-Path -Parent $installed) 'nuc11ph_service.pdb'
if (-not (Test-Path -LiteralPath $installedPdb)) { throw 'Reviewed revision-5 symbols are missing' }
if ((Get-FileHash -LiteralPath $installedPdb).Hash.ToLowerInvariant() -ne $expectedPdb) { throw 'Installed symbols are not the reviewed revision-5 pair' }
Push-Location (Join-Path $repo 'backend')
try {
    cargo --config 'profile.release.debug=1' --config 'profile.release.strip=false' build --locked --release -p nuc11ph-service
    if ($LASTEXITCODE) { throw 'Backend build failed' }
} finally { Pop-Location }
$backup = New-Item -ItemType Directory -Path (Join-Path $OutputDirectory 'revision5')
$candidate = New-Item -ItemType Directory -Path (Join-Path $OutputDirectory 'candidate')
Copy-Item -LiteralPath $installed, $installedPdb -Destination $backup.FullName
foreach ($file in Get-ChildItem -LiteralPath $backup.FullName -File) {
    $expected = if ($file.Extension -eq '.exe') { $expectedExe } else { $expectedPdb }
    if ((Get-FileHash -LiteralPath $file.FullName).Hash.ToLowerInvariant() -ne $expected) { throw 'Backup verification failed' }
}
$built = Join-Path $repo 'backend/target/release/nuc11ph-service.exe'
$candidateExe = Join-Path $candidate.FullName 'nuc11ph-service.exe'
Copy-Item -LiteralPath $built -Destination $candidateExe
$candidatePdb = Join-Path (Split-Path -Parent $built) 'nuc11ph_service.pdb'
$symbols = $null
if (Test-Path -LiteralPath $candidatePdb) {
    Copy-Item -LiteralPath $candidatePdb -Destination $candidate.FullName
    $symbols = (Get-FileHash -LiteralPath $candidatePdb).Hash.ToLowerInvariant()
}
if (-not $symbols) { throw 'Candidate symbols missing from the explicitly symbol-enabled build' }
python (Join-Path $PSScriptRoot 'check_backend_symbols.py') $candidateExe (Join-Path $candidate.FullName 'nuc11ph_service.pdb') (Join-Path $OutputDirectory 'candidate-symbols.json')
if ($LASTEXITCODE) { throw 'Candidate executable/PDB identity validation failed' }
$plan = [ordered]@{
    schema = 1; protocol_revision = 6; installed_executable = $installed
    original_executable_sha256 = $expectedExe; original_pdb_sha256 = $expectedPdb
    candidate_executable = $candidateExe
    candidate_executable_sha256 = (Get-FileHash -LiteralPath $candidateExe).Hash.ToLowerInvariant()
    candidate_pdb_sha256 = $symbols; backup_directory = $backup.FullName
    deployed = $false
    checks_remaining = @('Elevated read-only inspection', 'Controlled power restore', 'Stock Cool/Balanced readable restore', 'Selected lighting restore', 'Reconnect/resume reads')
}
$plan | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $OutputDirectory 'deployment-plan.json') -Encoding utf8
Write-Output (Join-Path $OutputDirectory 'deployment-plan.json')
