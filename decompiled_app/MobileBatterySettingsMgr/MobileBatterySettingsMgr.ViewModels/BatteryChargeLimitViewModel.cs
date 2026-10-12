using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using CustomizationMgr.Interfaces;
using Infrastructure.Converters;
using Infrastructure.Events;
using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using MobileBatterySettingsMgr.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using UniwillSdkClient.Events;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace MobileBatterySettingsMgr.ViewModels;

public class BatteryChargeLimitViewModel : BindableBase
{
	private UniwillServiceClient _client;

	private List<BatteryHealthMode> _modes;

	private bool _batteryModeCustomActive = true;

	private const int BATTERY_CHARGE_LIMIT_MIN = 50;

	private const int BATTERY_CHARGE_LIMIT_MAX = 100;

	private const int BATTERY_CHARGE_LIMIT_INTERVAL = 10;

	public const int BATTERY_CHARGE_MAX_HEIGHT = 330;

	public const int BATTERY_SLIDER_ARM_HEIGHT = 100;

	private const int BATTERY_CHARGE_LIMIT_BEST = 65;

	private const string PERCENT_SYMBOL_KEY = "PercentSymbol";

	private const int INVALID_BATTERY_PERCENT = -1;

	private int _batteryPercent = -1;

	private int _batteryChargeLimit;

	private int _lastCustomValue;

	private string _localizedPercentString;

	private int _customDefaultBatteryChargeLimit;

	private DelegateCommand _increaseLimit;

	private DelegateCommand _decreaseLimit;

	private DelegateCommand _setBatteryChargingLevel;

	public List<BatteryHealthMode> Modes
	{
		get
		{
			return _modes;
		}
		set
		{
			SetProperty(ref _modes, value, "Modes");
		}
	}

	public bool BatteryModeCustomActive
	{
		get
		{
			return _batteryModeCustomActive;
		}
		private set
		{
			SetProperty(ref _batteryModeCustomActive, value, "BatteryModeCustomActive");
		}
	}

	public int BatteryChargeLimitMin => 50;

	public int BatteryChargeLimitMax => 100;

	public int BatteryChargeLimitInterval => 10;

	public int BatteryChargeMaxHeight => 330;

	public int BatteryChargeHalfMaxHeight => 165;

	public int BatterySliderArmHeight => 100;

	public double BatterySliderArmOffset => 32.0;

	public int BatteryChargeLimitBest => 65;

	public int BatteryPercent
	{
		get
		{
			return _batteryPercent;
		}
		set
		{
			SetProperty(ref _batteryPercent, value, "BatteryPercent");
		}
	}

	public string BatteryPercentString
	{
		get
		{
			if (_batteryPercent != -1)
			{
				return _batteryPercent + _localizedPercentString;
			}
			return "";
		}
	}

	public string BatteryChargeLimitString
	{
		get
		{
			if (_batteryChargeLimit != -1)
			{
				return _batteryChargeLimit + _localizedPercentString;
			}
			return "";
		}
	}

	public int BatteryChargeLimit
	{
		get
		{
			return _batteryChargeLimit;
		}
		set
		{
			if (BatteryChargeLimit != value && value >= 50 && value <= 100)
			{
				SetProperty(ref _batteryChargeLimit, value, "BatteryChargeLimit");
				RaisePropertyChanged("BatteryChargeLimitString");
			}
		}
	}

	public DelegateCommand IncreaseLimit
	{
		get
		{
			return _increaseLimit;
		}
		set
		{
			SetProperty(ref _increaseLimit, value, "IncreaseLimit");
		}
	}

	public DelegateCommand DecreaseLimit
	{
		get
		{
			return _decreaseLimit;
		}
		set
		{
			SetProperty(ref _decreaseLimit, value, "DecreaseLimit");
		}
	}

	public DelegateCommand SetBatteryChargingLevel
	{
		get
		{
			return _setBatteryChargingLevel;
		}
		set
		{
			SetProperty(ref _setBatteryChargingLevel, value, "SetBatteryChargingLevel");
		}
	}

