namespace System.IO.Packaging;

/// <summary>Specifies the compression level for content that is stored in a <see cref="T:System.IO.Packaging.PackagePart" />. </summary>
public enum CompressionOption
{
	/// <summary>Compression is turned off. </summary>
	NotCompressed = -1,
	/// <summary>Compression is optimized for a balance between size and performance. </summary>
	Normal,
	/// <summary>Compression is optimized for size. </summary>
	Maximum,
	/// <summary>Compression is optimized for performance. </summary>
	Fast,
	/// <summary>Compression is optimized for high performance.</summary>
	SuperFast
}
