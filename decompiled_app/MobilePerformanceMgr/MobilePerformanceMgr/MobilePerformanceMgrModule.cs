using System;
using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using MobilePerformanceMgr.Models;
using MobilePerformanceMgr.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr;

public class MobilePerformanceMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	private const string STYLE_PATH = "Styles/MobilePerformanceStyles.xaml";

	private static readonly string RESOURCE_DICTIONARY_PATH = AssemblyNames.ApplicationPackFromAssemblyName("MobilePerformanceMgr") + "Styles/MobilePerformanceStyles.xaml";

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStrings("MobilePerformanceMgr");
		customResourceManager?.LoadComponentStyleDictionary(RESOURCE_DICTIONARY_PATH);
		customResourceManager?.AddKnownCustomSettingType(typeof(MobilePerformanceTuningCustomSettings), MobilePerformanceTuningCustomSettings.GetDefaultSettingsKingCounty());
		customResourceManager?.AddKnownCustomSettingTypes(new List<Type>
		{
			typeof(PerformanceProfile),
			typeof(PerformanceMode),
			typeof(ProfileIndex)
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
			Infrastructure.Models.FeatureMgr.FeatureType type = feature.Type;
			if ((uint)(type - 6) <= 2u && feature.SupportedInterfaces.Contains(Infrastructure.Models.FeatureMgr.FeatureInterfaceType.OdmSdk))
			{
				_containerRegistry.RegisterSingleton<IMobilePerformanceSettingsMgr, MobilePerformanceSettingsMgr>();
				_containerRegistry.RegisterForNavigation(typeof(MobilePerformanceView), "MobilePerformanceView");
				_containerRegistry.RegisterForNavigation(typeof(MobilePerformanceSummaryView), "MobilePerformanceSummaryView");
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(MobilePerformanceSummaryView));
				break;
			}
		}
	}
}
