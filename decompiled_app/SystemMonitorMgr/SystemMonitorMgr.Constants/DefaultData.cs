using System.Collections.Generic;
using SystemMonitorMgr.Models;
using IntelSdkClient.SystemMonitorService;

namespace SystemMonitorMgr.Constants;

public static class DefaultData
{
	public static readonly List<FanData> FANS = new List<FanData>
	{
		new FanData(FanType.SystemFan1),
		new FanData(FanType.SystemFan2)
	};
}
