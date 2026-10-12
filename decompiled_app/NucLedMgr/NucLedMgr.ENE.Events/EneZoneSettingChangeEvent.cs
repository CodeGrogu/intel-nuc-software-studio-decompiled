using System;
using IntelSdkClient.EneService;
using Prism.Events;

namespace NucLedMgr.ENE.Events;

public class EneZoneSettingChangeEvent : PubSubEvent<Tuple<LedPattern, bool>>
{
}
