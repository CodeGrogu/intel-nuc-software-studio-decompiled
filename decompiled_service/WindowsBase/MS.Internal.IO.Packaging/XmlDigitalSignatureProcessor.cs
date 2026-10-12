using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace MS.Internal.IO.Packaging;

internal class XmlDigitalSignatureProcessor
{
	internal static ContentType ContentType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal PackagePart SignaturePart
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal List<Uri> PartManifest
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal List<PackageRelationshipSelector> RelationshipManifest
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal X509Certificate2 Signer
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal byte[] SignatureValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal System.Security.Cryptography.Xml.Signature Signature
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal PackageDigitalSignature PackageSignature
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal DateTime SigningTime
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string TimeFormat
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal XmlDigitalSignatureProcessor(PackageDigitalSignatureManager manager, PackagePart signaturePart, PackageDigitalSignature packageSignature)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static PackageDigitalSignature Sign(PackageDigitalSignatureManager manager, PackagePart signaturePart, IEnumerable<Uri> parts, IEnumerable<PackageRelationshipSelector> relationshipSelectors, X509Certificate2 signer, string signatureId, bool embedCertificate, IEnumerable<DataObject> signatureObjects, IEnumerable<Reference> objectReferences)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool Verify()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal bool Verify(X509Certificate2 signer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal List<string> GetPartTransformList(Uri partName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static string GenerateDigestValue(Stream s, string transformName, HashAlgorithm hashAlgorithm)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static string GenerateDigestValue(Stream s, List<string> transforms, HashAlgorithm hashAlgorithm)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Stream GenerateRelationshipNodeStream(IEnumerable<PackageRelationship> relationships)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static HashAlgorithm GetHashAlgorithm(string hashAlgorithmName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool IsValidXmlCanonicalizationTransform(string transformName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static XmlDigitalSignatureProcessor()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
