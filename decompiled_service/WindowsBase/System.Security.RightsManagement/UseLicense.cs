using System.Collections.Generic;

namespace System.Security.RightsManagement;

/// <summary>Represents a license that enables access to protected rights managed content. </summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
public class UseLicense
{
	/// <summary>Gets the owner of the license. </summary>
	/// <returns>The owner of the license. </returns>
	public ContentUser Owner
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the content identifier created by the publisher.</summary>
	/// <returns>The content identifier created by the publisher.</returns>
	public Guid ContentId
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the application data dictionary that contains key/value pairs passed from the publishing application to the consuming application.</summary>
	/// <returns>The application data dictionary that contains key/value pairs passed from the publishing application to the consuming application.</returns>
	public IDictionary<string, string> ApplicationData
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.UseLicense" /> class. </summary>
	/// <param name="useLicense">A use license in serialized Extensible Rights Markup Language (XrML) form.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="useLicense" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Security.RightsManagement.RightsManagementException">
	///         <paramref name="useLicense" /> is invalid.</exception>
	public UseLicense(string useLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the serialized XrML string used to create this license.</summary>
	/// <returns>The serialized Extensible Rights Markup Language (XrML) string originally passed to the <see cref="M:System.Security.RightsManagement.UseLicense.#ctor(System.String)" /> constructor.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Binds the license to a given <see cref="T:System.Security.RightsManagement.SecureEnvironment" />.</summary>
	/// <param name="secureEnvironment">The environment to bind the license to.</param>
	/// <returns>A <see cref="T:System.Security.RightsManagement.CryptoProvider" /> instance if the license binding succeeded; otherwise, <see langword="null" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="secureEnvironment" /> is <see langword="null" />.</exception>
	public CryptoProvider Bind(SecureEnvironment secureEnvironment)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates if this license is equivalent to another given license.</summary>
	/// <param name="x">The license to compare.</param>
	/// <returns>
	///     <see langword="true" /> if both licenses are the equivalent; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object x)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the hash code associated with this license.</summary>
	/// <returns>A hash code for this license. </returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
