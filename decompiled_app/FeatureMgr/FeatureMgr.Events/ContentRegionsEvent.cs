using System;
using System.Collections.Generic;
using Prism.Events;

namespace FeatureMgr.Events;

public class ContentRegionsEvent : PubSubEvent<List<Tuple<int, List<string>>>>
{
}
