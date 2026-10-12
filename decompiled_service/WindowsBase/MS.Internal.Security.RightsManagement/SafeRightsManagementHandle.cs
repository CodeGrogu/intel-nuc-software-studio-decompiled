using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Security.RightsManagement;

[SecurityCritical(SecurityCriticalScope.Everything)]
internal sealed class SafeRightsManagementHandle : SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static SafeRightsManagementHandle InvalidHandle
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	private SafeRightsManagementHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal SafeRightsManagementHandle(uint handle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override bool ReleaseHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static SafeRightsManagementHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
