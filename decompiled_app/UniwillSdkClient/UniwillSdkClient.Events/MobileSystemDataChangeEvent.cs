using Prism.Events;
using UniwillSdkClient.UniwillService;

namespace UniwillSdkClient.Events;

public class MobileSystemDataChangeEvent : PubSubEvent<SystemMonitorData>
{
}
