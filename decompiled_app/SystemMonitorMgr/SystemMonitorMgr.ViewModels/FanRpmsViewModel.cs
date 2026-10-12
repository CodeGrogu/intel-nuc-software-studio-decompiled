using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

public class FanRpmsViewModel : FanFullViewModel
{
	public FanRpmsViewModel(IEventAggregator events)
		: base(events)
	{
	}
}
