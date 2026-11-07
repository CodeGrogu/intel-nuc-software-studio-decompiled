# Pure ACL-rule and path tests for the service-location guard, plus a static parse of deploy_backend6.ps1.
# Reads only repository sources and changes nothing.
$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot '../ServiceSecurity.psm1') -Force
$failures = 0
function Rule([string]$Sid, [string]$Rights, [string]$Type = 'Allow') {
    [Security.AccessControl.FileSystemAccessRule]::new(
        [Security.Principal.SecurityIdentifier]::new($Sid),
        [Security.AccessControl.FileSystemRights]$Rights,
        [Security.AccessControl.AccessControlType]$Type)
}
function Expect([string]$Name, [object[]]$Rules, [int]$Count) {
    $actual = @(Get-UnsafeWriteGrant -Rules $Rules).Count
    if ($actual -ne $Count) { Write-Output "FAIL $Name : expected $Count grant(s), got $actual"; $script:failures++ }
    else { Write-Output "ok   $Name" }
}
$trusted = 'S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464'
Expect 'Users read/execute is safe' @(Rule 'S-1-5-32-545' 'ReadAndExecute') 0
Expect 'Users modify is unsafe' @(Rule 'S-1-5-32-545' 'Modify') 1
Expect 'Everyone write is unsafe' @(Rule 'S-1-1-0' 'Write') 1
Expect 'Authenticated Users append is unsafe' @(Rule 'S-1-5-11' 'AppendData') 1
Expect 'INTERACTIVE deny is not a grant' @(Rule 'S-1-5-4' 'FullControl' 'Deny') 0
Expect 'SYSTEM, Administrators and TrustedInstaller are trusted' @(
    (Rule 'S-1-5-18' 'FullControl'), (Rule 'S-1-5-32-544' 'FullControl'), (Rule $trusted 'FullControl')) 0
Expect 'A user account with full control is unsafe' @(Rule 'S-1-5-21-1-2-3-1001' 'FullControl') 1
Expect 'Take ownership is unsafe' @(Rule 'S-1-5-32-545' 'TakeOwnership') 1
$inheritOnly = [Security.AccessControl.FileSystemAccessRule]::new(
    [Security.Principal.SecurityIdentifier]::new('S-1-5-32-545'), [Security.AccessControl.FileSystemRights]'Modify',
    [Security.AccessControl.InheritanceFlags]'ContainerInherit, ObjectInherit',
    [Security.AccessControl.PropagationFlags]'InheritOnly', [Security.AccessControl.AccessControlType]'Allow')
Expect 'Inherit-only rule does not grant on the object itself' @($inheritOnly) 0
function ExpectOwner([string]$Name, [string]$Sid, [bool]$Unsafe) {
    $actual = [bool](Get-UnsafeOwner -Owner ([Security.Principal.SecurityIdentifier]::new($Sid)))
    if ($actual -ne $Unsafe) { Write-Output "FAIL $Name : expected unsafe=$Unsafe"; $script:failures++ } else { Write-Output "ok   $Name" }
}
ExpectOwner 'A user-owned object is unsafe (owner can re-grant itself write)' 'S-1-5-21-1-2-3-1001' $true
ExpectOwner 'A Users-owned object is unsafe' 'S-1-5-32-545' $true
ExpectOwner 'An Administrators-owned object is safe' 'S-1-5-32-544' $false
ExpectOwner 'A SYSTEM-owned object is safe' 'S-1-5-18' $false
ExpectOwner 'A TrustedInstaller-owned object is safe' $trusted $false
$planned = New-AdminOnlyDirectorySecurity
Expect 'Relocation ACL grants no untrusted writer' @($planned.GetAccessRules($true, $true, [Security.Principal.SecurityIdentifier])) 0
if (!$planned.AreAccessRulesProtected) { Write-Output 'FAIL Relocation ACL must not inherit'; $failures++ } else { Write-Output 'ok   Relocation ACL does not inherit' }
if (@($planned.GetAccessRules($true, $true, [Security.Principal.SecurityIdentifier]) | Where-Object { $_.IdentityReference.Value -eq 'S-1-5-32-545' }).Count -ne 1) { Write-Output 'FAIL Users must keep read/execute'; $failures++ } else { Write-Output 'ok   Users keep read/execute' }
if (@($planned.GetAccessRules($true, $true, [Security.Principal.SecurityIdentifier]) | Where-Object { !($_.InheritanceFlags -band [Security.AccessControl.InheritanceFlags]::ObjectInherit) }).Count) { Write-Output 'FAIL Every admin-only rule must reach copied files'; $failures++ } else { Write-Output 'ok   Every admin-only rule reaches copied files' }
if ($planned.GetOwner([Security.Principal.SecurityIdentifier]).Value -ne 'S-1-5-32-544') { Write-Output 'FAIL Admin-only directory must be owned by Administrators'; $failures++ } else { Write-Output 'ok   Admin-only directory is owned by Administrators' }

