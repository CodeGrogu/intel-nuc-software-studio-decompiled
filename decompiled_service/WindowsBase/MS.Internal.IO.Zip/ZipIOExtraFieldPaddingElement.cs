using System.IO;

namespace MS.Internal.IO.Zip;

internal class ZipIOExtraFieldPaddingElement : ZipIOExtraFieldElement
{
	internal override ushort Size
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal override ushort SizeField
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ushort ConstantFieldId
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ushort MinimumFieldDataSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static ushort SignatureSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ushort PaddingSize
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

	internal static ZipIOExtraFieldPaddingElement CreateNew()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal override void ParseDataField(BinaryReader reader, ushort size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool MatchesPaddingSignature(byte[] sniffiedBytes)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal override void Save(BinaryWriter writer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipIOExtraFieldPaddingElement()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ZipIOExtraFieldPaddingElement()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
