using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Infrastructure.Models;
using MobileKeyboardSettingsMgr.Interface;
using MobileKeyboardSettingsMgr.Models;
using Prism.Events;
using Prism.Logging;
using UniwillSdkClient.Events;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardSettingsMgr.ViewModels;

internal class MobileSystemSettingsViewModel : BaseSummaryViewModel
{
	private UniwillServiceClient _client;

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private const bool NUM_PAD_DEFAULT = false;

	private const bool WIN_KEY_DEFAULT = true;

	private const bool TOUCHPAD_KEY_DEFAULT = false;

	private const bool FN_KEY_DEFAULT = false;

	private const bool OSD_DEFAULT = true;

	private List<ISystemSetting> _systemSettings = new List<ISystemSetting>();

	private bool _errorMessage = true;

	public string BatteryChargeLimitRegion => "BatteryChargeLimitRegion";

	public List<ISystemSetting> SystemSettings
	{
		get
		{
			return _systemSettings;
		}
		set
		{
			SetProperty(ref _systemSettings, value, "SystemSettings");
		}
	}

	public bool ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			SetProperty(ref _errorMessage, value, "ErrorMessage");
		}
	}

	public MobileSystemSettingsViewModel(IEventAggregator events, ILoggerFacade logger, ICustomResourceManager resourceManager, IUniwillClient client)
		: base(resourceManager, IconType.System)
	{
		MobileSystemSettingsViewModel mobileSystemSettingsViewModel = this;
		ErrorMessage = false;
		_events = events;
		_logger = logger;
		Task.Run(() =>
		{
			try
			{
				mobileSystemSettingsViewModel._client = client.Client;
				if (mobileSystemSettingsViewModel._client == null)
				{
					throw new ArgumentNullException("Unable to connect to SDK");
				}
				if (resourceManager.IsFirstLaunch())
				{
					if (resourceManager.CustomAppSettings?.Settings?.Find((object x) => x.GetType().Equals(typeof(MobileKeyboardCustomSettings))) is MobileKeyboardCustomSettings customSettings)
					{
						mobileSystemSettingsViewModel.LoadDefaults(customSettings);
					}
					else
					{
						mobileSystemSettingsViewModel.LoadDefaults();
					}
				}
				else
				{
					mobileSystemSettingsViewModel.LoadSettingsFromClient();
				}
				mobileSystemSettingsViewModel._events.GetEvent<KeyStateNotificationEvent>().Subscribe(mobileSystemSettingsViewModel.KeyStateNotification);
			}
			catch (Exception exception)
			{
				logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
				mobileSystemSettingsViewModel.ErrorMessage = true;
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private KeyStateModel FindKeyStateModel(KeyType Type)
	{
		foreach (ISystemSetting systemSetting in SystemSettings)
		{
			if (systemSetting.GetType().Equals(typeof(KeyStateModel)) && ((KeyStateModel)systemSetting).Type == Type)
			{
				return (KeyStateModel)systemSetting;
			}
		}
		return null;
	}

	private void KeyStateNotification(KeyState Key)
	{
		FindKeyStateModel(Key.Type)?.UpdateValue(Key.State);
	}

	private void LoadDefaults(MobileKeyboardCustomSettings customSettings = null)
	{
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = true;
		bool flag5 = false;
		if (customSettings != null)
		{
			flag = customSettings.OnScreenDisplay;
			flag2 = customSettings.FnKey;
			flag3 = customSettings.NumPad;
			flag4 = customSettings.WinKey;
			flag5 = customSettings.TouchPadKeyDisabled;
		}
		KeyState[] currentKeyStates = _client.GetCurrentKeyStates();
		foreach (KeyState keyState in currentKeyStates)
		{
			switch (keyState.Type)
			{
			case KeyType.DisableTouchPadToggle:
				if (keyState.State != flag5)
				{
					_client.EnableKey(new KeyState
					{
						Type = KeyType.DisableTouchPadToggle,
						State = flag5
					});
				}
				break;
			case KeyType.FnKey:
				if (keyState.State != flag2)
				{
					_client.EnableKey(new KeyState
					{
						Type = KeyType.FnKey,
						State = flag2
					});
				}
				break;
			case KeyType.NumPad:
				if (keyState.State != flag3)
				{
					_client.EnableKey(new KeyState
					{
						Type = KeyType.NumPad,
						State = flag3
					});
				}
				break;
			case KeyType.WinKey:
				if (keyState.State != flag4)
				{
					_client.EnableKey(new KeyState
					{
						Type = KeyType.WinKey,
						State = flag4
					});
				}
				break;
			}
		}
		SystemSettings = new List<ISystemSetting>
		{
			new KeyStateModel(KeyType.NumPad, flag3, _client.EnableKey),
			new KeyStateModel(KeyType.WinKey, flag4, _client.EnableKey),
			new KeyStateModel(KeyType.FnKey, flag2, _client.EnableKey),
			new KeyStateModel(KeyType.DisableTouchPadToggle, flag5, invert: true, _client.EnableKey),
			new OsdStateModel(_events, flag, _client.EnableOnScreenDisplay)
		};
		_client.EnableOnScreenDisplay(flag);
		_events.GetEvent<OsdEnabledEvent>().Publish(flag);
	}

	private void LoadSettingsFromClient()
	{
		KeyStateModel item = new KeyStateModel(KeyType.FnKey, value: false, _client.EnableKey);
		KeyStateModel item2 = new KeyStateModel(KeyType.NumPad, value: false, _client.EnableKey);
		KeyStateModel item3 = new KeyStateModel(KeyType.WinKey, value: true, _client.EnableKey);
		KeyStateModel item4 = new KeyStateModel(KeyType.DisableTouchPadToggle, value: false, invert: true, _client.EnableKey);
		KeyState[] currentKeyStates = _client.GetCurrentKeyStates();
		foreach (KeyState keyState in currentKeyStates)
		{
			switch (keyState.Type)
			{
			case KeyType.DisableTouchPadToggle:
				item4 = new KeyStateModel(KeyType.DisableTouchPadToggle, keyState.State, invert: true, _client.EnableKey);
				break;
			case KeyType.FnKey:
				item = new KeyStateModel(KeyType.FnKey, keyState.State, _client.EnableKey);
				break;
			case KeyType.NumPad:
				item2 = new KeyStateModel(KeyType.NumPad, keyState.State, _client.EnableKey);
				break;
			case KeyType.WinKey:
				item3 = new KeyStateModel(KeyType.WinKey, keyState.State, _client.EnableKey);
				break;
			}
		}
		OsdStateModel osdStateModel = LoadCurrentOsdState();
		SystemSettings = new List<ISystemSetting> { item2, item3, item, item4, osdStateModel };
		_events.GetEvent<OsdEnabledEvent>().Publish(osdStateModel.Value);
	}

	private OsdStateModel LoadCurrentOsdState()
	{
		if (!Directory.Exists(OsdStateModel.OSD_SETTINGS_DIRECTORY) || !Directory.EnumerateFileSystemEntries(OsdStateModel.OSD_SETTINGS_DIRECTORY).Any())
		{
			_client.EnableOnScreenDisplay(Enable: true);
			OsdStateModel.SerializeOsdState(state: true);
			return new OsdStateModel(_events, value: true, _client.EnableOnScreenDisplay);
		}
		bool previousOsdState = OsdStateModel.GetPreviousOsdState();
		return new OsdStateModel(_events, previousOsdState, _client.EnableOnScreenDisplay);
	}
}
