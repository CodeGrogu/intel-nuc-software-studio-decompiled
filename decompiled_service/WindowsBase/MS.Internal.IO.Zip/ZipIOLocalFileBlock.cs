using System;
using System.IO;

namespace MS.Internal.IO.Zip;

internal class ZipIOLocalFileBlock : IZipIOBlock, IDisposable
{
	public long Offset
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public long Size
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

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
	}

	internal uint Crc32
	{
		get
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

	internal DeflateOptionEnum DeflateOption
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

	internal bool FolderFlag
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool VolumeLabelFlag
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool GetDirtyFlag(bool closingFlag)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Move(long shiftSize)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SaveStreaming(bool closingFlag)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Save()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void UpdateReferences(bool closingFlag)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public PreSaveNotificationScanControlInstruction PreSaveNotification(long offset, long size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ZipIOLocalFileBlock SeekableLoad(ZipIOBlockManager blockManager, string fileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ZipIOLocalFileBlock CreateNew(ZipIOBlockManager blockManager, string fileName, CompressionMethodEnum compressionMethod, DeflateOptionEnum deflateOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal Stream GetStream(FileMode mode, FileAccess access)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void DeregisterExposedStream(Stream exposedStream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CheckDisposed()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected virtual void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private ZipIOLocalFileBlock()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
