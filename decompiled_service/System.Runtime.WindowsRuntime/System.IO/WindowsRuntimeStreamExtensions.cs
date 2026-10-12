using System.Runtime.CompilerServices;
using Windows.Storage.Streams;

namespace System.IO;

[__DynamicallyInvokable]
public static class WindowsRuntimeStreamExtensions
{
	private const int DefaultBufferSize = 16384;

	private static ConditionalWeakTable<object, Stream> WinRtToNetFxAdapterMap = new ConditionalWeakTable<object, Stream>();

	private static ConditionalWeakTable<Stream, NetFxToWinRtStreamAdapter> NetFxToWinRtAdapterMap = new ConditionalWeakTable<Stream, NetFxToWinRtStreamAdapter>();

	private static void EnsureAdapterBufferSize(Stream adapter, int requiredBufferSize, string methodName)
	{
		int num = 0;
		if (adapter is BufferedStream bufferedStream)
		{
			num = bufferedStream.BufferSize;
		}
		if (requiredBufferSize != num)
		{
			if (requiredBufferSize == 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_CannotChangeBufferSizeOfWinRtStreamAdapterToZero", methodName));
			}
			throw new InvalidOperationException(SR.GetString("InvalidOperation_CannotChangeBufferSizeOfWinRtStreamAdapter", methodName));
		}
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Stream AsStreamForRead(this IInputStream windowsRuntimeStream)
	{
		return AsStreamInternal(windowsRuntimeStream, 16384, "AsStreamForRead", forceBufferSize: false);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Stream AsStreamForRead(this IInputStream windowsRuntimeStream, int bufferSize)
	{
		return AsStreamInternal(windowsRuntimeStream, bufferSize, "AsStreamForRead", forceBufferSize: true);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Stream AsStreamForWrite(this IOutputStream windowsRuntimeStream)
	{
		return AsStreamInternal(windowsRuntimeStream, 16384, "AsStreamForWrite", forceBufferSize: false);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Stream AsStreamForWrite(this IOutputStream windowsRuntimeStream, int bufferSize)
	{
		return AsStreamInternal(windowsRuntimeStream, bufferSize, "AsStreamForWrite", forceBufferSize: true);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Stream AsStream(this IRandomAccessStream windowsRuntimeStream)
	{
		return AsStreamInternal(windowsRuntimeStream, 16384, "AsStream", forceBufferSize: false);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Stream AsStream(this IRandomAccessStream windowsRuntimeStream, int bufferSize)
	{
		return AsStreamInternal(windowsRuntimeStream, bufferSize, "AsStream", forceBufferSize: true);
	}

	private static Stream AsStreamInternal(object windowsRuntimeStream, int bufferSize, string invokedMethodName, bool forceBufferSize)
	{
		if (windowsRuntimeStream == null)
		{
			throw new ArgumentNullException("windowsRuntimeStream");
		}
		if (bufferSize < 0)
		{
			throw new ArgumentOutOfRangeException("bufferSize", SR.GetString("ArgumentOutOfRange_WinRtAdapterBufferSizeMayNotBeNegative"));
		}
		if (windowsRuntimeStream is NetFxToWinRtStreamAdapter netFxToWinRtStreamAdapter)
		{
			Stream managedStream = netFxToWinRtStreamAdapter.GetManagedStream();
			if (managedStream == null)
			{
				throw new ObjectDisposedException("windowsRuntimeStream", SR.GetString("ObjectDisposed_CannotPerformOperation"));
			}
			return managedStream;
		}
		if (WinRtToNetFxAdapterMap.TryGetValue(windowsRuntimeStream, out var value))
		{
			if (forceBufferSize)
			{
				EnsureAdapterBufferSize(value, bufferSize, invokedMethodName);
			}
			return value;
		}
		return AsStreamInternalFactoryHelper(windowsRuntimeStream, bufferSize, invokedMethodName, forceBufferSize);
	}

	private static Stream WinRtToNetFxAdapterMap_GetValue(object winRtStream)
	{
		return WinRtToNetFxAdapterMap.GetValue(winRtStream, (object wrtStr) => WinRtToNetFxStreamAdapter.Create(wrtStr));
	}

	private static Stream WinRtToNetFxAdapterMap_GetValue(object winRtStream, int bufferSize)
	{
		return WinRtToNetFxAdapterMap.GetValue(winRtStream, (object wrtStr) => new BufferedStream(WinRtToNetFxStreamAdapter.Create(wrtStr), bufferSize));
	}

	private static Stream AsStreamInternalFactoryHelper(object windowsRuntimeStream, int bufferSize, string invokedMethodName, bool forceBufferSize)
	{
		Stream stream = ((bufferSize == 0) ? WinRtToNetFxAdapterMap_GetValue(windowsRuntimeStream) : WinRtToNetFxAdapterMap_GetValue(windowsRuntimeStream, bufferSize));
		if (forceBufferSize)
		{
			EnsureAdapterBufferSize(stream, bufferSize, invokedMethodName);
		}
		WinRtToNetFxStreamAdapter winRtToNetFxStreamAdapter = stream as WinRtToNetFxStreamAdapter;
		if (winRtToNetFxStreamAdapter == null)
		{
			winRtToNetFxStreamAdapter = ((BufferedStream)stream).UnderlyingStream as WinRtToNetFxStreamAdapter;
		}
		winRtToNetFxStreamAdapter.SetWonInitializationRace();
		return stream;
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IInputStream AsInputStream(this Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (!stream.CanRead)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_CannotConvertNotReadableToInputStream"));
		}
		object obj = AsWindowsRuntimeStreamInternal(stream);
		return (IInputStream)((obj is IInputStream) ? obj : null);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IOutputStream AsOutputStream(this Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (!stream.CanWrite)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_CannotConvertNotWritableToOutputStream"));
		}
		object obj = AsWindowsRuntimeStreamInternal(stream);
		return (IOutputStream)((obj is IOutputStream) ? obj : null);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IRandomAccessStream AsRandomAccessStream(this Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (!stream.CanSeek)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_CannotConvertNotSeekableToRandomAccessStream"));
		}
		object obj = AsWindowsRuntimeStreamInternal(stream);
		return (IRandomAccessStream)((obj is IRandomAccessStream) ? obj : null);
	}

	private static object AsWindowsRuntimeStreamInternal(Stream stream)
	{
		WinRtToNetFxStreamAdapter winRtToNetFxStreamAdapter = stream as WinRtToNetFxStreamAdapter;
		if (winRtToNetFxStreamAdapter == null && stream is BufferedStream bufferedStream)
		{
			winRtToNetFxStreamAdapter = bufferedStream.UnderlyingStream as WinRtToNetFxStreamAdapter;
		}
		if (winRtToNetFxStreamAdapter != null)
		{
			object windowsRuntimeStream = winRtToNetFxStreamAdapter.GetWindowsRuntimeStream<object>();
			if (windowsRuntimeStream == null)
			{
				throw new ObjectDisposedException("stream", SR.GetString("ObjectDisposed_CannotPerformOperation"));
			}
			return windowsRuntimeStream;
		}
		if (NetFxToWinRtAdapterMap.TryGetValue(stream, out var value))
		{
			return value;
		}
		return AsWindowsRuntimeStreamInternalFactoryHelper(stream);
	}

	private static NetFxToWinRtStreamAdapter AsWindowsRuntimeStreamInternalFactoryHelper(Stream stream)
	{
		NetFxToWinRtStreamAdapter value = NetFxToWinRtAdapterMap.GetValue(stream, (Stream str) => NetFxToWinRtStreamAdapter.Create(str));
		value.SetWonInitializationRace();
		return value;
	}
}
