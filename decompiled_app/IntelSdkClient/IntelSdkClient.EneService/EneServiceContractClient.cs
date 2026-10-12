using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace IntelSdkClient.EneService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class EneServiceContractClient : ClientBase<IEneServiceContract>, IEneServiceContract
{
	public EneServiceContractClient()
	{
	}

	public EneServiceContractClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public EneServiceContractClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public EneServiceContractClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public EneServiceContractClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void EneCommand(string CommandName)
	{
		Channel.EneCommand(CommandName);
	}

	public Task EneCommandAsync(string CommandName)
	{
		return Channel.EneCommandAsync(CommandName);
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

	public byte[] LedConnectionStatus()
	{
		return Channel.LedConnectionStatus();
	}

	public Task<byte[]> LedConnectionStatusAsync()
	{
		return Channel.LedConnectionStatusAsync();
	}
}
