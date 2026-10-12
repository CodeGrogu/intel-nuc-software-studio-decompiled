using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace System.IO;

internal static class StreamOperationsImplementation
{
	internal static IAsyncOperationWithProgress<IBuffer, uint> ReadAsync_MemoryStream(Stream stream, IBuffer buffer, uint count)
	{
		buffer.put_Length(0u);
		MemoryStream memoryStream = stream as MemoryStream;
		try
		{
			IBuffer windowsRuntimeBuffer = memoryStream.GetWindowsRuntimeBuffer((int)memoryStream.Position, (int)count);
			if (windowsRuntimeBuffer.Length != 0)
			{
				memoryStream.Seek(windowsRuntimeBuffer.Length, SeekOrigin.Current);
			}
			return AsyncInfo.CreateCompletedOperation<IBuffer, uint>(windowsRuntimeBuffer);
		}
		catch (Exception error)
		{
			return AsyncInfo.CreateFaultedOperation<IBuffer, uint>(error);
		}
	}

	internal static IAsyncOperationWithProgress<IBuffer, uint> ReadAsync_AbstractStream(Stream stream, IBuffer buffer, uint count, InputStreamOptions options)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		int bytesRequested = (int)count;
		IBuffer dataBuffer = (IBuffer)(object)(buffer as WindowsRuntimeBuffer);
		if (dataBuffer == null)
		{
			dataBuffer = WindowsRuntimeBuffer.Create((int)Math.Min(2147483647u, buffer.Capacity));
		}
		Func<CancellationToken, IProgress<uint>, Task<IBuffer>> taskProvider = async (CancellationToken cancelToken, IProgress<uint> progressListener) =>
		{
			dataBuffer.put_Length(0u);
			dataBuffer.TryGetUnderlyingData(out var data, out var offset);
			bool flag = cancelToken.IsCancellationRequested;
			int bytesCompleted = 0;
			while (!flag)
			{
				int bytesRead = 0;
				try
				{
					bytesRead = await stream.ReadAsync(data, offset + bytesCompleted, bytesRequested - bytesCompleted, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
					bytesCompleted += bytesRead;
				}
				catch (OperationCanceledException)
				{
					if (bytesCompleted == 0 && bytesRead == 0)
					{
						throw;
					}
				}
				dataBuffer.put_Length((uint)bytesCompleted);
				flag = (int)options == 1 || bytesRead == 0 || bytesCompleted == bytesRequested || cancelToken.IsCancellationRequested;
				progressListener?.Report(dataBuffer.Length);
			}
			return dataBuffer;
		};
		return AsyncInfo.Run(taskProvider);
	}

	internal static IAsyncOperationWithProgress<uint, uint> WriteAsync_AbstractStream(Stream stream, IBuffer buffer)
	{
		Func<CancellationToken, IProgress<uint>, Task<uint>> taskProvider = ((!buffer.TryGetUnderlyingData(out var data, out var offset)) ? ((Func<CancellationToken, IProgress<uint>, Task<uint>>)(async (CancellationToken cancelToken, IProgress<uint> progressListener) =>
		{
			if (cancelToken.IsCancellationRequested)
			{
				return 0u;
			}
			uint bytesToWrite = buffer.Length;
			Stream stream2 = buffer.AsStream();
			int num = 16384;
			if (bytesToWrite < num)
			{
				num = (int)bytesToWrite;
			}
			await stream2.CopyToAsync(stream, num, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
			progressListener?.Report(bytesToWrite);
			return bytesToWrite;
		})) : ((Func<CancellationToken, IProgress<uint>, Task<uint>>)(async (CancellationToken cancelToken, IProgress<uint> progressListener) =>
		{
			if (cancelToken.IsCancellationRequested)
			{
				return 0u;
			}
			int bytesToWrite = (int)buffer.Length;
			await stream.WriteAsync(data, offset, bytesToWrite, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
			progressListener?.Report((uint)bytesToWrite);
			return (uint)bytesToWrite;
		})));
		return AsyncInfo.Run(taskProvider);
	}

	internal static IAsyncOperation<bool> FlushAsync_AbstractStream(Stream stream)
	{
		Func<CancellationToken, Task<bool>> taskProvider = async (CancellationToken cancelToken) =>
		{
			if (cancelToken.IsCancellationRequested)
			{
				return false;
			}
			await stream.FlushAsync(cancelToken).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		};
		return AsyncInfo.Run(taskProvider);
	}
}
