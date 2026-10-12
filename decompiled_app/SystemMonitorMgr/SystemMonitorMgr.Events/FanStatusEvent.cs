using System.Collections.Generic;
using SystemMonitorMgr.Models;
using Prism.Events;

namespace SystemMonitorMgr.Events;

public class FanStatusEvent : PubSubEvent<List<FanData>>
{
}
