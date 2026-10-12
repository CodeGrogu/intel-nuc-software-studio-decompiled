using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.IO;

[ComImport]
[Guid("5CA296B2-2C25-4D22-B785-B885C8201E6A")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IStorageItemHandleAccess
{
	[PreserveSig]
	int Create(HANDLE_ACCESS_OPTIONS accessOptions, HANDLE_SHARING_OPTIONS sharingOptions, HANDLE_OPTIONS options, IntPtr oplockBreakingHandler, out SafeFileHandle interopHandle);
}
