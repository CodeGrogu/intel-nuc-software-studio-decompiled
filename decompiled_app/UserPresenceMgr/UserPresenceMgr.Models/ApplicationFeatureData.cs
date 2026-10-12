using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ContextSensingClient;
using Prism.Events;
using UserPresenceMgr.Events;

namespace UserPresenceMgr.Models;

public static class ApplicationFeatureData
{
	private const FeatureType TYPE = FeatureType.APPLICATION;

	private static readonly string _appName = Assembly.GetEntryAssembly().GetName().Name;

	private static readonly List<string> _enabledEvents = new List<string> { "ExternalMonitor", "WalkAwayLock", "WakeOnApproach", "ProximityDistance" };

	public static void ApplyAppSettings(IEventAggregator events, AppFeatures features)
	{
		if (features.AppName != _appName)
		{
			SetOptionData payload = new SetOptionData(FeatureType.APPLICATION, FeatureProperty.AppName, _appName);
			events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload);
		}
		List<string> list = features.EnabledEvents.Except(_enabledEvents).ToList();
		List<string> list2 = _enabledEvents.Except(features.EnabledEvents).ToList();
		if (list.Count > 0 || list2.Count > 0)
		{
			SetOptionData payload2 = new SetOptionData(FeatureType.APPLICATION, FeatureProperty.EnabledEvents, _enabledEvents);
			events.GetEvent<UserPresenceUpdateSettingEvent>().Publish(payload2);
		}
	}
}
