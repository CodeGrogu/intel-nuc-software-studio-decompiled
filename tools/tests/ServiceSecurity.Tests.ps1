# Pure ACL-rule tests for the service-location guard. Reads no files and changes nothing.
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
$planned = New-AdminOnlyDirectorySecurity
Expect 'Relocation ACL grants no untrusted writer' @($planned.GetAccessRules($true, $true, [Security.Principal.SecurityIdentifier])) 0
if (!$planned.AreAccessRulesProtected) { Write-Output 'FAIL Relocation ACL must not inherit'; $failures++ } else { Write-Output 'ok   Relocation ACL does not inherit' }
if (@($planned.GetAccessRules($true, $true, [Security.Principal.SecurityIdentifier]) | Where-Object { $_.IdentityReference.Value -eq 'S-1-5-32-545' }).Count -ne 1) { Write-Output 'FAIL Users must keep read/execute'; $failures++ } else { Write-Output 'ok   Users keep read/execute' }
if ($failures) { exit 1 }
Write-Output 'ServiceSecurity tests passed.'
