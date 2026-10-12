using System;
using Intel.Cst.Client;
using Prism.Events;
using Prism.Mvvm;
using UserPresenceCstMgr.Events;

namespace UserPresenceCstMgr.Models;

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

	public int TimeIntervalMin => UserPresenceCstManager.TIME_INTERVAL_MIN;

	public int TimeIntervalMax => UserPresenceCstManager.TIME_INTERVAL_MAX;

	public int PercentIntervalMin => UserPresenceCstManager.PERCENT_MIN;

	public int PercentIntervalMax => UserPresenceCstManager.PERCENT_MAX;

	public bool WoaEnabled
	{
		get
		{
			return _woaEnabled;
		}
		set
		{
			SetProperty(ref _woaEnabled, value, "WoaEnabled");
			_events.GetEvent<UserPresenceCstEnableFeatureEvent>().Publish(new Tuple<FeatureType, bool>(FeatureType.WAKE, value));
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
			SetOptionData payload = new SetOptionData(FeatureType.WAKE, FeatureProperty.EnableWithExtMonitor, value);
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
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
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
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
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
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
			_events.GetEvent<UserPresenceCstUpdateSettingEvent>().Publish(payload);
		}
	}

	public WakeFeatureData(IEventAggregator events, WakeFeatures Features)
	{
		_events = events;
		_woaEnabled = Features.WOAEnabled;
		_externalMonitor = Features.EnableWakeWithExternalMonitor;
		_enableWakeWithExternalMonitor = Features.EnableWakeWithExternalMonitor;
		_wakeOnLowBattery = Features.WakeOnLowBattery;
		_lowBatteryRemainingPercentageLimit = Features.LowBatteryRemainingPercentageLimit;
	}

	public void UpdateData(WakeFeatures Features)
	{
		_woaEnabled = Features.WOAEnabled;
		RaisePropertyChanged("WoaEnabled");
		_externalMonitor = Features.EnableWakeWithExternalMonitor;
		RaisePropertyChanged("ExternalMonitor");
		_enableWakeWithExternalMonitor = Features.EnableWakeWithExternalMonitor;
		RaisePropertyChanged("EnableWakeWithExternalMonitor");
		_wakeOnLowBattery = Features.WakeOnLowBattery;
		RaisePropertyChanged("WakeOnLowBattery");
		_lowBatteryRemainingPercentageLimit = Features.LowBatteryRemainingPercentageLimit;
		RaisePropertyChanged("LowBatteryRemainingPercentageLimit");
	}
}
