using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using MS.Win32;

namespace MS.Internal.WindowsBase;

[SuppressUnmanagedCodeSecurity]
[SecurityCritical(SecurityCriticalScope.Everything)]
internal static class NativeMethodsSetLastError
{
	static NativeMethodsSetLastError()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "EnableWindowWrapper", ExactSpelling = true, SetLastError = true)]
	public static extern bool EnableWindow(HandleRef hWnd, bool enable);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetAncestorWrapper")]
	public static extern IntPtr GetAncestor(IntPtr hwnd, int gaFlags);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetKeyboardLayoutListWrapper", ExactSpelling = true, SetLastError = true)]
	public static extern int GetKeyboardLayoutList(int size, [Out][MarshalAs(UnmanagedType.LPArray)] IntPtr[] hkls);

	[DllImport("PresentationNative_v0400.dll", EntryPoint = "GetParentWrapper", SetLastError = true)]
	public static extern IntPtr GetParent(HandleRef hWnd);

	[DllImport("PresentationNative_v0400.dll", EntryPoint = "GetWindowWrapper", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongWrapper", SetLastError = true)]
	public static extern int GetWindowLong(HandleRef hWnd, int nIndex);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongWrapper", SetLastError = true)]
	public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongWrapper", SetLastError = true)]
	public static extern MS.Win32.NativeMethods.WndProc GetWindowLongWndProc(HandleRef hWnd, int nIndex);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtrWrapper", SetLastError = true)]
	public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtrWrapper", SetLastError = true)]
	public static extern IntPtr GetWindowLongPtr(HandleRef hWnd, int nIndex);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtrWrapper", SetLastError = true)]
	public static extern MS.Win32.NativeMethods.WndProc GetWindowLongPtrWndProc(HandleRef hWnd, int nIndex);

	[DllImport("PresentationNative_v0400.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "GetWindowTextWrapper", SetLastError = true)]
	public static extern int GetWindowText(HandleRef hWnd, [Out] StringBuilder lpString, int nMaxCount);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowTextLengthWrapper", SetLastError = true)]
	public static extern int GetWindowTextLength(HandleRef hWnd);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "MapWindowPointsWrapper", ExactSpelling = true, SetLastError = true)]
	public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [In][Out] ref MS.Win32.NativeMethods.RECT rect, int cPoints);

	[DllImport("PresentationNative_v0400.dll", EntryPoint = "SetFocusWrapper", SetLastError = true)]
	public static extern IntPtr SetFocus(HandleRef hWnd);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongWrapper")]
	public static extern int SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongWrapper")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongWrapper", SetLastError = true)]
	public static extern int SetWindowLongWndProc(HandleRef hWnd, int nIndex, MS.Win32.NativeMethods.WndProc dwNewLong);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtrWrapper")]
	public static extern IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtrWrapper")]
	public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtrWrapper", SetLastError = true)]
	public static extern IntPtr SetWindowLongPtrWndProc(HandleRef hWnd, int nIndex, MS.Win32.NativeMethods.WndProc dwNewLong);
}
