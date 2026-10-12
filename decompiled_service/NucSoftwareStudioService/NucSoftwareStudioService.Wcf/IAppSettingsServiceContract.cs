using System.Collections.Generic;
using System.ServiceModel;
using NucSoftwareStudioService.FeatureSupport.Models;

namespace NucSoftwareStudioService.Wcf;

[ServiceContract]
[ServiceKnownType(typeof(Feature[]))]
[ServiceKnownType(typeof(FeatureInterfaceType[]))]
public interface IAppSettingsServiceContract
{
	[OperationContract]
	List<Feature> GetSupportedFeatures();
}
