using System.Collections.Generic;
using SystemMonitorMgr.Enums;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Models;
using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

public class RightSideCpuUsageGaugeViewModel : CpuUsageGaugeViewModel
{
	private readonly IEventAggregator _events;

	private string _cpuString;

	public string CpuString
	{
		get
		{
			return _cpuString;
		}
		set
		{
			SetProperty(ref _cpuString, value, "CpuString");
		}
	}

	public RightSideCpuUsageGaugeViewModel(IEventAggregator events)
		: base(events)
	{
		_events = events;
		Gauge = new UsageGaugeDataContext(SystemDataType.Cpu, events.GetEvent<UsageDataEvent>());
		_events.GetEvent<SystemComponentNamesEvent>().Subscribe(GetStringName);
	}

	private void GetStringName(Dictionary<SystemDataType, string> obj)
	{
		CpuString = obj[SystemDataType.Cpu];
		_events.GetEvent<SystemComponentNamesEvent>().Unsubscribe(GetStringName);
	}
}
