using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntelSdkClient.NucLedService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "NucLedService.INucLedServiceContract")]
public interface INucLedServiceContract
{
	[OperationContract(Action = "http://tempuri.org/INucLedServiceContract/LedCommand", ReplyAction = "http://tempuri.org/INucLedServiceContract/LedCommandResponse")]
	byte[] LedCommand(string CommandName, byte[] Parameters);

	[OperationContract(Action = "http://tempuri.org/INucLedServiceContract/LedCommand", ReplyAction = "http://tempuri.org/INucLedServiceContract/LedCommandResponse")]
	Task<byte[]> LedCommandAsync(string CommandName, byte[] Parameters);

	[OperationContract(Action = "http://tempuri.org/INucLedServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/INucLedServiceContract/CheckConnectionHealthResponse")]
	bool CheckConnectionHealth();

	[OperationContract(Action = "http://tempuri.org/INucLedServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/INucLedServiceContract/CheckConnectionHealthResponse")]
	Task<bool> CheckConnectionHealthAsync();
}
