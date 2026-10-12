using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace System.IO;

internal abstract class NetFxToWinRtStreamAdapter : IDisposable
{
	private class InputStream : NetFxToWinRtStreamAdapter, IInputStream, IDisposable
	{
		internal InputStream(Stream stream, StreamReadOperationOptimization readOptimization)
			: base(stream, readOptimization)
		{
		}
	}

	private class OutputStream : NetFxToWinRtStreamAdapter, IOutputStream, IDisposable
	{
		internal OutputStream(Stream stream, StreamReadOperationOptimization readOptimization)
			: base(stream, readOptimization)
		{
		}
	}

	private class RandomAccessStream : NetFxToWinRtStreamAdapter, IRandomAccessStream, IDisposable, IInputStream, IOutputStream
	{
		internal RandomAccessStream(Stream stream, StreamReadOperationOptimization readOptimization)
			: base(stream, readOptimization)
		{
		}

		[SpecialName]
		void IRandomAccessStream.put_Size(ulong value)
		{
			Size = value;
		}
	}

	private class InputOutputStream : NetFxToWinRtStreamAdapter, IInputStream, IDisposable, IOutputStream
	{
		internal InputOutputStream(Stream stream, StreamReadOperationOptimization readOptimization)
			: base(stream, readOptimization)
		{
		}
	}

	private enum StreamReadOperationOptimization
	{
		AbstractStream,
		MemoryStream
	}

	private Stream managedStream;

	private bool leaveUnderlyingStreamOpen = true;

	private readonly StreamReadOperationOptimization readOptimization;

	public bool CanRead
	{
		get
		{
			Stream stream = EnsureNotDisposed();
			return stream.CanRead;
		}
	}

	public bool CanWrite
	{
		get
		{
			Stream stream = EnsureNotDisposed();
			return stream.CanWrite;
		}
	}

	public ulong Position
	{
		get
		{
			Stream stream = EnsureNotDisposed();
			return (ulong)stream.Position;
		}
	}

	public ulong Size
	{
		get
		{
			Stream stream = EnsureNotDisposed();
			return (ulong)stream.Length;
		}
		set
		{
			if (value > long.MaxValue)
			{
				ArgumentException ex = new ArgumentException(SR.GetString("IO_CannotSetSizeBeyondInt64MaxValue"));
				ex.SetErrorCode(-2147024809);
				throw ex;
			}
			Stream stream = EnsureNotDisposed();
			if (!stream.CanWrite)
			{
				InvalidOperationException ex2 = new InvalidOperationException(SR.GetString("InvalidOperation_CannotSetStreamSizeCannotWrite"));
				ex2.SetErrorCode(-2147483634);
				throw ex2;
			}
			stream.SetLength((long)value);
		}
	}

	internal static NetFxToWinRtStreamAdapter Create(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		StreamReadOperationOptimization streamReadOperationOptimization = StreamReadOperationOptimization.AbstractStream;
		if (stream.CanRead)
		{
			streamReadOperationOptimization = DetermineStreamReadOptimization(stream);
		}
		if (stream.CanSeek)
		{
			return new RandomAccessStream(stream, streamReadOperationOptimization);
		}
		if (stream.CanRead && stream.CanWrite)
		{
			return new InputOutputStream(stream, streamReadOperationOptimization);
		}
		if (stream.CanRead)
		{
			return new InputStream(stream, streamReadOperationOptimization);
		}
		if (stream.CanWrite)
		{
			return new OutputStream(stream, streamReadOperationOptimization);
		}
		throw new ArgumentException(SR.GetString("Argument_NotSufficientCapabilitiesToConvertToWinRtStream"));
	}

	private static StreamReadOperationOptimization DetermineStreamReadOptimization(Stream stream)
	{
		if (CanApplyReadMemoryStreamOptimization(stream))
		{
			return StreamReadOperationOptimization.MemoryStream;
		}
		return StreamReadOperationOptimization.AbstractStream;
	}

