using System;
using System.Collections;
using System.IO;
using System.Text;
using MS.Internal.IO.Packaging;

namespace MS.Internal.IO.Zip;

internal class ZipIOBlockManager : IDisposable, IEnumerable
{
	internal bool IsCentralDirectoryBlockLoaded
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ZipIOCentralDirectoryBlock CentralDirectoryBlock
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ZipIOZip64EndOfCentralDirectoryBlock Zip64EndOfCentralDirectoryBlock
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ZipIOZip64EndOfCentralDirectoryLocatorBlock Zip64EndOfCentralDirectoryLocatorBlock
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ZipIOEndOfCentralDirectoryBlock EndOfCentralDirectoryBlock
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal Stream Stream
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool Streaming
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal BinaryReader BinaryReader
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal BinaryWriter BinaryWriter
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal Encoding Encoding
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool DirtyFlag
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static int MaxFileNameSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CreateEndOfCentralDirectoryBlock()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void LoadEndOfCentralDirectoryBlock()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipIOLocalFileBlock CreateLocalFileBlock(string zipFileName, CompressionMethodEnum compressionMethod, DeflateOptionEnum deflateOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipIOLocalFileBlock LoadLocalFileBlock(string zipFileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void RemoveLocalFileBlock(ZipIOLocalFileBlock localFileBlock)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void MoveData(long moveBlockSourceOffset, long moveBlockTargetOffset, long moveBlockSize)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SaveStream(ZipIOLocalFileBlock blockRequestingFlush, bool closingFlag)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Save(bool closingFlag)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipIOBlockManager(Stream archiveStream, bool streaming, bool ownStream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static uint ToMsDosDateTime(DateTime dateTime)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static DateTime FromMsDosDateTime(uint dosDateTime)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string ValidateNormalizeFileName(string zipFileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int CopyBytes(short value, byte[] buffer, int offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int CopyBytes(int value, byte[] buffer, int offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int CopyBytes(long value, byte[] buffer, int offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int CopyBytes(ushort value, byte[] buffer, int offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int CopyBytes(uint value, byte[] buffer, int offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int CopyBytes(ulong value, byte[] buffer, int offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ulong ConvertToUInt64(uint loverAddressValue, uint higherAddressValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ZipIOVersionNeededToExtract CalcVersionNeededToExtractFromCompression(CompressionMethodEnum compression)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static PreSaveNotificationScanControlInstruction CommonPreSaveNotificationHandler(Stream stream, long offset, long size, long onDiskOffset, long onDiskSize, ref SparseMemoryStream cachePrefixStream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
