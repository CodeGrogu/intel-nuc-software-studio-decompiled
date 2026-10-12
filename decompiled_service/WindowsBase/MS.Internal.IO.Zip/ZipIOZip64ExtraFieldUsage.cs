namespace MS.Internal.IO.Zip;

internal enum ZipIOZip64ExtraFieldUsage
{
	None = 0,
	UncompressedSize = 1,
	CompressedSize = 2,
	OffsetOfLocalHeader = 4,
	DiskNumber = 8
}
