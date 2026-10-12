using System.IO;
using System.Security;

namespace MS.Internal.IO.Packaging.CompoundFile;

internal class CompoundFileDeflateTransform : IDeflateTransform
{
	[SecurityCritical]
	[SecurityTreatAsSafe]
	public void Decompress(Stream source, Stream sink)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public void Compress(Stream source, Stream sink)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public CompoundFileDeflateTransform()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
