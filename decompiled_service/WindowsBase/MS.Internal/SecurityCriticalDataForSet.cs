using System;
using System.Security;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[Serializable]
[FriendAccessAllowed]
internal struct SecurityCriticalDataForSet<T>
{
	[SecurityCritical]
	private T _value;

	internal T Value
	{
		[SecurityCritical]
		[SecurityTreatAsSafe]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecurityCritical]
	internal SecurityCriticalDataForSet(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
