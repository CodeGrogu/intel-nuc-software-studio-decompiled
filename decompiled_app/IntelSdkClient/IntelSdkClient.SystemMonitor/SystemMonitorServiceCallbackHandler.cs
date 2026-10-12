using System;
using IntelSdkClient.SystemMonitorService;
using Prism.Events;

namespace IntelSdkClient.SystemMonitor;

public class SystemMonitorServiceCallbackHandler : ISystemMonitorServiceContractCallback
{
	private IEventAggregator _events;

	public SystemMonitorServiceCallbackHandler(IEventAggregator events)
	{
		_events = events;
	}

	public void SystemMonitorDataCallback(FanSpeed[] FanData, SystemMonitorData SystemData)
	{
		_events.GetEvent<SystemMonitorDataEvent>().Publish(new Tuple<FanSpeed[], SystemMonitorData>(FanData, SystemData));
	}
}
