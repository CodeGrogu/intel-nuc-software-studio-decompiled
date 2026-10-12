using System.Security;

namespace System.Runtime.InteropServices.WindowsRuntime;

[ComImport]
[Guid("905a0fef-bc53-11df-8c49-001e4fc686da")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IBufferByteAccess
{
	unsafe byte* Buffer
	{
		[SecurityCritical]
		get;
	}
}
