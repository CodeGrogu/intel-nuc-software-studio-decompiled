using NucLedMgr.WMI.Models;
using Prism.Events;

namespace NucLedMgr.WMI.Events;

internal class LedColorBehaviorChangeEvent : PubSubEvent<Led>
{
}
