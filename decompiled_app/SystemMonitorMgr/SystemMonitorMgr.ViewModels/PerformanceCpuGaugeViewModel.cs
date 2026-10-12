using SystemMonitorMgr.Enums;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Interfaces;
using SystemMonitorMgr.Models;
using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

internal class PerformanceCpuGaugeViewModel : CpuUsageGaugeViewModel
{
	private readonly IEventAggregator _events;

	private ISystemMonitor _systemMonitor;

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

	public PerformanceCpuGaugeViewModel(IEventAggregator events, ISystemMonitor monitor)
		: base(events)
	{
		_events = events;
		_systemMonitor = monitor;
		Gauge = new UsageGaugeDataContext(SystemDataType.Cpu, events.GetEvent<UsageDataEvent>());
		CpuString = monitor.CpuName;
	}
}
