using System;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[FriendAccessAllowed]
[Flags]
internal enum ShutDownEvents : ushort
{
	DomainUnload = 1,
	ProcessExit = 2,
	DispatcherShutdown = 4,
	AppDomain = DomainUnload | ProcessExit,
	All = AppDomain | DispatcherShutdown
}
