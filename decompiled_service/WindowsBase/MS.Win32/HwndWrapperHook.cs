using System;
using System.Security;
using MS.Internal.WindowsBase;

namespace MS.Win32;

[SecurityCritical]
[FriendAccessAllowed]
internal delegate IntPtr HwndWrapperHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);
