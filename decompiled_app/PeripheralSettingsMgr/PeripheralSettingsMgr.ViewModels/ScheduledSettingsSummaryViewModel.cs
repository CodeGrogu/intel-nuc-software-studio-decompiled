using System.Collections.ObjectModel;
using Infrastructure.Converters;
using PeripheralSettingsMgr.Interfaces;
using PeripheralSettingsMgr.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace PeripheralSettingsMgr.ViewModels;

internal class ScheduledSettingsSummaryViewModel : BindableBase
{
	private readonly IRegionManager _regionMgr;

	private readonly IEventAggregator _events;

	private readonly IScheduledSettingsManager _settings;

	private DelegateCommand _openDetailView;

	public ObservableCollection<ScheduledSettingsData> ScheduledSettings { get; private set; }

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

	public ScheduledSettingsSummaryViewModel(IEventAggregator events, IRegionManager regionMgr, IScheduledSettingsManager settingsMgr)
	{
		_regionMgr = regionMgr;
		_events = events;
		_settings = settingsMgr;
		ScheduledSettings = _settings.ScheduledSettings;
		_openDetailView = new DelegateCommand(NavToDetailView);
	}

	private void NavToDetailView()
	{
		NavigationParameters navigationParameters = new NavigationParameters { 
		{
			"CardTitle",
			ResourceDictionaryConverter.GetString("NucScheduledSleepTitleString")
		} };
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("DetailView", navigationParameters);
		_regionMgr.Regions["DetailContentRegion"].RequestNavigate("ScheduledSettingsMainView");
	}
}
