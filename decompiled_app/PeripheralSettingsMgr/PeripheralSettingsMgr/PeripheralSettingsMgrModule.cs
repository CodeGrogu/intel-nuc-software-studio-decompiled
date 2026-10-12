using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using PeripheralSettingsMgr.Interfaces;
using PeripheralSettingsMgr.Models;
using PeripheralSettingsMgr.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PeripheralSettingsMgr;

public class PeripheralSettingsMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	private const string STYLE_PATH = "Styles/NucDisplayModeStyles.xaml";

	private static readonly string RESOURCE_DICTIONARY_PATH = AssemblyNames.ApplicationPackFromAssemblyName("PeripheralSettingsMgr") + "Styles/NucDisplayModeStyles.xaml";

	private const string DISPLAY_MODE_SUMMARY_VIEW = "DisplayModeSummaryView";

	private const string DISPLAY_MODE_VIEW = "DisplayModeView";

	private const string SCHEDULED_SETTINGS_SUMMARY_VIEW = "ScheduledSettingsSummaryView";

	private const string SCHEDULED_SETTINGS_VIEW = "ScheduledSettingsMainView";

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStyleDictionary(RESOURCE_DICTIONARY_PATH);
		customResourceManager?.LoadComponentStrings("PeripheralSettingsMgr");
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		_containerRegistry = containerRegistry;
	}

	private void LoadRegions(List<Feature> features)
	{
		foreach (Feature feature in features)
		{
			switch (feature.Type)
			{
			case FeatureType.DisplaySettings:
				if (feature.SupportedInterfaces.Contains(FeatureInterfaceType.Nss))
				{
					_containerRegistry.RegisterSingleton<IDisplaySettingsManager, DisplaySettingsManager>();
					_containerRegistry.RegisterForNavigation(typeof(DisplayModeSummaryView), "DisplayModeSummaryView");
					_containerRegistry.RegisterForNavigation(typeof(DisplayModeView), "DisplayModeView");
					_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(DisplayModeSummaryView));
				}
				break;
			case FeatureType.NucWakeSleepSettings:
				_containerRegistry.RegisterForNavigation(typeof(ScheduledSettingsSummaryView), "ScheduledSettingsSummaryView");
				_containerRegistry.RegisterForNavigation(typeof(ScheduledSettingsMainView), "ScheduledSettingsMainView");
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(ScheduledSettingsSummaryView));
				_containerRegistry.RegisterSingleton<IScheduledSettingsManager, ScheduledSettingsManager>();
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(ScheduledSettingsMainView));
				break;
			}
		}
	}
}
