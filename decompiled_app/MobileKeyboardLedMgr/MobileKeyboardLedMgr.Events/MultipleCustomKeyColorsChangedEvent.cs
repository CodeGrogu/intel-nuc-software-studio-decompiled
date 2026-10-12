using System.Collections.Generic;
using MobileKeyboardLedMgr.Models.Profiles;
using Prism.Events;

namespace MobileKeyboardLedMgr.Events;

internal class MultipleCustomKeyColorsChangedEvent : PubSubEvent<List<KeyColorData>>
{
}
