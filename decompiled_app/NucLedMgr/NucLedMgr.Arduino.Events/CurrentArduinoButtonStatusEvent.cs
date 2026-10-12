using IntelSdkClient.ArduinoService;
using Prism.Events;

namespace NucLedMgr.Arduino.Events;

public class CurrentArduinoButtonStatusEvent : PubSubEvent<BntStatus>
{
}
