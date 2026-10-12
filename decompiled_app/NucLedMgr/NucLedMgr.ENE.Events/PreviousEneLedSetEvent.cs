using System.Collections.Generic;
using IntelSdkClient.EneService;
using Prism.Events;

namespace NucLedMgr.ENE.Events;

internal class PreviousEneLedSetEvent : PubSubEvent<List<LedPattern>>
{
}
