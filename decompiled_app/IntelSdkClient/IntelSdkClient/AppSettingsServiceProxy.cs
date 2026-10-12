using System.ServiceModel;
using IntelSdkClient.AppSettingsService;

namespace IntelSdkClient;

public class AppSettingsServiceProxy : ClientBase<IAppSettingsServiceContract>
{
	public Feature[] GetSupportedFeatures()
	{
		return Channel.GetSupportedFeatures();
	}
}
