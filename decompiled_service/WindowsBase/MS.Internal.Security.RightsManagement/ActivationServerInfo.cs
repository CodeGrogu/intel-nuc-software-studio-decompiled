using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Security.RightsManagement;

[StructLayout(LayoutKind.Sequential)]
[SecurityCritical(SecurityCriticalScope.Everything)]
internal class ActivationServerInfo
{
	public uint Version;

	[MarshalAs(UnmanagedType.LPWStr)]
	internal string PubKey;

	[MarshalAs(UnmanagedType.LPWStr)]
	internal string Url;

	public ActivationServerInfo()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
