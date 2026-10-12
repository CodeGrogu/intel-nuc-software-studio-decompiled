using MS.Internal.WindowsBase;

namespace System.Windows;

[FriendAccessAllowed]
internal interface ISealable
{
	bool CanSeal { get; }

	bool IsSealed { get; }

	void Seal();
}
