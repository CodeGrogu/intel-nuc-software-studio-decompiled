using System.IO;

namespace MS.Internal.IO.Packaging.CompoundFile;

internal class VersionedStream : Stream
{
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

	public override long Length
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

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

	protected Stream BaseStream
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override int ReadByte()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void WriteByte(byte b)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void SetLength(long newLength)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Flush()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal VersionedStream(Stream baseStream, VersionedStreamOwner versionOwner)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected VersionedStream(Stream baseStream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected void CheckDisposed()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
