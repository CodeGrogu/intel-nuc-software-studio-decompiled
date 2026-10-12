using System.Collections.Generic;
using Infrastructure.Utilities;
using Prism.Events;
using UniwillSdkClient.Constants;
using UniwillSdkClient.Events;
using UniwillSdkClient.UniwillService;

namespace UniwillSdkClient.Models;

public class UniwillCallbackHandler : IUniwillServiceCallback
{
	private readonly IEventAggregator _events;

	public UniwillCallbackHandler(IEventAggregator events)
	{
		_events = events;
	}

	public void BatteryStateNotification(BatteryState CurrentState)
	{
		_events.GetEvent<BatteryStateEvent>().Publish(CurrentState);
	}

	public void BenchmarkMode(bool On)
	{
		_events.GetEvent<BenchmarkModeEvent>().Publish(On);
	}

	public void CurrentKeyboardLedProfileNotification(KeyboardLedProfile CurrentProfile)
	{
	}

	public void CurrentLightbarProfileNotification(LightbarProfile CurrentProfile)
	{
		_events.GetEvent<LightbarProfileNotificationEvent>().Publish(CurrentProfile);
	}

	public void CurrentProfile(ProfileIndex Index)
	{
		_events.GetEvent<CurrentPerformanceProfileEvent>().Publish(Index);
	}

	public void DisplayModeCallback(DisplayMode Mode, bool Current)
	{
		if (Current)
		{
			_events.GetEvent<SelectedDisplayModeEvent>().Publish(Mode);
		}
	}

	public void EnableKeyboardLedsNotification(bool Enable)
	{
	}

	public void IdleTimerNotification(int AcSeconds, int DcSeconds)
	{
	}

	public void KeyboardBrightnessNotification(int Ac, int Dc)
	{
		if ((PowerUtil.IsPluggedIn() && KeyboardBrightness.FUNCTION_KEY_VALUES.Contains(Ac)) || (!PowerUtil.IsPluggedIn() && KeyboardBrightness.FUNCTION_KEY_VALUES.Contains(Dc)))
		{
			_events.GetEvent<KeyboardBrightnessEvent>().Publish((Ac, Dc));
		}
	}

	public void KeyStateNotification(KeyState State)
	{
		_events.GetEvent<KeyStateNotificationEvent>().Publish(State);
	}

	public void LightBarIdleTimerNotification(int AcSeconds, int DcSeconds)
	{
		_events.GetEvent<LightbarIdleTimerEvent>().Publish((AcSeconds, DcSeconds));
	}

	public void OsdNotification(OsdType Type, int Status)
	{
		_events.GetEvent<OsdEnabledEvent>().Publish(payload: true);
		if (Type != OsdType.PerformanceMode)
		{
			_events.GetEvent<OsdNotificationEvent>().Publish(new KeyValuePair<OsdType, int>(Type, Status));
		}
	}

	public void PassiveCoolingMode(bool Disabled)
	{
		_events.GetEvent<PassiveCoolingEvent>().Publish(Disabled);
	}

	public void ProfileSetting(PerformanceProfile Profile)
	{
		_events.GetEvent<ProfileSettingEvent>().Publish(Profile);
	}

	public void ProjectAuroraStateNotification(bool Enable)
	{
	}

	public void SysMonitorNotification(SystemMonitorData Data)
	{
		_events.GetEvent<MobileSystemDataChangeEvent>().Publish(Data);
	}

	public void UpdateFanCurveSetting(FanCurveSetting Setting)
	{
	}

	public void WhisperModeTypeNotification(WhisperModeType Type)
	{
		_events.GetEvent<WhisperModeTypeEvent>().Publish(Type);
	}

	public void WhisperModeFpsNotification(int Fps)
	{
		_events.GetEvent<WhisperModeFpsEvent>().Publish(Fps);
	}
}
