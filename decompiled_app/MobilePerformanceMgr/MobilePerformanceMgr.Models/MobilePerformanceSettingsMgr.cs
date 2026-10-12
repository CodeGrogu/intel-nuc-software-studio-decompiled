using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Infrastructure.Converters;
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using Infrastructure.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;
using UniwillSdkClient.Events;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr.Models;

internal class MobilePerformanceSettingsMgr : NavBaseSummaryViewModel, IMobilePerformanceSettingsMgr
{
	private UniwillServiceClient _client;

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private bool osdEnabled;

	private bool _errorMessage = true;

	private bool _defaultPassiveCoolingMode;

	private bool _passiveCoolingMode;

	private bool _defaultBenchmarkMode;

	private bool _benchmarkMode;

	private bool _whisperModeSupported;

	private bool _whisperMode;

	private int _whisperModeSetting = 3;

	private int _whisperModeFps = 60;

	private readonly List<int> _profileOptions = new List<int> { 1, 2, 3 };

	public readonly List<int> _performanceLimitedFanOptions = new List<int> { 1, 2 };

	private PerformanceProfile _defaultSelectedProfile;

	private PerformanceProfile _selectedProfile;

	private ObservableCollection<PerformanceProfile> _defaultCurrentProfiles;

	private ObservableCollection<PerformanceProfile> _currentProfiles;

	private DelegateCommand _restoreDefaultCommand;

	private DelegateCommand<PerformanceProfile> _selectedProfileChanged;

	private DelegateCommand<PerformanceProfile> _updateProfiles;

	public string CpuUsageRegion => "CpuUsageRegion";

	public string dGpuUsageRegion => "DgpuUsageRegion";

	public string DetailedCpuUsageRegion => "RightCpuUsageRegion";

	public string DetaileddGpuUsageRegion => "RightDgpuUsageRegion";

	public string FullFanRegion => "FullFanRegion";

	public string IconFanRegion => "IconFanRegion";

	public string RpmFanRegion => "RpmFanRegion";

	public string PerfTuningFanRegion => "PerformanceTuningFanRegion";

	public string HorizontalFanRegion => "HorizontalFanRegion";

