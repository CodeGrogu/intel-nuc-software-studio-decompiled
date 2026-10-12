using System;
using MS.Internal.WindowsBase;

namespace MS.Internal.IO.Packaging;

[FriendAccessAllowed]
internal struct BLOB
{
	public uint cbSize;

	public IntPtr pBlobData;
}
