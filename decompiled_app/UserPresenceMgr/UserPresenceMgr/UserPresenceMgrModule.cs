using System.Collections.Generic;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models.FeatureMgr;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UserPresenceMgr.Interfaces;
using UserPresenceMgr.Models;
using UserPresenceMgr.Views;

namespace UserPresenceMgr;

public class UserPresenceMgrModule : IModule
{
	private IRegionManager _regionMgr;

	private IContainerRegistry _containerRegistry;

	private const string USER_PRESENCE_SIMPLE_SUMMARY_VIEW = "UserPresenceSimpleSummaryView";

	public void OnInitialized(IContainerProvider containerProvider)
	{
		_regionMgr = containerProvider.Resolve<IRegionManager>();
		containerProvider.Resolve<IEventAggregator>().GetEvent<FeaturesEvent>().Subscribe(LoadRegions, keepSubscriberReferenceAlive: true);
		containerProvider.Resolve<ICustomResourceManager>()?.LoadComponentStrings("UserPresenceMgr");
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
				_containerRegistry.RegisterForNavigation(typeof(UserPresenceSimpleSummaryView), "UserPresenceSimpleSummaryView");
				_regionMgr.RegisterViewWithRegion("ContentRegion" + feature.PriorityLevel + feature.Index, typeof(UserPresenceSimpleSummaryView));
				_containerRegistry.RegisterSingleton<IUserPresenceManager, UserPresenceManager>();
				break;
			}
		}
	}
}
