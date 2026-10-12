using System;
using System.Security;
using System.Security.Permissions;
using MS.Internal.WindowsBase;

namespace MS.Internal.Permissions;

[Serializable]
[FriendAccessAllowed]
internal class RightsManagementPermission : InternalParameterlessPermissionBase
{
	public RightsManagementPermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public RightsManagementPermission(PermissionState state)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override IPermission Copy()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
