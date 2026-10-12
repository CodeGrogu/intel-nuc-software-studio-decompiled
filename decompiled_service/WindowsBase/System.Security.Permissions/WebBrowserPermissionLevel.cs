namespace System.Security.Permissions;

/// <summary>Specifies the permission level for a Web browser.</summary>
public enum WebBrowserPermissionLevel
{
	/// <summary>A Web browser that cannot navigate frames to HTML.</summary>
	None,
	/// <summary>A Web browser that can safely navigate frames to HTML but with several restrictions.</summary>
	Safe,
	/// <summary>A Web browser that can navigate without restrictions.</summary>
	Unrestricted
}
