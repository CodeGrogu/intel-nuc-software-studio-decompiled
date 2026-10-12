using Prism.Events;
using UserPresenceMgr.Models;

namespace UserPresenceMgr.Events;

public class UserPresenceUpdateSettingEvent : PubSubEvent<SetOptionData>
{
}
