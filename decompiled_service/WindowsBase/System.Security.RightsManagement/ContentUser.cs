using System.Collections.Generic;

namespace System.Security.RightsManagement;

/// <summary>Represents a user or user-group for granting access to rights managed content.  </summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
public class ContentUser
{
	[SecurityTreatAsSafe]
	internal sealed class ContentUserComparer : IEqualityComparer<ContentUser>
	{
		bool IEqualityComparer<ContentUser>.Equals(ContentUser user1, ContentUser user2)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		int IEqualityComparer<ContentUser>.GetHashCode(ContentUser user)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public ContentUserComparer()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecurityTreatAsSafe]
	internal static readonly ContentUserComparer _contentUserComparer;

	/// <summary>Gets the <see cref="T:System.Security.RightsManagement.AuthenticationType" /> specified to the <see cref="M:System.Security.RightsManagement.ContentUser.#ctor(System.String,System.Security.RightsManagement.AuthenticationType)" /> constructor.</summary>
	/// <returns>The <see cref="T:System.Security.RightsManagement.AuthenticationType" /> specified to the <see cref="M:System.Security.RightsManagement.ContentUser.#ctor(System.String,System.Security.RightsManagement.AuthenticationType)" /> constructor.</returns>
	public AuthenticationType AuthenticationType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the user or group name specified to the <see cref="M:System.Security.RightsManagement.ContentUser.#ctor(System.String,System.Security.RightsManagement.AuthenticationType)" /> constructor.</summary>
	/// <returns>The user or group name specified to the <see cref="M:System.Security.RightsManagement.ContentUser.#ctor(System.String,System.Security.RightsManagement.AuthenticationType)" /> constructor.</returns>
	public string Name
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets an instance of the "Anyone" <see cref="T:System.Security.RightsManagement.ContentUser" /> persona.</summary>
	/// <returns>An instance of the "Anyone" <see cref="T:System.Security.RightsManagement.ContentUser" /> persona.</returns>
	public static ContentUser AnyoneUser
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets an instance of the "Owner" <see cref="T:System.Security.RightsManagement.ContentUser" /> persona.</summary>
	/// <returns>An instance of the "Owner" <see cref="T:System.Security.RightsManagement.ContentUser" /> persona.</returns>
	public static ContentUser OwnerUser
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string AuthenticationProviderType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.ContentUser" /> class.</summary>
	/// <param name="name">The user or group name.</param>
	/// <param name="authenticationType">The method for authentication.</param>
	public ContentUser(string name, AuthenticationType authenticationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether the user is currently authenticated.</summary>
	/// <returns>
	///     <see langword="true" /> if the user is currently authenticated; otherwise, <see langword="false" />.  The default is <see langword="false" /> until authenticated.</returns>
	public bool IsAuthenticated()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether this <see cref="T:System.Security.RightsManagement.ContentUser" /> is equivalent to another given instance.</summary>
	/// <param name="obj">The user instance to compare for equality.</param>
	/// <returns>
	///     <see langword="true" /> if <see cref="P:System.Security.RightsManagement.ContentUser.Name" /> and <see cref="P:System.Security.RightsManagement.ContentUser.AuthenticationType" /> are the same for both this user and the given user; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a computed hash code based on the user <see cref="P:System.Security.RightsManagement.ContentUser.Name" /> and <see cref="P:System.Security.RightsManagement.ContentUser.AuthenticationType" />.</summary>
	/// <returns>A hash code computed from the user <see cref="P:System.Security.RightsManagement.ContentUser.Name" /> and <see cref="P:System.Security.RightsManagement.ContentUser.AuthenticationType" />.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityTreatAsSafe]
	internal bool GenericEquals(ContentUser userObj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool CompareToAnyone(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool CompareToOwner(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static ContentUser()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