	public BatteryChargeLimitViewModel(ICustomResourceManager customResourceManager, ILoggerFacade logger, IUniwillClient client, IEventAggregator events)
	{
		BatteryChargeLimitViewModel batteryChargeLimitViewModel = this;
		Task.Run(() =>
		{
			try
			{
				batteryChargeLimitViewModel._client = client.Client;
				batteryChargeLimitViewModel.Modes = new List<BatteryHealthMode>
				{
					new BatteryHealthMode(BatteryMode.BestBattery, value: false, batteryChargeLimitViewModel.SetBatteryMode),
					new BatteryHealthMode(BatteryMode.Custom, batteryChargeLimitViewModel._client == null, batteryChargeLimitViewModel.SetBatteryMode),
					new BatteryHealthMode(BatteryMode.AcMode, value: false, batteryChargeLimitViewModel.SetBatteryMode)
				};
				if (batteryChargeLimitViewModel._client == null)
				{
					throw new ArgumentNullException("Unable to connect to SDK");
				}
				batteryChargeLimitViewModel.BatteryChargeLimit = (batteryChargeLimitViewModel._lastCustomValue = (batteryChargeLimitViewModel.LoadCustomSettings(customResourceManager) ? batteryChargeLimitViewModel._customDefaultBatteryChargeLimit : batteryChargeLimitViewModel._client.GetBatteryChargingLevel()));
				BatteryMode batteryMode = batteryChargeLimitViewModel._client.GetBatteryMode();
				foreach (BatteryHealthMode mode in batteryChargeLimitViewModel.Modes)
				{
					mode.Value = batteryMode == mode.Type;
				}
				batteryChargeLimitViewModel._localizedPercentString = ResourceDictionaryConverter.GetString("PercentSymbol");
				batteryChargeLimitViewModel.IncreaseLimit = new DelegateCommand(batteryChargeLimitViewModel.IncreaseBatteryLimit);
				batteryChargeLimitViewModel.DecreaseLimit = new DelegateCommand(batteryChargeLimitViewModel.DecreaseBatteryLimit);
				batteryChargeLimitViewModel.SetBatteryChargingLevel = new DelegateCommand(batteryChargeLimitViewModel.SendToSdk);
				events.GetEvent<BatteryStatusEvent>().Subscribe(batteryChargeLimitViewModel.UpdateBattery);
				events.GetEvent<BatteryStateEvent>().Subscribe(batteryChargeLimitViewModel.OnNotifyBatteryState);
			}
			catch (Exception exception)
			{
				logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			}
			batteryChargeLimitViewModel.RaisePropertyChanged("BatteryChargeLimitString");
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void SetBatteryMode(BatteryMode mode)
	{
		if (_client == null)
		{
			throw new ArgumentNullException("Unable to connect to SDK");
		}
		BatteryModeCustomActive = mode == BatteryMode.Custom;
		if (mode != BatteryMode.Custom && _client.GetBatteryMode() == BatteryMode.Custom)
		{
			_lastCustomValue = BatteryChargeLimit;
		}
		switch (mode)
		{
		case BatteryMode.AcMode:
			BatteryChargeLimit = BatteryChargeLimitMax;
			break;
		case BatteryMode.BestBattery:
			BatteryChargeLimit = BatteryChargeLimitBest;
			break;
		case BatteryMode.Custom:
			BatteryChargeLimit = _lastCustomValue;
			break;
		}
		_client.SetBatteryMode(mode);
	}

	private void OnNotifyBatteryState(BatteryState state)
	{
		string message = string.Empty;
		switch (state)
		{
		case BatteryState.ErmTimeThresholdReached:
			message = ResourceDictionaryConverter.GetString("Toast_Message_ERMTimeThreshold");
			break;
		case BatteryState.TimeAt100Elapsed:
			message = ResourceDictionaryConverter.GetString("Toast_Message_TimeAt100");
			break;
		}
		ToastManager.ShowToastNotification(ResourceDictionaryConverter.GetString("Toast_Title_BatteryHealth"), message, 1, (ToastNotificationActivatedEventArgsCompat args) =>
		{
			WindowHelper.AccessMainWindow((Window window) =>
			{
				window.WindowState = WindowState.Normal;
				window.Activate();
			});
		});
	}

	private void IncreaseBatteryLimit()
	{
		BatteryChargeLimit++;
		SendToSdk();
	}

	private void DecreaseBatteryLimit()
	{
		BatteryChargeLimit--;
		SendToSdk();
	}

	private void SendToSdk()
	{
		_client.SetBatteryChargingLevelAsync(_batteryChargeLimit);
	}

	private bool LoadCustomSettings(ICustomResourceManager customResourceManager)
	{
		if (customResourceManager.IsFirstLaunch() && customResourceManager.CustomAppSettings?.Settings?.Find((object x) => x.GetType().Equals(typeof(BatteryChargeLimitCustomSetting))) is BatteryChargeLimitCustomSetting batteryChargeLimitCustomSetting)
		{
			_customDefaultBatteryChargeLimit = batteryChargeLimitCustomSetting.DefaultLimit;
			_client.SetBatteryChargingLevel(_customDefaultBatteryChargeLimit);
			return true;
		}
		return false;
	}

	private void UpdateBattery(int batteryPercent)
	{
		BatteryPercent = batteryPercent;
		RaisePropertyChanged("BatteryPercentString");
	}
}
