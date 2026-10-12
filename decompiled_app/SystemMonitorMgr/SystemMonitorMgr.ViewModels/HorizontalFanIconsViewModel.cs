using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

public class HorizontalFanIconsViewModel : FanFullViewModel
{
	public HorizontalFanIconsViewModel(IEventAggregator events)
		: base(events)
	{
	}
}
