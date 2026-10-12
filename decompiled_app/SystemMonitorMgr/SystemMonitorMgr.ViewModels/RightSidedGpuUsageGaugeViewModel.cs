using System.Collections.Generic;
using SystemMonitorMgr.Enums;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Models;
using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

public class RightSidedGpuUsageGaugeViewModel : dGpuUsageGaugeViewModel
{
	private readonly IEventAggregator _events;

	private string _gpuString;

	public string GpuString
	{
		get
		{
			return _gpuString;
		}
		set
		{
			SetProperty(ref _gpuString, value, "GpuString");
		}
	}

	public RightSidedGpuUsageGaugeViewModel(IEventAggregator events)
		: base(events)
	{
		_events = events;
		Gauge = new UsageGaugeDataContext(SystemDataType.dGpu, events.GetEvent<UsageDataEvent>());
		_events.GetEvent<SystemComponentNamesEvent>().Subscribe(GetStringName);
	}

	private void GetStringName(Dictionary<SystemDataType, string> obj)
	{
		GpuString = obj[SystemDataType.dGpu];
		_events.GetEvent<SystemComponentNamesEvent>().Unsubscribe(GetStringName);
	}
}
