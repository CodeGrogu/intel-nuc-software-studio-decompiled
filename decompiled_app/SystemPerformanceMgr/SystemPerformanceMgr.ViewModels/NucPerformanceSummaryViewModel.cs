using SystemPerformanceMgr.Interfaces;
using SystemPerformanceMgr.Models.Events;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using FeatureMgr.Enums;
using Infrastructure.Converters;
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using IntelSdkClient.PerformanceTuningService;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace SystemPerformanceMgr.ViewModels;

public class NucPerformanceSummaryViewModel : BaseDetailViewModel
{
	private readonly IRegionManager _regionManager;

	private readonly IEventAggregator _events;

	private readonly INucPerformanceModeManager _performanceModeManager;

	private const string PERFORMANCE_CARD_TITLE_KEY = "PerformanceCardTitleString";

	private const string PERFORMANCE_VIEW = "PerformanceView";

	private const string CPU_MODE_PROPERTY_NAME = "CpuMode";

	private PowerProfile _cpuMode;

	private const string FAN_MODE_PROPERTY_NAME = "FanMode";

	private string _fanMode;

	private bool _isSerpentlVisible;

	private DelegateCommand _openDetailView;

	private DelegateCommand _openErrorMessageBox;

	public string CpuUsageRegion => "CpuUsageRegion";

	public string FullFanRegion => "NucPerformanceSummaryFanView";

	public string RCFullFanRegion => "PerformanceTuningFanRCView";

	public string dGpuUsageRegion => "DgpuUsageRegion";

	public string PerformanceFanRegionPH => "PerformanceTuningFanPHView";

	public bool Supported { get; }

	public PowerProfile CpuMode
	{
		get
		{
			return _cpuMode;
		}
		set
		{
			SetProperty(ref _cpuMode, value, "CpuMode");
		}
	}

	public string FanMode
	{
		get
		{
			return _fanMode;
		}
		set
		{
			SetProperty(ref _fanMode, value, "FanMode");
		}
	}

	public bool IsSerpentlVisible
	{
		get
		{
			return _isSerpentlVisible;
		}
		set
		{
			SetProperty(ref _isSerpentlVisible, value, "IsSerpentlVisible");
		}
	}

	public string PerformanceDriverError { get; }

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

	public NucPerformanceSummaryViewModel(IEventAggregator events, IRegionManager regionManager, INucPerformanceModeManager performanceModeManager, ICustomResourceManager resourceManager)
		: base(resourceManager, IconType.Performance)
	{
		_regionManager = regionManager;
		_events = events;
		_performanceModeManager = performanceModeManager;
		PerformanceDriverError = _performanceModeManager.PerformanceDriverError;
		_openErrorMessageBox = new DelegateCommand(OpenErrorMessage);
		if (_performanceModeManager.NucType == NucType.Serpent)
		{
			_isSerpentlVisible = true;
		}
		else
		{
			_isSerpentlVisible = false;
		}
		Supported = _performanceModeManager.PowerModesSupported;
		if (Supported)
		{
			_openDetailView = new DelegateCommand(NavToDetailView);
			events.GetEvent<PowerModeChangedEvent>().Subscribe(UpdatePowerMode);
			events.GetEvent<FanModeChangedEvent>().Subscribe(UpdateFanMode);
			_cpuMode = _performanceModeManager.CurrentPowerMode;
			_fanMode = _performanceModeManager.CurrentFanMode;
		}
	}

	private void UpdatePowerMode()
	{
		CpuMode = _performanceModeManager.CurrentPowerMode;
		RaisePropertyChanged("CpuMode");
	}

	private void UpdateFanMode()
	{
		FanMode = _performanceModeManager.CurrentFanMode;
		RaisePropertyChanged("FanMode");
	}

	private void NavToDetailView()
	{
		NavigationParameters navigationParameters = new NavigationParameters { 
		{
			"CardTitle",
			ResourceDictionaryConverter.GetString("PerformanceCardTitleString")
		} };
		_regionManager.Regions["MainContentRegion"].RequestNavigate("DetailView", navigationParameters);
		_regionManager.Regions["DetailContentRegion"].RequestNavigate("PerformanceView");
	}

	private void OpenErrorMessage()
	{
		ErrorHandler.PublishError(PerformanceDriverError, ErrorType.SERVICE_OUTDATED, critical: true, msgIsKey: true);
		_events.GetEvent<ApplicationCloseEvent>().Publish(payload: false);
	}
}
