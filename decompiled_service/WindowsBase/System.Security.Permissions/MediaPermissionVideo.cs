namespace System.Security.Permissions;

/// <summary>Specifies the permission level for video.</summary>
public enum MediaPermissionVideo
{
	/// <summary>Permission that denies all video playback.</summary>
	NoVideo,
	/// <summary>Permission that only allows video playback of files that are located at the application's site of origin.</summary>
	SiteOfOriginVideo,
	/// <summary>Permission that allows safe, limited video playback, which includes denying stream access to the image file.</summary>
	SafeVideo,
	/// <summary>Permission that allows unrestricted video playback.</summary>
	AllVideo
}
