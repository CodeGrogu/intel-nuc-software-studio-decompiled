using System.CodeDom.Compiler;
using System.ServiceModel;

namespace IntelSdkClient.SystemMonitorService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface ISystemMonitorServiceContractCallback
{
	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/ISystemMonitorServiceContract/SystemMonitorDataCallback")]
	void SystemMonitorDataCallback(FanSpeed[] FanData, SystemMonitorData SystemData);
}
