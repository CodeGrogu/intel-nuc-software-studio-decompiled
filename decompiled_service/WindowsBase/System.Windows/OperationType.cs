using MS.Internal.WindowsBase;

namespace System.Windows;

[FriendAccessAllowed]
internal enum OperationType : byte
{
	Unknown,
	AddChild,
	RemoveChild,
	Inherit,
	ChangeMutableDefaultValue
}
