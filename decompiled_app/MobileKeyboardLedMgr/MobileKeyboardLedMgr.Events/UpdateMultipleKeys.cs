using System.Collections.Generic;
using MobileKeyboardLedMgr.Models;
using Prism.Events;

namespace MobileKeyboardLedMgr.Events;

internal class UpdateMultipleKeys : PubSubEvent<List<KeyUiData>>
{
}
