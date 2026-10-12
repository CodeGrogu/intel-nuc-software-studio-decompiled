using System.Collections;
using System.Collections.Generic;

namespace System.IO.Packaging;

/// <summary>Represents a collection of <see cref="T:System.IO.Packaging.PackagePart" /> objects.</summary>
public class PackagePartCollection : IEnumerable<PackagePart>, IEnumerable
{
	/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
	/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an enumerator that iterates through the collection.</summary>
	/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> object that can be used to iterate through the collection.</returns>
	IEnumerator<PackagePart> IEnumerable<PackagePart>.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an enumerator for iterating through the parts in the collection. </summary>
	/// <returns>An enumerator for iterating through the <see cref="T:System.IO.Packaging.PackagePart" /> elements in the collection.</returns>
	public IEnumerator<PackagePart> GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PackagePartCollection(SortedList<PackUriHelper.ValidatedPartUri, PackagePart> partList)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
