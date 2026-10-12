using System;
using IntelSdkClient.SystemMonitorService;
using Prism.Events;

namespace IntelSdkClient.SystemMonitor;

public class SystemMonitorDataEvent : PubSubEvent<Tuple<FanSpeed[], SystemMonitorData>>
{
}
