using CustomizationMgr.Models;
using Prism.Events;

namespace CustomizationMgr.Events;

public class CustomSettingsEvent : PubSubEvent<CustomSetting>
{
}
