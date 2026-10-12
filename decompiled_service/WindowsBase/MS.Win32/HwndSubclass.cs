using System;
using System.Security;
using MS.Internal.Interop;
using MS.Internal.WindowsBase;

namespace MS.Win32;

[FriendAccessAllowed]
internal class HwndSubclass : IDisposable
{
	internal static readonly WindowMessage DetachMessage;

	[SecurityCritical]
	static HwndSubclass()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal HwndSubclass(HwndWrapperHook hook)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public virtual void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal IntPtr Attach(IntPtr hwnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal bool Detach(bool force)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal bool CriticalDetach(bool force)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal void RequestDetach(bool force)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void RequestDetach(IntPtr hwnd, IntPtr subclass, bool force)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal IntPtr SubclassWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal IntPtr CriticalAttach(IntPtr hwnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
