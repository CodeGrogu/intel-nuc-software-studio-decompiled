using SystemMonitorMgr.Enums;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Models;
using Prism.Events;
using Prism.Mvvm;

namespace SystemMonitorMgr.ViewModels;

public class CpuUsageGaugeViewModel : BindableBase
{
	private UsageGaugeDataContext _gauge;

	public UsageGaugeDataContext Gauge
	{
		get
		{
			return _gauge;
		}
		set
		{
			SetProperty(ref _gauge, value, "Gauge");
		}
	}

	public CpuUsageGaugeViewModel(IEventAggregator events)
	{
		Gauge = new UsageGaugeDataContext(SystemDataType.Cpu, events.GetEvent<UsageDataEvent>());
	}
}
