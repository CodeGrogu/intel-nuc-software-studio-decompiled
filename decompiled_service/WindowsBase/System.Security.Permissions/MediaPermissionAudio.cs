namespace System.Security.Permissions;

/// <summary>Specifies the permission level for audio.</summary>
public enum MediaPermissionAudio
{
	/// <summary>Permission that denies all audio playback.</summary>
	NoAudio,
	/// <summary>Permission that only allows audio playback of files that are located at the application's site of origin.</summary>
	SiteOfOriginAudio,
	/// <summary>Permission that allows safe, limited audio playback, which includes denying stream access to the image file.</summary>
	SafeAudio,
	/// <summary>Permission that allows unrestricted audio playback.</summary>
	AllAudio
}
