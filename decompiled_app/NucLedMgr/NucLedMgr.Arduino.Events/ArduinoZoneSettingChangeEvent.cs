using System;
using IntelSdkClient.ArduinoService;
using Prism.Events;

namespace NucLedMgr.Arduino.Events;

public class ArduinoZoneSettingChangeEvent : PubSubEvent<Tuple<LedPattern, bool>>
{
}
