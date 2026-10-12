using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Security.RightsManagement;

[StructLayout(LayoutKind.Sequential)]
[SecurityCritical(SecurityCriticalScope.Everything)]
internal class BoundLicenseParams
{
	internal uint uVersion;

	internal uint hEnablingPrincipal;

	internal uint hSecureStore;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string wszRightsRequested;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string wszRightsGroup;

	internal uint DRMIDuVersion;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string DRMIDIdType;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string DRMIDId;

	internal uint cAuthenticatorCount;

	internal IntPtr rghAuthenticators;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string wszDefaultEnablingPrincipalCredentials;

	internal uint dwFlags;

	public BoundLicenseParams()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
