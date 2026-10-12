using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace IntelSdkClient.ArduinoService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ArduinoServiceContractClient : ClientBase<IArduinoServiceContract>, IArduinoServiceContract
{
	public ArduinoServiceContractClient()
	{
	}

	public ArduinoServiceContractClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ArduinoServiceContractClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ArduinoServiceContractClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ArduinoServiceContractClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void ArdCommand(string CommandName)
	{
		Channel.ArdCommand(CommandName);
	}

	public Task ArdCommandAsync(string CommandName)
	{
		return Channel.ArdCommandAsync(CommandName);
	}

	public LedStatus GetCurrentPatterns()
	{
		return Channel.GetCurrentPatterns();
	}

	public Task<LedStatus> GetCurrentPatternsAsync()
	{
		return Channel.GetCurrentPatternsAsync();
	}

	public void SetLedPattern(LedPattern[] ledPattern)
	{
		Channel.SetLedPattern(ledPattern);
	}

	public Task SetLedPatternAsync(LedPattern[] ledPattern)
	{
		return Channel.SetLedPatternAsync(ledPattern);
	}

	public bool CheckConnectionHealth()
	{
		return Channel.CheckConnectionHealth();
	}

	public Task<bool> CheckConnectionHealthAsync()
	{
		return Channel.CheckConnectionHealthAsync();
	}

	public string GetControllerIdentifier()
	{
		return Channel.GetControllerIdentifier();
	}

	public Task<string> GetControllerIdentifierAsync()
	{
		return Channel.GetControllerIdentifierAsync();
	}

	public string CheckSeriportConnection()
	{
		return Channel.CheckSeriportConnection();
	}

	public Task<string> CheckSeriportConnectionAsync()
	{
		return Channel.CheckSeriportConnectionAsync();
	}
}
