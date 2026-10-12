using System;
using System.Security;
using System.Security.Permissions;

namespace MS.Internal.Permissions;

[Serializable]
[AttributeUsage(AttributeTargets.Method)]
internal sealed class UserInitiatedRoutedEventPermissionAttribute : CodeAccessSecurityAttribute
{
	public UserInitiatedRoutedEventPermissionAttribute(SecurityAction action)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override IPermission CreatePermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