	private static bool CanApplyReadMemoryStreamOptimization(Stream stream)
	{
		if (!(stream is MemoryStream memoryStream))
		{
			return false;
		}
		try
		{
			byte[] buffer = memoryStream.GetBuffer();
			return true;
		}
		catch (UnauthorizedAccessException)
		{
			return false;
		}
	}

	private NetFxToWinRtStreamAdapter(Stream stream, StreamReadOperationOptimization readOptimization)
	{
		this.readOptimization = readOptimization;
		managedStream = stream;
	}

	internal void SetWonInitializationRace()
	{
		leaveUnderlyingStreamOpen = false;
	}

	public Stream GetManagedStream()
	{
		return managedStream;
	}

	private Stream EnsureNotDisposed()
	{
		Stream stream = managedStream;
		if (stream == null)
		{
			ObjectDisposedException ex = new ObjectDisposedException(SR.GetString("ObjectDisposed_CannotPerformOperation"));
			ex.SetErrorCode(-2147483629);
			throw ex;
		}
		return stream;
	}

	void IDisposable.Dispose()
	{
		Stream stream = managedStream;
		if (stream != null)
		{
			managedStream = null;
			if (!leaveUnderlyingStreamOpen)
			{
				stream.Dispose();
			}
		}
	}

	public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (count < 0 || int.MaxValue < count)
		{
			ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("count");
			ex.SetErrorCode(-2147024809);
			throw ex;
		}
		if (buffer.Capacity < count)
		{
			ArgumentException ex2 = new ArgumentException(SR.GetString("Argument_InsufficientBufferCapacity"));
			ex2.SetErrorCode(-2147024809);
			throw ex2;
		}
		if ((int)options != 0 && (int)options != 1 && (int)options != 2)
		{
			ArgumentOutOfRangeException ex3 = new ArgumentOutOfRangeException("options", SR.GetString("ArgumentOutOfRange_InvalidInputStreamOptionsEnumValue"));
			ex3.SetErrorCode(-2147024809);
			throw ex3;
		}
		Stream stream = EnsureNotDisposed();
		return readOptimization switch
		{
			StreamReadOperationOptimization.MemoryStream => StreamOperationsImplementation.ReadAsync_MemoryStream(stream, buffer, count), 
			StreamReadOperationOptimization.AbstractStream => StreamOperationsImplementation.ReadAsync_AbstractStream(stream, buffer, count, options), 
			_ => (IAsyncOperationWithProgress<IBuffer, uint>)null, 
		};
	}

	public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (buffer.Capacity < buffer.Length)
		{
			ArgumentException ex = new ArgumentException(SR.GetString("Argument_BufferLengthExceedsCapacity"));
			ex.SetErrorCode(-2147024809);
			throw ex;
		}
		Stream stream = EnsureNotDisposed();
		return StreamOperationsImplementation.WriteAsync_AbstractStream(stream, buffer);
	}

	public IAsyncOperation<bool> FlushAsync()
	{
		Stream stream = EnsureNotDisposed();
		return StreamOperationsImplementation.FlushAsync_AbstractStream(stream);
	}

	public void Seek(ulong position)
	{
		if (position > long.MaxValue)
		{
			ArgumentException ex = new ArgumentException(SR.GetString("IO_CannotSeekBeyondInt64MaxValue"));
			ex.SetErrorCode(-2147024809);
			throw ex;
		}
		Stream stream = EnsureNotDisposed();
		stream.Seek((long)position, SeekOrigin.Begin);
	}

	private static void ThrowCloningNotSuported(string methodName)
	{
		NotSupportedException ex = new NotSupportedException(SR.GetString("NotSupported_CloningNotSupported", methodName));
		ex.SetErrorCode(-2147467263);
		throw ex;
	}

	public IRandomAccessStream CloneStream()
	{
		ThrowCloningNotSuported("CloneStream");
		return null;
	}

	public IInputStream GetInputStreamAt(ulong position)
	{
		ThrowCloningNotSuported("GetInputStreamAt");
		return null;
	}

	public IOutputStream GetOutputStreamAt(ulong position)
	{
		ThrowCloningNotSuported("GetOutputStreamAt");
		return null;
	}
}
