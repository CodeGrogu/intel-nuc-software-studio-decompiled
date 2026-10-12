using System.Collections.Generic;
using SystemPerformanceMgr.Interfaces;
using SystemPerformanceMgr.Models;
using SystemPerformanceMgr.RaptorSystemPerformanceMgr.Interfaces;
using SystemPerformanceMgr.RaptorSystemPerformanceMgr.Models;
using SystemPerformanceMgr.RaptorSystemPerformanceMgr.Views;
using SystemPerformanceMgr.Views;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using Infrastructure.Events;
using Infrastructure.Models.FeatureMgr;
using IntelSdkClient.PerformanceTuning;
using IntelSdkClient.PerformanceTuning.Interface;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace SystemPerformanceMgr;

public class SystemPerformanceMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	private NucType nucType;

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		containerProvider.Resolve<ICustomResourceManager>()?.LoadComponentStrings("SystemPerformanceMgr");
		nucType = PlatformDetail.GetNucType();
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
			FeatureType type = feature.Type;
			if ((uint)(type - 6) <= 2u && feature.SupportedInterfaces.Contains(FeatureInterfaceType.Nss))
			{
				if (nucType == NucType.Raptor)
				{
					_containerRegistry.RegisterForNavigation(typeof(RNPerformanceSummaryView), "PerformanceSummaryView");
					_containerRegistry.RegisterForNavigation(typeof(RNPerformanceView), "PerformanceView");
					_containerRegistry.RegisterSingleton<IRNPerformanceModeManager, RNPerformanceModeManager>();
					_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(RNPerformanceSummaryView));
				}
				else
				{
					_containerRegistry.RegisterForNavigation(typeof(NucPerformanceSummaryView), "PerformanceSummaryView");
					_containerRegistry.RegisterForNavigation(typeof(NucPerformanceView), "PerformanceView");
					_containerRegistry.RegisterSingleton<INucPerformanceModeManager, NssPerformanceModeManager>();
					_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(NucPerformanceSummaryView));
				}
				_containerRegistry.RegisterSingleton<IPerformanceTuningClient, PerformanceTuningClient>();
				break;
			}
		}
	}
}
