namespace MS.Internal.IO.Zip;

internal enum ZipIOVersionNeededToExtract : ushort
{
	StoredData = 10,
	VolumeLabel = 11,
	DeflatedData = 20,
	Zip64FileFormat = 45
}
