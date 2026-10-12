using System;
using System.Security;
using System.Security.Permissions;
using MS.Internal.WindowsBase;

namespace MS.Internal.Permissions;

[Serializable]
[FriendAccessAllowed]
internal class CompoundFileIOPermission : InternalParameterlessPermissionBase
{
	public CompoundFileIOPermission()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public CompoundFileIOPermission(PermissionState state)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override IPermission Copy()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
