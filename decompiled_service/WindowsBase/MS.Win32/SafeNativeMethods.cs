using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.WindowsBase;

namespace MS.Win32;

[FriendAccessAllowed]
internal static class SafeNativeMethods
{
	[Flags]
	internal enum PlaySoundFlags
	{
		SND_SYNC = 0,
		SND_ASYNC = 1,
		SND_NODEFAULT = 2,
		SND_MEMORY = 4,
		SND_LOOP = 8,
		SND_NOSTOP = 0x10,
		SND_PURGE = 0x40,
		SND_APPLICATION = 0x80,
		SND_NOWAIT = 0x2000,
		SND_ALIAS = 0x10000,
		SND_FILENAME = 0x20000,
		SND_RESOURCE = 0x40000
	}

	public const uint CT_CTYPE1 = 1u;

	public const uint CT_CTYPE2 = 2u;

	public const uint CT_CTYPE3 = 4u;

	public const ushort C1_SPACE = 8;

	public const ushort C1_PUNCT = 16;

	public const ushort C1_BLANK = 64;

	public const ushort C3_NONSPACING = 1;

	public const ushort C3_DIACRITIC = 2;

	public const ushort C3_VOWELMARK = 4;

	public const ushort C3_KATAKANA = 16;

	public const ushort C3_HIRAGANA = 32;

	public const ushort C3_HALFWIDTH = 64;

	public const ushort C3_FULLWIDTH = 128;

	public const ushort C3_IDEOGRAPH = 256;

	public const ushort C3_KASHIDA = 512;

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int GetMessagePos()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr GetKeyboardLayout(int dwLayout)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr ActivateKeyboardLayout(HandleRef hkl, int uFlags)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int GetKeyboardLayoutList(int size, [Out][MarshalAs(UnmanagedType.LPArray)] IntPtr[] hkls)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void GetMonitorInfo(HandleRef hmonitor, [In][Out] NativeMethods.MONITORINFOEX info)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr MonitorFromPoint(NativeMethods.POINTSTRUCT pt, int flags)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr MonitorFromRect(ref NativeMethods.RECT rect, int flags)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr MonitorFromWindow(HandleRef handle, int flags)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static NativeMethods.CursorHandle LoadCursor(HandleRef hInst, IntPtr iconId)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr GetCursor()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int ShowCursor(bool show)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static bool AdjustWindowRectEx(ref NativeMethods.RECT lpRect, int dwStyle, bool bMenu, int dwExStyle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void GetClientRect(HandleRef hWnd, [In][Out] ref NativeMethods.RECT rect)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void GetWindowRect(HandleRef hWnd, [In][Out] ref NativeMethods.RECT rect)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int GetDoubleClickTime()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool IsWindowEnabled(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool IsWindowVisible(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static bool ReleaseCapture()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static void SetTimer(HandleRef hWnd, int nIDEvent, int uElapse)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool TrySetTimer(HandleRef hWnd, int nIDEvent, int uElapse)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool KillTimer(HandleRef hwnd, int idEvent)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int GetTickCount()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int MessageBeep(int uType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool IsWindowUnicode(HandleRef hWnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr SetCursor(HandleRef hcursor)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr SetCursor(SafeHandle hcursor)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static void ScreenToClient(HandleRef hWnd, [In][Out] NativeMethods.POINT pt)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int GetCurrentProcessId()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int GetCurrentThreadId()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr GetCapture()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static IntPtr SetCapture(HandleRef hwnd)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static int MapVirtualKey(int nVirtKey, int nMapType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static bool InSendMessage()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool IsUxThemeActive()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool SetCaretPos(int x, int y)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool DestroyCaret()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int GetCaretBlinkTime()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool GetStringTypeEx(uint locale, uint infoType, char[] sourceString, int count, ushort[] charTypes)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static int GetSysColor(int nIndex)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool IsClipboardFormatAvailable(int format)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static bool IsDebuggerPresent()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static void QueryPerformanceCounter(out long lpPerformanceCount)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public static void QueryPerformanceFrequency(out long lpFrequency)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static int GetMessageTime()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static int GetWindowStyle(HandleRef hWnd, bool exStyle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
