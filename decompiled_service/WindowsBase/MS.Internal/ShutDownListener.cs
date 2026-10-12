using System;
using System.Security;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[FriendAccessAllowed]
internal abstract class ShutDownListener : WeakReference
{
	[SecurityCritical]
	internal ShutDownListener(object target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal ShutDownListener(object target, ShutDownEvents events)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal abstract void OnShutDown(object target, object sender, EventArgs e);

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal void StopListening()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
