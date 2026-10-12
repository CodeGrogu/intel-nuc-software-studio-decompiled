using System.Collections.Generic;
using MS.Internal.Security.RightsManagement;

namespace System.Security.RightsManagement;

/// <summary>Represents an unsigned rights managed <see cref="T:System.Security.RightsManagement.PublishLicense" /> or an unsigned <see cref="T:System.Security.RightsManagement.PublishLicense" /> template.</summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
public class UnsignedPublishLicense
{
	/// <summary>Gets or sets the content owner. </summary>
	/// <returns>The user who owns the published content.</returns>
	public ContentUser Owner
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

	/// <summary>Gets or sets the contact name for the author or publisher of the content.</summary>
	/// <returns>The contact name for the author or publisher of the content.</returns>
	public string ReferralInfoName
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

	/// <summary>Gets or sets the contact URI for the author or publisher of the content.</summary>
	/// <returns>The contact uniform resource identifier (URI) for the author or publisher of the content.</returns>
	public Uri ReferralInfoUri
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

	/// <summary>Gets or sets the publisher-created content identifier. </summary>
	/// <returns>The publisher-created rights-managed content identifier.</returns>
	public Guid ContentId
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

	/// <summary>Gets a collection of assigned user rights.</summary>
	/// <returns>A collection of the assigned user rights that are provided in this license.</returns>
	public ICollection<ContentGrant> Grants
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a collection of name and description pairs.</summary>
	/// <returns>A collection of name and description pairs.</returns>
	public IDictionary<int, LocalizedNameDescriptionPair> LocalizedNameDescriptionDictionary
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal int RightValidityIntervalDays
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

	internal IDictionary<string, string> ApplicationSpecificDataDictionary
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal RevocationPoint RevocationPoint
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.UnsignedPublishLicense" /> class. </summary>
	public UnsignedPublishLicense()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.UnsignedPublishLicense" /> class from a specified XrML publish-license template. </summary>
	/// <param name="publishLicenseTemplate">The Extensible Rights Markup Language (XrML) publish-license template to use to create this license.</param>
	public UnsignedPublishLicense(string publishLicenseTemplate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a signed <see cref="T:System.Security.RightsManagement.PublishLicense" /> and returns a <see cref="T:System.Security.RightsManagement.UseLicense" /> for the document author.</summary>
	/// <param name="secureEnvironment">The secure environment for license activation and binding.</param>
	/// <param name="authorUseLicense">A returned <see cref="T:System.Security.RightsManagement.UseLicense" /> for the document author.</param>
	/// <returns>The signed <see cref="T:System.Security.RightsManagement.PublishLicense" /> that is created by signing this <see cref="T:System.Security.RightsManagement.UnsignedPublishLicense" />.</returns>
	public PublishLicense Sign(SecureEnvironment secureEnvironment, out UseLicense authorUseLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a serialized template created from the XrML of the <see cref="T:System.Security.RightsManagement.UnsignedPublishLicense" />.</summary>
	/// <returns>A serialized template created from the XrML of the <see cref="T:System.Security.RightsManagement.UnsignedPublishLicense" />.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal UnsignedPublishLicense(SafeRightsManagementHandle boundLicenseHandle, string publishLicenseTemplate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
