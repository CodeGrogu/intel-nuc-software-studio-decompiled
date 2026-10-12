using System;
using System.IO;
using System.Security;

namespace MS.Internal.IO.Packaging.CompoundFile;

[SecurityCritical(SecurityCriticalScope.Everything)]
[SecurityTreatAsSafe]
internal static class SafeNativeCompoundFileMethods
{
	internal static void UpdateModeFlagFromFileAccess(FileAccess access, ref int grfMode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int SafeStgCreateDocfileOnStream(Stream s, int grfMode, out IStorage ppstgOpen)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int SafeStgOpenStorageOnStream(Stream s, int grfMode, out IStorage ppstgOpen)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int SafeStgCreateStorageEx(string pwcsName, int grfMode, int stgfmt, int grfAttrs, IntPtr pStgOptions, IntPtr reserved2, ref Guid riid, out IStorage ppObjectOpen)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int SafeStgOpenStorageEx(string pwcsName, int grfMode, int stgfmt, int grfAttrs, IntPtr pStgOptions, IntPtr reserved2, ref Guid riid, out IStorage ppObjectOpen)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int SafePropVariantClear(ref PROPVARIANT pvar)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
