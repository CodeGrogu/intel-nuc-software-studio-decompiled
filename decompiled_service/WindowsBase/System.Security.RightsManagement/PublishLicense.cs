namespace System.Security.RightsManagement;

/// <summary>Represents a signed rights managed publish license.</summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
public class PublishLicense
{
	/// <summary>Gets the contact name for the author or publisher of the content.</summary>
	/// <returns>The contact name for the author or publisher of the content.</returns>
	public string ReferralInfoName
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the contact URI for the author or publisher of the content.</summary>
	/// <returns>The contact uniform resource identifier (URI) for the author or publisher of the content.</returns>
	public Uri ReferralInfoUri
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the publisher-created content identifier. </summary>
	/// <returns>The publisher-created content identifier.</returns>
	public Guid ContentId
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the URI to use for acquiring a <see cref="T:System.Security.RightsManagement.UseLicense" />.</summary>
	/// <returns>The URI to use for acquiring a <see cref="T:System.Security.RightsManagement.UseLicense" />.</returns>
	public Uri UseLicenseAcquisitionUrl
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.PublishLicense" /> class from a specified serialized and signed <see cref="T:System.Security.RightsManagement.PublishLicense" />.</summary>
	/// <param name="signedPublishLicense">A signed and serialized publish license.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="signedPublishLicense" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Security.RightsManagement.RightsManagementException">The license is invalid.</exception>
	public PublishLicense(string signedPublishLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a decrypted <see cref="T:System.Security.RightsManagement.UnsignedPublishLicense" /> version of this signed <see cref="T:System.Security.RightsManagement.PublishLicense" />.</summary>
	/// <param name="cryptoProvider">The rights management service to use for decrypting the license.</param>
	/// <returns>A decrypted, unsigned version of this license.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="cryptoProvider" /> is <see langword="null" />.</exception>
	public UnsignedPublishLicense DecryptUnsignedPublishLicense(CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the serialized XrML string that was used to create this license.</summary>
	/// <returns>The serialized Extensible Rights Markup Language (XrML) string that was used to create this license.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to acquire a <see cref="T:System.Security.RightsManagement.UseLicense" /> for a user or user group in a specified <see cref="T:System.Security.RightsManagement.SecureEnvironment" />.</summary>
	/// <param name="secureEnvironment">The secure environment for license activation and binding.</param>
	/// <returns>The <see cref="T:System.Security.RightsManagement.UseLicense" /> for a user or user group in the specified <paramref name="secureEnvironment" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="secureEnvironment" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Security.RightsManagement.RightsManagementException">The authentication failed.</exception>
	public UseLicense AcquireUseLicense(SecureEnvironment secureEnvironment)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to acquire a <see cref="T:System.Security.RightsManagement.UseLicense" /> for a user or user group in a specified <see cref="T:System.Security.RightsManagement.SecureEnvironment" />.</summary>
	/// <param name="secureEnvironment">The secure environment for license activation and binding.</param>
	/// <returns>The <see cref="T:System.Security.RightsManagement.UseLicense" /> for a user or user group in the specified <paramref name="secureEnvironment" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="secureEnvironment" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Security.RightsManagement.RightsManagementException">The authentication failed.</exception>
	public UseLicense AcquireUseLicenseNoUI(SecureEnvironment secureEnvironment)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
