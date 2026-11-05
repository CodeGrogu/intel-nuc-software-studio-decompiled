# NUC Control

Native WinUI 3 frontend, pure .NET Core/client library, and xUnit tests. The maintained app lives here; the earlier external C# project remains a reference.

From this directory with .NET SDK 10.0.401:

```powershell
dotnet restore NucControl.slnx --locked-mode
dotnet test --project NucControl.Tests/NucControl.Tests.csproj
dotnet build NucControl.App/NucControl.App.csproj --no-restore
./Publish.ps1
```

The solution opts into Microsoft.Testing.Platform for .NET 10. Tests and demo mode require no hardware writes. Build and publication use the repository VERSION; release-please owns version bumps and the changelog.

The backend uses length-prefixed externally tagged JSON. Revision-6 controls are described in [the protocol document](../docs/CONTROL_PANEL_PROTOCOL.md). The UI never calls legacy combined profiles, synthesized fan modes, or BIOS-save APIs.
