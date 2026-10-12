using System.ServiceModel;
using NucSoftwareStudioService.LedControl.Models;
using NucSoftwareStudioService.Wcf;

namespace NucSoftwareStudioService.LedControl.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class AlexaSettingsService : IAlexaSettingsServiceContract
{
	public delegate void EnableAlexa(bool Enable);

	public delegate void TuneAlexaEffects(AlexaAnimationSettings AnimationSettings);

	public delegate void SaveSettings(AlexaLedSettings Settings);

	private readonly EnableAlexa _enableAlexa;

	private readonly TuneAlexaEffects _tuneAlexa;

	private readonly SaveSettings _saveSettings;

	private readonly AlexaLedSettings _settings;

	public AlexaSettingsService()
	{
	}

	public AlexaSettingsService(AlexaLedSettings Settings, SaveSettings SaveSettingsDelegate, EnableAlexa EnableAlexa, TuneAlexaEffects TuneAlexa)
	{
		_saveSettings = SaveSettingsDelegate;
		_settings = Settings;
		_enableAlexa = EnableAlexa;
		_tuneAlexa = TuneAlexa;
	}

	public AlexaAnimationSettings GetAlexaAnimationSettings()
	{
		return _settings.AlexaAnimationSettings;
	}

	public bool GetLightBarState()
	{
		return _settings.Enabled;
	}

	public void SetAlexaAnimationSettings(AlexaAnimationSettings Settings)
	{
		_tuneAlexa(Settings);
		_settings.AlexaAnimationSettings = Settings;
		_saveSettings(_settings);
	}

	public void SetLightBarState(bool enable)
	{
		_enableAlexa(enable);
		_settings.Enabled = enable;
		_saveSettings(_settings);
	}

	public bool CheckConnectionHealth()
	{
		return true;
	}
}
