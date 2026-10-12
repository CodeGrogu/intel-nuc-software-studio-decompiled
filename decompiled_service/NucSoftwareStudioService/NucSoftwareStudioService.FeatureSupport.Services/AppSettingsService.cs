using System.Collections.Generic;
using System.ServiceModel;
using Infrastructure.Logging;
using NucSoftwareStudioService.FeatureSupport.Models;
using NucSoftwareStudioService.FeatureSupport.Wmi;
using NucSoftwareStudioService.Wcf;

namespace NucSoftwareStudioService.FeatureSupport.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class AppSettingsService : IAppSettingsServiceContract
{
	public List<Feature> SupportedFeatures { get; private set; }

	public AppSettingsService(NssServiceLogger logger)
	{
		SupportedFeatures = PlatformCapabilityWmiControl.GetSupportedFeatures(logger);
	}

	public List<Feature> GetSupportedFeatures()
	{
		if (SupportedFeatures == null)
		{
			SupportedFeatures = PlatformCapabilityWmiControl.GetSupportedFeatures();
		}
		return SupportedFeatures;
	}
}
