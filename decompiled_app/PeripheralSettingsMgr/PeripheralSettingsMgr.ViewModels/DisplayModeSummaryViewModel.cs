using System.Collections.ObjectModel;
using Infrastructure.Converters;
using Infrastructure.Events;
using PeripheralSettingsMgr.Interfaces;
using PeripheralSettingsMgr.Models.DisplayModes;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace PeripheralSettingsMgr.ViewModels;

public class DisplayModeSummaryViewModel : BindableBase
{
	private readonly IRegionManager _regionMgr;

	private readonly IDisplaySettingsManager _settingsMgr;

	private readonly IEventAggregator _events;

	private DelegateCommand _openDetailView;

	public ObservableCollection<DisplayModeProfile> Modes => _settingsMgr.Profiles;

	public DisplayModeProfile CurrentMode
	{
		get
		{
			return _settingsMgr.CurrentProfile;
		}
		set
		{
			_settingsMgr.CurrentProfile = value;
			RaisePropertyChanged("CurrentMode");
		}
	}

	public DelegateCommand OpenDetailView
	{
		get
		{
			return _openDetailView;
		}
		set
		{
			SetProperty(ref _openDetailView, value, "OpenDetailView");
		}
	}

	public DisplayModeSummaryViewModel(IEventAggregator events, IRegionManager regionMgr, IDisplaySettingsManager settingsMgr)
	{
		_regionMgr = regionMgr;
		_settingsMgr = settingsMgr;
		_events = events;
		_openDetailView = new DelegateCommand(NavToDetailView);
		_events.GetEvent<NavigateToProfilesView>().Subscribe(RefreshUi);
	}

	private void RefreshUi()
	{
		RaisePropertyChanged("Modes");
		RaisePropertyChanged("CurrentMode");
	}

	private void NavToDetailView()
	{
		NavigationParameters navigationParameters = new NavigationParameters { 
		{
			"CardTitle",
			ResourceDictionaryConverter.GetString("DisplayModeSummaryCardTitleString")
		} };
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("DetailView", navigationParameters);
		_regionMgr.Regions["DetailContentRegion"].RequestNavigate("DisplayModeView");
	}
}
