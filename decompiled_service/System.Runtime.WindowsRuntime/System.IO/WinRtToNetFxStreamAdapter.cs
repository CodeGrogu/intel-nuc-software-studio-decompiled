using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Versioning;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace System.IO;

[ComVisible(true)]
internal class WinRtToNetFxStreamAdapter : Stream, IDisposable
{
	private byte[] oneByteBuffer;

	private bool leaveUnderlyingStreamOpen = true;

	private object winRtStream;

	private readonly bool canRead;

	private readonly bool canWrite;

	private readonly bool canSeek;

	private byte[] OneByteBuffer
	{
		get
		{
			byte[] array = oneByteBuffer;
			if (array == null)
			{
				array = (oneByteBuffer = new byte[1]);
			}
			return array;
		}
	}

	public override bool CanRead
	{
		get
		{
			if (canRead)
			{
				return winRtStream != null;
			}
			return false;
		}
	}

	public override bool CanWrite
	{
		get
		{
			if (canWrite)
			{
				return winRtStream != null;
			}
			return false;
		}
	}

	public override bool CanSeek
	{
		get
		{
			if (canSeek)
			{
				return winRtStream != null;
			}
			return false;
		}
	}

	public override long Length
	{
		get
		{
			IRandomAccessStream val = EnsureNotDisposed<IRandomAccessStream>();
			if (!canSeek)
			{
				throw new NotSupportedException(SR.GetString("NotSupported_CannotUseLength_StreamNotSeekable"));
			}
			ulong size = val.Size;
			if (size > long.MaxValue)
			{
				throw new IOException(SR.GetString("IO_UnderlyingWinRTStreamTooLong_CannotUseLengthOrPosition"));
			}
			return (long)size;
		}
	}

