using System;
using System.Diagnostics;
using System.ServiceModel;
using Alexa.Extensions;
using Infrastructure.Logging;
using NucSoftwareStudioService.FeatureSupport.Models;
using NucSoftwareStudioService.LedControl.Interfaces;
using NucSoftwareStudioService.LedControl.Models;
using NucSoftwareStudioService.LedControl.USB;
using NucSoftwareStudioService.LedControl.WMI;
using NucSoftwareStudioService.Wcf;

namespace NucSoftwareStudioService.LedControl.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class AlexaStateService : IAlexaStateServiceContract
{
	private readonly AlexaLedSettings _settings;

	private readonly ILedControl _ledControl;

	private NssServiceLogger _Logger;

	public AlexaStateService()
	{
	}

	public AlexaStateService(AlexaLedSettings Settings, NssServiceLogger logger = null)
	{
		_settings = Settings;
		_Logger = logger;
		if (Settings.LedInterface == FeatureInterfaceType.Usb)
		{
			_ledControl = new HidLedControl();
		}
		else if (Settings.LedInterface == FeatureInterfaceType.Wmi)
		{
			_ledControl = new WmiLedControl();
		}
	}

	public void SetAlexaState(AlexaState newState)
	{
		if (_settings.Enabled && _ledControl != null)
		{
			try
			{
				byte state = AlexaConstants.AlexaStateToSetting(newState);
				_ledControl.SetEffect(state, _settings.AlexaAnimationSettings.Speed, _settings.AlexaAnimationSettings.Brightness, _settings.AlexaAnimationSettings.Direction);
			}
			catch (Exception ex)
			{
				_Logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
			}
		}
	}

	public void EnableAlexa(bool Enable)
	{
		_settings.Enabled = Enable;
		if (!Enable)
		{
			_ledControl.TurnOff();
		}
	}

	public void TuneAlexaEffects(AlexaAnimationSettings AnimationSettings)
	{
		_settings.AlexaAnimationSettings = AnimationSettings;
	}

	public bool CheckConnectionHealth()
	{
		return true;
	}
}
