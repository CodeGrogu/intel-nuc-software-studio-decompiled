using MS.Internal.IO.Packaging.CompoundFile;

namespace System.IO.Packaging;

/// <summary>Provides access and information for manipulating I/O streams within a <see cref="T:System.IO.Packaging.Package" />.  </summary>
public class StreamInfo
{
	/// <summary>Gets the <see cref="T:System.IO.Packaging.CompressionOption" /> setting for the stream.</summary>
	/// <returns>The compression option setting for the package stream.</returns>
	public CompressionOption CompressionOption
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the <see cref="T:System.IO.Packaging.EncryptionOption" /> setting for the stream.</summary>
	/// <returns>The encryption option setting for the package stream.</returns>
	public EncryptionOption EncryptionOption
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the name of the stream.</summary>
	/// <returns>The name of this stream.</returns>
	public string Name
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool StreamInfoDisposed
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal CompoundFileStreamReference StreamReference
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal StreamInfo(StorageInfo parent, string streamName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal StreamInfo(StorageInfo parent, string streamName, CompressionOption compressionOption, EncryptionOption encryptionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a stream opened in a default <see cref="T:System.IO.FileMode" /> and <see cref="T:System.IO.FileAccess" />.</summary>
	/// <returns>The I/O stream opened in a default <see cref="T:System.IO.Packaging.Package" /> root <see cref="T:System.IO.FileMode" /> and <see cref="T:System.IO.FileAccess" />..</returns>
	public Stream GetStream()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an I/O stream opened in a specified <see cref="T:System.IO.FileMode" />.</summary>
	/// <param name="mode">The file mode in which to open the stream.</param>
	/// <returns>The stream opened in the specified file <paramref name="mode" />.</returns>
	public Stream GetStream(FileMode mode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an I/O stream opened in a specified <see cref="T:System.IO.FileMode" /> and with a given <see cref="T:System.IO.FileAccess" />.</summary>
	/// <param name="mode">The file mode in which to open the stream.</param>
	/// <param name="access">The file access mode in which to read or write to the stream.</param>
	/// <returns>The I/O stream opened in the specified <see cref="T:System.IO.FileMode" /> and with a given <see cref="T:System.IO.FileAccess" />.</returns>
	public Stream GetStream(FileMode mode, FileAccess access)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal Stream Create()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal Stream Create(string dataSpaceLabel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal Stream Create(FileMode mode, FileAccess access, string dataSpace)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Delete()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool InternalExists()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CheckDisposedStatus()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CheckAccessMode(int grfMode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
