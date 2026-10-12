using IntelSdkClient.EneService;
using Prism.Events;

namespace NucLedMgr.ENE.Events;

public class CurrentEnePatternsEvent : PubSubEvent<LedStatus>
{
}
