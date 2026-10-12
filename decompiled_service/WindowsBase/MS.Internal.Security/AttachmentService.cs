using System;
using System.Security;
using MS.Internal.WindowsBase;

namespace MS.Internal.Security;

[FriendAccessAllowed]
internal sealed class AttachmentService : IDisposable
{
	[SecurityCritical]
	[SecurityTreatAsSafe]
	private AttachmentService()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void SaveWithUI(IntPtr parent, Uri source, Uri target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	~AttachmentService()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
