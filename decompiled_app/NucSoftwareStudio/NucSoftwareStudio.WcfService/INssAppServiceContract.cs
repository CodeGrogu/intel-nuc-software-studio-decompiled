using System.ServiceModel;

namespace NucSoftwareStudio.WcfService;

[ServiceContract]
public interface INssAppServiceContract
{
	[OperationContract]
	void ShowMainWindow();
}
