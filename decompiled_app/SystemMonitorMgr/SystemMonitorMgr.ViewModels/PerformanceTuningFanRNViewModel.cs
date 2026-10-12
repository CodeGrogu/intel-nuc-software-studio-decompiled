using System.Collections.Generic;
using SystemMonitorMgr.Constants;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Models;
using IntelSdkClient.SystemMonitorService;
using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

public class PerformanceTuningFanRNViewModel : FanFullViewModel
{
	private List<FanData> _fans;

	public new List<FanData> Fans
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

	public PerformanceTuningFanRNViewModel(IEventAggregator events)
		: base(events)
	{
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
