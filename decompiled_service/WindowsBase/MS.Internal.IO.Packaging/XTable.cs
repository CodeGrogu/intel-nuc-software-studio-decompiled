namespace MS.Internal.IO.Packaging;

internal static class XTable
{
	internal enum ID
	{
		OpcSignatureNamespace,
		OpcSignatureNamespacePrefix,
		OpcSignatureNamespaceAttribute,
		W3CSignatureNamespaceRoot,
		RelationshipsTransformName,
		SignatureTagName,
		OpcSignatureAttrValue,
		SignedInfoTagName,
		ReferenceTagName,
		SignatureMethodTagName,
		ObjectTagName,
		KeyInfoTagName,
		ManifestTagName,
		TransformTagName,
		TransformsTagName,
		AlgorithmAttrName,
		SourceIdAttrName,
		OpcAttrValue,
		OpcLinkAttrValue,
		TargetAttrName,
		SignatureValueTagName,
		UriAttrName,
		DigestMethodTagName,
		DigestValueTagName,
		SignaturePropertiesTagName,
		SignaturePropertyTagName,
		SignatureTimeTagName,
		SignatureTimeFormatTagName,
		SignatureTimeValueTagName,
		RelationshipsTagName,
		RelationshipReferenceTagName,
		RelationshipsGroupReferenceTagName,
		SourceTypeAttrName,
		SignaturePropertyIdAttrName,
		SignaturePropertyIdAttrValue,
		_IllegalValue
	}

	internal struct TableEntry
	{
		internal ID id;

		internal string s;

		internal TableEntry(ID index, string str)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static string Get(ID i)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static XTable()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
