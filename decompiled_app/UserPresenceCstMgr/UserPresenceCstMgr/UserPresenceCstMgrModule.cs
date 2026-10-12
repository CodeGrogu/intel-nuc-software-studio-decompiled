using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models.FeatureMgr;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UserPresenceCstMgr.Interfaces;
using UserPresenceCstMgr.Models;
using UserPresenceCstMgr.Views;

namespace UserPresenceCstMgr;

public class UserPresenceCstMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	private const string USER_PRESENCE_CST_SIMPLE_SUMMARY_VIEW = "UserPresenceCstSimpleSummaryView";

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		containerProvider.Resolve<ICustomResourceManager>()?.LoadComponentStrings("UserPresenceCstMgr");
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		_containerRegistry = containerRegistry;
	}

	private void LoadRegions(List<Feature> features)
	{
		foreach (Feature feature in features)
		{
			if (feature.Type == FeatureType.UserPresence)
			{
				_containerRegistry.RegisterForNavigation(typeof(UserPresenceCstSimpleSummaryView), "UserPresenceCstSimpleSummaryView");
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(UserPresenceCstSimpleSummaryView));
				_containerRegistry.RegisterSingleton<IUserPresenceCstManager, UserPresenceCstManager>();
				break;
			}
		}
	}
}
