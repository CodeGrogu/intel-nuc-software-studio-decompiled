namespace MS.Internal.IO.Zip;

internal class ZipIORawDataFileBlock : IZipIOBlock
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

	internal long DiskImageShift
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

	internal static ZipIORawDataFileBlock Assign(ZipIOBlockManager blockManager, long offset, long size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool DiskImageContains(IZipIOBlock block)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool DiskImageContains(long offset)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SplitIntoPrefixSuffix(IZipIOBlock block, out ZipIORawDataFileBlock prefixBlock, out ZipIORawDataFileBlock suffixBlock)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private ZipIORawDataFileBlock()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
