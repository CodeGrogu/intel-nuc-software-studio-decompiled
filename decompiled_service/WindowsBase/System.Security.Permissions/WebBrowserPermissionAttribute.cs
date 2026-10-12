namespace System.Security.Permissions;

/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.WebBrowserPermission" /> to be applied to code using declarative security.</summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class WebBrowserPermissionAttribute : CodeAccessSecurityAttribute
{
	/// <summary>Gets or sets the current Web browser permission level for the <see cref="T:System.Security.Permissions.WebBrowserPermissionAttribute" />.</summary>
	/// <returns>The state of the current Web browser permission level.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.WebBrowserPermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
	/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values.</param>
	public WebBrowserPermissionAttribute(SecurityAction action)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates and returns a new instance of the <see cref="T:System.Security.Permissions.WebBrowserPermission" /> class.</summary>
	/// <returns>A <see cref="T:System.Security.Permissions.WebBrowserPermission" /> corresponding to the security declaration.</returns>
	public override IPermission CreatePermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
