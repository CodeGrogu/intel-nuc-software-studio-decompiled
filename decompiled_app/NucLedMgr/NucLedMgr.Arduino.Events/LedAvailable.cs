using Prism.Events;

namespace NucLedMgr.Arduino.Events;

public class LedAvailable : PubSubEvent<bool>
{
}
