using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace NucSoftwareStudio.AppSettingsService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class AppSettingsServiceContractClient : ClientBase<IAppSettingsServiceContract>, IAppSettingsServiceContract
{
	public AppSettingsServiceContractClient()
	{
	}

	public AppSettingsServiceContractClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AppSettingsServiceContractClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AppSettingsServiceContractClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AppSettingsServiceContractClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public Feature[] GetSupportedFeatures()
	{
		return Channel.GetSupportedFeatures();
	}

	public Task<Feature[]> GetSupportedFeaturesAsync()
	{
		return Channel.GetSupportedFeaturesAsync();
	}
}
