using IntelSdkClient.PerformanceTuningService;
using Prism.Events;

namespace IntelSdkClient.Events;

public class SwitchCpuModesEvent : PubSubEvent<PowerProfile>
{
}
