using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Security.RightsManagement;

[StructLayout(LayoutKind.Sequential)]
[SecurityCritical(SecurityCriticalScope.Everything)]
internal class SystemTime
{
	internal static uint Size
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal SystemTime(DateTime dateTime)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal SystemTime(byte[] dataBuffer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal DateTime GetDateTime(DateTime defaultValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
