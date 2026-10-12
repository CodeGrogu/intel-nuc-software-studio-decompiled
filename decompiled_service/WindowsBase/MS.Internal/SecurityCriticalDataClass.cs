using System.Security;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[FriendAccessAllowed]
internal class SecurityCriticalDataClass<T>
{
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
	internal SecurityCriticalDataClass(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
