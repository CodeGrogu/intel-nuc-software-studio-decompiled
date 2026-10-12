using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MS.Internal.Interop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
[BestFitMapping(false)]
internal class WIN32_FIND_DATAW
{
	public FileAttributes dwFileAttributes;

	public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

	public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

	public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

	public int nFileSizeHigh;

	public int nFileSizeLow;

	public int dwReserved0;

	public int dwReserved1;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
	public string cFileName;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
	public string cAlternateFileName;

	public WIN32_FIND_DATAW()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
