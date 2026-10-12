using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace System.IO;

internal static class WinRtIOHelper
{
	internal const int DefaultIOBufferSize = 12288;

	internal static ExceptionDispatchInfo NativeExceptionToIOExceptionInfo(Exception nativeException)
	{
		if (!nativeException.GetType().Equals(typeof(Exception)) && !(nativeException is ExternalException))
		{
			return ExceptionDispatchInfo.Capture(nativeException);
		}
		string text = nativeException.Message;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = SR.GetString("IO_General");
		}
		return ExceptionDispatchInfo.Capture(new IOException(text, nativeException));
	}

	internal static void EnsureResultsInUserBuffer(IBuffer userBuffer, IBuffer resultBuffer)
	{
		if (!resultBuffer.IsSameData(userBuffer))
		{
			resultBuffer.CopyTo(userBuffer);
			userBuffer.put_Length(resultBuffer.Length);
		}
	}
}
