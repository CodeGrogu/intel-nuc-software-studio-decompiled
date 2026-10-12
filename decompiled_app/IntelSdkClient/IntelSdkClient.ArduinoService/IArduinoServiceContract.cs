using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntelSdkClient.ArduinoService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "ArduinoService.IArduinoServiceContract")]
public interface IArduinoServiceContract
{
	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/ArdCommand", ReplyAction = "http://tempuri.org/IArduinoServiceContract/ArdCommandResponse")]
	void ArdCommand(string CommandName);

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/ArdCommand", ReplyAction = "http://tempuri.org/IArduinoServiceContract/ArdCommandResponse")]
	Task ArdCommandAsync(string CommandName);

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/GetCurrentPatterns", ReplyAction = "http://tempuri.org/IArduinoServiceContract/GetCurrentPatternsResponse")]
	LedStatus GetCurrentPatterns();

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/GetCurrentPatterns", ReplyAction = "http://tempuri.org/IArduinoServiceContract/GetCurrentPatternsResponse")]
	Task<LedStatus> GetCurrentPatternsAsync();

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/SetLedPattern", ReplyAction = "http://tempuri.org/IArduinoServiceContract/SetLedPatternResponse")]
	void SetLedPattern(LedPattern[] ledPattern);

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/SetLedPattern", ReplyAction = "http://tempuri.org/IArduinoServiceContract/SetLedPatternResponse")]
	Task SetLedPatternAsync(LedPattern[] ledPattern);

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/IArduinoServiceContract/CheckConnectionHealthResponse")]
	bool CheckConnectionHealth();

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/CheckConnectionHealth", ReplyAction = "http://tempuri.org/IArduinoServiceContract/CheckConnectionHealthResponse")]
	Task<bool> CheckConnectionHealthAsync();

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/GetControllerIdentifier", ReplyAction = "http://tempuri.org/IArduinoServiceContract/GetControllerIdentifierResponse")]
	string GetControllerIdentifier();

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/GetControllerIdentifier", ReplyAction = "http://tempuri.org/IArduinoServiceContract/GetControllerIdentifierResponse")]
	Task<string> GetControllerIdentifierAsync();

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/CheckSeriportConnection", ReplyAction = "http://tempuri.org/IArduinoServiceContract/CheckSeriportConnectionResponse")]
	string CheckSeriportConnection();

	[OperationContract(Action = "http://tempuri.org/IArduinoServiceContract/CheckSeriportConnection", ReplyAction = "http://tempuri.org/IArduinoServiceContract/CheckSeriportConnectionResponse")]
	Task<string> CheckSeriportConnectionAsync();
}
