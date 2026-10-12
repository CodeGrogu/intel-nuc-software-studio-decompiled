using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsBase;

namespace MS.Internal.IO.Packaging;

[StructLayout(LayoutKind.Explicit)]
[FriendAccessAllowed]
internal struct PROPSPECunion
{
	[FieldOffset(0)]
	internal IntPtr name;

	[FieldOffset(0)]
	internal uint propId;
}
