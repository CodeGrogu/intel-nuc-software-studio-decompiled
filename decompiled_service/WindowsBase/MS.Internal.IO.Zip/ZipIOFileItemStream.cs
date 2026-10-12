using System.IO;

namespace MS.Internal.IO.Zip;

internal class ZipIOFileItemStream : Stream
{
	public override bool CanRead
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool CanSeek
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override bool CanWrite
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override long Length
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override long Position
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

	internal bool DirtyFlag
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool DataChanged
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal long Offset
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override void SetLength(long newLength)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Flush()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipIOFileItemStream(ZipIOBlockManager blockManager, ZipIOLocalFileBlock block, long persistedOffset, long persistedSize)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PreSaveNotificationScanControlInstruction PreSaveNotification(long offset, long size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Move(long shiftSize)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SaveStreaming()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Save()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
