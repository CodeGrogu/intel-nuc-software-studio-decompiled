using System;
using System.Runtime.InteropServices;
using System.Text;

internal class Interop
{
	internal class Errors
	{
		internal const int ERROR_FILE_NOT_FOUND = 2;

		internal const int ERROR_PATH_NOT_FOUND = 3;

		internal const int ERROR_ACCESS_DENIED = 5;

		internal const int ERROR_SHARING_VIOLATION = 32;

		internal const int ERROR_FILE_EXISTS = 80;

		internal const int ERROR_INVALID_PARAMETER = 87;

		internal const int ERROR_ALREADY_EXISTS = 183;

		internal const int ERROR_FILENAME_EXCED_RANGE = 206;

		internal const int ERROR_OPERATION_ABORTED = 995;
	}

	internal class Kernel32
	{
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		private const int FORMAT_MESSAGE_FROM_HMODULE = 2048;

		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		private const int ERROR_INSUFFICIENT_BUFFER = 122;

		private const int InitialBufferSize = 256;

		private const int BufferSizeIncreaseFactor = 4;

		private const int MaxAllowedBufferSize = 66560;

		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Unicode, EntryPoint = "FormatMessageW", SetLastError = true)]
		private static extern int FormatMessage(int dwFlags, IntPtr lpSource, uint dwMessageId, int dwLanguageId, [Out] StringBuilder lpBuffer, int nSize, IntPtr[] arguments);

		internal static string GetMessage(int errorCode)
		{
			return GetMessage(IntPtr.Zero, errorCode);
		}

		internal static string GetMessage(IntPtr moduleHandle, int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			do
			{
				if (TryGetErrorMessage(moduleHandle, errorCode, stringBuilder, out var errorMsg))
				{
					return errorMsg;
				}
				stringBuilder.Capacity *= 4;
			}
			while (stringBuilder.Capacity < 66560);
			return $"Unknown error (0x{errorCode:x})";
		}

		private static bool TryGetErrorMessage(IntPtr moduleHandle, int errorCode, StringBuilder sb, out string errorMsg)
		{
			errorMsg = "";
			int num = 12800;
			if (moduleHandle != IntPtr.Zero)
			{
				num |= 0x800;
			}
			if (FormatMessage(num, moduleHandle, (uint)errorCode, 0, sb, sb.Capacity, null) != 0)
			{
				int num2;
				for (num2 = sb.Length; num2 > 0; num2--)
				{
					char c = sb[num2 - 1];
					if (c > ' ' && c != '.')
					{
						break;
					}
				}
				errorMsg = sb.ToString(0, num2);
			}
			else
			{
				if (Marshal.GetLastWin32Error() == 122)
				{
					return false;
				}
				errorMsg = $"Unknown error (0x{errorCode:x})";
			}
			return true;
		}
	}
}
