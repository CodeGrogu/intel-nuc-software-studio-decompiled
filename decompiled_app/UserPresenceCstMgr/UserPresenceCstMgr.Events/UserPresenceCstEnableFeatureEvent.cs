using System;
using Intel.Cst.Client;
using Prism.Events;

namespace UserPresenceCstMgr.Events;

public class UserPresenceCstEnableFeatureEvent : PubSubEvent<Tuple<FeatureType, bool>>
{
}
