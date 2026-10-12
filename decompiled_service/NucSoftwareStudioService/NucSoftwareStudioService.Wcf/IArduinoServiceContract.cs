using System.Collections.Generic;
using System.ServiceModel;
using NucSoftwareStudioService.LedControl.Models;

namespace NucSoftwareStudioService.Wcf;

[ServiceContract]
[ServiceKnownType(typeof(LedStatus[]))]
[ServiceKnownType(typeof(LedPattern[]))]
internal interface IArduinoServiceContract
{
	[OperationContract]
	void ArdCommand(string CommandName);

	[OperationContract]
	LedStatus GetCurrentPatterns();

	[OperationContract]
	void SetLedPattern(List<LedPattern> ledPattern);

	[OperationContract]
	bool CheckConnectionHealth();

	[OperationContract]
	string GetControllerIdentifier();

	[OperationContract]
	string CheckSeriportConnection();
}
