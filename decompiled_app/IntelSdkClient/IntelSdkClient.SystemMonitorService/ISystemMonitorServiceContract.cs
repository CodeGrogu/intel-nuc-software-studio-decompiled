using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntelSdkClient.SystemMonitorService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "SystemMonitorService.ISystemMonitorServiceContract", CallbackContract = typeof(ISystemMonitorServiceContractCallback))]
public interface ISystemMonitorServiceContract
{
	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/ISystemMonitorServiceContract/Subscribe")]
	void Subscribe();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/ISystemMonitorServiceContract/Subscribe")]
	Task SubscribeAsync();
}
