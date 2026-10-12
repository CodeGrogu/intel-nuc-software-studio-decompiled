using System.ServiceModel;
using NucSoftwareStudioService.LedControl.Models;

namespace NucSoftwareStudioService.Wcf;

[ServiceContract]
public interface IAlexaSettingsServiceContract
{
	[OperationContract]
	void SetLightBarState(bool enable);

	[OperationContract]
	bool GetLightBarState();

	[OperationContract]
	void SetAlexaAnimationSettings(AlexaAnimationSettings Settings);

	[OperationContract]
	AlexaAnimationSettings GetAlexaAnimationSettings();

	[OperationContract]
	bool CheckConnectionHealth();
}
