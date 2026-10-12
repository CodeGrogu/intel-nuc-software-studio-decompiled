using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Security.Cryptography;
using System.Xml;

namespace MS.Internal.IO.Packaging;

internal static class XmlSignatureManifest
{
	internal static void ParseManifest(PackageDigitalSignatureManager manager, XmlReader reader, out List<Uri> partManifest, out List<PartManifestEntry> partEntryManifest, out List<PackageRelationshipSelector> relationshipManifest)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static XmlNode GenerateManifest(PackageDigitalSignatureManager manager, XmlDocument xDoc, HashAlgorithm hashAlgorithm, IEnumerable<Uri> parts, IEnumerable<PackageRelationshipSelector> relationshipSelectors)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
