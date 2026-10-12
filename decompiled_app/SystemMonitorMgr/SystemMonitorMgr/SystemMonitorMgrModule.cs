using System.Collections.Generic;
using SystemMonitorMgr.Interfaces;
using SystemMonitorMgr.Models;
using SystemMonitorMgr.Views;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace SystemMonitorMgr;

public class SystemMonitorMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	private IContainerProvider _containerProvider;

	private const string STYLE_PATH = "Style/SystemMonitorStyle.xaml";

	private static readonly string RESOURCE_DICTIONARY_PATH = AssemblyNames.ApplicationPackFromAssemblyName("SystemMonitorMgr") + "Style/SystemMonitorStyle.xaml";

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_containerProvider = containerProvider;
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStyleDictionary(RESOURCE_DICTIONARY_PATH);
		customResourceManager?.LoadComponentStrings("SystemMonitorMgr");
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		_containerRegistry = containerRegistry;
	}

	private void LoadRegions(List<Feature> features)
	{
		foreach (Feature feature in features)
		{
			if (feature.Type == FeatureType.SystemMonitor)
			{
				if (feature.SupportedInterfaces.Contains(FeatureInterfaceType.Nss))
				{
					_containerRegistry.RegisterSingleton<ISystemMonitor, GenericSystemMonitor>();
					_containerProvider.Resolve<ISystemMonitor>();
				}
				else if (feature.SupportedInterfaces.Contains(FeatureInterfaceType.OdmSdk))
				{
					_containerRegistry.RegisterSingleton<ISystemMonitor, UniwillSystemMonitor>();
					_containerProvider.Resolve<ISystemMonitor>();
				}
				_regionMgr.RegisterViewWithRegion("CpuUsageRegion", typeof(CpuUsageGaugeView));
				_regionMgr.RegisterViewWithRegion("DgpuUsageRegion", typeof(dGpuUsageGaugeView));
				_regionMgr.RegisterViewWithRegion("RightCpuUsageRegion", typeof(RightSideCpuUsageGaugeView));
				_regionMgr.RegisterViewWithRegion("RightDgpuUsageRegion", typeof(RightSidedGpuUsageGaugeView));
				_regionMgr.RegisterViewWithRegion("FullFanRegion", typeof(FanFullView));
				_regionMgr.RegisterViewWithRegion("NucPerformanceSummaryFanView", typeof(NucPerformanceSummaryFanView));
				_regionMgr.RegisterViewWithRegion("RpmFanRegion", typeof(FanRpmsView));
				_regionMgr.RegisterViewWithRegion("IconFanRegion", typeof(FanIconsView));
				_regionMgr.RegisterViewWithRegion("PerformanceTuningFanRegion", typeof(PerformanceTuningFanIconsView));
				_regionMgr.RegisterViewWithRegion("HorizontalFanRegion", typeof(HorizontalFanIconsView));
				_regionMgr.RegisterViewWithRegion("PerformanceTuningCpuRegion", typeof(PerformanceCpuGaugeView));
				_regionMgr.RegisterViewWithRegion("PerformanceTuningGpuRegion", typeof(PerformanceGpuGaugeView));
				_regionMgr.RegisterViewWithRegion("PerformanceTuningFanView", typeof(PerformanceTuningFanView));
				_regionMgr.RegisterViewWithRegion("PerformanceTuningFanPHView", typeof(PerformanceTuningFanPHView));
				_regionMgr.RegisterViewWithRegion("PerformanceTuningFanRCView", typeof(PerformanceTuningFanRCView));
				_regionMgr.RegisterViewWithRegion("PerformanceTunningFanRNView", typeof(PerformanceTuningFanRNView));
				if (feature.PriorityLevel != 0)
				{
					_containerRegistry.RegisterForNavigation(typeof(SystemMonitorSummaryView), "SystemMonitorSummaryView");
					_containerRegistry.RegisterForNavigation(typeof(SystemMonitorCardView), "SystemMonitorCardView");
					_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(SystemMonitorSummaryView));
				}
				else
				{
					_containerRegistry.RegisterForNavigation(typeof(SystemMonitorButtonView), "SystemMonitorButtonView");
					_containerRegistry.RegisterForNavigation(typeof(SystemMonitorView), "SystemMonitorView");
					_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(SystemMonitorButtonView));
				}
			}
		}
	}
}
