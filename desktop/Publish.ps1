[CmdletBinding()]
param([string]$OutputDirectory = (Join-Path $PSScriptRoot 'artifacts'))
$ErrorActionPreference = 'Stop'
Push-Location $PSScriptRoot
try {
    $version = (Get-Content -LiteralPath (Join-Path $PSScriptRoot '../VERSION') -Raw).Trim()
    if ($version -notmatch '^\d+\.\d+\.\d+$') { throw 'Invalid repository VERSION' }
    $destination = Join-Path $OutputDirectory "NUC-Control-$version-win-x64"
    # A new destination prevents stale runtime files leaking between versions/builds.
    if (Test-Path -LiteralPath $destination) { throw "Output already exists: $destination. Choose a fresh output directory." }
    dotnet restore NucControl.App/NucControl.App.csproj --locked-mode
    if ($LASTEXITCODE) { throw 'Locked restore failed' }
    dotnet publish NucControl.App/NucControl.App.csproj --no-restore -c Release -p:Version=$version -o $destination
    if ($LASTEXITCODE) { throw 'Publish failed' }
    # ApplicationIcon embeds the PE icon; WinUI/tray also need this physical content file.
    $publishedIcon = Join-Path $destination 'Assets/NucControl.ico'
    $sourceIcon = Join-Path $PSScriptRoot 'NucControl.App/Assets/NucControl.ico'
    if (!(Test-Path -LiteralPath $publishedIcon -PathType Leaf) -or
        (Get-FileHash -LiteralPath $publishedIcon -Algorithm SHA256).Hash -ne
        (Get-FileHash -LiteralPath $sourceIcon -Algorithm SHA256).Hash) {
        throw 'Published runtime icon is missing or differs from the owned source asset'
    }
    foreach ($resource in @('App.xbf', 'MainWindow.xbf', 'NucControl.pri')) {
        $resourcePath = Join-Path $destination $resource
        if (!(Test-Path -LiteralPath $resourcePath -PathType Leaf) -or (Get-Item -LiteralPath $resourcePath).Length -eq 0) {
            throw "Published WinUI runtime resource is missing or empty: $resource"
        }
    }
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'PORTABLE.md') -Destination (Join-Path $destination 'README.md')
    $zip = "$destination.zip"
    Compress-Archive -LiteralPath $destination -DestinationPath $zip
    $hash = (Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash.ToLowerInvariant()
    "$hash  $([IO.Path]::GetFileName($zip))" | Set-Content -LiteralPath "$zip.sha256" -Encoding ascii
    Write-Output $zip
} finally { Pop-Location }
