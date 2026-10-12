using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.IO;

[ComImport]
[Guid("DF19938F-5462-48A0-BE65-D2A3271A08D6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IStorageFolderHandleAccess
{
	[PreserveSig]
	int Create([MarshalAs(UnmanagedType.LPWStr)] string fileName, HANDLE_CREATION_OPTIONS creationOptions, HANDLE_ACCESS_OPTIONS accessOptions, HANDLE_SHARING_OPTIONS sharingOptions, HANDLE_OPTIONS options, IntPtr oplockBreakingHandler, out SafeFileHandle interopHandle);
}
