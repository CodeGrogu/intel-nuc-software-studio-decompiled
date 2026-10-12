using System.ServiceModel;
using IntelSdkClient.AlexaSettingsService;

namespace IntelSdkClient;

public class AlexaSettingsServiceProxy : ClientBase<IAlexaSettingsServiceContract>
{
	public void SetLightBarState(bool enable)
	{
		Channel.SetLightBarState(enable);
	}

	public bool GetLightBarState()
	{
		return Channel.GetLightBarState();
	}

	public void SetAlexaAnimationSettings(AlexaAnimationSettings Settings)
	{
		Channel.SetAlexaAnimationSettings(Settings);
	}

	public AlexaAnimationSettings GetAlexaAnimationSettings()
	{
		return Channel.GetAlexaAnimationSettings();
	}
}
