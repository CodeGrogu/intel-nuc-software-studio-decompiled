using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace NucSoftwareStudio.AlexaStateService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class AlexaStateServiceContractClient : ClientBase<IAlexaStateServiceContract>, IAlexaStateServiceContract
{
	public AlexaStateServiceContractClient()
	{
	}

	public AlexaStateServiceContractClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AlexaStateServiceContractClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlexaStateServiceContractClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlexaStateServiceContractClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void SetAlexaState(AlexaState newState)
	{
		Channel.SetAlexaState(newState);
	}

	public Task SetAlexaStateAsync(AlexaState newState)
	{
		return Channel.SetAlexaStateAsync(newState);
	}
}
