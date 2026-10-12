using System.Security;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[FriendAccessAllowed]
internal struct SecurityCriticalData<T>
{
	[SecurityCritical]
	private T _value;

	internal T Value
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal SecurityCriticalData(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
