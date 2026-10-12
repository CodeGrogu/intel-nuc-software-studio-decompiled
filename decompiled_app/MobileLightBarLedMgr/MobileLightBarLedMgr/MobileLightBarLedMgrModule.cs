using System;
using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models.FeatureMgr;
using MobileLightBarLedMgr.Interfaces;
using MobileLightBarLedMgr.Models;
using MobileLightBarLedMgr.ViewModels;
using MobileLightBarLedMgr.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UniwillSdkClient.UniwillService;

namespace MobileLightBarLedMgr;

public class MobileLightBarLedMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStrings("MobileLightBarLedMgr");
		customResourceManager?.AddKnownCustomSettingType(typeof(MobileLightBarCustomSettings), MobileLightBarCustomSettings.GetDefaultSettingsKingCounty());
		customResourceManager?.AddKnownCustomSettingTypes(new List<Type>
		{
			typeof(LightbarProfile),
			typeof(LightbarSetting),
			typeof(LightBarEffect),
			typeof(Tuple<int, int>)
		});
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		_containerRegistry = containerRegistry;
	}

	private void LoadRegions(List<Infrastructure.Models.FeatureMgr.Feature> features)
	{
		foreach (Infrastructure.Models.FeatureMgr.Feature feature in features)
		{
			if (feature.Type == Infrastructure.Models.FeatureMgr.FeatureType.LightbarSingleColor)
			{
				_containerRegistry.RegisterSingleton<IMobileLightbarSettingsMgr, MobileLightbarSettingsMgr>();
				_containerRegistry.RegisterForNavigation(typeof(LightbarDetailView), "LightbarDetailView");
				_containerRegistry.RegisterForNavigation(typeof(LightbarSummaryView), "LightbarSummaryView");
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(LightbarSummaryView));
				break;
			}
		}
	}
}
