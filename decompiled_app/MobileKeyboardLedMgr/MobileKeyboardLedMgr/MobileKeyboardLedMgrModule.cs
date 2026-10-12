using System;
using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using MobileKeyboardLedMgr.Interfaces;
using MobileKeyboardLedMgr.Models;
using MobileKeyboardLedMgr.Storage.Data;
using MobileKeyboardLedMgr.Storage.Managers;
using MobileKeyboardLedMgr.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MobileKeyboardLedMgr;

public class MobileKeyboardLedMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	private const string STYLE_PATH = "Styles/MobileKeyboardLedStyles.xaml";

	private static readonly string RESOURCE_DICTIONARY_PATH = AssemblyNames.ApplicationPackFromAssemblyName("MobileKeyboardLedMgr");

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStyleDictionary(RESOURCE_DICTIONARY_PATH + "Styles/MobileKeyboardLedStyles.xaml");
		customResourceManager?.LoadComponentStrings("MobileKeyboardLedMgr");
		customResourceManager?.AddKnownCustomSettingType(typeof(KeyboardLedCustomSettings), KeyboardLedCustomSettings.GetDefaultSettings());
		customResourceManager?.AddKnownCustomSettingTypes(new List<Type>
		{
			typeof(Pattern),
			typeof(Key)
		});
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		_containerRegistry = containerRegistry;
	}

	private void LoadRegions(List<Feature> features)
	{
		foreach (Feature feature in features)
		{
			if (feature.Type == FeatureType.KeyboardLedIndividualKeyControl)
			{
				_containerRegistry.RegisterSingleton<IKeyboardProfileManager, KeyboardProfileManager>();
				_containerRegistry.RegisterSingleton<IKeyboardPatternManager, KeyboardPatternManager>();
				_containerRegistry.RegisterSingleton<IKeyboardColorManager, KeyboardColorManager>();
				_containerRegistry.RegisterSingleton<IKeyGroupsFileManager, KeyGroupsFileManager>();
				_containerRegistry.RegisterSingleton<ISettingsFileManager, SettingsFileManager>();
				_containerRegistry.RegisterForNavigation(typeof(KeyboardSummaryView), "KeyboardSummaryView");
				_containerRegistry.RegisterForNavigation(typeof(KeyboardView), "KeyboardView");
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(KeyboardSummaryView));
				break;
			}
		}
	}
}
