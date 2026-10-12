using System.IO;
using MS.Internal.WindowsBase;

namespace MS.Internal.IO.Packaging.CompoundFile;

[FriendAccessAllowed]
internal class FormatVersion
{
	public VersionPair ReaderVersion
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

	public VersionPair WriterVersion
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

	public VersionPair UpdaterVersion
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

	public string FeatureIdentifier
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	private FormatVersion()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public FormatVersion(string featureId, VersionPair version)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public FormatVersion(string featureId, VersionPair writerVersion, VersionPair readerVersion, VersionPair updaterVersion)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static FormatVersion LoadFromStream(Stream stream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public int SaveToStream(Stream stream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public bool IsReadableBy(VersionPair version)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public bool IsUpdatableBy(VersionPair version)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static FormatVersion LoadFromStream(Stream stream, out int bytesRead)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