function Check([string]$Name, [scriptblock]$Test) {
    $ok = $false
    try { $ok = [bool](& $Test) } catch { Write-Output "FAIL $Name : $($_.Exception.Message)"; $script:failures++; return }
    if ($ok) { Write-Output "ok   $Name" } else { Write-Output "FAIL $Name"; $script:failures++ }
}
function Throws([string]$Name, [scriptblock]$Test) {
    try { & $Test; Write-Output "FAIL $Name : expected a refusal"; $script:failures++ } catch { Write-Output "ok   $Name" }
}
$stage = 'C:\repo\runtime\completion\backend6-1'
$root = 'C:\Program Files\NucControl\deployment-0123456789abcdef0123456789abcdef'
$copy = Get-TrustedCopyPlan -CandidateExecutable "$stage\candidate\nuc11ph-service.exe" -BackupDirectory "$stage\revision5" -TrustedRoot $root
$roles = 'CandidateExecutable', 'CandidatePdb', 'BackupExecutable', 'BackupPdb'
Check 'Copy plan covers the candidate and revision-5 pairs' { (@($copy.Keys) -join ',') -eq ($roles -join ',') }
Check 'Every copy lands inside the admin-only root' { @($roles | Where-Object { !$copy[$_].Destination.StartsWith("$root\", [StringComparison]::OrdinalIgnoreCase) }).Count -eq 0 }
Check 'Every copy is read from the staging directory' { @($roles | Where-Object { !$copy[$_].Source.StartsWith("$stage\", [StringComparison]::OrdinalIgnoreCase) }).Count -eq 0 }
Check 'Candidate symbols are read beside the candidate' { $copy.CandidatePdb.Source -eq "$stage\candidate\nuc11ph_service.pdb" }
Check 'Copies keep the installed file names' { (Split-Path -Leaf $copy.CandidateExecutable.Destination) -eq 'nuc11ph-service.exe' -and (Split-Path -Leaf $copy.BackupPdb.Destination) -eq 'nuc11ph_service.pdb' }
Check 'Candidate and backup copies do not collide' { @($roles | ForEach-Object { $copy[$_].Destination } | Sort-Object -Unique).Count -eq 4 }
Throws 'A root inside the staging directory is refused' { Get-TrustedCopyPlan -CandidateExecutable "$stage\candidate\nuc11ph-service.exe" -BackupDirectory "$stage\revision5" -TrustedRoot "$stage\candidate\trusted" }
Throws 'A staged file inside the root is refused' { Get-TrustedCopyPlan -CandidateExecutable "$root\candidate\nuc11ph-service.exe" -BackupDirectory "$stage\revision5" -TrustedRoot $root }
Throws 'A relative root is refused' { Get-TrustedCopyPlan -CandidateExecutable "$stage\candidate\nuc11ph-service.exe" -BackupDirectory "$stage\revision5" -TrustedRoot 'trusted' }
Check 'Sibling prefixes are not treated as inside' { !(Test-PathWithin -Path 'C:\a\bc\x.exe' -Directory 'C:\a\b') -and (Test-PathWithin -Path 'C:\a\b\x.exe' -Directory 'C:\a\b\') }

# deploy_backend6.ps1 must not run, hash or copy any staged path except to make the admin-only copy.
$deployScript = Join-Path $PSScriptRoot '../deploy_backend6.ps1'
$ast = [Management.Automation.Language.Parser]::ParseFile($deployScript, [ref]$null, [ref]$null)
$staged = @($ast.FindAll({ param($node) $node -is [Management.Automation.Language.MemberExpressionAst] -and
    $node.Expression -is [Management.Automation.Language.VariableExpressionAst] -and $node.Expression.VariablePath.UserPath -eq 'plan' -and
    $node.Member.Extent.Text -in 'candidate_executable', 'backup_directory' }, $true))
$outside = @($staged | Where-Object {
    $parent = $_.Parent
    while ($parent -and $parent -isnot [Management.Automation.Language.CommandAst]) { $parent = $parent.Parent }
    !$parent -or $parent.GetCommandName() -ne 'Get-TrustedCopyPlan' })
Check 'deploy_backend6.ps1 reads staged paths only to plan the admin-only copy' { $staged.Count -eq 2 -and $outside.Count -eq 0 }
Check 'deploy_backend6.ps1 never uses a copy-plan source path' { @($ast.FindAll({ param($node) $node -is [Management.Automation.Language.MemberExpressionAst] -and $node.Member.Extent.Text -eq 'Source' }, $true)).Count -eq 0 }
$copyCall = @($ast.FindAll({ param($node) $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq 'Copy-ToAdminOnlyDirectory' }, $true))
$firstHash = @($ast.FindAll({ param($node) $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq 'Check-Hash' -and $node.Parent.Parent -isnot [Management.Automation.Language.FunctionDefinitionAst] }, $true) | Sort-Object { $_.Extent.StartOffset } | Select-Object -First 1)
Check 'deploy_backend6.ps1 makes the admin-only copy before any hash check' { $copyCall.Count -eq 1 -and $firstHash.Count -eq 1 -and $copyCall[0].Extent.StartOffset -lt $firstHash[0].Extent.StartOffset }
if ($failures) { exit 1 }
Write-Output 'ServiceSecurity tests passed.'
