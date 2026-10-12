namespace System.Security.Permissions;

/// <summary>The <see cref="T:System.Security.Permissions.MediaPermission" /> describes a set of security permissions that controls the ability for audio, image, and video media to work in a partial-trust Windows Presentation Foundation (WPF) application.</summary>
[Serializable]
public sealed class MediaPermission : CodeAccessPermission, IUnrestrictedPermission
{
	/// <summary>Gets the current value of the audio permission level.</summary>
	/// <returns>The audio permission level.</returns>
	public MediaPermissionAudio Audio
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the current value of the video permission level.</summary>
	/// <returns>The media permission level.</returns>
	public MediaPermissionVideo Video
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the current value of the image permission level.</summary>
	/// <returns>The image permission level.</returns>
	public MediaPermissionImage Image
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.MediaPermission" /> class.</summary>
	public MediaPermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.MediaPermission" /> class by specifying a permission state.</summary>
	/// <param name="state">An enumerated value of <see cref="T:System.Security.Permissions.PermissionState" />.</param>
	public MediaPermission(PermissionState state)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.MediaPermission" /> class by specifying the audio permission level.</summary>
	/// <param name="permissionAudio">An enumerated value of <see cref="T:System.Security.Permissions.MediaPermissionAudio" />.</param>
	public MediaPermission(MediaPermissionAudio permissionAudio)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.MediaPermission" /> class by specifying the video permission level.</summary>
	/// <param name="permissionVideo">An enumerated value of <see cref="T:System.Security.Permissions.MediaPermissionVideo" />.</param>
	public MediaPermission(MediaPermissionVideo permissionVideo)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.MediaPermission" /> class by specifying the image permission level.</summary>
	/// <param name="permissionImage">An enumerated value of <see cref="T:System.Security.Permissions.MediaPermissionImage" />.</param>
	public MediaPermission(MediaPermissionImage permissionImage)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.MediaPermission" /> class by specifying the audio, video, and image permission levels.</summary>
	/// <param name="permissionAudio">An enumerated value of <see cref="T:System.Security.Permissions.MediaPermissionAudio" />.</param>
	/// <param name="permissionVideo">An enumerated value of <see cref="T:System.Security.Permissions.MediaPermissionVideo" />.</param>
	/// <param name="permissionImage">An enumerated value of <see cref="T:System.Security.Permissions.MediaPermissionImage" />.</param>
	public MediaPermission(MediaPermissionAudio permissionAudio, MediaPermissionVideo permissionVideo, MediaPermissionImage permissionImage)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value indicating whether the current permission is unrestricted.</summary>
	/// <returns>
	///     <see langword="true" /> if the audio, image, and video permissions are all unrestricted; otherwise, <see langword="false" />.</returns>
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
	/// <param name="target">A permission to combine with the current permission. It must be of the same type as the current permission. </param>
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

	internal static void VerifyMediaPermissionAudio(MediaPermissionAudio level)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void VerifyMediaPermissionVideo(MediaPermissionVideo level)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void VerifyMediaPermissionImage(MediaPermissionImage level)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
