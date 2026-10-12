using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using IntelSdkClient.Arduino;
using IntelSdkClient.Arduino.Interface;
using IntelSdkClient.ENE;
using IntelSdkClient.ENE.Interface;
using NucLedMgr.Arduino.Interfaces;
using NucLedMgr.Arduino.Models;
using NucLedMgr.Arduino.Views;
using NucLedMgr.ENE.Interfaces;
using NucLedMgr.ENE.Models;
using NucLedMgr.ENE.Views;
using NucLedMgr.Interfaces;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models;
using NucLedMgr.WMI.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace NucLedMgr;

public class NucLedMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	private const string STYLE_PATH = "Styles/NucLedMgrStyles.xaml";

	private static readonly string RESOURCE_DICTIONARY_PATH = AssemblyNames.ApplicationPackFromAssemblyName("NucLedMgr") + "Styles/NucLedMgrStyles.xaml";

	private const string ARDUINO_LED_VIEW = "ArduinoLedView";

	private const string ARDUINO_LED_SUMMARY_VIEW = "ArduinoLedSummaryView";

	private const string NUC_LED_VIEW = "NucLedView";

	private const string WMI_LED_SUMMARY_VIEW = "WmiLedSummaryView";

	private const string ENE_LED_SUMMARY_VIEW = "EneLedSummaryView";

	public const string ENE_LED_VIEW = "EneLedView";

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStyleDictionary(RESOURCE_DICTIONARY_PATH);
		customResourceManager?.LoadComponentStrings("NucLedMgr");
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		_containerRegistry = containerRegistry;
	}

	private void LoadRegions(List<Feature> features)
	{
		NucType nucType = PlatformDetail.GetNucType();
		foreach (Feature feature in features)
		{
			if (feature.Type != FeatureType.ChassisLed)
			{
				continue;
			}
			if (feature.SupportedInterfaces.Contains(FeatureInterfaceType.Arduino))
			{
				if (nucType == NucType.Raptor)
				{
					_containerRegistry.RegisterSingleton<IEnePatternManager, EnePatternManager>();
					_containerRegistry.RegisterSingleton<IColorManager, EneColorManager>();
					_containerRegistry.RegisterSingleton<IEneClient, EneClient>();
					_containerRegistry.RegisterForNavigation(typeof(EneLedView), "EneLedView");
					_containerRegistry.RegisterForNavigation(typeof(EneLedSummaryView), "EneLedSummaryView");
					_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(EneLedSummaryView));
				}
				else
				{
					_containerRegistry.RegisterSingleton<IArduinoPatternManager, ArduinoPatternManager>();
					_containerRegistry.RegisterSingleton<IColorManager, ArduinoColorManager>();
					_containerRegistry.RegisterSingleton<IArduinoClient, ArduinoClient>();
					_containerRegistry.RegisterForNavigation(typeof(ArduinoLedView), "ArduinoLedView");
					_containerRegistry.RegisterForNavigation(typeof(ArduinoLedSummaryView), "ArduinoLedSummaryView");
					_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(ArduinoLedSummaryView));
				}
			}
			if (feature.SupportedInterfaces.Contains(FeatureInterfaceType.Wmi))
			{
				_containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();
				_containerRegistry.RegisterSingleton<IColorManager, ColorManager>();
				_containerRegistry.RegisterForNavigation(typeof(NucLedView), "NucLedView");
				_containerRegistry.RegisterForNavigation(typeof(WmiLedSummaryView), "WmiLedSummaryView");
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(WmiLedSummaryView));
			}
		}
	}
}
