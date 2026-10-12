using System.Collections.Generic;
using MS.Internal.IO.Packaging;
using MS.Internal.IO.Zip;

namespace System.IO.Packaging;

/// <summary>Represents a part that is stored in a <see cref="T:System.IO.Packaging.ZipPackage" />.</summary>
public sealed class ZipPackagePart : PackagePart
{
	internal List<PieceInfo> PieceDescriptors
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ZipFileInfo ZipFileInfo
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected override Stream GetStreamCore(FileMode mode, FileAccess access)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipPackagePart(ZipPackage container, ZipFileInfo zipFileInfo, PackUriHelper.ValidatedPartUri partUri, string contentType, CompressionOption compressionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipPackagePart(ZipPackage container, ZipArchive zipArchive, PackUriHelper.ValidatedPartUri partUri, string contentType, CompressionOption compressionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipPackagePart(ZipPackage container, ZipArchive zipArchive, List<PieceInfo> pieces, PackUriHelper.ValidatedPartUri partUri, string contentType, CompressionOption compressionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
