using System.IO;

namespace MS.Internal.IO.Zip;

internal class ZipIOLocalFileDataDescriptor
{
	internal long Size
	{
		get
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
		set
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
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ZipIOLocalFileDataDescriptor CreateNew()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ZipIOLocalFileDataDescriptor ParseRecord(BinaryReader reader, long compressedSizeFromCentralDir, long uncompressedSizeFromCentralDir, uint crc32FromCentralDir, ushort versionNeededToExtract)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Save(BinaryWriter writer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public ZipIOLocalFileDataDescriptor()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
