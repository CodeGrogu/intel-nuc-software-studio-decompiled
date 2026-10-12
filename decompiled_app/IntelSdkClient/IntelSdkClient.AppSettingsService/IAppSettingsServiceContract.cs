using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntelSdkClient.AppSettingsService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "AppSettingsService.IAppSettingsServiceContract")]
public interface IAppSettingsServiceContract
{
	[OperationContract(Action = "http://tempuri.org/IAppSettingsServiceContract/GetSupportedFeatures", ReplyAction = "http://tempuri.org/IAppSettingsServiceContract/GetSupportedFeaturesResponse")]
	Feature[] GetSupportedFeatures();

	[OperationContract(Action = "http://tempuri.org/IAppSettingsServiceContract/GetSupportedFeatures", ReplyAction = "http://tempuri.org/IAppSettingsServiceContract/GetSupportedFeaturesResponse")]
	Task<Feature[]> GetSupportedFeaturesAsync();
}
