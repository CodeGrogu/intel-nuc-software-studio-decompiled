namespace System.IO.Packaging;

/// <summary> Specifies the event args provided to the <see cref="T:System.IO.Packaging.InvalidSignatureEventHandler" />. </summary>
public class SignatureVerificationEventArgs : EventArgs
{
	/// <summary> Gets the digital signature being verified. </summary>
	/// <returns>The digital signature being verified.</returns>
	public PackageDigitalSignature Signature
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary> Gets the signature verification error. </summary>
	/// <returns>The signature verification error.</returns>
	public VerifyResult VerifyResult
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal SignatureVerificationEventArgs(PackageDigitalSignature signature, VerifyResult result)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
