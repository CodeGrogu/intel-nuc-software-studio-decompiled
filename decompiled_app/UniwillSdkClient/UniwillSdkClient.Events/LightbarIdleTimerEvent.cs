using Prism.Events;

namespace UniwillSdkClient.Events;

public class LightbarIdleTimerEvent : PubSubEvent<(int Ac, int Dc)>
{
}
