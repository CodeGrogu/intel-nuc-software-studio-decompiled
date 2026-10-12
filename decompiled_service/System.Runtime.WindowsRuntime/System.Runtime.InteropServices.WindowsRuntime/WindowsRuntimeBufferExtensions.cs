using System.IO;
using Windows.Storage.Streams;

namespace System.Runtime.InteropServices.WindowsRuntime;

[__DynamicallyInvokable]
public static class WindowsRuntimeBufferExtensions
{
	private class WindowsRuntimeBufferUnmanagedMemoryStream : UnmanagedMemoryStream
	{
		private IBuffer sourceBuffer;

		internal unsafe WindowsRuntimeBufferUnmanagedMemoryStream(IBuffer sourceBuffer, byte* dataPtr)
			: base(dataPtr, sourceBuffer.Length, sourceBuffer.Capacity, FileAccess.ReadWrite)
		{
			this.sourceBuffer = sourceBuffer;
		}
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IBuffer AsBuffer(this byte[] source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return source.AsBuffer(0, source.Length, source.Length);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IBuffer AsBuffer(this byte[] source, int offset, int length)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (source.Length - offset < length)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		return source.AsBuffer(offset, length, length);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IBuffer AsBuffer(this byte[] source, int offset, int length, int capacity)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		if (source.Length - offset < length)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		if (source.Length - offset < capacity)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		if (capacity < length)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientBufferCapacity"));
		}
		return (IBuffer)(object)new WindowsRuntimeBuffer(source, offset, length, capacity);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static void CopyTo(this byte[] source, IBuffer destination)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		source.CopyTo(0, destination, 0u, source.Length);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static void CopyTo(this byte[] source, int sourceIndex, IBuffer destination, uint destinationIndex, int count)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (sourceIndex < 0)
		{
			throw new ArgumentOutOfRangeException("sourceIndex");
		}
		if (destinationIndex < 0)
		{
			throw new ArgumentOutOfRangeException("destinationIndex");
		}
		if (source.Length <= sourceIndex)
		{
			throw new ArgumentException(SR.GetString("Argument_IndexOutOfArrayBounds"), "sourceIndex");
		}
		if (source.Length - sourceIndex < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		if (destination.Capacity - destinationIndex < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientSpaceInTargetBuffer"));
		}
		if (destination.TryGetUnderlyingData(out var underlyingDataArray, out var underlyingDataArrayStartOffset))
		{
			Array.Copy(source, sourceIndex, underlyingDataArray, underlyingDataArrayStartOffset + destinationIndex, count);
			return;
		}
		IntPtr pointerAtOffset = destination.GetPointerAtOffset(destinationIndex);
		Marshal.Copy(source, sourceIndex, pointerAtOffset, count);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static byte[] ToArray(this IBuffer source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return source.ToArray(0u, checked((int)source.Length));
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static byte[] ToArray(this IBuffer source, uint sourceIndex, int count)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (sourceIndex < 0)
		{
			throw new ArgumentOutOfRangeException("sourceIndex");
		}
		if (source.Capacity <= sourceIndex)
		{
			throw new ArgumentException(SR.GetString("Argument_BufferIndexExceedsCapacity"));
		}
		if (source.Capacity - sourceIndex < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientSpaceInSourceBuffer"));
		}
		byte[] array = new byte[count];
		source.CopyTo(sourceIndex, array, 0, count);
		return array;
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static void CopyTo(this IBuffer source, byte[] destination)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		source.CopyTo(0u, destination, 0, checked((int)source.Length));
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static void CopyTo(this IBuffer source, uint sourceIndex, byte[] destination, int destinationIndex, int count)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (sourceIndex < 0)
		{
			throw new ArgumentOutOfRangeException("sourceIndex");
		}
		if (destinationIndex < 0)
		{
			throw new ArgumentOutOfRangeException("destinationIndex");
		}
		if (source.Capacity <= sourceIndex)
		{
			throw new ArgumentException(SR.GetString("Argument_BufferIndexExceedsCapacity"));
		}
		if (source.Capacity - sourceIndex < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientSpaceInSourceBuffer"));
		}
		if (destination.Length <= destinationIndex)
		{
			throw new ArgumentException(SR.GetString("Argument_IndexOutOfArrayBounds"));
		}
		if (destination.Length - destinationIndex < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		if (source.TryGetUnderlyingData(out var underlyingDataArray, out var underlyingDataArrayStartOffset))
		{
			Array.Copy(underlyingDataArray, underlyingDataArrayStartOffset + sourceIndex, destination, destinationIndex, count);
			return;
		}
		IntPtr pointerAtOffset = source.GetPointerAtOffset(sourceIndex);
		Marshal.Copy(pointerAtOffset, destination, destinationIndex, count);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static void CopyTo(this IBuffer source, IBuffer destination)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		source.CopyTo(0u, destination, 0u, source.Length);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static void CopyTo(this IBuffer source, uint sourceIndex, IBuffer destination, uint destinationIndex, uint count)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (sourceIndex < 0)
		{
			throw new ArgumentOutOfRangeException("sourceIndex");
		}
		if (destinationIndex < 0)
		{
			throw new ArgumentOutOfRangeException("destinationIndex");
		}
		if (source.Capacity <= sourceIndex)
		{
			throw new ArgumentException(SR.GetString("Argument_BufferIndexExceedsCapacity"));
		}
		if (source.Capacity - sourceIndex < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientSpaceInSourceBuffer"));
		}
		if (destination.Capacity <= destinationIndex)
		{
			throw new ArgumentException(SR.GetString("Argument_BufferIndexExceedsCapacity"));
		}
		if (destination.Capacity - destinationIndex < count)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientSpaceInTargetBuffer"));
		}
		bool flag = source.TryGetUnderlyingData(out var underlyingDataArray, out var underlyingDataArrayStartOffset);
		bool flag2 = destination.TryGetUnderlyingData(out var underlyingDataArray2, out var underlyingDataArrayStartOffset2);
		if (flag & flag2)
		{
			Array.Copy(underlyingDataArray, underlyingDataArrayStartOffset + (int)sourceIndex, underlyingDataArray2, underlyingDataArrayStartOffset2 + (int)destinationIndex, (int)count);
		}
		else if (flag)
		{
			IntPtr pointerAtOffset = destination.GetPointerAtOffset(destinationIndex);
			Marshal.Copy(underlyingDataArray, underlyingDataArrayStartOffset + (int)sourceIndex, pointerAtOffset, (int)count);
		}
		else if (flag2)
		{
			IntPtr pointerAtOffset2 = source.GetPointerAtOffset(sourceIndex);
			Marshal.Copy(pointerAtOffset2, underlyingDataArray2, underlyingDataArrayStartOffset2 + (int)destinationIndex, (int)count);
		}
		else
		{
			IntPtr pointerAtOffset2 = source.GetPointerAtOffset(sourceIndex);
			IntPtr pointerAtOffset = destination.GetPointerAtOffset(destinationIndex);
			MemCopy(pointerAtOffset2, pointerAtOffset, count);
		}
	}

	internal static bool TryGetUnderlyingData(this IBuffer buffer, out byte[] underlyingDataArray, out int underlyingDataArrayStartOffset)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (!(buffer is WindowsRuntimeBuffer windowsRuntimeBuffer))
		{
			underlyingDataArray = null;
			underlyingDataArrayStartOffset = -1;
			return false;
		}
		windowsRuntimeBuffer.GetUnderlyingData(out underlyingDataArray, out underlyingDataArrayStartOffset);
		return true;
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public unsafe static bool IsSameData(this IBuffer buffer, IBuffer otherBuffer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (otherBuffer == null)
		{
			return false;
		}
		if (buffer == otherBuffer)
		{
			return true;
		}
		bool flag = buffer.TryGetUnderlyingData(out var underlyingDataArray, out var underlyingDataArrayStartOffset);
		bool flag2 = otherBuffer.TryGetUnderlyingData(out var underlyingDataArray2, out var underlyingDataArrayStartOffset2);
		if (flag != flag2)
		{
			return false;
		}
		if (flag)
		{
			if (underlyingDataArray == underlyingDataArray2)
			{
				return underlyingDataArrayStartOffset == underlyingDataArrayStartOffset2;
			}
			return false;
		}
		IBufferByteAccess bufferByteAccess = (IBufferByteAccess)buffer;
		IBufferByteAccess bufferByteAccess2 = (IBufferByteAccess)otherBuffer;
		return bufferByteAccess.Buffer == bufferByteAccess2.Buffer;
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IBuffer GetWindowsRuntimeBuffer(this MemoryStream underlyingStream)
	{
		if (underlyingStream == null)
		{
			throw new ArgumentNullException("underlyingStream");
		}
		underlyingStream.InternalGetOriginAndLength(out var origin, out var _);
		byte[] buffer = underlyingStream.GetBuffer();
		return (IBuffer)(object)new WindowsRuntimeBuffer(buffer, origin, (int)underlyingStream.Length, underlyingStream.Capacity);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IBuffer GetWindowsRuntimeBuffer(this MemoryStream underlyingStream, int positionInStream, int length)
	{
		if (underlyingStream == null)
		{
			throw new ArgumentNullException("underlyingStream");
		}
		if (positionInStream < 0)
		{
			throw new ArgumentOutOfRangeException("positionInStream");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (underlyingStream.Capacity <= positionInStream)
		{
			throw new ArgumentException(SR.GetString("Argument_StreamPositionBeyondEOS"));
		}
		underlyingStream.InternalGetOriginAndLength(out var origin, out var _);
		byte[] buffer = underlyingStream.GetBuffer();
		int capacity = Math.Min(length, underlyingStream.Capacity - positionInStream);
		int length3 = Math.Max(0, Math.Min(length, (int)underlyingStream.Length - positionInStream));
		return (IBuffer)(object)new WindowsRuntimeBuffer(buffer, origin + positionInStream, length3, capacity);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public unsafe static Stream AsStream(this IBuffer source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source.TryGetUnderlyingData(out var underlyingDataArray, out var underlyingDataArrayStartOffset))
		{
			return new MemoryStream(underlyingDataArray, underlyingDataArrayStartOffset, (int)source.Capacity, writable: true, publiclyVisible: true);
		}
		IBufferByteAccess bufferByteAccess = (IBufferByteAccess)source;
		return new WindowsRuntimeBufferUnmanagedMemoryStream(source, bufferByteAccess.Buffer);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public unsafe static byte GetByte(this IBuffer source, uint byteOffset)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (byteOffset < 0)
		{
			throw new ArgumentOutOfRangeException("byteOffset");
		}
		if (source.Capacity <= byteOffset)
		{
			throw new ArgumentException(SR.GetString("Argument_BufferIndexExceedsCapacity"), "byteOffset");
		}
		if (source.TryGetUnderlyingData(out var underlyingDataArray, out var underlyingDataArrayStartOffset))
		{
			return underlyingDataArray[underlyingDataArrayStartOffset + byteOffset];
		}
		IntPtr pointerAtOffset = source.GetPointerAtOffset(byteOffset);
		try
		{
			byte* ptr = (byte*)(void*)pointerAtOffset;
			return *ptr;
		}
		catch (NullReferenceException ex)
		{
			throw new AccessViolationException(ex.Message, ex);
		}
	}

	private unsafe static IntPtr GetPointerAtOffset(this IBuffer buffer, uint offset)
	{
		byte* buffer2 = ((IBufferByteAccess)buffer).Buffer;
		return (IntPtr)(buffer2 + offset);
	}

	private unsafe static void MemCopy(IntPtr src, IntPtr dst, uint count)
	{
		if (count > int.MaxValue)
		{
			MemCopy(src, dst, 2147483647u);
			MemCopy(src + int.MaxValue, dst + int.MaxValue, count - int.MaxValue);
		}
		else if (Math.Abs(src.ToInt64() - dst.ToInt64()) < count)
		{
			byte[] array = new byte[count];
			Marshal.Copy(src, array, 0, (int)count);
			Marshal.Copy(array, 0, dst, (int)count);
		}
		else
		{
			Buffer.Memcpy((byte*)(void*)dst, (byte*)(void*)src, (int)count);
		}
	}
}
