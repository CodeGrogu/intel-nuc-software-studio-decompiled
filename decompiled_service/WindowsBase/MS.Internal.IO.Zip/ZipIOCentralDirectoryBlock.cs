using System.Collections;

namespace MS.Internal.IO.Zip;

internal class ZipIOCentralDirectoryBlock : IZipIOBlock
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

	internal int Count
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsZip64BitRequiredForStoring
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

	internal static ZipIOCentralDirectoryBlock SeekableLoad(ZipIOBlockManager blockManager)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ZipIOCentralDirectoryBlock CreateNew(ZipIOBlockManager blockManager)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void AddFileBlock(ZipIOLocalFileBlock fileBlock)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void RemoveFileBlock(string fileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool FileExists(string fileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipIOCentralDirectoryFileHeader GetCentralDirectoryFileHeader(string fileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ICollection GetFileNamesCollection()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ZipIOCentralDirectoryBlock()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private ZipIOCentralDirectoryBlock()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
