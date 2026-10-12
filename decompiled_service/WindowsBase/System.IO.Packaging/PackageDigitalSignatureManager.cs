using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;

namespace System.IO.Packaging;

/// <summary>Provides a utility class for the creation and verification of digital signatures in a <see cref="T:System.IO.Packaging.Package" />.    </summary>
public sealed class PackageDigitalSignatureManager
{
	/// <summary>Gets a value that indicates whether the package contains any signatures. </summary>
	/// <returns>
	///     <see langword="true" /> if the package contains signatures; otherwise, <see langword="false" />.</returns>
	public bool IsSigned
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a collection of all the signatures contained in the package. </summary>
	/// <returns>A collection of all the <see cref="T:System.IO.Packaging.PackageDigitalSignature" /> objects.</returns>
	public ReadOnlyCollection<PackageDigitalSignature> Signatures
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a dictionary that contains each defined <see cref="P:System.IO.Packaging.PackagePart.ContentType" /> and its associated XML <see cref="T:System.Security.Cryptography.Xml.Transform" />.<see cref="P:System.Security.Cryptography.Xml.Transform.Algorithm" /> identifier. </summary>
	/// <returns>A dictionary that contains each defined <see cref="P:System.IO.Packaging.PackagePart.ContentType" /> and its associated XML <see cref="T:System.Security.Cryptography.Xml.Transform" />.<see cref="P:System.Security.Cryptography.Xml.Transform.Algorithm" /> identifier.</returns>
	public Dictionary<string, string> TransformMapping
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets a handle to the parent window for displaying a certificate selection dialog box. </summary>
	/// <returns>The handle of the parent window to use to display the certificate selection dialog box.</returns>
	public IntPtr ParentWindow
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

	/// <summary>Gets or sets the URI identifier for the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> instance used to create and verify signatures. </summary>
	/// <returns>The <see cref="T:System.Security.Cryptography.Xml.SignedXml" /> URI identifier for the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> instance used to create and verify signatures.</returns>
	/// <exception cref="T:System.ArgumentNullException">The string for the URI to set is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">The string for the URI to set is empty.</exception>
	public string HashAlgorithm
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

	/// <summary>Gets or sets the X.509 certificate embedding option used by the <see cref="M:System.IO.Packaging.PackageDigitalSignatureManager.Sign(System.Collections.Generic.IEnumerable{System.Uri})" /> method to digitally sign package parts. </summary>
	/// <returns>One of the <see cref="T:System.IO.Packaging.CertificateEmbeddingOption" /> values. </returns>
	public CertificateEmbeddingOption CertificateOption
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

	/// <summary>Gets or sets the date/time format used to create a signature <see cref="P:System.IO.Packaging.PackageDigitalSignature.SigningTime" />. </summary>
	/// <returns>The date/time format used to create a signature <see cref="P:System.IO.Packaging.PackageDigitalSignature.SigningTime" />.</returns>
	public string TimeFormat
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

