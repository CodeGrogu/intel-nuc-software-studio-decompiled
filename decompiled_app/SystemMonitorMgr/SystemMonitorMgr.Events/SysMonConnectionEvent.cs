using Infrastructure.Models;
using Prism.Events;

namespace SystemMonitorMgr.Events;

public class SysMonConnectionEvent : PubSubEvent<ClientState>
{
}
