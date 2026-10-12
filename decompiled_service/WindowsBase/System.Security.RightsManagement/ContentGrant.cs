namespace System.Security.RightsManagement;

/// <summary>Represents a right granted to a user to access information in a rights managed document.</summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
public class ContentGrant
{
	/// <summary>Gets the user who is granted the access <see cref="P:System.Security.RightsManagement.ContentGrant.Right" />.</summary>
	/// <returns>The user that the access <see cref="P:System.Security.RightsManagement.ContentGrant.Right" /> is granted to, as specified to the <see cref="M:System.Security.RightsManagement.ContentGrant.#ctor(System.Security.RightsManagement.ContentUser,System.Security.RightsManagement.ContentRight)" /> constructor.</returns>
	public ContentUser User
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the <see cref="T:System.Security.RightsManagement.ContentRight" /> that is granted.</summary>
	/// <returns>The access right that is granted to the <see cref="P:System.Security.RightsManagement.ContentGrant.User" />, as specified to the <see cref="M:System.Security.RightsManagement.ContentGrant.#ctor(System.Security.RightsManagement.ContentUser,System.Security.RightsManagement.ContentRight)" /> constructor.</returns>
	public ContentRight Right
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the starting date and time that the granted <see cref="P:System.Security.RightsManagement.ContentGrant.Right" /> begins.</summary>
	/// <returns>The start date and time that the granted <see cref="P:System.Security.RightsManagement.ContentGrant.Right" /> begins, or <see cref="T:System.DateTime" />.<see cref="F:System.DateTime.MinValue" /> if there is there is no starting limitation.</returns>
	public DateTime ValidFrom
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the ending date and time that the granted <see cref="P:System.Security.RightsManagement.ContentGrant.Right" /> expires.</summary>
	/// <returns>The end date and time that the granted <see cref="P:System.Security.RightsManagement.ContentGrant.Right" /> expires, or <see cref="T:System.DateTime" />.<see cref="F:System.DateTime.MaxValue" /> if there is no ending limitation.</returns>
	public DateTime ValidUntil
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.ContentGrant" /> class that grants a specified <see cref="T:System.Security.RightsManagement.ContentUser" /> a specified <see cref="T:System.Security.RightsManagement.ContentRight" />.</summary>
	/// <param name="user">The user the access right is granted to.</param>
	/// <param name="right">The access right that is granted.</param>
	public ContentGrant(ContentUser user, ContentRight right)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.ContentGrant" /> class that grants a specified <see cref="T:System.Security.RightsManagement.ContentUser" /> a specified <see cref="T:System.Security.RightsManagement.ContentRight" /> for a specified <see cref="T:System.DateTime" /> duration.</summary>
	/// <param name="user">The user the access right is granted to.</param>
	/// <param name="right">The access right that is granted.</param>
	/// <param name="validFrom">The starting date and time that the right begins.</param>
	/// <param name="validUntil">The ending date and time that the right expires.</param>
	public ContentGrant(ContentUser user, ContentRight right, DateTime validFrom, DateTime validUntil)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
