using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using MS.Internal.IO.Packaging;

namespace System.IO.Packaging;

/// <summary>Represents a digital signature that is applied to a set of package parts and relationships.</summary>
public class PackageDigitalSignature
{
	/// <summary>Gets a collection of all the <see cref="T:System.IO.Packaging.PackagePart" /> objects signed with the signature. </summary>
	/// <returns>A collection that contains all the package parts signed with the signature.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public ReadOnlyCollection<Uri> SignedParts
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the list of the <see cref="T:System.IO.Packaging.PackageRelationship" /> parts that have been signed with the signature.</summary>
	/// <returns>The list of the <see cref="T:System.IO.Packaging.PackageRelationship" /> parts that have been signed with the signature.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public ReadOnlyCollection<PackageRelationshipSelector> SignedRelationshipSelectors
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the <see cref="T:System.IO.Packaging.PackagePart" /> that contains the signature. </summary>
	/// <returns>The package part that contains the signature.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public PackagePart SignaturePart
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the X.509 certificate of the signer. </summary>
	/// <returns>The digital certificate of the signer, or <see langword="null" /> if the certificate is not stored in the <see cref="T:System.IO.Packaging.Package" />.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public X509Certificate Signer
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the date and time that the signature was created. </summary>
	/// <returns>The date and time that the signature was created.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public DateTime SigningTime
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the format of the date and time returned by the <see cref="P:System.IO.Packaging.PackageDigitalSignature.SigningTime" /> property. </summary>
	/// <returns>The format of the date and time returned by the <see cref="P:System.IO.Packaging.PackageDigitalSignature.SigningTime" /> property.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public string TimeFormat
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the encrypted hash value of the <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" />. </summary>
	/// <returns>A byte array that contains the encrypted hash value of the <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" />.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public byte[] SignatureValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the URI string that identifies the signature type. </summary>
	/// <returns>A URI string that identifies the signature type. The default is <see cref="F:System.Security.Cryptography.Xml.SignedXml.XmlDsigC14NTransformUrl" />.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public string SignatureType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the XML digital signature.</summary>
	/// <returns>The XML digital signature.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public System.Security.Cryptography.Xml.Signature Signature
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

	/// <summary>Gets the X.509 certificate embedding option. </summary>
	/// <returns>One of the <see cref="T:System.IO.Packaging.CertificateEmbeddingOption" /> values that specifies the option for the digital signature.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public CertificateEmbeddingOption CertificateEmbeddingOption
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Returns an ordered list of the <see cref="T:System.Security.Cryptography.Xml.Transform" /> operations applied to a given part. </summary>
	/// <param name="partName">The <see cref="T:System.Uri" /> of the <see cref="T:System.IO.Packaging.PackagePart" /> to return the transform list for.</param>
	/// <returns>An ordered list of URI strings, such as <see cref="F:System.Security.Cryptography.Xml.SignedXml.XmlDsigCanonicalizationUrl" /> or <see cref="F:System.Security.Cryptography.Xml.SignedXml.XmlDsigC14NTransformUrl" />, that represents the canonical XML transformations applied to the part with the given <paramref name="partName" /> URI.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public List<string> GetPartTransformList(Uri partName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Verifies the digital signature against an X.509 certificate. </summary>
	/// <returns>
	///     <see cref="F:System.IO.Packaging.VerifyResult.Success" /> if the verification succeeded; otherwise, one of the <see cref="T:System.IO.Packaging.VerifyResult" /> values that identifies a problem. </returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	public VerifyResult Verify()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Verifies the digital signature against a given X.509 certificate. </summary>
	/// <param name="signingCertificate">The signer's X.509 certificate to verify the digital signature against.</param>
	/// <returns>
	///     <see cref="F:System.IO.Packaging.VerifyResult.Success" /> if the verification succeeded; otherwise, one of the <see cref="T:System.IO.Packaging.VerifyResult" /> values that identifies a problem.</returns>
	/// <exception cref="T:System.InvalidOperationException">The digital <see cref="P:System.IO.Packaging.PackageDigitalSignature.Signature" /> has been deleted.</exception>
	[SecurityCritical]
	[SecurityTreatAsSafe]
	public VerifyResult Verify(X509Certificate signingCertificate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PackageDigitalSignature(PackageDigitalSignatureManager manager, XmlDigitalSignatureProcessor processor)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PackageDigitalSignature(PackageDigitalSignatureManager manager, PackagePart signaturePart)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Invalidate()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal CertificatePart GetCertificatePart()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SetCertificatePart(CertificatePart certificatePart)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
