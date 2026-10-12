using MS.Internal.WindowsBase;

namespace System.Windows;

[FriendAccessAllowed]
internal abstract class DeferredReference
{
	internal abstract object GetValue(BaseValueSourceInternal valueSource);

	internal abstract Type GetValueType();

	protected DeferredReference()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
