using MS.Internal.WindowsBase;

namespace System.Windows;

[FriendAccessAllowed]
internal enum UpdateResult
{
	ValueChanged = 1,
	NotificationSent = 2,
	InheritedValueOverridden = 4
}
