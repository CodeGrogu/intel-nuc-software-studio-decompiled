using System;
using ContextSensingClient;
using Prism.Events;

namespace UserPresenceMgr.Events;

public class UserPresenceEnableFeatureEvent : PubSubEvent<Tuple<FeatureType, bool>>
{
}
