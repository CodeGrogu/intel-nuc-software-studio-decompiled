using System;
using System.IO;

namespace MS.Internal.IO.Packaging.CompoundFile;

internal abstract class CompoundFileReference : IComparable
{
	public abstract string FullName { get; }

	int IComparable.CompareTo(object ob)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Equals(object o)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int Save(CompoundFileReference reference, BinaryWriter writer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static CompoundFileReference Load(BinaryReader reader, out int bytesRead)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected CompoundFileReference()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
