using MobileKeyboardLedMgr.Models.Profiles;
using Prism.Events;

namespace MobileKeyboardLedMgr.Events;

internal class ToggleColorEvent : PubSubEvent<ColorPickerColors>
{
}
