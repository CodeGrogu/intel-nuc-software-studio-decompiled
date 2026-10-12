using System.Collections;
using System.IO;

namespace MS.Internal.IO.Packaging.CompoundFile;

internal static class ContainerUtilities
{
	internal static readonly char PathSeparator;

	internal static readonly string PathSeparatorAsString;

	internal static int Int16Size
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static int Int32Size
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static int Int64Size
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static CaseInsensitiveOrdinalStringComparer StringCaseInsensitiveComparer
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static int CalculateDWordPadBytesLength(int length)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int WriteByteLengthPrefixedDWordPaddedUnicodeString(BinaryWriter writer, string outputString)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string ReadByteLengthPrefixedDWordPaddedUnicodeString(BinaryReader reader)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string ReadByteLengthPrefixedDWordPaddedUnicodeString(BinaryReader reader, out int bytesRead)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void CheckAgainstNull(object paramRef, string testStringIdentifier)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string[] ConvertBackSlashPathToStringArrayPath(string backSlashPath)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string ConvertStringArrayPathToBackSlashPath(IList arrayPath)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string ConvertStringArrayPathToBackSlashPath(IList storages, string streamName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void CheckStringAgainstNullAndEmpty(string testString, string testStringIdentifier)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void CheckStringAgainstReservedName(string nameString, string nameStringIdentifier)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool IsReservedName(string nameString)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void CheckStringForEmbeddedPathSeparator(string testString, string testStringIdentifier)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ContainerUtilities()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
