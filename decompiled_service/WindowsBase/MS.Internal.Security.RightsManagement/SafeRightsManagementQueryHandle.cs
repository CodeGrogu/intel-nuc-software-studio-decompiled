using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Security.RightsManagement;

[SecurityCritical(SecurityCriticalScope.Everything)]
internal sealed class SafeRightsManagementQueryHandle : SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	private SafeRightsManagementQueryHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal SafeRightsManagementQueryHandle(uint handle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override bool ReleaseHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
