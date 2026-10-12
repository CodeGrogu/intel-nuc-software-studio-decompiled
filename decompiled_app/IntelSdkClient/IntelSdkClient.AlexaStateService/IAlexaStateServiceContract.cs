using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntelSdkClient.AlexaStateService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "AlexaStateService.IAlexaStateServiceContract")]
public interface IAlexaStateServiceContract
{
	[OperationContract(Action = "http://tempuri.org/IAlexaStateServiceContract/SetAlexaState", ReplyAction = "http://tempuri.org/IAlexaStateServiceContract/SetAlexaStateResponse")]
	void SetAlexaState(AlexaState newState);

	[OperationContract(Action = "http://tempuri.org/IAlexaStateServiceContract/SetAlexaState", ReplyAction = "http://tempuri.org/IAlexaStateServiceContract/SetAlexaStateResponse")]
	Task SetAlexaStateAsync(AlexaState newState);

	[OperationContract(Action = "http://tempuri.org/IAlexaStateServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/IAlexaStateServiceContract/CheckConnectionHealthResponse")]
	bool CheckConnectionHealth();

	[OperationContract(Action = "http://tempuri.org/IAlexaStateServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/IAlexaStateServiceContract/CheckConnectionHealthResponse")]
	Task<bool> CheckConnectionHealthAsync();
}
