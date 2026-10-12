using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

public class FanIconsViewModel : FanFullViewModel
{
	public FanIconsViewModel(IEventAggregator events)
		: base(events)
	{
	}
}
