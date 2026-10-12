namespace System.Security.Permissions;

/// <summary>The <see cref="T:System.Security.Permissions.WebBrowserPermission" /> object controls the ability to create the WebBrowser control.</summary>
[Serializable]
public sealed class WebBrowserPermission : CodeAccessPermission, IUnrestrictedPermission
{
	/// <summary>Gets or sets the current value of the Web browser permission level.</summary>
	/// <returns>The current value of the Web browser permission level.</returns>
	public WebBrowserPermissionLevel Level
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.WebBrowserPermission" /> class.</summary>
	public WebBrowserPermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.WebBrowserPermission" /> class by specifying a permission state.</summary>
	/// <param name="state">An enumerated value of <see cref="T:System.Security.Permissions.PermissionState" />.</param>
	public WebBrowserPermission(PermissionState state)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.WebBrowserPermission" /> class by specifying the Web browser permission level.</summary>
	/// <param name="webBrowserPermissionLevel">An enumerated value of <see cref="T:System.Security.Permissions.WebBrowserPermissionLevel" />.</param>
	public WebBrowserPermission(WebBrowserPermissionLevel webBrowserPermissionLevel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value indicating whether the current permission is unrestricted.</summary>
	/// <returns>
	///     <see langword="true" /> if the <see cref="P:System.Security.Permissions.WebBrowserPermission.Level" /> property is set to <see cref="F:System.Security.Permissions.WebBrowserPermissionLevel.Unrestricted" />; otherwise, <see langword="false" />.</returns>
	public bool IsUnrestricted()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the current permission is a subset of the specified permission.</summary>
	/// <param name="target">A permission that is to be tested for the subset relationship. This permission must be of the same type as the current permission.</param>
	/// <returns>
	///     <see langword="true" /> if the current permission is a subset of the specified permission; otherwise, <see langword="false" />.</returns>
	public override bool IsSubsetOf(IPermission target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates and returns a permission that is the intersection of the current permission and the specified permission.</summary>
	/// <param name="target">A permission to intersect with the current permission. It must be of the same type as the current permission.</param>
	/// <returns>The intersection of two permissions is a permission that describes the state that they both describe in common. Only a demand that passes both original permissions will be valid for the intersected permission.</returns>
	public override IPermission Intersect(IPermission target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a permission that is the union of the current permission and the specified permission.</summary>
	/// <param name="target">A permission to combine with the current permission. It must be of the same type as the current permission.</param>
	/// <returns>A new permission that represents the union of the current permission and the specified permission.</returns>
	public override IPermission Union(IPermission target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates and returns an identical copy of the current permission.</summary>
	/// <returns>A copy of the current permission.</returns>
	public override IPermission Copy()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates an XML encoding of the permission and its current state.</summary>
	/// <returns>An XML encoding of the permission, including any state information.</returns>
	public override SecurityElement ToXml()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Reconstructs a permission with a specified state from an XML encoding.</summary>
	/// <param name="securityElement">The XML encoding to use to reconstruct the permission.</param>
	public override void FromXml(SecurityElement securityElement)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void VerifyWebBrowserPermissionLevel(WebBrowserPermissionLevel level)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
