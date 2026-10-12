using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Xml;

namespace MS.Internal.IO.Packaging;

internal class InternalRelationshipCollection : IEnumerable<PackageRelationship>, IEnumerable
{
	IEnumerator IEnumerable.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	IEnumerator<PackageRelationship> IEnumerable<PackageRelationship>.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public List<PackageRelationship>.Enumerator GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal InternalRelationshipCollection(PackagePart part)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal InternalRelationshipCollection(Package package)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PackageRelationship Add(Uri targetUri, TargetMode targetMode, string relationshipType, string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PackageRelationship GetRelationship(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Delete(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Clear()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Flush()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CloseInStreamingCreationMode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ThrowIfInvalidRelationshipType(string relationshipType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ThrowIfInvalidXsdId(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void WriteRelationshipsAsXml(XmlWriter writer, IEnumerable<PackageRelationship> relationships, bool alwaysWriteTargetModeAttribute, bool inStreamingProduction)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static InternalRelationshipCollection()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
