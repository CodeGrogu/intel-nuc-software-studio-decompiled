using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using Infrastructure.Converters;
using Infrastructure.Events;
using NucLedMgr.Interfaces;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Events;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace NucLedMgr.WMI.ViewModels;

public class WmiLedSummaryViewModel : BindableBase
{
	private readonly IRegionManager _regionMgr;

	private readonly IEventAggregator _events;

	private readonly ISettingsManager _settingsMgr;

	private readonly IColorManager _colorMgr;

	private bool _processComplete;

	private LedProfile _selectedProfile;

	private DelegateCommand _openDetailView;

	public Led PowerButton
	{
		get
		{
			return _settingsMgr.PowerButton;
		}
		set
		{
		}
	}

	public Led Front1
	{
		get
		{
			return _settingsMgr.Front1;
		}
		set
		{
		}
	}

	public Led Front2
	{
		get
		{
			return _settingsMgr.Front2;
		}
		set
		{
		}
	}

	public Led Front3
	{
		get
		{
			return _settingsMgr.Front3;
		}
		set
		{
		}
	}

	public Led Skull
	{
		get
		{
			return _settingsMgr.Skull;
		}
		set
		{
		}
	}

	public Led Eyes
	{
		get
		{
			return _settingsMgr.Eyes;
		}
		set
		{
		}
	}

	public Led Hdd
	{
		get
		{
			return _settingsMgr.Hdd;
		}
		set
		{
		}
	}

	public bool On
	{
		get
		{
			return !_settingsMgr.Disabled;
		}
		set
		{
			ProcessComplete = false;
			Task.Run(() =>
			{
				_settingsMgr.Disabled = !value;
			}).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public bool ProcessComplete
	{
		get
		{
			return _processComplete;
		}
		set
		{
			SetProperty(ref _processComplete, value, "ProcessComplete");
		}
	}

	public ObservableCollection<LedProfile> Profiles => _settingsMgr.Profiles;

	public LedProfile SelectedProfile
	{
		get
		{
			return _selectedProfile;
		}
		set
		{
			if (value != null)
			{
				SetProperty(ref _selectedProfile, value, "SelectedProfile");
				if (IsSettingsSet)
				{
					_settingsMgr.CurrentProfile = value;
				}
				RaisePropertyChanged("SelectedProfile");
			}
		}
	}

	public bool LedsAvailable { get; set; }

	public NucType NucType { get; set; }

	private bool IsSettingsSet { get; set; }

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

	public bool ShowIntelLogo { get; private set; }

	public BitmapSource AppLogo { get; private set; }

	public WmiLedSummaryViewModel(IEventAggregator events, IRegionManager regionMgr, ISettingsManager settingsMgr, IColorManager colorMgr, ICustomResourceManager resourceManager)
	{
		_regionMgr = regionMgr;
		_events = events;
		_settingsMgr = settingsMgr;
		_colorMgr = colorMgr;
		LedsAvailable = _settingsMgr.LedsLoaded;
		NucType = _settingsMgr.NucType;
		_selectedProfile = _settingsMgr.CurrentProfile;
		IsSettingsSet = true;
		KeyValuePair<AppLogoType, BitmapSource> keyValuePair = resourceManager.AppLogo();
		if (keyValuePair.Key == AppLogoType.Default)
		{
			ShowIntelLogo = true;
		}
		else
		{
			ShowIntelLogo = false;
			if (keyValuePair.Key == AppLogoType.Custom)
			{
				AppLogo = keyValuePair.Value;
			}
		}
		_events.GetEvent<LedErrorEvent>().Subscribe(LedError, ThreadOption.UIThread);
		if (NucType != NucType.Error)
		{
			_events.GetEvent<LedsLoadedEvent>().Subscribe(ChassisVisable, ThreadOption.UIThread);
			_openDetailView = new DelegateCommand(NavToDetailView);
			_events.GetEvent<NavigateToProfilesView>().Subscribe(RefreshUI);
			_events.GetEvent<LedColorBehaviorChangeEvent>().Subscribe(UpdateUiPattern);
			_events.GetEvent<UpdateUIEvent>().Subscribe(RefreshUI);
			_events.GetEvent<ProfileChangedEvent>().Subscribe(ChangeProfile);
			_events.GetEvent<ProcessingCompleteEvent>().Subscribe(EnableToggle);
			_processComplete = true;
		}
		else
		{
			LedsAvailable = true;
		}
	}

	private void EnableToggle(bool completed)
	{
		ProcessComplete = completed;
	}

	private void ChangeProfile(LedProfile newProf)
	{
		IsSettingsSet = false;
		SelectedProfile = newProf;
		RaisePropertyChanged("SelectedProfile");
		IsSettingsSet = true;
	}

	private void LedError(NucType nucType)
	{
		NucType = nucType;
		RaisePropertyChanged("NucType");
		OpenDetailView = null;
		RaisePropertyChanged("OpenDetailView");
	}

	private void ChassisVisable(bool b)
	{
		LedsAvailable = b;
		RaisePropertyChanged("LedsAvailable");
		RefreshUI();
	}

	private void NavToDetailView()
	{
		NavigationParameters navigationParameters = new NavigationParameters { 
		{
			"CardTitle",
			ResourceDictionaryConverter.GetString("ChassisLedCardTitleString")
		} };
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("DetailView", navigationParameters);
		_regionMgr.Regions["DetailContentRegion"].RequestNavigate("NucLedView");
	}

	private void RefreshUI()
	{
		RaisePropertyChanged("PowerButton");
		RaisePropertyChanged("Skull");
		RaisePropertyChanged("Eyes");
		RaisePropertyChanged("Front1");
		RaisePropertyChanged("Front2");
		RaisePropertyChanged("Front3");
		RaisePropertyChanged("HDD");
		RaisePropertyChanged("LedsAvailable");
		RaisePropertyChanged("SelectedLed");
		RaisePropertyChanged("On");
		RaisePropertyChanged("SelectedProfile");
		RaisePropertyChanged("Profiles");
	}

	private void UpdateUiPattern(Led led)
	{
		switch (led.LedType)
		{
		case LedType.PowerButton:
			RaisePropertyChanged("PowerButton");
			break;
		case LedType.Front1:
			RaisePropertyChanged("Front1");
			break;
		case LedType.Front2:
			RaisePropertyChanged("Front2");
			break;
		case LedType.Front3:
			RaisePropertyChanged("Front3");
			break;
		case LedType.Skull:
			RaisePropertyChanged("Skull");
			break;
		case LedType.Eyes:
			RaisePropertyChanged("Eyes");
			break;
		case LedType.Hdd:
			RaisePropertyChanged("HDD");
			break;
		}
	}
}
