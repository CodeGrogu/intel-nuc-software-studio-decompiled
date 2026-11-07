# Guards for the LocalSystem service location. Read-only helpers; nothing here changes an ACL.
Set-StrictMode -Version Latest

$TrustedWriterSids = @(
    'S-1-5-18',      # LocalSystem
    'S-1-5-32-544',  # BUILTIN\Administrators
    'S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464'  # NT SERVICE\TrustedInstaller
)
# WriteData, AppendData, WriteExtendedAttributes, DeleteSubdirectoriesAndFiles, WriteAttributes,
# Delete, ChangePermissions, TakeOwnership, GENERIC_ALL, GENERIC_WRITE.
$WriteMask = 0x2 -bor 0x4 -bor 0x10 -bor 0x40 -bor 0x100 -bor 0x10000 -bor 0x40000 -bor 0x80000 -bor 0x10000000 -bor 0x40000000
$ReviewedServiceDirectory = Join-Path $env:ProgramFiles 'NucControl\Backend'

function Get-UnsafeWriteGrant {
    [CmdletBinding()]
    param([AllowEmptyCollection()][Security.AccessControl.AuthorizationRule[]]$Rules)
    foreach ($rule in @($Rules)) {
        if ($rule -isnot [Security.AccessControl.FileSystemAccessRule]) { continue }
        if ($rule.AccessControlType -ne [Security.AccessControl.AccessControlType]::Allow) { continue }
        if ($rule.PropagationFlags -band [Security.AccessControl.PropagationFlags]::InheritOnly) { continue }
        $sid = $rule.IdentityReference.Translate([Security.Principal.SecurityIdentifier]).Value
        if ($TrustedWriterSids -contains $sid) { continue }
        if (([int64]$rule.FileSystemRights -band $WriteMask) -ne 0) { "$sid $($rule.FileSystemRights)" }
    }
}

function Get-UnsafeOwner {
    # An owner holds implicit WRITE_DAC, so it can re-grant itself write despite the DACL.
    [CmdletBinding()]
    param([Parameter(Mandatory)][Security.Principal.SecurityIdentifier]$Owner)
    if ($TrustedWriterSids -notcontains $Owner.Value) { "owner $($Owner.Value)" }
}

function Assert-AdminOnlyWritable {
    [CmdletBinding()]
    param([Parameter(Mandatory)][string]$Path)
    $full = [IO.Path]::GetFullPath($Path)
    foreach ($target in @($full, (Split-Path -Parent $full))) {
        $acl = Get-Acl -LiteralPath $target
        $grants = @(Get-UnsafeWriteGrant -Rules @($acl.GetAccessRules($true, $true, [Security.Principal.SecurityIdentifier])))
        $grants += @(Get-UnsafeOwner -Owner $acl.GetOwner([Security.Principal.SecurityIdentifier]))
        if ($grants.Count) { throw "User-writable service location: ${target}: $($grants -join '; ')" }
    }
}

function New-AdminOnlyDirectorySecurity {
    # Protected DACL: SYSTEM/Administrators full control, Users read and execute, nothing inherited.
    $security = [Security.AccessControl.DirectorySecurity]::new()
    $security.SetAccessRuleProtection($true, $false)
    $security.SetOwner([Security.Principal.SecurityIdentifier]::new('S-1-5-32-544'))
    $inherit = [Security.AccessControl.InheritanceFlags]'ContainerInherit, ObjectInherit'
    foreach ($entry in @(@('S-1-5-18', 'FullControl'), @('S-1-5-32-544', 'FullControl'), @('S-1-5-32-545', 'ReadAndExecute'))) {
        $security.AddAccessRule([Security.AccessControl.FileSystemAccessRule]::new(
            [Security.Principal.SecurityIdentifier]::new($entry[0]), [Security.AccessControl.FileSystemRights]$entry[1],
            $inherit, [Security.AccessControl.PropagationFlags]::None, [Security.AccessControl.AccessControlType]::Allow))
    }
    return $security
}

function Read-BackendProtocolRevision {
    # Sends one GetBackendInfo frame; read-only.
    $pipe = [IO.Pipes.NamedPipeClientStream]::new('.', 'Nuc11PhStudioV1', [IO.Pipes.PipeDirection]::InOut, [IO.Pipes.PipeOptions]::Asynchronous)
    try {
        $pipe.Connect(1000)
        $body = [Text.Encoding]::UTF8.GetBytes('"GetBackendInfo"')
        $frame = [BitConverter]::GetBytes([int]$body.Length) + $body
        $write = $pipe.WriteAsync($frame, 0, $frame.Length)
        if (!$write.Wait(1000)) { throw 'Pipe write timeout' }; $write.GetAwaiter().GetResult()
        $readBytes = {
            param([int]$Count)
            $buffer = New-Object byte[] $Count; $offset = 0
            while ($offset -lt $Count) {
                $read = $pipe.ReadAsync($buffer, $offset, $Count - $offset)
                if (!$read.Wait(1000)) { throw 'Pipe read timeout' }
                $n = $read.GetAwaiter().GetResult(); if ($n -eq 0) { throw 'Truncated pipe response' }; $offset += $n
            }
            return , $buffer
        }
        $length = [BitConverter]::ToInt32((& $readBytes 4), 0)
        if ($length -le 0 -or $length -gt 262144) { throw 'Invalid pipe frame' }
        $reply = [Text.Encoding]::UTF8.GetString((& $readBytes $length)) | ConvertFrom-Json
        return $reply.BackendInfo.protocol_revision
    } finally { $pipe.Dispose() }
}

Export-ModuleMember -Function Get-UnsafeWriteGrant, Get-UnsafeOwner, Assert-AdminOnlyWritable, New-AdminOnlyDirectorySecurity, Read-BackendProtocolRevision -Variable ReviewedServiceDirectory
