using System;
using System.Security;

namespace MS.Win32;

internal static class ManagedWndProcTracker
{
	[SecurityCritical]
	[SecurityTreatAsSafe]
	static ManagedWndProcTracker()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void TrackHwndSubclass(HwndSubclass subclass, IntPtr hwnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void UnhookHwndSubclass(HwndSubclass subclass)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
