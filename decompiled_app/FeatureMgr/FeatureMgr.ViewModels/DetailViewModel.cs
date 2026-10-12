using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace FeatureMgr.ViewModels;

public class DetailViewModel : BindableBase, INavigationAware
{
	private readonly IRegionManager _regionMgr;

	private readonly IEventAggregator _events;

	private DelegateCommand _navHome;

	private string _cardTitle;

	public DelegateCommand NavHome
	{
		get
		{
			return _navHome;
		}
		set
		{
			SetProperty(ref _navHome, value, "NavHome");
		}
	}

	public string CardTitle
	{
		get
		{
			return _cardTitle;
		}
		set
		{
			SetProperty(ref _cardTitle, value, "CardTitle");
		}
	}

	public DetailViewModel(IRegionManager regionMgr, IEventAggregator events)
	{
		_regionMgr = regionMgr;
		_events = events;
		_navHome = new DelegateCommand(NavigateHome);
	}

	private void NavigateHome()
	{
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("ProfilesView");
		_events.GetEvent<NavigateToHomeEvent>().Publish();
	}

	public void OnNavigatedTo(NavigationContext navigationContext)
	{
		if (navigationContext.Parameters.Count > 0)
		{
			CardTitle = navigationContext.Parameters["CardTitle"] as string;
		}
		else
		{
			CardTitle = "";
		}
	}

	public bool IsNavigationTarget(NavigationContext navigationContext)
	{
		return true;
	}

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
	}
}
