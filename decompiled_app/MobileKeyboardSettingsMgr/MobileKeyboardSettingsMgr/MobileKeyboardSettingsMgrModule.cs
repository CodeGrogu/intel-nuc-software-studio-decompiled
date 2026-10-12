using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models.FeatureMgr;
using MobileKeyboardSettingsMgr.Models;
using MobileKeyboardSettingsMgr.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MobileKeyboardSettingsMgr;

public class MobileKeyboardSettingsMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStrings("MobileKeyboardSettingsMgr");
		customResourceManager?.AddKnownCustomSettingType(typeof(MobileKeyboardCustomSettings), MobileKeyboardCustomSettings.GetDefaultSettingKingCounty());
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		_containerRegistry = containerRegistry;
	}

	private void LoadRegions(List<Feature> features)
	{
		foreach (Feature feature in features)
		{
			FeatureType type = feature.Type;
			if ((uint)(type - 11) <= 2u && feature.SupportedInterfaces.Contains(FeatureInterfaceType.OdmSdk))
			{
				_containerRegistry.RegisterForNavigation(typeof(MobileSystemSettingsView), "MobileSystemSettingsView");
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(MobileSystemSettingsView));
				break;
			}
		}
	}
}
