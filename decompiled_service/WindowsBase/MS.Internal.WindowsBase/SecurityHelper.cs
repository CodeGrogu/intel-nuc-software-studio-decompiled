using System;
using System.Security;
using Microsoft.Win32;

namespace MS.Internal.WindowsBase;

[FriendAccessAllowed]
internal static class SecurityHelper
{
	[SecuritySafeCritical]
	internal static bool CheckUnmanagedCodePermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void DemandUnmanagedCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void DemandRightsManagementPermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void DemandCompoundFileIOPermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void DemandPathDiscovery(string path)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecuritySafeCritical]
	internal static void RunClassConstructor(Type t)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecuritySafeCritical]
	internal static Exception GetExceptionForHR(int hr)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecuritySafeCritical]
	internal static void ThrowExceptionForHR(int hr)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecuritySafeCritical]
	internal static int GetHRForException(Exception exception)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void DemandUIWindowPermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void DemandUnrestrictedUIPermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool AreStringTypesEqual(string m1, string m2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static object ReadRegistryValue(RegistryKey baseRegistryKey, string keyName, string valueName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static SecurityHelper()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
