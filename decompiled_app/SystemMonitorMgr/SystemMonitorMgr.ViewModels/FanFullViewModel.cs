using System.Collections.Generic;
using SystemMonitorMgr.Constants;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Models;
using Prism.Events;
using Prism.Mvvm;

namespace SystemMonitorMgr.ViewModels;

public class FanFullViewModel : BindableBase
{
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

	public FanFullViewModel(IEventAggregator events)
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
