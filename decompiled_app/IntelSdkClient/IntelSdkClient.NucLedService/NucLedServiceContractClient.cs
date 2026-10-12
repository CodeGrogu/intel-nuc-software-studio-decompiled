using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace IntelSdkClient.NucLedService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class NucLedServiceContractClient : ClientBase<INucLedServiceContract>, INucLedServiceContract
{
	public NucLedServiceContractClient()
	{
	}

	public NucLedServiceContractClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public NucLedServiceContractClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public NucLedServiceContractClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public NucLedServiceContractClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public byte[] LedCommand(string CommandName, byte[] Parameters)
	{
		return Channel.LedCommand(CommandName, Parameters);
	}

	public Task<byte[]> LedCommandAsync(string CommandName, byte[] Parameters)
	{
		return Channel.LedCommandAsync(CommandName, Parameters);
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
