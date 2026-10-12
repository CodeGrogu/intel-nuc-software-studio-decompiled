using System.Collections.Generic;
using Prism.Events;
using UniwillSdkClient.UniwillService;

namespace UniwillSdkClient.Events;

public class OsdNotificationEvent : PubSubEvent<KeyValuePair<OsdType, int>>
{
}
