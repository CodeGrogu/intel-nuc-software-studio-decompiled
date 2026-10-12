using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Compression;

internal static class ZLibNative
{
	public enum FlushCode
	{
		NoFlush,
		PartialFlush,
		SyncFlush,
		FullFlush,
		Finish,
		Block
	}

	public enum ErrorCode
	{
		Ok = 0,
		StreamEnd = 1,
		NeedDictionary = 2,
		ErrorNo = -1,
		StreamError = -2,
		DataError = -3,
		MemError = -4,
		BufError = -5,
		VersionError = -6
	}

	public enum CompressionLevel
	{
		NoCompression = 0,
		BestSpeed = 1,
		BestCompression = 9,
		DefaultCompression = -1
	}

	public enum CompressionStrategy
	{
		Filtered = 1,
		HuffmanOnly = 2,
		Rle = 3,
		Fixed = 4,
		DefaultStrategy = 0
	}

	public enum CompressionMethod
	{
		Deflated = 8
	}

	internal struct ZStream
	{
		internal IntPtr nextIn;

		internal uint availIn;

		internal uint totalIn;

		internal IntPtr nextOut;

		internal uint availOut;

		internal uint totalOut;

		internal IntPtr msg;

		internal IntPtr state;

		internal IntPtr zalloc;

		internal IntPtr zfree;

		internal IntPtr opaque;

		internal int dataType;

		internal uint adler;

		internal uint reserved;
	}

	[SecurityCritical]
	public sealed class ZLibStreamHandle : SafeHandleMinusOneIsInvalid
	{
		public enum State
		{
			NotInitialized,
			InitializedForDeflate,
			InitializedForInflate,
			Disposed
		}

		public State InitializationState
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public IntPtr NextIn
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
			[SecurityCritical]
			set
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public uint AvailIn
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
			[SecurityCritical]
			set
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public uint TotalIn
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public IntPtr NextOut
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
			[SecurityCritical]
			set
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public uint AvailOut
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
			[SecurityCritical]
			set
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public uint TotalOut
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public int DataType
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public uint Adler
		{
			[SecurityCritical]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public ZLibStreamHandle()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public ErrorCode DeflateInit2_(CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public ErrorCode Deflate(FlushCode flush)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public ErrorCode DeflateEnd()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public ErrorCode InflateInit2_(int windowBits)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public ErrorCode Inflate(FlushCode flush)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public ErrorCode InflateEnd()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		public string GetErrorMessage()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[SecurityCritical]
		internal static int ZLibCompileFlags()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public const string ZLibNativeDllName = "clrcompression.dll";

	public const string ZLibVersion = "1.2.3";

	internal static readonly IntPtr ZNullPtr;

	public const int Deflate_DefaultWindowBits = -15;

	public const int Deflate_DefaultMemLevel = 8;

	[SecurityCritical]
	public static ErrorCode CreateZLibStreamForDeflate(out ZLibStreamHandle zLibStreamHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static ErrorCode CreateZLibStreamForDeflate(out ZLibStreamHandle zLibStreamHandle, CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static ErrorCode CreateZLibStreamForInflate(out ZLibStreamHandle zLibStreamHandle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static ErrorCode CreateZLibStreamForInflate(out ZLibStreamHandle zLibStreamHandle, int windowBits)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public static int ZLibCompileFlags()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ZLibNative()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
