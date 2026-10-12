using System.Collections.ObjectModel;
using System.Linq;
using CustomizationMgr.Interfaces;
using Infrastructure.Converters;
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using NucLedMgr.ENE.Enums;
using NucLedMgr.ENE.Events;
using NucLedMgr.ENE.Interfaces;
using NucLedMgr.ENE.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace NucLedMgr.ENE.ViewModels;

public class EneLedSummaryViewModel : BindableBase
{
	private readonly IRegionManager _regionMgr;

	private readonly IEventAggregator _events;

	private readonly IEnePatternManager _patternMgr;

	private DelegateCommand _openDetailView;

	private DelegateCommand _openErrorMessageBox;

	private DelegateCommand _reset;

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

	public ObservableCollection<EneLedData> EneZones => _patternMgr.EneZones;

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

	public DelegateCommand Reset
	{
		get
		{
			return _reset;
		}
		set
		{
			SetProperty(ref _reset, value, "Reset");
		}
	}

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

	public EneLedData LogoHeader { get; set; }

	public EneLedData LeftHeader { get; set; }

	public EneLedData RightHeader { get; set; }

	public EneLedData FrontHeader { get; set; }

	public EneLedSummaryViewModel(IEventAggregator events, IRegionManager regionMgr, IEnePatternManager patternMgr, ICustomResourceManager resourceManager)
	{
		_regionMgr = regionMgr;
		_events = events;
		_patternMgr = patternMgr;
		if (string.IsNullOrWhiteSpace(SerialPortError))
		{
			LogoHeader = _patternMgr.EneZones.FirstOrDefault((EneLedData x) => x.Index == EneLedZone.LogoHeader);
			LeftHeader = _patternMgr.EneZones.FirstOrDefault((EneLedData x) => x.Index == EneLedZone.LeftHeader);
			RightHeader = _patternMgr.EneZones.FirstOrDefault((EneLedData x) => x.Index == EneLedZone.RightHeader);
			FrontHeader = _patternMgr.EneZones.FirstOrDefault((EneLedData x) => x.Index == EneLedZone.FrontHeader);
		}
		_openErrorMessageBox = new DelegateCommand(OpenErrorMessage);
		_reset = new DelegateCommand(_patternMgr.Reset);
		_openDetailView = new DelegateCommand(NavToDetailView);
		_events.GetEvent<NavigateToProfilesView>().Subscribe(RefreshUi);
		_events.GetEvent<RefreshEneUiEvent>().Subscribe(RefreshUi);
	}

	private void RefreshUi()
	{
		RaisePropertyChanged("SelectedProfile");
	}

	private void NavToDetailView()
	{
		NavigationParameters navigationParameters = new NavigationParameters { 
		{
			"CardTitle",
			ResourceDictionaryConverter.GetString("ChassisLedCardTitleString")
		} };
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("DetailView", navigationParameters);
		_regionMgr.Regions["DetailContentRegion"].RequestNavigate("EneLedView");
	}

	private void OpenErrorMessage()
	{
		ErrorHandler.PublishError(SerialPortError, ErrorType.SERIALPORT_DRIVER_MISSING, critical: true, msgIsKey: true);
		_events.GetEvent<ApplicationCloseEvent>().Publish(payload: false);
	}

	private void RefreshUi(bool Enabled)
	{
		RaisePropertyChanged("On");
		RaisePropertyChanged("EneZones");
		RaisePropertyChanged("LogoHeader");
		RaisePropertyChanged("FrontHeader");
		RaisePropertyChanged("LeftHeader");
		RaisePropertyChanged("RightHeader");
		RaisePropertyChanged("EneZones");
		RaisePropertyChanged("SelectedProfile");
		RaisePropertyChanged("LedsAvailable");
	}
}
