using System.Collections.Generic;
using IntelSdkClient.ArduinoService;
using Prism.Events;

namespace NucLedMgr.Arduino.Events;

internal class PreviousLedSetEvent : PubSubEvent<List<LedPattern>>
{
}
