using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace IntelSdkClient.AlexaSettingsService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class AlexaSettingsServiceContractClient : ClientBase<IAlexaSettingsServiceContract>, IAlexaSettingsServiceContract
{
	public AlexaSettingsServiceContractClient()
	{
	}

	public AlexaSettingsServiceContractClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AlexaSettingsServiceContractClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlexaSettingsServiceContractClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlexaSettingsServiceContractClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void SetLightBarState(bool enable)
	{
		Channel.SetLightBarState(enable);
	}

	public Task SetLightBarStateAsync(bool enable)
	{
		return Channel.SetLightBarStateAsync(enable);
	}

	public bool GetLightBarState()
	{
		return Channel.GetLightBarState();
	}

	public Task<bool> GetLightBarStateAsync()
	{
		return Channel.GetLightBarStateAsync();
	}

	public void SetAlexaAnimationSettings(AlexaAnimationSettings Settings)
	{
		Channel.SetAlexaAnimationSettings(Settings);
	}

	public Task SetAlexaAnimationSettingsAsync(AlexaAnimationSettings Settings)
	{
		return Channel.SetAlexaAnimationSettingsAsync(Settings);
	}

	public AlexaAnimationSettings GetAlexaAnimationSettings()
	{
		return Channel.GetAlexaAnimationSettings();
	}

	public Task<AlexaAnimationSettings> GetAlexaAnimationSettingsAsync()
	{
		return Channel.GetAlexaAnimationSettingsAsync();
	}

	public bool CheckConnectionHealth()
	{
		return Channel.CheckConnectionHealth();
	}

	public Task<bool> CheckConnectionHealthAsync()
	{
		return Channel.CheckConnectionHealthAsync();
	}
}
