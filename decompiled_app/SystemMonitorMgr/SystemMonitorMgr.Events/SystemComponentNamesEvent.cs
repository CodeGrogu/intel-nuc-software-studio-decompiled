using System.Collections.Generic;
using SystemMonitorMgr.Enums;
using Prism.Events;

namespace SystemMonitorMgr.Events;

public class SystemComponentNamesEvent : PubSubEvent<Dictionary<SystemDataType, string>>
{
}
