using System.IO;

namespace MS.Internal.IO.Zip;

internal class ZipIOExtraFieldElement
{
	internal virtual ushort SizeField
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal virtual byte[] DataField
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal virtual ushort Size
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ushort MinimumSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ZipIOExtraFieldElement Parse(BinaryReader reader, ZipIOZip64ExtraFieldUsage zip64extraFieldUsage)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void ParseDataField(BinaryReader reader, ushort size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void Save(BinaryWriter writer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipIOExtraFieldElement(ushort id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ZipIOExtraFieldElement()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
