using System.Collections.Generic;
using SystemMonitorMgr.Constants;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Models;
using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

internal class PerformanceTuningFanViewModel : FanFullViewModel
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

	public PerformanceTuningFanViewModel(IEventAggregator events)
		: base(events)
	{
		events.GetEvent<FanStatusEvent>().Subscribe(UpdateFanStatus);
		_fans = DefaultData.FANS;
	}

	private void UpdateFanStatus(List<FanData> data)
	{
		Fans = data;
		RaisePropertyChanged("Fans");
	}
}