	/// <summary>Gets the uniform resource identifier (URI) of the signature origin part.</summary>
	/// <returns>The URI of the signature origin part.</returns>
	public Uri SignatureOrigin
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the type of default signature origin relationship.</summary>
	/// <returns>The type of default signature origin relationship.</returns>
	public static string SignatureOriginRelationshipType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a URI string that identifies the default hash algorithm used to create and verify signatures. </summary>
	/// <returns>A URI string that identifies the default hash algorithm used to create and verify signatures.</returns>
	public static string DefaultHashAlgorithm
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal Package Package
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when <see cref="M:System.IO.Packaging.PackageDigitalSignatureManager.VerifySignatures(System.Boolean)" /> encounters an invalid signature. </summary>
	public event InvalidSignatureEventHandler InvalidSignatureEvent
	{
		[CompilerGenerated]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.IO.Packaging.PackageDigitalSignatureManager" /> class for use with a specified <see cref="T:System.IO.Packaging.Package" />. </summary>
	/// <param name="package">The package associated with this signature manager.</param>
	public PackageDigitalSignatureManager(Package package)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Prompts the user for an X.509 certificate, which is then used to digitally sign a specified list of package parts.</summary>
	/// <param name="parts">The list of uniform resource identifiers (URIs) for the <see cref="T:System.IO.Packaging.PackagePart" /> elements to sign.</param>
	/// <returns>The digital signature used to sign the list of <paramref name="parts" />.</returns>
	public PackageDigitalSignature Sign(IEnumerable<Uri> parts)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Signs a list of package parts with a given X.509 certificate. </summary>
	/// <param name="parts">The list of uniform resource identifiers (URIs) for the <see cref="T:System.IO.Packaging.PackagePart" /> elements to sign.</param>
	/// <param name="certificate">The X.509 certificate to use to digitally sign each of the specified <paramref name="parts" />.</param>
	/// <returns>The digital signature used to sign the given list of <paramref name="parts" />; or <see langword="null" /> if no certificate could be found or the user clicked "Cancel" in the certificate selection dialog box.</returns>
	public PackageDigitalSignature Sign(IEnumerable<Uri> parts, X509Certificate certificate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Signs a list of package parts and package relationships with a given X.509 certificate. </summary>
	/// <param name="parts">The list of uniform resource identifiers (URIs) for the <see cref="T:System.IO.Packaging.PackagePart" /> objects to sign.</param>
	/// <param name="certificate">The X.509 certificate to use to digitally sign each of the specified parts and relationships.</param>
	/// <param name="relationshipSelectors">The list of <see cref="T:System.IO.Packaging.PackageRelationship" /> objects to sign.</param>
	/// <returns>The digital signature used to sign the elements specified in the <paramref name="parts" /> and <paramref name="relationshipSelectors" /> lists.</returns>
	/// <exception cref="T:System.ArgumentException">Neither <paramref name="parts" /> nor <paramref name="relationshipSelectors" /> specify any objects to sign.</exception>
	public PackageDigitalSignature Sign(IEnumerable<Uri> parts, X509Certificate certificate, IEnumerable<PackageRelationshipSelector> relationshipSelectors)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Signs a list of package parts and package relationships with a given X.509 certificate and identifier (ID).</summary>
	/// <param name="parts">The list of uniform resource identifiers (URIs) for the <see cref="T:System.IO.Packaging.PackagePart" /> objects to sign.</param>
	/// <param name="certificate">The X.509 certificate to use to digitally sign each of the specified parts and relationships.</param>
	/// <param name="relationshipSelectors">The list of <see cref="T:System.IO.Packaging.PackageRelationship" /> objects to sign.</param>
	/// <param name="signatureId">An identification string to associate with the signature.</param>
	/// <returns>The digital signature used to sign the elements specified in the <paramref name="parts" /> and <paramref name="relationshipSelectors" /> lists.</returns>
	/// <exception cref="T:System.ArgumentException">Neither <paramref name="parts" /> nor <paramref name="relationshipSelectors" /> specify any elements to sign.</exception>
	public PackageDigitalSignature Sign(IEnumerable<Uri> parts, X509Certificate certificate, IEnumerable<PackageRelationshipSelector> relationshipSelectors, string signatureId)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Signs a list of package parts, package relationships, or custom objects with a specified X.509 certificate and signature identifier (ID).</summary>
	/// <param name="parts">The list of uniform resource identifiers (URIs) for the <see cref="T:System.IO.Packaging.PackagePart" /> objects to sign.</param>
	/// <param name="certificate">The X.509 certificate to use to digitally sign each of the specified parts and relationships.</param>
	/// <param name="relationshipSelectors">The list of <see cref="T:System.IO.Packaging.PackageRelationship" /> objects to sign.</param>
	/// <param name="signatureId">An identification string to associate with the signature.</param>
	/// <param name="signatureObjects">A list of custom data objects to sign.</param>
	/// <param name="objectReferences">A list of references to custom objects to sign.</param>
	/// <returns>The digital signature used to sign the elements specified in the <paramref name="parts" /> and <paramref name="relationshipSelectors" /> lists.</returns>
	/// <exception cref="T:System.ArgumentException">Neither <paramref name="parts" />, <paramref name="relationshipSelectors" />, <paramref name="signatureObjects" />, nor <paramref name="objectReferences" /> specify any elements to sign.</exception>
	/// <exception cref="T:System.InvalidOperationException">A <see cref="P:System.IO.Packaging.PackagePart.ContentType" /> of a part being signed references an empty, <see langword="null" />, or undefined <see cref="P:System.IO.Packaging.PackageDigitalSignatureManager.TransformMapping" />.</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="signatureId" /> is not <see langword="null" /> and is not a valid XML schema ID (for example, begins with a leading numeric digit).</exception>
	[SecurityCritical]
	public PackageDigitalSignature Sign(IEnumerable<Uri> parts, X509Certificate certificate, IEnumerable<PackageRelationshipSelector> relationshipSelectors, string signatureId, IEnumerable<DataObject> signatureObjects, IEnumerable<Reference> objectReferences)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Countersigns all the signatures in the package with a user-selected X.509 certificate.</summary>
	/// <returns>The signature that was added as a countersign; or <see langword="null" /> if no certificate could be located or the user canceled the certificate selection dialog.</returns>
	public PackageDigitalSignature Countersign()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Countersigns all the signatures in the package with a specified X.509 certificate. </summary>
	/// <param name="certificate">The X.509 certificate to add as a countersign signature.</param>
	/// <returns>The signature that was added as a countersign.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="certificate" /> is <see langword="null" /></exception>
	public PackageDigitalSignature Countersign(X509Certificate certificate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Countersigns a list of signatures with a given X.509 certificate.</summary>
	/// <param name="certificate">The X.509 certificate to countersign each of the specified <paramref name="signatures" />.</param>
	/// <param name="signatures">The list of signatures to countersign.</param>
	/// <returns>The digital signature used to countersign each of the <paramref name="signatures" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">Either the <paramref name="certificate" /> or <paramref name="signatures" /> parameter is <see langword="null" />.</exception>
	/// <exception cref="T:System.InvalidOperationException">The package contains no signed <see cref="T:System.IO.Packaging.PackagePart" /> objects.</exception>
	/// <exception cref="T:System.ArgumentException">The package contains no <see cref="T:System.IO.Packaging.PackageDigitalSignature" /> parts.</exception>
	public PackageDigitalSignature Countersign(X509Certificate certificate, IEnumerable<Uri> signatures)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Verifies the signatures on all signed parts within the package. </summary>
	/// <param name="exitOnFailure">
	///       <see langword="true" /> to exit on first failure; otherwise, <see langword="false" /> to continue and check all signatures.</param>
	/// <returns>
	///     <see cref="F:System.IO.Packaging.VerifyResult.Success" /> (value 0) if all signatures are verified successfully; otherwise, an enumeration that identifies the error.</returns>
	public VerifyResult VerifySignatures(bool exitOnFailure)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the digital signature with a given signature uniform resource identifier (URI). </summary>
	/// <param name="signatureUri">The URI of the <see cref="T:System.IO.Packaging.PackageDigitalSignature" /> to remove.</param>
	public void RemoveSignature(Uri signatureUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes all digital signatures from the package. </summary>
	public void RemoveAllSignatures()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the digital signature for a given signature uniform resource identifier (URI). </summary>
	/// <param name="signatureUri">The URI of the digital signature to return.</param>
	/// <returns>The digital signature for the specified <paramref name="signatureUri" />, or <see langword="null" /> if a signature for the given <paramref name="signatureUri" /> cannot be found.</returns>
	public PackageDigitalSignature GetSignature(Uri signatureUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Verifies a given X.509 certificate. </summary>
	/// <param name="certificate">The X.509 certificate to verify.</param>
	/// <returns>
	///     <see cref="F:System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError" /> (value 0) if the certificate verified successfully; otherwise, a bitwise enumeration of error flags.</returns>
	[SecurityCritical]
	[SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public static X509ChainStatusFlags VerifyCertificate(X509Certificate certificate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static X509Certificate PromptForSigningCertificate(IntPtr hwndParent)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static PackageDigitalSignatureManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
