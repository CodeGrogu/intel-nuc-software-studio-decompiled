using System;
using System.Security;
using System.Security.Permissions;
using MS.Internal.WindowsBase;

namespace MS.Internal.Permissions;

[Serializable]
[FriendAccessAllowed]
internal abstract class InternalParameterlessPermissionBase : CodeAccessPermission, IUnrestrictedPermission
{
	protected InternalParameterlessPermissionBase(PermissionState state)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public bool IsUnrestricted()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override SecurityElement ToXml()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void FromXml(SecurityElement elem)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override IPermission Intersect(IPermission target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool IsSubsetOf(IPermission target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override IPermission Union(IPermission target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
