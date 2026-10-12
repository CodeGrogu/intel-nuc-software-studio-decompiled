using System.Collections.Generic;
using SystemMonitorMgr.Constants;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Interfaces;
using SystemMonitorMgr.Models;
using Prism.Events;
using Prism.Mvvm;

namespace SystemMonitorMgr.ViewModels;

public class NucPerformanceSummaryFanViewModel : BindableBase
{
	private readonly ISystemMonitor _monitor;

	private List<FanData> _fans;

	public List<FanData> Fans
	{
		get
		{
			return _fans;
		}
		set
		{
			SetProperty(ref _fans, value, "Fans");
		}
	}

	public bool IsUniwill => _monitor.GetType() == typeof(UniwillSystemMonitor);

	public NucPerformanceSummaryFanViewModel(IEventAggregator events, ISystemMonitor monitor)
	{
		_monitor = monitor;
		events.GetEvent<FanStatusEvent>().Subscribe(UpdateFanStatus);
		_fans = DefaultData.FANS;
	}

	private void UpdateFanStatus(List<FanData> data)
	{
		Fans = data;
		RaisePropertyChanged("Fans");
	}
}
