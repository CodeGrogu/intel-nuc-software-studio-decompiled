# Parse maintained scripts only. Never execute hardware investigation scripts.
$ErrorActionPreference = 'Stop'
$toolsRoot = $PSScriptRoot
$failed = $false
$scripts = @(Get-ChildItem -LiteralPath $toolsRoot -Filter '*.ps1' -Recurse -File)
$desktopRoot = Join-Path $PSScriptRoot '../desktop'
if (Test-Path -LiteralPath $desktopRoot) {
    $scripts += @(Get-ChildItem -LiteralPath $desktopRoot -Filter '*.ps1' -Recurse -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj|artifacts)[\\/]' })
}
foreach ($script in $scripts) {
    $tokens = $null
    $parseErrors = $null
    $null = [System.Management.Automation.Language.Parser]::ParseFile(
        $script.FullName, [ref]$tokens, [ref]$parseErrors)
    foreach ($parseError in $parseErrors) {
        Write-Output "$($script.Name): $parseError"
        $failed = $true
    }
}
if ($failed) { exit 1 }
Write-Output "Parsed $($scripts.Count) maintained PowerShell script(s)."
