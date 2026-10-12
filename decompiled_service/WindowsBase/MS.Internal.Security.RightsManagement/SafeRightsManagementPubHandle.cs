using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Security.RightsManagement;

[SecurityCritical(SecurityCriticalScope.Everything)]
internal sealed class SafeRightsManagementPubHandle : SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static SafeRightsManagementPubHandle InvalidHandle
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	private SafeRightsManagementPubHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal SafeRightsManagementPubHandle(uint handle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override bool ReleaseHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static SafeRightsManagementPubHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
