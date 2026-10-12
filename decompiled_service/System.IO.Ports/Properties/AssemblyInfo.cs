using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;

[assembly: CLSCompliant(true)]
[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
[assembly: UnsupportedOSPlatform("browser")]
[assembly: AssemblyDefaultAlias("System.IO.Ports")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: AssemblyMetadata("Serviceable", "True")]
[assembly: AssemblyMetadata("PreferInbox", "True")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: AssemblyDescription("System.IO.Ports")]
[assembly: AssemblyFileVersion("5.0.421.11614")]
[assembly: AssemblyInformationalVersion("5.0.4+f27d33729518f5aa478aa818b7b4f54a4d50bef1")]
[assembly: AssemblyProduct("Microsoft® .NET")]
[assembly: AssemblyTitle("System.IO.Ports")]
[assembly: AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: AssemblyVersion("5.0.0.0")]
[assembly: TypeForwardedTo(typeof(Handshake))]
[assembly: TypeForwardedTo(typeof(Parity))]
[assembly: TypeForwardedTo(typeof(SerialData))]
[assembly: TypeForwardedTo(typeof(SerialDataReceivedEventArgs))]
[assembly: TypeForwardedTo(typeof(SerialDataReceivedEventHandler))]
[assembly: TypeForwardedTo(typeof(SerialError))]
[assembly: TypeForwardedTo(typeof(SerialErrorReceivedEventArgs))]
[assembly: TypeForwardedTo(typeof(SerialErrorReceivedEventHandler))]
[assembly: TypeForwardedTo(typeof(SerialPinChange))]
[assembly: TypeForwardedTo(typeof(SerialPinChangedEventArgs))]
[assembly: TypeForwardedTo(typeof(SerialPinChangedEventHandler))]
[assembly: TypeForwardedTo(typeof(SerialPort))]
[assembly: TypeForwardedTo(typeof(StopBits))]
