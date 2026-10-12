using NucLedMgr.Arduino.Enums;
using Prism.Events;

namespace NucLedMgr.Arduino.Events;

public class SetAllArduinoZonesToPatternEvent : PubSubEvent<ArduinoPatternType>
{
}
