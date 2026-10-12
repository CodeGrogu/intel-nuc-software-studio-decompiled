using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntelSdkClient.EneService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "EneService.IEneServiceContract")]
public interface IEneServiceContract
{
	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/EneCommand", ReplyAction = "http://tempuri.org/IEneServiceContract/EneCommandResponse")]
	void EneCommand(string CommandName);

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/EneCommand", ReplyAction = "http://tempuri.org/IEneServiceContract/EneCommandResponse")]
	Task EneCommandAsync(string CommandName);

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/GetCurrentPatterns", ReplyAction = "http://tempuri.org/IEneServiceContract/GetCurrentPatternsResponse")]
	LedStatus GetCurrentPatterns();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/GetCurrentPatterns", ReplyAction = "http://tempuri.org/IEneServiceContract/GetCurrentPatternsResponse")]
	Task<LedStatus> GetCurrentPatternsAsync();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/SetLedPattern", ReplyAction = "http://tempuri.org/IEneServiceContract/SetLedPatternResponse")]
	void SetLedPattern(LedPattern[] ledPattern);

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/SetLedPattern", ReplyAction = "http://tempuri.org/IEneServiceContract/SetLedPatternResponse")]
	Task SetLedPatternAsync(LedPattern[] ledPattern);

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/IEneServiceContract/CheckConnectionHealthResponse")]
	bool CheckConnectionHealth();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/IEneServiceContract/CheckConnectionHealthResponse")]
	Task<bool> CheckConnectionHealthAsync();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/GetControllerIdentifier", ReplyAction = "http://tempuri.org/IEneServiceContract/GetControllerIdentifierResponse")]
	string GetControllerIdentifier();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/GetControllerIdentifier", ReplyAction = "http://tempuri.org/IEneServiceContract/GetControllerIdentifierResponse")]
	Task<string> GetControllerIdentifierAsync();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/CheckSeriportConnection", ReplyAction = "http://tempuri.org/IEneServiceContract/CheckSeriportConnectionResponse")]
	string CheckSeriportConnection();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/CheckSeriportConnection", ReplyAction = "http://tempuri.org/IEneServiceContract/CheckSeriportConnectionResponse")]
	Task<string> CheckSeriportConnectionAsync();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/LedConnectionStatus", ReplyAction = "http://tempuri.org/IEneServiceContract/LedConnectionStatusResponse")]
	byte[] LedConnectionStatus();

	[OperationContract(Action = "http://tempuri.org/IEneServiceContract/LedConnectionStatus", ReplyAction = "http://tempuri.org/IEneServiceContract/LedConnectionStatusResponse")]
	Task<byte[]> LedConnectionStatusAsync();
}
