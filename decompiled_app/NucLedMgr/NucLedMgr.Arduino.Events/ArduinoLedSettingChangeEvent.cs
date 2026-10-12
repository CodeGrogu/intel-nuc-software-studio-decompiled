using IntelSdkClient.ArduinoService;
using Prism.Events;

namespace NucLedMgr.Arduino.Events;

internal class ArduinoLedSettingChangeEvent : PubSubEvent<LedPattern>
{
}
