using CustomizationMgr.Interfaces;
using FeatureMgr.Interfaces;
using FeatureMgr.Models;
using FeatureMgr.Views;
using Infrastructure.Events;
using Infrastructure.Models;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.Models;

namespace FeatureMgr;

public class FeatureMgrModule : IModule
{
	public void OnInitialized(IContainerProvider containerProvider)
	{
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeatureProfileTypeEvent>().Publish(typeof(FeatureSettings));
		containerProvider.Resolve<IRegionManager>().Regions["MainContentRegion"].RequestNavigate("ProfilesView");
		containerProvider.Resolve<ICustomResourceManager>()?.LoadComponentStrings("FeatureMgr");
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterSingleton<IFeatureManager, FeatureManager>();
		containerRegistry.RegisterSingleton<IUniwillClient, UniwillClient>();
		containerRegistry.RegisterSingleton<ISdkManager, SdkManager>();
		containerRegistry.RegisterForNavigation(typeof(DetailView), "DetailView");
		containerRegistry.RegisterForNavigation(typeof(ProfilesView), "ProfilesView");
	}
}
