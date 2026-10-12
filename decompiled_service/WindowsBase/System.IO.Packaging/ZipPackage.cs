using MS.Internal.IO.Zip;

namespace System.IO.Packaging;

/// <summary>Implements a derived subclass of the abstract <see cref="T:System.IO.Packaging.Package" /> base class—the <see cref="T:System.IO.Packaging.ZipPackage" /> class uses a ZIP archive as the container store. This class cannot be inherited.</summary>
public sealed class ZipPackage : Package
{
	protected override PackagePart CreatePartCore(Uri partUri, string contentType, CompressionOption compressionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override PackagePart GetPartCore(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void DeletePartCore(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override PackagePart[] GetPartsCore()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void FlushCore()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipPackage(string path, FileMode mode, FileAccess access, FileShare share, bool streaming)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ZipPackage(Stream s, FileMode mode, FileAccess access, bool streaming)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string GetZipItemNameFromOpcName(string opcName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static string GetOpcNameFromZipItemName(string zipItemName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void GetZipCompressionMethodFromOpcCompressionOption(CompressionOption compressionOption, out CompressionMethodEnum compressionMethod, out DeflateOptionEnum deflateOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ZipPackage()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
