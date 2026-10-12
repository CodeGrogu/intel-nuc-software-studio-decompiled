using System;
using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using MobileDisplayModeMgr.Interfaces;
using MobileDisplayModeMgr.Models;
using MobileDisplayModeMgr.ViewModels;
using MobileDisplayModeMgr.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UniwillSdkClient.UniwillService;

namespace MobileDisplayModeMgr;

public class MobileDisplayModeMgrModule : IModule
{
	private const string STYLE_PATH = "Styles/MobileDisplayModeStyles.xaml";

	private static readonly string RESOURCE_DICTIONARY_PATH = AssemblyNames.ApplicationPackFromAssemblyName("MobileDisplayModeMgr") + "Styles/MobileDisplayModeStyles.xaml";

	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStyleDictionary(RESOURCE_DICTIONARY_PATH);
		customResourceManager?.LoadComponentStrings("MobileDisplayModeMgr");
		customResourceManager?.AddKnownCustomSettingType(typeof(MobileDisplayModeCustomSettings), MobileDisplayModeCustomSettings.GetDefaultSettingsKingCounty());
		customResourceManager?.AddKnownCustomSettingTypes(new List<Type>
		{
			typeof(DisplayMode),
			typeof(DisplayModeType)
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
			if (feature.Type == Infrastructure.Models.FeatureMgr.FeatureType.DisplaySettings)
			{
				if (feature.SupportedInterfaces.Contains(Infrastructure.Models.FeatureMgr.FeatureInterfaceType.OdmSdk))
				{
					_containerRegistry.RegisterSingleton<IMobileDisplayModeSettingsMgr, MobileDisplayModeSettingsMgr>();
					_containerRegistry.RegisterForNavigation(typeof(MobileDisplayModeView), "MobileDisplayModeView");
					_containerRegistry.RegisterForNavigation(typeof(MobileDisplayModeSummaryView), "MobileDisplayModeSummaryView");
					_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(MobileDisplayModeSummaryView));
				}
				break;
			}
		}
	}
}
