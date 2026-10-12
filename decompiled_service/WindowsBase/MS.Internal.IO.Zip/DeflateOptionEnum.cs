namespace MS.Internal.IO.Zip;

internal enum DeflateOptionEnum : byte
{
	Normal = 0,
	Maximum = 2,
	Fast = 4,
	SuperFast = 6,
	None = byte.MaxValue
}
