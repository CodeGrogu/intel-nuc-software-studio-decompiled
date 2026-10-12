using System.ServiceModel;
using Alexa.Extensions;

namespace NucSoftwareStudioService.Wcf;

[ServiceContract]
public interface IAlexaStateServiceContract
{
	[OperationContract]
	void SetAlexaState(AlexaState newState);

	[OperationContract]
	bool CheckConnectionHealth();
}
