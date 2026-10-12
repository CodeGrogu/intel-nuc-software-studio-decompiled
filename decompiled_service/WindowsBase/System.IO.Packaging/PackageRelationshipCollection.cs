using System.Collections;
using System.Collections.Generic;
using MS.Internal.IO.Packaging;

namespace System.IO.Packaging;

/// <summary>Represents a collection of <see cref="T:System.IO.Packaging.PackageRelationship" /> elements that are owned by a given <see cref="T:System.IO.Packaging.PackagePart" /> or the <see cref="T:System.IO.Packaging.Package" />. </summary>
public class PackageRelationshipCollection : IEnumerable<PackageRelationship>, IEnumerable
{
	/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
	/// <returns>Do not use—use <see cref="M:System.IO.Packaging.PackageRelationshipCollection.GetEnumerator" />.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an enumerator for iterating through the relationships in the collection. </summary>
	/// <returns>An enumerator for iterating through the <see cref="T:System.IO.Packaging.PackageRelationship" /> elements in the collection.</returns>
	public IEnumerator<PackageRelationship> GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PackageRelationshipCollection(InternalRelationshipCollection relationships, string filter)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
