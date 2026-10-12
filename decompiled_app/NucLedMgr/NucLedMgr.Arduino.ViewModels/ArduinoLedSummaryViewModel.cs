using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using Infrastructure.Converters;
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using NucLedMgr.Arduino.Enums;
using NucLedMgr.Arduino.Events;
using NucLedMgr.Arduino.Interfaces;
using NucLedMgr.Arduino.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace NucLedMgr.Arduino.ViewModels;

public class ArduinoLedSummaryViewModel : BindableBase
{
	private readonly IRegionManager _regionMgr;

	private readonly IEventAggregator _events;

	private readonly IArduinoPatternManager _patternMgr;

	private DelegateCommand _openDetailView;

	private DelegateCommand _openErrorMessageBox;

	public NucType NucType { get; set; }

	public ArduinoLedData PowerButton { get; set; }

	public ArduinoLedData FrontPanel { get; set; }

	public ArduinoLedData FrontConnectors { get; set; }

	public ArduinoLedData LeftSide { get; set; }

	public ArduinoLedData RightSide { get; set; }

	public ObservableCollection<LedProfile> Profiles => _patternMgr.Profiles;

	public LedProfile SelectedProfile
	{
		get
		{
			return _patternMgr.CurrentProfile;
		}
		set
		{
			_patternMgr.CurrentProfile = value;
			RaisePropertyChanged("SelectedProfile");
		}
	}

	public string SerialPortError => _patternMgr.SerialPortError;

	public bool On
	{
		get
		{
			return _patternMgr.On;
		}
		set
		{
			_patternMgr.On = value;
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

	public DelegateCommand OpenErrorMessageBox
	{
		get
		{
			return _openErrorMessageBox;
		}
		set
		{
			SetProperty(ref _openErrorMessageBox, value, "OpenErrorMessageBox");
		}
	}

	public bool ShowIntelLogo { get; private set; }

	public BitmapSource AppLogo { get; private set; }

	public ArduinoLedSummaryViewModel(IEventAggregator events, IRegionManager regionMgr, IArduinoPatternManager patternMgr, ICustomResourceManager resourceManager)
	{
		_regionMgr = regionMgr;
		_events = events;
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
		_openDetailView = new DelegateCommand(NavToDetailView);
		_openErrorMessageBox = new DelegateCommand(OpenErrorMessage);
		_events.GetEvent<NavigateToProfilesView>().Subscribe(RefreshUi);
		_events.GetEvent<ArduinoZoneColorPatternChangeEvent>().Subscribe(UpdateUiPattern);
		_events.GetEvent<RefreshArduinoUiEvent>().Subscribe(RefreshUi);
		_patternMgr = patternMgr;
		NucType = _patternMgr.NucType;
		PowerButton = _patternMgr.ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.Error);
		FrontPanel = _patternMgr.ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.Skull);
		FrontConnectors = _patternMgr.ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.BottomFront);
		LeftSide = _patternMgr.ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.BottomLeft);
		RightSide = _patternMgr.ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.BottomRight);
		RaisePropertyChanged("LedsAvailable");
	}

	private void OpenErrorMessage()
	{
		ErrorHandler.PublishError(SerialPortError, ErrorType.SERIALPORT_DRIVER_MISSING, critical: true, msgIsKey: true);
		_events.GetEvent<ApplicationCloseEvent>().Publish(payload: false);
	}

	private void RefreshUi()
	{
		RaisePropertyChanged("PowerButton");
		RaisePropertyChanged("FrontPanel");
		RaisePropertyChanged("FrontConnectors");
		RaisePropertyChanged("LeftSide");
		RaisePropertyChanged("RightSide");
		RaisePropertyChanged("SelectedProfile");
		RaisePropertyChanged("LedsAvailable");
	}

	private void RefreshUi(bool Enabled)
	{
		RaisePropertyChanged("On");
		RaisePropertyChanged("ArduinoZones");
		RaisePropertyChanged("FrontPanel");
		RaisePropertyChanged("FrontConnectors");
		RaisePropertyChanged("LeftSide");
		RaisePropertyChanged("RightSide");
		RaisePropertyChanged("ArduinoZones");
		RaisePropertyChanged("SelectedZone");
		RaisePropertyChanged("SelectedProfile");
		RaisePropertyChanged("LedsAvailable");
	}

	private void UpdateUiPattern(MonsterCoveZone Zone)
	{
		switch (Zone)
		{
		case MonsterCoveZone.Skull:
			RaisePropertyChanged("FrontPanel");
			break;
		case MonsterCoveZone.BottomRight:
			RaisePropertyChanged("RightSide");
			break;
		case MonsterCoveZone.BottomLeft:
			RaisePropertyChanged("LeftSide");
			break;
		case MonsterCoveZone.BottomFront:
			RaisePropertyChanged("FrontConnectors");
			break;
		}
	}

	private void NavToDetailView()
	{
		NavigationParameters navigationParameters = new NavigationParameters { 
		{
			"CardTitle",
			ResourceDictionaryConverter.GetString("ChassisLedCardTitleString")
		} };
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("DetailView", navigationParameters);
		_regionMgr.Regions["DetailContentRegion"].RequestNavigate("ArduinoLedView");
	}
}
