namespace System.Security.Permissions;

/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.MediaPermission" /> to be applied to code using declarative security.</summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class MediaPermissionAttribute : CodeAccessSecurityAttribute
{
	/// <summary>Gets or sets the audio permission level for the <see cref="T:System.Security.Permissions.MediaPermissionAttribute" />.</summary>
	/// <returns>The state of the <see cref="T:System.Security.Permissions.MediaPermissionAudio" />.</returns>
	public MediaPermissionAudio Audio
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

	/// <summary>Gets or sets the video permission level for the <see cref="T:System.Security.Permissions.MediaPermissionAttribute" />.</summary>
	/// <returns>The state of the <see cref="T:System.Security.Permissions.MediaPermissionVideo" />.</returns>
	public MediaPermissionVideo Video
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

	/// <summary>Gets or sets the image permission level for the <see cref="T:System.Security.Permissions.MediaPermissionAttribute" />.</summary>
	/// <returns>The state of the <see cref="T:System.Security.Permissions.MediaPermissionImage" />.</returns>
	public MediaPermissionImage Image
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

	/// <summary>Initializes a new instance of <see cref="T:System.Security.Permissions.MediaPermissionAttribute" />.</summary>
	/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values.</param>
	public MediaPermissionAttribute(SecurityAction action)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates and returns a new <see cref="T:System.Security.Permissions.MediaPermission" />.</summary>
	/// <returns>A <see cref="T:System.Security.Permissions.MediaPermission" /> that corresponds to this attribute.</returns>
	public override IPermission CreatePermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
