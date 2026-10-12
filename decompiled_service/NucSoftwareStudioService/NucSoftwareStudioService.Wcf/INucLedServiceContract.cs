using System.ServiceModel;

namespace NucSoftwareStudioService.Wcf;

[ServiceContract]
public interface INucLedServiceContract
{
	[OperationContract]
	byte[] LedCommand(string CommandName, byte[] Parameters);

	[OperationContract]
	bool CheckConnectionHealth();
}
