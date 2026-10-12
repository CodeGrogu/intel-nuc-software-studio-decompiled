using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace IntelSdkClient.SystemMonitorService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class SystemMonitorServiceContractClient : DuplexClientBase<ISystemMonitorServiceContract>, ISystemMonitorServiceContract
{
	public SystemMonitorServiceContractClient(InstanceContext callbackInstance)
		: base(callbackInstance)
	{
	}

	public SystemMonitorServiceContractClient(InstanceContext callbackInstance, string endpointConfigurationName)
		: base(callbackInstance, endpointConfigurationName)
	{
	}

	public SystemMonitorServiceContractClient(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)
		: base(callbackInstance, endpointConfigurationName, remoteAddress)
	{
	}

	public SystemMonitorServiceContractClient(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(callbackInstance, endpointConfigurationName, remoteAddress)
	{
	}

	public SystemMonitorServiceContractClient(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
		: base(callbackInstance, binding, remoteAddress)
	{
	}

	public void Subscribe()
	{
		Channel.Subscribe();
	}

	public Task SubscribeAsync()
	{
		return Channel.SubscribeAsync();
	}
}
