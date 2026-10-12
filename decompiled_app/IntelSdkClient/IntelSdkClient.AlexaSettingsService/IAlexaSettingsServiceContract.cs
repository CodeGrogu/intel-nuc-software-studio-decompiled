using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntelSdkClient.AlexaSettingsService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "AlexaSettingsService.IAlexaSettingsServiceContract")]
public interface IAlexaSettingsServiceContract
{
	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/SetLightBarState", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/SetLightBarStateResponse")]
	void SetLightBarState(bool enable);

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/SetLightBarState", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/SetLightBarStateResponse")]
	Task SetLightBarStateAsync(bool enable);

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/GetLightBarState", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/GetLightBarStateResponse")]
	bool GetLightBarState();

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/GetLightBarState", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/GetLightBarStateResponse")]
	Task<bool> GetLightBarStateAsync();

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/SetAlexaAnimationSettings", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/SetAlexaAnimationSettingsResponse")]
	void SetAlexaAnimationSettings(AlexaAnimationSettings Settings);

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/SetAlexaAnimationSettings", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/SetAlexaAnimationSettingsResponse")]
	Task SetAlexaAnimationSettingsAsync(AlexaAnimationSettings Settings);

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/GetAlexaAnimationSettings", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/GetAlexaAnimationSettingsResponse")]
	AlexaAnimationSettings GetAlexaAnimationSettings();

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/GetAlexaAnimationSettings", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/GetAlexaAnimationSettingsResponse")]
	Task<AlexaAnimationSettings> GetAlexaAnimationSettingsAsync();

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/CheckConnectionHealthResponse")]
	bool CheckConnectionHealth();

	[OperationContract(Action = "http://tempuri.org/IAlexaSettingsServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/IAlexaSettingsServiceContract/CheckConnectionHealthResponse")]
	Task<bool> CheckConnectionHealthAsync();
}
