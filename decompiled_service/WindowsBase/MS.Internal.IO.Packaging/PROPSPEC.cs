using MS.Internal.WindowsBase;

namespace MS.Internal.IO.Packaging;

[FriendAccessAllowed]
internal struct PROPSPEC
{
	internal uint propType;

	internal PROPSPECunion union;
}
