using System.Collections.Generic;
using System.IO;

namespace MS.Internal.IO.Packaging;

internal class SparseMemoryStream : Stream
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

	internal List<MemoryStreamBlock> MemoryBlockCollection
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal long MemoryConsumption
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

	internal void WriteToStream(Stream stream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal SparseMemoryStream(long lowWaterMark, long highWaterMark)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal SparseMemoryStream(long lowWaterMark, long highWaterMark, bool autoCloseSmallBlockGaps)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
