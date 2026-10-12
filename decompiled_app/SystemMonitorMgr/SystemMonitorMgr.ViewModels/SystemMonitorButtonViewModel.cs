using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace SystemMonitorMgr.ViewModels;

public class SystemMonitorButtonViewModel : BindableBase
{
	private readonly IRegionManager _regionMgr;

	private readonly IEventAggregator _events;

	private DelegateCommand _launchSystemMonitor;

	public DelegateCommand LaunchSystemMonitor
	{
		get
		{
			return _launchSystemMonitor;
		}
		set
		{
			SetProperty(ref _launchSystemMonitor, value, "LaunchSystemMonitor");
		}
	}

	public SystemMonitorButtonViewModel(IRegionManager regionMgr, IEventAggregator events)
	{
		_regionMgr = regionMgr;
		_events = events;
		_launchSystemMonitor = new DelegateCommand(ShowSystemMonitor);
	}

	private void ShowSystemMonitor()
	{
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("DetailView");
		_regionMgr.Regions["DetailContentRegion"].RequestNavigate("SystemMonitorView");
		_events.GetEvent<SysMonViewOpenedEvent>().Publish(payload: true);
	}
}
