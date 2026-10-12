using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;
using Windows.Storage.Streams;

namespace System.Runtime.InteropServices.WindowsRuntime;

[__DynamicallyInvokable]
public sealed class WindowsRuntimeBuffer : IBuffer, IBufferByteAccess, IMarshal, IAgileObject
{
	private enum MSHCTX
	{
		Local,
		NoSharedMem,
		DifferentMachine,
		InProc,
		CrossCtx
	}

	private enum MSHLFLAGS
	{
		Normal = 0,
		TableStrong = 1,
		TableWeak = 2,
		NoPing = 4
	}

	private const string WinTypesDLL = "WinTypes.dll";

	[ThreadStatic]
	private static IMarshal winRtMarshalProxy;

	private byte[] data;

	private int dataStartOffs;

	private int usefulDataLength;

	private int maxDataCapacity;

	private IntPtr dataPtr = IntPtr.Zero;

	uint IBuffer.Capacity => (uint)maxDataCapacity;

	uint IBuffer.Length
	{
		get
		{
			return (uint)usefulDataLength;
		}
		set
		{
			if (value > ((IBuffer)this).Capacity)
			{
				ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(SR.GetString("Argument_BufferLengthExceedsCapacity"), "value");
				ex.SetErrorCode(-2147483637);
				throw ex;
			}
			usefulDataLength = (int)value;
		}
	}

	unsafe byte* IBufferByteAccess.Buffer
	{
		[SecurityCritical]
		get
		{
			if (data.Length == 0)
			{
				return null;
			}
			IntPtr intPtr = Volatile.Read(ref dataPtr);
			if (intPtr != IntPtr.Zero)
			{
				return (byte*)(void*)intPtr;
			}
			return PinUnderlyingData();
		}
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IBuffer Create(int capacity)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		return (IBuffer)(object)new WindowsRuntimeBuffer(capacity);
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static IBuffer Create(byte[] data, int offset, int length, int capacity)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		if (data.Length - offset < length)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		if (data.Length - offset < capacity)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		if (capacity < length)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientBufferCapacity"));
		}
		byte[] destinationArray = new byte[capacity];
		Array.Copy(data, offset, destinationArray, 0, length);
		return (IBuffer)(object)new WindowsRuntimeBuffer(destinationArray, 0, length, capacity);
	}

	[DllImport("WinTypes.dll", CallingConvention = CallingConvention.StdCall)]
	[SuppressUnmanagedCodeSecurity]
	private static extern int RoGetBufferMarshaler(out IMarshal bufferMarshalerPtr);

	private static void EnsureHasMarshalProxy()
	{
		if (winRtMarshalProxy != null)
		{
			return;
		}
		try
		{
			int num = RoGetBufferMarshaler(out var bufferMarshalerPtr);
			winRtMarshalProxy = bufferMarshalerPtr;
			if (num != 0)
			{
				Exception ex = new Exception(string.Format("{0} ({1}!RoGetBufferMarshaler)", SR.GetString("WinRtCOM_Error"), "WinTypes.dll"));
				ex.SetErrorCode(num);
				throw ex;
			}
			if (bufferMarshalerPtr == null)
			{
				throw new NullReferenceException(string.Format("{0} ({1}!RoGetBufferMarshaler)", SR.GetString("WinRtCOM_Error"), "WinTypes.dll"));
			}
		}
		catch (DllNotFoundException inner)
		{
			throw new NotImplementedException(SR.GetString("NotImplemented_NativeRoutineNotFound", string.Format("{0}!RoGetBufferMarshaler", "WinTypes.dll")), inner);
		}
		catch (EntryPointNotFoundException inner2)
		{
			throw new NotImplementedException(SR.GetString("NotImplemented_NativeRoutineNotFound", string.Format("{0}!RoGetBufferMarshaler", "WinTypes.dll")), inner2);
		}
	}

	internal WindowsRuntimeBuffer(int capacity)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		data = new byte[capacity];
		dataStartOffs = 0;
		usefulDataLength = 0;
		maxDataCapacity = capacity;
		dataPtr = IntPtr.Zero;
	}

	internal WindowsRuntimeBuffer(byte[] data, int offset, int length, int capacity)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		if (data.Length - offset < length)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		if (data.Length - offset < capacity)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientArrayElementsAfterOffset"));
		}
		if (capacity < length)
		{
			throw new ArgumentException(SR.GetString("Argument_InsufficientBufferCapacity"));
		}
		this.data = data;
		dataStartOffs = offset;
		usefulDataLength = length;
		maxDataCapacity = capacity;
		dataPtr = IntPtr.Zero;
	}

	internal void GetUnderlyingData(out byte[] underlyingDataArray, out int underlyingDataArrayStartOffset)
	{
		underlyingDataArray = data;
		underlyingDataArrayStartOffset = dataStartOffs;
	}

	private unsafe byte* PinUnderlyingData()
	{
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = Marshal.GetIUnknownForObject(this);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
				}
			}
		}
		Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, null);
		NativeOverlapped* ptr = null;
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		IntPtr intPtr2;
		try
		{
		}
		finally
		{
			try
			{
				ptr = overlapped.UnsafePack(null, data);
				ptr->InternalLow = (IntPtr)259;
				fixed (byte* ptr2 = &data[dataStartOffs])
				{
					intPtr2 = (IntPtr)ptr2;
				}
				flag = Interlocked.CompareExchange(ref dataPtr, intPtr2, IntPtr.Zero) == IntPtr.Zero;
			}
			finally
			{
				if (flag)
				{
					WindowsRuntimeBufferHelper.StoreOverlappedInCCW(this, ptr);
				}
				else if (ptr != null)
				{
					Overlapped.Unpack(ptr);
					Overlapped.Free(ptr);
				}
			}
		}
		return (byte*)(void*)intPtr2;
	}

	void IMarshal.DisconnectObject(uint dwReserved)
	{
		EnsureHasMarshalProxy();
		winRtMarshalProxy.DisconnectObject(dwReserved);
	}

	void IMarshal.GetMarshalSizeMax(ref Guid riid, IntPtr pv, uint dwDestContext, IntPtr pvDestContext, uint mshlflags, out uint pSize)
	{
		EnsureHasMarshalProxy();
		winRtMarshalProxy.GetMarshalSizeMax(ref riid, pv, dwDestContext, pvDestContext, mshlflags, out pSize);
	}

	void IMarshal.GetUnmarshalClass(ref Guid riid, IntPtr pv, uint dwDestContext, IntPtr pvDestContext, uint mshlFlags, ref Guid pCid)
	{
		EnsureHasMarshalProxy();
		winRtMarshalProxy.GetUnmarshalClass(ref riid, pv, dwDestContext, pvDestContext, mshlFlags, ref pCid);
	}

	void IMarshal.MarshalInterface(IntPtr pStm, ref Guid riid, IntPtr pv, uint dwDestContext, IntPtr pvDestContext, uint mshlflags)
	{
		EnsureHasMarshalProxy();
		winRtMarshalProxy.MarshalInterface(pStm, ref riid, pv, dwDestContext, pvDestContext, mshlflags);
	}

	void IMarshal.ReleaseMarshalData(IntPtr pStm)
	{
		EnsureHasMarshalProxy();
		winRtMarshalProxy.ReleaseMarshalData(pStm);
	}

	void IMarshal.UnmarshalInterface(IntPtr pStm, ref Guid riid, out IntPtr ppv)
	{
		EnsureHasMarshalProxy();
		winRtMarshalProxy.UnmarshalInterface(pStm, ref riid, out ppv);
	}
}
