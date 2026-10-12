using MobileKeyboardLedMgr.Models.Profiles;
using Prism.Events;

namespace MobileKeyboardLedMgr.Events;

public class CustomKeyColorChangeEvent : PubSubEvent<KeyColorData>
{
}
