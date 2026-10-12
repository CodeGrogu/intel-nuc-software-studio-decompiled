using System.ServiceModel;

namespace NucSoftwareStudioService.Wcf;

[ServiceContract(CallbackContract = typeof(ISystemMonitorCallbackContract))]
public interface ISystemMonitorServiceContract
{
	[OperationContract(IsOneWay = true)]
	void Subscribe();
}
