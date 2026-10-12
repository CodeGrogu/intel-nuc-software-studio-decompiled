using System;
using System.Collections.Generic;
using IntelSdkClient.AppSettingsService;

namespace IntelSdkClient.SupportedFeatures;

public class FeatureClient
{
	public List<Feature> GetSupportedFeatures()
	{
		AppSettingsServiceProxy appSettingsServiceProxy = new AppSettingsServiceProxy();
		try
		{
			return new List<Feature>(appSettingsServiceProxy.GetSupportedFeatures());
		}
		catch (Exception)
		{
			return new List<Feature>();
		}
	}
}
