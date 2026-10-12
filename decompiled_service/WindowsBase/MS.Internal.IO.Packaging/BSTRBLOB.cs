using System;
using MS.Internal.WindowsBase;

namespace MS.Internal.IO.Packaging;

[FriendAccessAllowed]
internal struct BSTRBLOB
{
	public uint cbSize;

	public IntPtr pData;
}
