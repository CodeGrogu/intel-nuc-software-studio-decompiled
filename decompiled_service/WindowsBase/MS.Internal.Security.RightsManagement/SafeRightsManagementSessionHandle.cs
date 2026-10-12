using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Security.RightsManagement;

[SecurityCritical(SecurityCriticalScope.Everything)]
internal sealed class SafeRightsManagementSessionHandle : SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	private SafeRightsManagementSessionHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal SafeRightsManagementSessionHandle(uint handle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected override bool ReleaseHandle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
