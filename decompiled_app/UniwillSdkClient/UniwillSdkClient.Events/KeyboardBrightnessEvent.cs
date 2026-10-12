using Prism.Events;

namespace UniwillSdkClient.Events;

public class KeyboardBrightnessEvent : PubSubEvent<(int Ac, int Dc)>
{
}
