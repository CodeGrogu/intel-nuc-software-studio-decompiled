using System.Collections.ObjectModel;
using MS.Internal.Security.RightsManagement;

namespace System.Security.RightsManagement;

/// <summary>Represents a secure client session for user activation, license binding, and other rights management operations.</summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
public class SecureEnvironment : IDisposable
{
	/// <summary>Gets the user or user-group specified when the <see cref="T:System.Security.RightsManagement.SecureEnvironment" /> was created. </summary>
	/// <returns>The user or user-group specified to the <see cref="M:System.Security.RightsManagement.SecureEnvironment.Create(System.String,System.Security.RightsManagement.ContentUser)" /> method when the <see cref="T:System.Security.RightsManagement.SecureEnvironment" /> was created.</returns>
	public ContentUser User
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the <see cref="P:System.Security.RightsManagement.SecureEnvironment.ApplicationManifest" /> specified when the <see cref="T:System.Security.RightsManagement.SecureEnvironment" /> was created.</summary>
	/// <returns>The application manifest specified to the <see cref="Overload:System.Security.RightsManagement.SecureEnvironment.Create" /> method when the <see cref="T:System.Security.RightsManagement.SecureEnvironment" /> is created.</returns>
	public string ApplicationManifest
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ClientSession ClientSession
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Creates a secure client session for a specified user with a given rights manifest.</summary>
	/// <param name="applicationManifest">The application rights manifest.</param>
	/// <param name="user">The user or user-group for granting access to rights managed content.</param>
	/// <returns>A secure client session for activation, license binding, and other rights management operations.</returns>
	public static SecureEnvironment Create(string applicationManifest, ContentUser user)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a secure client session given an application rights manifest, <see cref="T:System.Security.RightsManagement.AuthenticationType" />, and <see cref="T:System.Security.RightsManagement.UserActivationMode" />.</summary>
	/// <param name="applicationManifest">The application rights manifest.</param>
	/// <param name="authentication">The method of authentication.</param>
	/// <param name="userActivationMode">The type of the user rights account certificate.</param>
	/// <returns>A secure client session for activation, license binding, and other rights management operations.</returns>
	public static SecureEnvironment Create(string applicationManifest, AuthenticationType authentication, UserActivationMode userActivationMode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether a given user has been activated for accessing rights managed content.</summary>
	/// <param name="user">The user or user-group for granting access to rights managed content.</param>
	/// <returns>
	///     <see langword="true" /> if the given <paramref name="user" /> has been activated for accessing rights managed content; otherwise, <see langword="false" />.</returns>
	public static bool IsUserActivated(ContentUser user)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the license activation for a specified user.</summary>
	/// <param name="user">The user to remove the license activation for.</param>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="user" /> parameter is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The given <paramref name="user" /> is not authenticated with either Windows authentication or Passport authentication.</exception>
	public static void RemoveActivatedUser(ContentUser user)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a list of the activated users.</summary>
	/// <returns>A list of the currently activated users.</returns>
	public static ReadOnlyCollection<ContentUser> GetActivatedUsers()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Releases all resources used by the <see cref="T:System.Security.RightsManagement.SecureEnvironment" />.</summary>
	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.RightsManagement.SecureEnvironment" /> and optionally releases the managed resources. </summary>
	/// <param name="disposing">
	///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
	protected virtual void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private SecureEnvironment()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