	public override long Position
	{
		get
		{
			IRandomAccessStream val = EnsureNotDisposed<IRandomAccessStream>();
			if (!canSeek)
			{
				throw new NotSupportedException(SR.GetString("NotSupported_CannotUsePosition_StreamNotSeekable"));
			}
			ulong position = val.Position;
			if (position > long.MaxValue)
			{
				throw new IOException(SR.GetString("IO_UnderlyingWinRTStreamTooLong_CannotUseLengthOrPosition"));
			}
			return (long)position;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("Position", SR.GetString("ArgumentOutOfRange_IO_CannotSeekToNegativePosition"));
			}
			IRandomAccessStream val = EnsureNotDisposed<IRandomAccessStream>();
			if (!canSeek)
			{
				throw new NotSupportedException(SR.GetString("NotSupported_CannotUsePosition_StreamNotSeekable"));
			}
			val.Seek((ulong)value);
		}
	}

	internal static WinRtToNetFxStreamAdapter Create(object windowsRuntimeStream)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected Obj, but got Unknown
		if (windowsRuntimeStream == null)
		{
			throw new ArgumentNullException("windowsRuntimeStream");
		}
		bool flag = windowsRuntimeStream is IInputStream;
		bool flag2 = windowsRuntimeStream is IOutputStream;
		bool flag3 = windowsRuntimeStream is IRandomAccessStream;
		if (!flag && !flag2 && !flag3)
		{
			throw new ArgumentException(SR.GetString("Argument_ObjectMustBeWinRtStreamToConvertToNetFxStream"));
		}
		if (flag3)
		{
			IRandomAccessStream val = (IRandomAccessStream)windowsRuntimeStream;
			if (!flag && val.CanRead)
			{
				throw new ArgumentException(SR.GetString("Argument_InstancesImplementingIRASThatCanReadMustImplementIIS"));
			}
			if (!flag2 && val.CanWrite)
			{
				throw new ArgumentException(SR.GetString("Argument_InstancesImplementingIRASThatCanWriteMustImplementIOS"));
			}
			if (!val.CanRead)
			{
				flag = false;
			}
			if (!val.CanWrite)
			{
				flag2 = false;
			}
		}
		if (!flag && !flag2)
		{
			throw new ArgumentException(SR.GetString("Argument_WinRtStreamCannotReadOrWrite"));
		}
		return new WinRtToNetFxStreamAdapter(windowsRuntimeStream, flag, flag2, flag3);
	}

	private WinRtToNetFxStreamAdapter(object winRtStream, bool canRead, bool canWrite, bool canSeek)
	{
		this.winRtStream = winRtStream;
		this.canRead = canRead;
		this.canWrite = canWrite;
		this.canSeek = canSeek;
	}

	internal void SetWonInitializationRace()
	{
		leaveUnderlyingStreamOpen = false;
	}

	public TWinRtStream GetWindowsRuntimeStream<TWinRtStream>() where TWinRtStream : class
	{
		object obj = winRtStream;
		if (obj == null)
		{
			return null;
		}
		return obj as TWinRtStream;
	}

	private TWinRtStream EnsureNotDisposed<TWinRtStream>() where TWinRtStream : class
	{
		object obj = winRtStream;
		if (obj == null)
		{
			throw new ObjectDisposedException(SR.GetString("ObjectDisposed_CannotPerformOperation"));
		}
		return obj as TWinRtStream;
	}

	private void EnsureNotDisposed()
	{
		if (winRtStream == null)
		{
			throw new ObjectDisposedException(SR.GetString("ObjectDisposed_CannotPerformOperation"));
		}
	}

	private void EnsureCanRead()
	{
		if (!canRead)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_CannotReadFromStream"));
		}
	}

	private void EnsureCanWrite()
	{
		if (!canWrite)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_CannotWriteToStream"));
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && winRtStream != null && !leaveUnderlyingStreamOpen && winRtStream is IDisposable disposable)
		{
			disposable.Dispose();
		}
		winRtStream = null;
		base.Dispose(disposing);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		IRandomAccessStream val = EnsureNotDisposed<IRandomAccessStream>();
		if (!canSeek)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_CannotSeekInStream"));
		}
		switch (origin)
		{
		case SeekOrigin.Begin:
			Position = offset;
			return offset;
		case SeekOrigin.Current:
		{
			long position = Position;
			if (long.MaxValue - position < offset)
			{
				throw new IOException(SR.GetString("IO_CannotSeekBeyondInt64MaxValue"));
			}
			long num5 = position + offset;
			if (num5 < 0)
			{
				throw new IOException(SR.GetString("ArgumentOutOfRange_IO_CannotSeekToNegativePosition"));
			}
			Position = num5;
			return num5;
		}
		case SeekOrigin.End:
		{
			ulong size = val.Size;
			long num3;
			if (size > long.MaxValue)
			{
				if (offset >= 0)
				{
					throw new IOException(SR.GetString("IO_CannotSeekBeyondInt64MaxValue"));
				}
				ulong num = ((offset == long.MinValue) ? 9223372036854775808uL : ((ulong)(-offset)));
				ulong num2 = size - num;
				if (num2 > long.MaxValue)
				{
					throw new IOException(SR.GetString("IO_CannotSeekBeyondInt64MaxValue"));
				}
				num3 = (long)num2;
			}
			else
			{
				long num4 = (long)size;
				if (long.MaxValue - num4 < offset)
				{
					throw new IOException(SR.GetString("IO_CannotSeekBeyondInt64MaxValue"));
				}
				num3 = num4 + offset;
				if (num3 < 0)
				{
					throw new IOException(SR.GetString("ArgumentOutOfRange_IO_CannotSeekToNegativePosition"));
				}
			}
			Position = num3;
			return num3;
		}
		default:
			throw new ArgumentException("origin");
		}
	}

	public override void SetLength(long value)
	{
		if (value < 0)
		{
			throw new ArgumentOutOfRangeException("value", SR.GetString("ArgumentOutOfRange_CannotResizeStreamToNegative"));
		}
		IRandomAccessStream val = EnsureNotDisposed<IRandomAccessStream>();
		if (!canSeek)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_CannotSeekInStream"));
		}
		EnsureCanWrite();
		val.put_Size((ulong)value);
		if (val.Size < val.Position)
		{
			val.Seek((ulong)value);
		}
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return BeginRead(buffer, offset, count, callback, state, usedByBlockingWrapper: false);
	}

	private IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state, bool usedByBlockingWrapper)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (buffer.Length - offset < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientSpaceInTargetBuffer"));
		}
		IInputStream val = EnsureNotDisposed<IInputStream>();
		EnsureCanRead();
		IBuffer val2 = buffer.AsBuffer(offset, count);
		IAsyncOperationWithProgress<IBuffer, uint> asyncStreamReadOperation = val.ReadAsync(val2, (uint)count, (InputStreamOptions)1);
		return new StreamReadAsyncResult(asyncStreamReadOperation, val2, callback, state, !usedByBlockingWrapper);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException("asyncResult");
		}
		EnsureNotDisposed();
		EnsureCanRead();
		if (!(asyncResult is StreamOperationAsyncResult streamOperationAsyncResult))
		{
			throw new ArgumentException(SR.GetString("Argument_UnexpectedAsyncResult"), "asyncResult");
		}
		streamOperationAsyncResult.Wait();
		try
		{
			if (!streamOperationAsyncResult.ProcessCompletedOperationInCallback)
			{
				streamOperationAsyncResult.ProcessCompletedOperation();
			}
			if (streamOperationAsyncResult.HasError)
			{
				streamOperationAsyncResult.CloseStreamOperation();
				streamOperationAsyncResult.ThrowCachedError();
			}
			long bytesCompleted = streamOperationAsyncResult.BytesCompleted;
			return (int)bytesCompleted;
		}
		finally
		{
			streamOperationAsyncResult.CloseStreamOperation();
		}
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (buffer.Length - offset < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientSpaceInTargetBuffer"));
		}
		EnsureNotDisposed();
		EnsureCanRead();
		cancellationToken.ThrowIfCancellationRequested();
		return ReadAsyncInternal(buffer, offset, count, cancellationToken);
	}

	public override int Read([In][Out] byte[] buffer, int offset, int count)
	{
		IAsyncResult asyncResult = BeginRead(buffer, offset, count, null, null, usedByBlockingWrapper: true);
		return EndRead(asyncResult);
	}

	public override int ReadByte()
	{
		byte[] array = OneByteBuffer;
		if (Read(array, 0, 1) == 0)
		{
			return -1;
		}
		return array[0];
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return BeginWrite(buffer, offset, count, callback, state, usedByBlockingWrapper: false);
	}

	private IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state, bool usedByBlockingWrapper)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (buffer.Length - offset < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		IOutputStream val = EnsureNotDisposed<IOutputStream>();
		EnsureCanWrite();
		IBuffer val2 = buffer.AsBuffer(offset, count);
		IAsyncOperationWithProgress<uint, uint> asyncStreamWriteOperation = val.WriteAsync(val2);
		return new StreamWriteAsyncResult(asyncStreamWriteOperation, callback, state, !usedByBlockingWrapper);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException("asyncResult");
		}
		EnsureNotDisposed();
		EnsureCanWrite();
		if (!(asyncResult is StreamOperationAsyncResult streamOperationAsyncResult))
		{
			throw new ArgumentException(SR.GetString("Argument_UnexpectedAsyncResult"), "asyncResult");
		}
		streamOperationAsyncResult.Wait();
		try
		{
			if (!streamOperationAsyncResult.ProcessCompletedOperationInCallback)
			{
				streamOperationAsyncResult.ProcessCompletedOperation();
			}
			if (streamOperationAsyncResult.HasError)
			{
				streamOperationAsyncResult.CloseStreamOperation();
				streamOperationAsyncResult.ThrowCachedError();
			}
		}
		finally
		{
			streamOperationAsyncResult.CloseStreamOperation();
		}
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (buffer.Length - offset < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		IOutputStream val = EnsureNotDisposed<IOutputStream>();
		EnsureCanWrite();
		cancellationToken.ThrowIfCancellationRequested();
		IBuffer val2 = buffer.AsBuffer(offset, count);
		IAsyncOperationWithProgress<uint, uint> source = val.WriteAsync(val2);
		return source.AsTask<uint, uint>(cancellationToken);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		IAsyncResult asyncResult = BeginWrite(buffer, offset, count, null, null, usedByBlockingWrapper: true);
		EndWrite(asyncResult);
	}

	public override void WriteByte(byte value)
	{
		byte[] array = OneByteBuffer;
		array[0] = value;
		Write(array, 0, 1);
	}

	public override void Flush()
	{
		if (!CompatibilitySwitch.IsEnabled("WinRtToNetFxStreamAdapter_FlushAsyncOnFlush"))
		{
			return;
		}
		IOutputStream val = EnsureNotDisposed<IOutputStream>();
		if (!canWrite)
		{
			return;
		}
		IAsyncOperation<bool> asyncStreamFlushOperation = val.FlushAsync();
		StreamFlushAsyncResult streamFlushAsyncResult = new StreamFlushAsyncResult(asyncStreamFlushOperation, processCompletedOperationInCallback: false);
		streamFlushAsyncResult.Wait();
		try
		{
			streamFlushAsyncResult.ProcessCompletedOperation();
			if (streamFlushAsyncResult.HasError)
			{
				streamFlushAsyncResult.CloseStreamOperation();
				streamFlushAsyncResult.ThrowCachedError();
			}
		}
		finally
		{
			streamFlushAsyncResult.CloseStreamOperation();
		}
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		IOutputStream val = EnsureNotDisposed<IOutputStream>();
		if (!canWrite)
		{
			return Task.CompletedTask;
		}
		cancellationToken.ThrowIfCancellationRequested();
		IAsyncOperation<bool> source = val.FlushAsync();
		return source.AsTask<bool>(cancellationToken);
	}

	private async Task<int> ReadAsyncInternal(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		IInputStream val = EnsureNotDisposed<IInputStream>();
		try
		{
			IBuffer userBuffer = buffer.AsBuffer(offset, count);
			IAsyncOperationWithProgress<IBuffer, uint> source = val.ReadAsync(userBuffer, (uint)count, (InputStreamOptions)1);
			IBuffer val2 = await source.AsTask<IBuffer, uint>(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (val2 == null)
			{
				return 0;
			}
			WinRtIOHelper.EnsureResultsInUserBuffer(userBuffer, val2);
			return (int)val2.Length;
		}
		catch (Exception nativeException)
		{
			WinRtIOHelper.NativeExceptionToIOExceptionInfo(nativeException).Throw();
			return 0;
		}
	}
}
