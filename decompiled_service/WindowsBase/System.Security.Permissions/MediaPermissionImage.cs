namespace System.Security.Permissions;

/// <summary>Specifies the permission level for images.</summary>
public enum MediaPermissionImage
{
	/// <summary>Permission that denies all images to be displayed.</summary>
	NoImage,
	/// <summary>Permission that only allows display of images that are located at the application's site of origin.</summary>
	SiteOfOriginImage,
	/// <summary>Permission that allows safe, limited image display, which includes denying stream access to the image file.</summary>
	SafeImage,
	/// <summary>Permission that allows unrestricted image display.</summary>
	AllImage
}
