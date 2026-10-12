using System;
using ContextSensingClient;
using Prism.Events;
using Prism.Mvvm;
using UserPresenceMgr.Events;

namespace UserPresenceMgr.Models;

public class WakeFeatureData : BindableBase
{
	private readonly IEventAggregator _events;

	private const FeatureType TYPE = FeatureType.WAKE;

	private const string WOA_ENABLED_PROPERTY = "WoaEnabled";

	private bool _woaEnabled;

	private const string EXTERNAL_MONITOR_PROPERTY = "ExternalMonitor";

	private bool _externalMonitor;

	private const string WAKE_EXTERNAL_MONITOR_PROPERTY = "EnableWakeWithExternalMonitor";

	private bool _enableWakeWithExternalMonitor;

	private const string WAKE_LOW_BATTERY_PROPERTY = "WakeOnLowBattery";

	private bool _wakeOnLowBattery;

	private const string LOW_BATTERY_REMAINING_PROPERTY = "LowBatteryRemainingPercentageLimit";

	private int _lowBatteryRemainingPercentageLimit;

	public int TimeIntervalMin => UserPresenceManager.TIME_INTERVAL_MIN;

	public int TimeIntervalMax => UserPresenceManager.TIME_INTERVAL_MAX;

	public int PercentIntervalMin => UserPresenceManager.PERCENT_MIN;

	public int PercentIntervalMax => UserPresenceManager.PERCENT_MAX;

	public bool WoaEnabled
	{
		get
		{
			return _woaEnabled;
		}
		set
		{
			SetProperty(ref _woaEnabled, value, "WoaEnabled");
			_events.GetEvent<UserPresenceEnableFeatureEvent>().Publish(new Tuple<FeatureType, bool>(FeatureType.WAKE, value));
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
			SetOptionData payload = new SetOptionData(FeatureType.WAKE, FeatureProperty.ExternalMonitor, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
	}

	public bool EnableWakeWithExternalMonitor
	{
		get
		{
			return _enableWakeWithExternalMonitor;
		}
		set
		{
			SetProperty(ref _enableWakeWithExternalMonitor, value, "EnableWakeWithExternalMonitor");
			SetOptionData payload = new SetOptionData(FeatureType.WAKE, FeatureProperty.EnableWithExtMonitor, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
	}

	public bool WakeOnLowBattery
	{
		get
		{
			return _wakeOnLowBattery;
		}
		set
		{
			SetProperty(ref _wakeOnLowBattery, value, "WakeOnLowBattery");
			SetOptionData payload = new SetOptionData(FeatureType.WAKE, FeatureProperty.WakeOnLowBattery, value);
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
			SetOptionData payload = new SetOptionData(FeatureType.WAKE, FeatureProperty.LowBatteryRemainingPercentageLimit, value);
			_events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
	}

	public WakeFeatureData(IEventAggregator events, WakeFeatures Features)
	{
		_events = events;
		_woaEnabled = Features.WOAEnabled;
		_externalMonitor = Features.ExternalMonitor;
		_enableWakeWithExternalMonitor = Features.EnableWakeWithExternalMonitor;
		_wakeOnLowBattery = Features.WakeOnLowBattery;
		_lowBatteryRemainingPercentageLimit = Features.LowBatteryRemainingMinutesLimit;
	}

	public void UpdateData(WakeFeatures Features)
	{
		_woaEnabled = Features.WOAEnabled;
		RaisePropertyChanged("WoaEnabled");
		_externalMonitor = Features.ExternalMonitor;
		RaisePropertyChanged("ExternalMonitor");
		_enableWakeWithExternalMonitor = Features.EnableWakeWithExternalMonitor;
		RaisePropertyChanged("EnableWakeWithExternalMonitor");
		_wakeOnLowBattery = Features.WakeOnLowBattery;
		RaisePropertyChanged("WakeOnLowBattery");
		_lowBatteryRemainingPercentageLimit = Features.LowBatteryRemainingMinutesLimit;
		RaisePropertyChanged("LowBatteryRemainingPercentageLimit");
	}
}
