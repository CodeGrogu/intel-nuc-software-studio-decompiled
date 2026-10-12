using System;
using MS.Internal.WindowsBase;

namespace MS.Internal.IO.Packaging;

[FriendAccessAllowed]
internal struct CArray
{
	public uint cElems;

	public IntPtr pElems;
}
