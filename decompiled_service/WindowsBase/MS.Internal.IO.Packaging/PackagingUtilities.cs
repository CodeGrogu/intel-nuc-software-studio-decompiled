using System.IO;
using System.Xml;
using MS.Internal.WindowsBase;

namespace MS.Internal.IO.Packaging;

[FriendAccessAllowed]
internal static class PackagingUtilities
{
	internal static readonly string RelationshipNamespaceUri;

	internal static readonly ContentType RelationshipPartContentType;

	internal const string ContainerFileExtension = "xps";

	internal const string XamlFileExtension = "xaml";

	internal static object IsoStoreSyncRoot
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static object IsolatedStorageFileLock
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static void PerformInitailReadAndVerifyEncoding(XmlTextReader reader)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void VerifyStreamReadArgs(Stream s, byte[] buffer, int offset, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void VerifyStreamWriteArgs(Stream s, byte[] buffer, int offset, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int ReliableRead(Stream stream, byte[] buffer, int offset, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int ReliableRead(Stream stream, byte[] buffer, int offset, int requestedCount, int requiredCount)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int ReliableRead(BinaryReader reader, byte[] buffer, int offset, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int ReliableRead(BinaryReader reader, byte[] buffer, int offset, int requestedCount, int requiredCount)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static long CopyStream(Stream sourceStream, Stream targetStream, long bytesToCopy, int bufferSize)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Stream CreateUserScopedIsolatedStorageFileStreamWithRandomName(int retryCount, out string fileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void CalculateOverlap(long block1Offset, long block1Size, long block2Offset, long block2Size, out long overlapBlockOffset, out long overlapBlockSize)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int GetNonXmlnsAttributeCount(XmlReader reader)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static PackagingUtilities()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
