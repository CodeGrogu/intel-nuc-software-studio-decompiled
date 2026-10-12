using System;
using System.IO;

namespace MS.Internal.IO.Zip;

internal sealed class ZipArchive : IDisposable
{
	internal FileAccess OpenAccess
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ZipArchive OpenOnFile(string path, FileMode mode, FileAccess access, FileShare share, bool streaming)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ZipArchive OpenOnStream(Stream stream, FileMode mode, FileAccess access, bool streaming)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipFileInfo AddFile(string zipFileName, CompressionMethodEnum compressionMethod, DeflateOptionEnum deflateOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipFileInfo GetFile(string zipFileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool FileExists(string zipFileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void DeleteFile(string zipFileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipFileInfoCollection GetFiles()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Flush()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Close()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void VerifyVersionNeededToExtract(ushort version)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ZipArchive()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private ZipArchive()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
