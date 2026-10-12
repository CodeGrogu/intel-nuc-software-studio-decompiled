using System.Collections.Generic;
using AlexaLightBarMgr.Interfaces;
using AlexaLightBarMgr.Storage.Managers;
using AlexaLightBarMgr.Views;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models.FeatureMgr;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AlexaLightBarMgr;

public class AlexaLightBarMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		containerProvider.Resolve<ICustomResourceManager>()?.LoadComponentStrings("AlexaLightBarMgr");
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		_containerRegistry = containerRegistry;
	}

	private void LoadRegions(List<Feature> features)
	{
		foreach (Feature feature in features)
		{
			if (feature.Type == FeatureType.LightbarMultiColor && feature.SupportedInterfaces.Contains(FeatureInterfaceType.Alexa))
			{
				_containerRegistry.RegisterSingleton<IAlexaSettingsFileManager, AlexaSettingsFileManager>();
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(AlexaLightbarView));
			}
		}
	}
}