	public bool ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			SetProperty(ref _errorMessage, value, "ErrorMessage");
			RefreshUI();
		}
	}

	public bool PassiveCoolingMode
	{
		get
		{
			return _passiveCoolingMode;
		}
		set
		{
			if (_passiveCoolingMode != value)
			{
				SetProperty(ref _passiveCoolingMode, value, "PassiveCoolingMode");
				_client.DisablePassiveCoolingMode(!value);
			}
		}
	}

	public bool BenchmarkMode
	{
		get
		{
			return _benchmarkMode;
		}
		set
		{
			if (BenchmarkMode != value)
			{
				SetProperty(ref _benchmarkMode, value, "BenchmarkMode");
				_client.ApplyBenchmarkMode(value);
				if (_selectedProfile != null && !value)
				{
					_client.DisablePassiveCoolingMode(!_passiveCoolingMode);
				}
			}
		}
	}

	public bool WhisperModeSupported
	{
		get
		{
			return _whisperModeSupported;
		}
		set
		{
			SetProperty(ref _whisperModeSupported, value, "WhisperModeSupported");
		}
	}

	public bool WhisperMode
	{
		get
		{
			return _whisperMode;
		}
		set
		{
			SetProperty(ref _whisperMode, value, "WhisperMode");
			if (value)
			{
				_client.SetWhisperModeFps(WhisperModeFps);
				_client.SetWhisperModeType((WhisperModeType)WhisperModeSetting);
			}
			else
			{
				_client.SetWhisperModeType(WhisperModeType.Off);
			}
		}
	}

	public int WhisperModeSetting
	{
		get
		{
			return _whisperModeSetting;
		}
		set
		{
			if (WhisperModeSetting != value && value >= 1 && value <= 3)
			{
				SetProperty(ref _whisperModeSetting, value, "WhisperModeSetting");
			}
		}
	}

	public int WhisperModeFps
	{
		get
		{
			return _whisperModeFps;
		}
		set
		{
			if (WhisperModeFps != value && value >= 30 && value <= 60)
			{
				SetProperty(ref _whisperModeFps, value, "WhisperModeFps");
			}
		}
	}

	public List<int> ProfileOptions => _profileOptions;

	public List<int> PerformanceLimitedFanOptions => _performanceLimitedFanOptions;

	public PerformanceProfile SelectedProfile
	{
		get
		{
			return _selectedProfile;
		}
		set
		{
			if (SelectedProfile != value)
			{
				SetProperty(ref _selectedProfile, value, "SelectedProfile");
				_client.UpdateProfile(_selectedProfile.Index, _selectedProfile.Ac, _selectedProfile.Dc);
				_client.ApplyProfile(_selectedProfile.Index);
				if (osdEnabled)
				{
					_events.GetEvent<OsdNotificationEvent>().Publish(new KeyValuePair<OsdType, int>(OsdType.PerformanceMode, (int)_selectedProfile.Index));
				}
			}
		}
	}

	public ObservableCollection<PerformanceProfile> CurrentProfiles
	{
		get
		{
			return _currentProfiles;
		}
		set
		{
			SetProperty(ref _currentProfiles, value, "CurrentProfiles");
		}
	}

	public DelegateCommand RestoreDefaultCommand
	{
		get
		{
			return _restoreDefaultCommand;
		}
		set
		{
			SetProperty(ref _restoreDefaultCommand, value, "RestoreDefaultCommand");
		}
	}

	public DelegateCommand<PerformanceProfile> SelectedProfileChanged
	{
		get
		{
			return _selectedProfileChanged;
		}
		set
		{
			SetProperty(ref _selectedProfileChanged, value, "SelectedProfileChanged");
		}
	}

	public DelegateCommand<PerformanceProfile> UpdateProfiles
	{
		get
		{
			return _updateProfiles;
		}
		set
		{
			SetProperty(ref _updateProfiles, value, "UpdateProfiles");
		}
	}

	public DelegateCommand ApplyWhisperModeType { get; }

	public DelegateCommand ApplyWhisperModeFps { get; }

	public DelegateCommand WhisperModeErrorAction { get; }

	public MobilePerformanceSettingsMgr(IEventAggregator events, IRegionManager regionMgr, ILoggerFacade logger, ICustomResourceManager resourceManager, IUniwillClient client)
		: base(regionMgr, resourceManager, MobilePerformanceTuningConstants.PERFORMANCE_CARD_TITLE_STRING, "MobilePerformanceView", IconType.Performance)
	{
		MobilePerformanceSettingsMgr mobilePerformanceSettingsMgr = this;
		ErrorMessage = false;
		_events = events;
		_logger = logger;
		_restoreDefaultCommand = new DelegateCommand(RestoreDefaults);
		_selectedProfileChanged = new DelegateCommand<PerformanceProfile>(SwitchProfiles);
		_updateProfiles = new DelegateCommand<PerformanceProfile>(UpdateProfile);
		ApplyWhisperModeType = new DelegateCommand(ApplyWhisperModeTypeToSdk);
		ApplyWhisperModeFps = new DelegateCommand(ApplyWhisperModeFpsToSdk);
		WhisperModeErrorAction = new DelegateCommand(WhisperModeErrorDisplay);
		_events.GetEvent<OsdEnabledEvent>().Subscribe(ExternalOsdEnabled);
		_events.GetEvent<BenchmarkModeEvent>().Subscribe(BenchmarkModeEnabled);
		_events.GetEvent<WhisperModeTypeEvent>().Subscribe(OnWhisperModeTypeEvent);
		_events.GetEvent<WhisperModeFpsEvent>().Subscribe(OnWhisperModeFpsEvent);
		PublishTrayAppMenuItems();
		Task.Run(() =>
		{
			try
			{
				mobilePerformanceSettingsMgr._client = client.Client;
				if (mobilePerformanceSettingsMgr._client == null)
				{
					throw new ArgumentNullException("Unable to connect to SDK");
				}
				mobilePerformanceSettingsMgr._currentProfiles = new ObservableCollection<PerformanceProfile>();
				mobilePerformanceSettingsMgr._defaultCurrentProfiles = new ObservableCollection<PerformanceProfile>();
				mobilePerformanceSettingsMgr.SubscribeToCallbackEvents();
				if (mobilePerformanceSettingsMgr.IsWhisperModeSupported())
				{
					WhisperModeType whisperModeType = mobilePerformanceSettingsMgr._client.GetWhisperModeType();
					if (whisperModeType != WhisperModeType.Off)
					{
						mobilePerformanceSettingsMgr._whisperMode = true;
						mobilePerformanceSettingsMgr.RaisePropertyChanged("WhisperMode");
						mobilePerformanceSettingsMgr.WhisperModeSetting = (int)whisperModeType;
						mobilePerformanceSettingsMgr.WhisperModeFps = mobilePerformanceSettingsMgr._client.GetWhisperModeFps();
					}
				}
				if (resourceManager.CustomAppSettings?.Settings?.Find((object x) => x.GetType().Equals(typeof(MobilePerformanceTuningCustomSettings))) is MobilePerformanceTuningCustomSettings customSettings)
				{
					mobilePerformanceSettingsMgr.LoadCustomDefaults(customSettings);
				}
				else
				{
					mobilePerformanceSettingsMgr.LoadClientDefaults();
				}
				if (resourceManager.IsFirstLaunch())
				{
					mobilePerformanceSettingsMgr.RestoreDefaults();
				}
				else
				{
					mobilePerformanceSettingsMgr.GetPerformanceState();
				}
				mobilePerformanceSettingsMgr.RefreshUI();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				mobilePerformanceSettingsMgr.ErrorMessage = true;
				mobilePerformanceSettingsMgr.OpenDetailView = null;
				logger.Log(NssDebugLogger.ExceptionMessageFormatter(ex), Category.Exception, Priority.High);
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void BenchmarkModeEnabled(bool enabled)
	{
		BenchmarkMode = enabled;
	}

	private void OnWhisperModeTypeEvent(WhisperModeType whisperModeType)
	{
		if (whisperModeType == WhisperModeType.Off)
		{
			_whisperMode = false;
			RaisePropertyChanged("WhisperMode");
		}
		else
		{
			_whisperMode = true;
			RaisePropertyChanged("WhisperMode");
			WhisperModeSetting = (int)whisperModeType;
		}
	}

	private void OnWhisperModeFpsEvent(int fps)
	{
		WhisperModeFps = fps;
	}

	private void PublishTrayAppMenuItems()
	{
		Dictionary<string, EventHandler> payload = new Dictionary<string, EventHandler>
		{
			{
				ResourceDictionaryConverter.GetString("SwitchToPerformanceString"),
				(object s, EventArgs e) =>
				{
					SwitchProfiles(ProfileIndex.Performance);
				}
			},
			{
				ResourceDictionaryConverter.GetString("SwitchToBalancedString"),
				(object s, EventArgs e) =>
				{
					SwitchProfiles(ProfileIndex.Balanced);
				}
			},
			{
				ResourceDictionaryConverter.GetString("SwitchToBatterySaverString"),
				(object s, EventArgs e) =>
				{
					SwitchProfiles(ProfileIndex.BatterySaver);
				}
			}
		};
		_events.GetEvent<AddTrayAppMenuItemsEvent>().Publish(payload);
	}

	private void ExternalOsdEnabled(bool enabled)
	{
		osdEnabled = enabled;
	}

	private void SubscribeToCallbackEvents()
	{
		_events.GetEvent<CurrentPerformanceProfileEvent>().Subscribe(CurrentProfileCallback);
		_events.GetEvent<PassiveCoolingEvent>().Subscribe(PassiveCoolingCallback);
		_events.GetEvent<ProfileSettingEvent>().Subscribe(ProfileSettingCallback);
	}

	private void CurrentProfileCallback(ProfileIndex index)
	{
		if (BenchmarkMode)
		{
			BenchmarkMode = false;
			if (osdEnabled)
			{
				_events.GetEvent<OsdNotificationEvent>().Publish(new KeyValuePair<OsdType, int>(OsdType.PerformanceMode, (int)_selectedProfile.Index));
			}
			_client.ApplyProfile(_selectedProfile.Index);
			_client.UpdateProfile(_selectedProfile.Index, _selectedProfile.Ac, _selectedProfile.Dc);
			return;
		}
		if (_selectedProfile.Index != index)
		{
			_selectedProfile = CurrentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == index);
		}
		if (osdEnabled)
		{
			_events.GetEvent<OsdNotificationEvent>().Publish(new KeyValuePair<OsdType, int>(OsdType.PerformanceMode, (int)_selectedProfile.Index));
		}
		RefreshUI();
	}

	private void PassiveCoolingCallback(bool disabled)
	{
		PassiveCoolingMode = !disabled;
	}

	private void ProfileSettingCallback(PerformanceProfile newProfile)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			PerformanceProfile performanceProfile = CurrentProfiles[CurrentProfiles.IndexOf(CurrentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == newProfile.Index))];
			if (performanceProfile.Ac.CpuMode != newProfile.Ac.CpuMode && performanceProfile.Ac.DGpuMode != newProfile.Ac.DGpuMode && performanceProfile.Ac.FanMode != newProfile.Ac.FanMode && performanceProfile.Dc.CpuMode != newProfile.Dc.CpuMode && performanceProfile.Dc.DGpuMode != newProfile.Dc.DGpuMode && performanceProfile.Dc.FanMode != newProfile.Dc.FanMode)
			{
				performanceProfile = newProfile;
				RefreshUI();
			}
		});
	}

	private void LoadClientDefaults()
	{
		_defaultBenchmarkMode = false;
		_defaultPassiveCoolingMode = true;
		_defaultCurrentProfiles = new ObservableCollection<PerformanceProfile>(_client.GetDefaultProfiles());
		_defaultSelectedProfile = _defaultCurrentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == _client.GetDefaultProfile());
	}

	private bool CopyMode(bool mode)
	{
		if (mode)
		{
			return true;
		}
		return false;
	}

	private PerformanceProfile CopyProfile(PerformanceProfile profile)
	{
		return new PerformanceProfile
		{
			Ac = new PerformanceMode
			{
				CpuMode = profile.Ac.CpuMode,
				DGpuMode = profile.Ac.DGpuMode,
				FanMode = profile.Ac.FanMode
			},
			Dc = new PerformanceMode
			{
				CpuMode = profile.Dc.CpuMode,
				DGpuMode = profile.Dc.DGpuMode,
				FanMode = profile.Dc.FanMode
			},
			Index = profile.Index
		};
	}

	private void LoadCustomDefaults(MobilePerformanceTuningCustomSettings customSettings)
	{
		_defaultBenchmarkMode = CopyMode(customSettings.BenchmarkMode);
		_defaultPassiveCoolingMode = CopyMode(customSettings.PassiveCoolingMode);
		foreach (PerformanceProfile currentProfile in customSettings.CurrentProfiles)
		{
			_defaultCurrentProfiles.Add(CopyProfile(currentProfile));
		}
		_defaultSelectedProfile = _defaultCurrentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == (ProfileIndex)customSettings.SelectedProfileIndex);
		if (_defaultSelectedProfile == null)
		{
			_defaultSelectedProfile = _defaultCurrentProfiles[0];
		}
	}

	public void UpdateProfile(PerformanceProfile profile)
	{
		profile.Ac.DGpuMode = profile.Ac.CpuMode;
		profile.Dc.DGpuMode = profile.Dc.CpuMode;
		if (profile.Ac.CpuMode == 1 && profile.Ac.FanMode == 3)
		{
			profile.Ac.FanMode = 1;
		}
		if (profile.Dc.CpuMode == 1 && profile.Dc.FanMode == 3)
		{
			profile.Dc.FanMode = 1;
		}
		RefreshCurrentProfiles();
		_client.UpdateProfile(profile.Index, profile.Ac, profile.Dc);
	}

	private void RefreshCurrentProfiles()
	{
		ObservableCollection<PerformanceProfile> observableCollection = new ObservableCollection<PerformanceProfile>();
		foreach (PerformanceProfile currentProfile in _currentProfiles)
		{
			observableCollection.Add(CopyProfile(currentProfile));
		}
		_currentProfiles.Clear();
		foreach (PerformanceProfile item in observableCollection)
		{
			PerformanceProfile performanceProfile = CopyProfile(item);
			_currentProfiles.Add(performanceProfile);
			if (performanceProfile.Index == _selectedProfile.Index)
			{
				_selectedProfile = performanceProfile;
			}
		}
		RefreshUI();
	}

	private void SwitchProfiles(PerformanceProfile newProfile)
	{
		if (newProfile != null)
		{
			SelectedProfile = CurrentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == newProfile.Index);
		}
	}

	private void SwitchProfiles(ProfileIndex index)
	{
		SelectedProfile = CurrentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == index);
	}

	public void RestoreDefaults()
	{
		PerformanceState currentState = _client.GetCurrentState();
		_currentProfiles.Clear();
		foreach (PerformanceProfile defaultCurrentProfile in _defaultCurrentProfiles)
		{
			_currentProfiles.Add(CopyProfile(defaultCurrentProfile));
		}
		_selectedProfile = _currentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == _defaultSelectedProfile.Index);
		PerformanceProfile[] currentProfiles = _client.GetCurrentProfiles();
		foreach (PerformanceProfile sdkProfile in currentProfiles)
		{
			PerformanceProfile performanceProfile = _currentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == sdkProfile.Index);
			if (sdkProfile.Ac.CpuMode != performanceProfile.Ac.CpuMode || sdkProfile.Ac.FanMode != performanceProfile.Ac.FanMode || sdkProfile.Ac.DGpuMode != performanceProfile.Ac.DGpuMode || sdkProfile.Dc.CpuMode != performanceProfile.Dc.CpuMode || sdkProfile.Dc.FanMode != performanceProfile.Dc.FanMode || sdkProfile.Dc.DGpuMode != performanceProfile.Dc.DGpuMode)
			{
				_client.UpdateProfile(performanceProfile.Index, performanceProfile.Ac, performanceProfile.Dc);
			}
		}
		if (currentState.SelectedProfileIndex != _selectedProfile.Index)
		{
			_client.ApplyProfile(_selectedProfile.Index);
		}
		_passiveCoolingMode = _defaultPassiveCoolingMode;
		if (_passiveCoolingMode != currentState.PassiveCooling)
		{
			_client.DisablePassiveCoolingMode(!_defaultPassiveCoolingMode);
		}
		_benchmarkMode = _defaultBenchmarkMode;
		if (_benchmarkMode != currentState.BenchmarkMode)
		{
			_client.ApplyBenchmarkMode(_benchmarkMode);
		}
		_whisperMode = false;
		_whisperModeSetting = 3;
		_whisperModeFps = 60;
		_client.SetWhisperModeType(WhisperModeType.Off);
		RefreshUI();
	}

	private void ApplyWhisperModeTypeToSdk()
	{
		_client.SetWhisperModeType((WhisperModeType)_whisperModeSetting);
	}

	private void ApplyWhisperModeFpsToSdk()
	{
		_client.SetWhisperModeFps(_whisperModeFps);
	}

	private void WhisperModeErrorDisplay()
	{
		ErrorHandler.PublishError("WhisperModeUpdateInstructions", ErrorType.SERVICE_OUTDATED, critical: true, msgIsKey: true);
	}

	private void GetPerformanceState()
	{
		PerformanceState state = _client.GetCurrentState();
		PerformanceProfile[] currentProfiles = _client.GetCurrentProfiles();
		foreach (PerformanceProfile item in currentProfiles)
		{
			_currentProfiles.Add(item);
		}
		_selectedProfile = CurrentProfiles.FirstOrDefault((PerformanceProfile p) => p.Index == state.SelectedProfileIndex);
		_passiveCoolingMode = state.PassiveCooling;
		_benchmarkMode = state.BenchmarkMode;
		RefreshUI();
	}

	private void RefreshUI()
	{
		RaisePropertyChanged("PassiveCoolingMode");
		RaisePropertyChanged("BenchmarkMode");
		RaisePropertyChanged("SelectedProfile");
		RaisePropertyChanged("CurrentProfiles");
		RaisePropertyChanged("WhisperMode");
		RaisePropertyChanged("WhisperModeSetting");
		RaisePropertyChanged("WhisperModeFps");
	}

	private bool IsWhisperModeSupported()
	{
		Feature[] supportedFeatures = _client.GetSupportedFeatures();
		for (int i = 0; i < supportedFeatures.Length; i++)
		{
			if (supportedFeatures[i].Type == FeatureType.WhisperMode)
			{
				WhisperModeSupported = true;
				return true;
			}
		}
		WhisperModeSupported = false;
		return false;
	}
}
