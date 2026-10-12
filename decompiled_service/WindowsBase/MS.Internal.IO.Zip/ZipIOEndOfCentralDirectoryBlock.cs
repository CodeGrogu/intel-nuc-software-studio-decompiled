namespace MS.Internal.IO.Zip;

internal class ZipIOEndOfCentralDirectoryBlock : IZipIOBlock
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

	internal uint NumberOfThisDisk
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal uint NumberOfTheDiskWithTheStartOfTheCentralDirectory
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal uint TotalNumberOfEntriesInTheCentralDirectoryOnThisDisk
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal uint TotalNumberOfEntriesInTheCentralDirectory
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal uint SizeOfTheCentralDirectory
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal uint OffsetOfStartOfCentralDirectory
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool ContainValuesHintingToPossibilityOfZip64
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

	internal static ZipIOEndOfCentralDirectoryBlock SeekableLoad(ZipIOBlockManager blockManager)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static ZipIOEndOfCentralDirectoryBlock CreateNew(ZipIOBlockManager blockManager, long offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ValidateZip64TriggerValues()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ZipIOEndOfCentralDirectoryBlock()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private ZipIOEndOfCentralDirectoryBlock()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
