using NucLedMgr.WMI.Models;
using Prism.Events;

namespace NucLedMgr.WMI.Events;

internal class ProfileChangedEvent : PubSubEvent<LedProfile>
{
}
