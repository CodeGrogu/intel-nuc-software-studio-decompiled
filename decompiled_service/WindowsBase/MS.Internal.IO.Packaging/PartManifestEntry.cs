using System;
using System.Collections.Generic;
using System.IO.Packaging;

namespace MS.Internal.IO.Packaging;

internal struct PartManifestEntry
{
	private Uri _owningPartUri;

	private Uri _uri;

	private ContentType _contentType;

	private string _hashAlgorithm;

	private string _hashValue;

	private List<string> _transforms;

	private List<PackageRelationshipSelector> _relationshipSelectors;

	internal bool IsRelationshipEntry
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal Uri Uri
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ContentType ContentType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string HashAlgorithm
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string HashValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal List<string> Transforms
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal List<PackageRelationshipSelector> RelationshipSelectors
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal Uri OwningPartUri
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal PartManifestEntry(Uri uri, ContentType contentType, string hashAlgorithm, string hashValue, List<string> transforms, List<PackageRelationshipSelector> relationshipSelectors)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
