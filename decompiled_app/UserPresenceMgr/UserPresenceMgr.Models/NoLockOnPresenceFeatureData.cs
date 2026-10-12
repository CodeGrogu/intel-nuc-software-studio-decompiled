using System;
using ContextSensingClient;
using Prism.Events;
using Prism.Mvvm;
using UserPresenceMgr.Events;

namespace UserPresenceMgr.Models;

public class NoLockOnPresenceFeatureData : BindableBase
{
	private readonly IEventAggregator _events;

	private const FeatureType TYPE = FeatureType.NOLOCKONPRESENCE;

	private const string NLOP_ENABLED_PROPERTY = "NlopEnabled";

	private bool _nlopEnabled;

	private const string EXTERNAL_MONITOR_PROPERTY = "ExternalMonitor";

	private bool _externalMonitor;

	private const string ENABLE_WITH_EXTERNAL_MONITOR_PROPERTY = "EnableNLoPWithExternalMonitor";

	private bool _enableNLoPWithExternalMonitor;

	private const string EMABLE_ON_BATTERY_PROPERTY = "EnableOnBattery";

	private bool _enableOnBattery;

	private const string LOW_BATTERY_REMAINING_LIMIT_PROPERTY = "LowBatteryRemainingPercentageLimit";

	private int _lowBatteryRemainingPercentageLimit;

	private const string WAIT_INTERVAL_PROPERTY = "WaitInterval";

	private int _waitInterval;

	private const string FAILSAFE_TIMEOUT_PROPERTY = "FailSafeTimeout";

	private int _failSafeTimeout;

	public int TimeIntervalMin => UserPresenceManager.TIME_INTERVAL_MIN;

	public int TimeIntervalMax => UserPresenceManager.TIME_INTERVAL_MAX;

	public int PercentIntervalMin => UserPresenceManager.PERCENT_MIN;

	public int PercentIntervalMax => UserPresenceManager.PERCENT_MAX;

	public bool NlopEnabled
	{
		get
		{
			return _nlopEnabled;
		}
		set
		{
			SetProperty(ref _nlopEnabled, value, "NlopEnabled");
			_events.GetEvent<UserPresenceEnableFeatureEvent>().Publish(new Tuple<FeatureType, bool>(FeatureType.NOLOCKONPRESENCE, value));
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
			SetOptionData payload = new SetOptionData(FeatureType.NOLOCKONPRESENCE, FeatureProperty.ExternalMonitor, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
	}

	public bool EnableNLoPWithExternalMonitor
	{
		get
		{
			return _enableNLoPWithExternalMonitor;
		}
		set
		{
			SetProperty(ref _enableNLoPWithExternalMonitor, value, "EnableNLoPWithExternalMonitor");
			SetOptionData payload = new SetOptionData(FeatureType.NOLOCKONPRESENCE, FeatureProperty.EnableWithExtMonitor, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
	}

	public bool EnableOnBattery
	{
		get
		{
			return _enableOnBattery;
		}
		set
		{
			SetProperty(ref _enableOnBattery, value, "EnableOnBattery");
			SetOptionData payload = new SetOptionData(FeatureType.NOLOCKONPRESENCE, FeatureProperty.EnableOnBattery, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
	}

	public int LowBatteryRemainingPercentageLimit
	{
		get
		{
			return _lowBatteryRemainingPercentageLimit;
		}
		set
		{
			SetProperty(ref _lowBatteryRemainingPercentageLimit, value, "LowBatteryRemainingPercentageLimit");
			SetOptionData payload = new SetOptionData(FeatureType.NOLOCKONPRESENCE, FeatureProperty.LowBatteryRemainingPercentageLimit, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
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
			SetOptionData payload = new SetOptionData(FeatureType.NOLOCKONPRESENCE, FeatureProperty.WaitInterval, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
	}

	public int FailSafeTimeout
	{
		get
		{
			return _failSafeTimeout;
		}
		set
		{
			SetProperty(ref _failSafeTimeout, value, "FailSafeTimeout");
			SetOptionData payload = new SetOptionData(FeatureType.NOLOCKONPRESENCE, FeatureProperty.FailSafeTimeout, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
	}

	public NoLockOnPresenceFeatureData(IEventAggregator events, NoLockOnPresenceFeatures Features)
	{
		_events = events;
		_nlopEnabled = Features.NLoPEnabled;
		_externalMonitor = Features.ExternalMonitor;
		_enableNLoPWithExternalMonitor = Features.EnableNLoPWithExternalMonitor;
		_enableOnBattery = Features.EnableOnBattery;
		_lowBatteryRemainingPercentageLimit = Features.LowBatteryRemainingMinutesLimit;
		_waitInterval = Features.WaitInterval;
		_failSafeTimeout = Features.FailSafeTimeout;
	}

	public void UpdateData(NoLockOnPresenceFeatures Features)
	{
		_nlopEnabled = Features.NLoPEnabled;
		RaisePropertyChanged("NlopEnabled");
		_externalMonitor = Features.ExternalMonitor;
		RaisePropertyChanged("ExternalMonitor");
		_enableNLoPWithExternalMonitor = Features.EnableNLoPWithExternalMonitor;
		RaisePropertyChanged("EnableNLoPWithExternalMonitor");
		_enableOnBattery = Features.EnableOnBattery;
		RaisePropertyChanged("EnableOnBattery");
		_lowBatteryRemainingPercentageLimit = Features.LowBatteryRemainingMinutesLimit;
		RaisePropertyChanged("LowBatteryRemainingPercentageLimit");
		_waitInterval = Features.WaitInterval;
		RaisePropertyChanged("WaitInterval");
		_failSafeTimeout = Features.FailSafeTimeout;
		RaisePropertyChanged("FailSafeTimeout");
	}
}
