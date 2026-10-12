using System.IO;
using System.Text;

namespace MS.Internal.IO.Zip;

internal class ZipIOLocalFileHeader
{
	internal ushort VersionNeededToExtract
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ushort GeneralPurposeBitFlag
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal CompressionMethodEnum CompressionMethod
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal uint LastModFileDateTime
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

	internal uint Crc32
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

	internal long CompressedSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal long UncompressedSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string FileName
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal long Size
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool StreamingCreationFlag
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

	internal DeflateOptionEnum DeflateOption
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

	internal static int FixedMinimalRecordSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ZipIOLocalFileHeader CreateNew(string fileName, Encoding encoding, CompressionMethodEnum compressionMethod, DeflateOptionEnum deflateOption, bool streaming)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ZipIOLocalFileHeader ParseRecord(BinaryReader reader, Encoding encoding)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Save(BinaryWriter writer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void UpdateZip64Structures(long compressedSize, long uncompressedSize, long offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void UpdatePadding(long headerSizeChange)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private ZipIOLocalFileHeader()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
