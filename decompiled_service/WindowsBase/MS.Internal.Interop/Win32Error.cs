using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Interop;

[StructLayout(LayoutKind.Explicit)]
internal struct Win32Error
{
	[FieldOffset(0)]
	private readonly int _value;

	public static readonly Win32Error ERROR_SUCCESS;

	public static readonly Win32Error ERROR_INVALID_FUNCTION;

	public static readonly Win32Error ERROR_FILE_NOT_FOUND;

	public static readonly Win32Error ERROR_PATH_NOT_FOUND;

	public static readonly Win32Error ERROR_TOO_MANY_OPEN_FILES;

	public static readonly Win32Error ERROR_ACCESS_DENIED;

	public static readonly Win32Error ERROR_INVALID_HANDLE;

	public static readonly Win32Error ERROR_OUTOFMEMORY;

	public static readonly Win32Error ERROR_NO_MORE_FILES;

	public static readonly Win32Error ERROR_SHARING_VIOLATION;

	public static readonly Win32Error ERROR_INVALID_PARAMETER;

	public static readonly Win32Error ERROR_INSUFFICIENT_BUFFER;

	public static readonly Win32Error ERROR_NESTING_NOT_ALLOWED;

	public static readonly Win32Error ERROR_KEY_DELETED;

	public static readonly Win32Error ERROR_NO_MATCH;

	public static readonly Win32Error ERROR_BAD_DEVICE;

	public static readonly Win32Error ERROR_CANCELLED;

	public static readonly Win32Error ERROR_INVALID_WINDOW_HANDLE;

	public static readonly Win32Error ERROR_TIMEOUT;

	public static readonly Win32Error ERROR_INVALID_DATATYPE;

	public Win32Error(int i)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static explicit operator HRESULT(Win32Error error)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public HRESULT ToHRESULT()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static Win32Error GetLastError()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static bool operator ==(Win32Error errLeft, Win32Error errRight)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static bool operator !=(Win32Error errLeft, Win32Error errRight)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static Win32Error()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
