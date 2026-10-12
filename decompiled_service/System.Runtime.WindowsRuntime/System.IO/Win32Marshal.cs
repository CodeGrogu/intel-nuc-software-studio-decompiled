namespace System.IO;

internal static class Win32Marshal
{
	internal static Exception GetExceptionForWin32Error(int errorCode, string path)
	{
		switch (errorCode)
		{
		case 2:
			if (path.Length == 0)
			{
				return new FileNotFoundException("IO_FileNotFound");
			}
			return new FileNotFoundException(SR.GetString("IO_FileNotFound_FileName", path), path);
		case 3:
			if (path.Length == 0)
			{
				return new DirectoryNotFoundException("IO_PathNotFound_NoPathName");
			}
			return new DirectoryNotFoundException(SR.GetString("IO_PathNotFound_Path", path));
		case 5:
			if (path.Length == 0)
			{
				return new UnauthorizedAccessException("UnauthorizedAccess_IODenied_NoPathName");
			}
			return new UnauthorizedAccessException(SR.GetString("UnauthorizedAccess_IODenied_Path", path));
		case 183:
			if (path.Length != 0)
			{
				return new IOException(SR.GetString("IO_AlreadyExists_Name", path), MakeHRFromErrorCode(errorCode));
			}
			break;
		case 206:
			return new PathTooLongException("IO_PathTooLong");
		case 87:
			return new IOException(GetMessage(errorCode), MakeHRFromErrorCode(errorCode));
		case 32:
			if (path.Length == 0)
			{
				return new IOException("IO_SharingViolation_NoFileName", MakeHRFromErrorCode(errorCode));
			}
			return new IOException(SR.GetString("IO_SharingViolation_File", path), MakeHRFromErrorCode(errorCode));
		case 80:
			if (path.Length != 0)
			{
				return new IOException(SR.GetString("IO_FileExists_Name", path), MakeHRFromErrorCode(errorCode));
			}
			break;
		case 995:
			return new OperationCanceledException();
		}
		return new IOException(GetMessage(errorCode), MakeHRFromErrorCode(errorCode));
	}

	internal static int MakeHRFromErrorCode(int errorCode)
	{
		if ((0xFFFF0000u & errorCode) != 0L)
		{
			return errorCode;
		}
		return -2147024896 | errorCode;
	}

	internal static int TryMakeWin32ErrorCodeFromHR(int hr)
	{
		if ((0xFFFF0000u & hr) == 2147942400u)
		{
			hr &= 0xFFFF;
		}
		return hr;
	}

	internal static string GetMessage(int errorCode)
	{
		return Interop.Kernel32.GetMessage(errorCode);
	}
}
