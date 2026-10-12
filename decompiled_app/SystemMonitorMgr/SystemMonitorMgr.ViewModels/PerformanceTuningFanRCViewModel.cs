using System.Collections.Generic;
using SystemMonitorMgr.Constants;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Interfaces;
using SystemMonitorMgr.Models;
using IntelSdkClient.SystemMonitorService;
using Prism.Events;
using Prism.Mvvm;

namespace SystemMonitorMgr.ViewModels;

internal class PerformanceTuningFanRCViewModel : BindableBase
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

	public PerformanceTuningFanRCViewModel(IEventAggregator events, ISystemMonitor monitor)
	{
		_monitor = monitor;
		events.GetEvent<FanStatusEvent>().Subscribe(UpdateFanStatus);
		_fans = DefaultData.FANS;
	}

	private void UpdateFanStatus(List<FanData> data)
	{
		Fans = data;
		int num = Fans.FindIndex((FanData fan) => fan.Type == FanType.CPUFan2);
		if (num > 0)
		{
			FanData item = data[num];
			Fans.RemoveAt(num);
			Fans.Insert(1, item);
		}
		RaisePropertyChanged("Fans");
	}
}
