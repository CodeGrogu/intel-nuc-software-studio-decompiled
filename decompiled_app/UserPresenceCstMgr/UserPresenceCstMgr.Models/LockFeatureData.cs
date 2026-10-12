using System;
using Intel.Cst.Client;
using Prism.Events;
using Prism.Mvvm;
using UserPresenceCstMgr.Events;

namespace UserPresenceCstMgr.Models;

public class LockFeatureData : BindableBase
{
	private readonly IEventAggregator _events;

	private const int DIMMING_DIVISOR = 12;

	private static readonly int LOCK_INTERVAL = (int)TimeSpan.FromSeconds(30.0).TotalMilliseconds;

	private const FeatureType TYPE = FeatureType.LOCK;

	private const string WAL_ENABLED_PROPERTY = "WalEnabled";

	private bool _walEnabled;

	private const string EXTERNAL_MONITOR_PROPERTY = "ExternalMonitor";

	private bool _externalMonitor;

	private const string ENABLE_EXTERNAL_MONITOR_LOCK_PROPERTY = "EnableLockWithExternalMonitor";

	private bool _enableLockWithExternalMonitor;

	private const string PRE_DIM_INTERVAL_PROPERTY = "PreDimInterval";

	private int _preDimInterval;

	private const string WAIT_INTERVAL_PROPERTY = "WaitInterval";

	private int _waitInterval;

	private const string DIM_SCREEN_PROPERTY = "DimScreen";

	private bool _dimScreen;

	private const string POWER_REQUEST_PROPERTY = "PowerRequest";

	private bool _powerRequest;

	private const string LOCK_AFTER_DIM_PROPERTY = "LockAfterDimInterval";

	private int _lockAfterDimInterval;

	private const string DISPLAY_OFF_PROPERTY = "DisplayOffAfterLock";

	private bool _displayOffAfterLock;

	private const string DIMMING_INTERVAL_PROPERTY = "DimmingInterval";

	private int _dimmingInterval;

	private const string NOT_PRESENT_DIM_TARGET_PROPERTY = "NotPresentDimTarget";

	private int _notPresentDimTarget;

	public int TimeIntervalMin => UserPresenceCstManager.TIME_INTERVAL_MIN;

	public int TimeIntervalMax => UserPresenceCstManager.TIME_INTERVAL_MAX;

	public int PercentIntervalMin => UserPresenceCstManager.PERCENT_MIN;

	public int PercentIntervalMax => UserPresenceCstManager.PERCENT_MAX;

	public int DimmingDivisor => 12;

	public int LockInterval => LOCK_INTERVAL;

	public bool WalEnabled
	{
		get
		{
			return _walEnabled;
		}
		set
		{
			SetProperty(ref _walEnabled, value, "WalEnabled");
			_events.GetEvent<UserPresenceCstEnableFeatureEvent>().Publish(new Tuple<FeatureType, bool>(FeatureType.LOCK, value));
		}
	}

	public bool ExternalMonitor
	{
		get
		{
			return _externalMonitor;
		}
		set
		{
			SetProperty(ref _externalMonitor, value, "ExternalMonitor");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.EnableWithExtMonitor, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public bool EnableLockWithExternalMonitor
	{
		get
		{
			return _enableLockWithExternalMonitor;
		}
		set
		{
			SetProperty(ref _enableLockWithExternalMonitor, value, "EnableLockWithExternalMonitor");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.EnableWithExtMonitor, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public int PreDimInterval
	{
		get
		{
			return _preDimInterval;
		}
		set
		{
			SetProperty(ref _preDimInterval, value, "PreDimInterval");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.PreDimInterval, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public int WaitInterval
	{
		get
		{
			return _waitInterval;
		}
		set
		{
			SetProperty(ref _waitInterval, value, "WaitInterval");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.WaitInterval, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public bool DimScreen
	{
		get
		{
			return _dimScreen;
		}
		set
		{
			SetProperty(ref _dimScreen, value, "DimScreen");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.DimScreen, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public bool PowerRequest
	{
		get
		{
			return _powerRequest;
		}
		set
		{
			SetProperty(ref _powerRequest, value, "PowerRequest");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.PowerRequest, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public int LockAfterDimInterval
	{
		get
		{
			return _lockAfterDimInterval;
		}
		set
		{
			SetProperty(ref _lockAfterDimInterval, value, "LockAfterDimInterval");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.LockAfterDimInterval, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public bool DisplayOffAfterLock
	{
		get
		{
			return _displayOffAfterLock;
		}
		set
		{
			SetProperty(ref _displayOffAfterLock, value, "DisplayOffAfterLock");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.DisplayOffAfterLock, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public int DimmingInterval
	{
		get
		{
			return _dimmingInterval;
		}
		set
		{
			SetProperty(ref _dimmingInterval, value, "DimmingInterval");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.DimmingInterval, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public int NotPresentDimTarget
	{
		get
		{
			return _notPresentDimTarget;
		}
		set
		{
			SetProperty(ref _notPresentDimTarget, value, "NotPresentDimTarget");
			SetOptionData payload = new SetOptionData(FeatureType.LOCK, FeatureProperty.NotPresentDimTarget, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public LockFeatureData(IEventAggregator events, LockFeatures Features)
	{
		_events = events;
		_walEnabled = Features.WALEnabled;
		_externalMonitor = Features.EnableLockWithExternalMonitor;
		_enableLockWithExternalMonitor = Features.EnableLockWithExternalMonitor;
		_preDimInterval = Features.PreDimInterval;
		_waitInterval = Features.WaitInterval;
		_dimScreen = Features.DimScreen;
		_powerRequest = Features.PowerRequest;
		_lockAfterDimInterval = Features.LockAfterDimInterval;
		_displayOffAfterLock = Features.DisplayOffAfterLock;
		_dimmingInterval = Features.DimmingInterval;
		_notPresentDimTarget = Features.NotPresentDimTarget;
	}

	public void UpdateData(LockFeatures Features)
	{
		_walEnabled = Features.WALEnabled;
		RaisePropertyChanged("WalEnabled");
		_externalMonitor = Features.EnableLockWithExternalMonitor;
		RaisePropertyChanged("ExternalMonitor");
		_enableLockWithExternalMonitor = Features.EnableLockWithExternalMonitor;
		RaisePropertyChanged("EnableLockWithExternalMonitor");
		_preDimInterval = Features.PreDimInterval;
		RaisePropertyChanged("PreDimInterval");
		_waitInterval = Features.WaitInterval;
		RaisePropertyChanged("WaitInterval");
		_dimScreen = Features.DimScreen;
		RaisePropertyChanged("DimScreen");
		_powerRequest = Features.PowerRequest;
		RaisePropertyChanged("PowerRequest");
		_lockAfterDimInterval = Features.LockAfterDimInterval;
		RaisePropertyChanged("LockAfterDimInterval");
		_displayOffAfterLock = Features.DisplayOffAfterLock;
		RaisePropertyChanged("DisplayOffAfterLock");
		_dimmingInterval = Features.DimmingInterval;
		RaisePropertyChanged("DimmingInterval");
		_notPresentDimTarget = Features.NotPresentDimTarget;
		RaisePropertyChanged("NotPresentDimTarget");
	}
}
