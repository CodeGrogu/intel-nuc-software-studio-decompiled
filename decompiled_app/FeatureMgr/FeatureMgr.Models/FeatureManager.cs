using System;
using System.Collections.Generic;
using System.Linq;
using FeatureMgr.Enums;
using FeatureMgr.Events;
using FeatureMgr.Interfaces;
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using Infrastructure.Models.FeatureMgr;
using Infrastructure.Utilities;
using Prism.Events;
using Prism.Modularity;

namespace FeatureMgr.Models;

public class FeatureManager : IFeatureManager
{
	private List<Feature> _supported = new List<Feature>();

	private readonly ISdkManager _sdkManager;

	private readonly IEventAggregator _events;

	private readonly IModuleManager _moduleManager;

	private NucType _nucType;

	public NucType NucType
	{
		get
		{
			return _nucType;
		}
		set
		{
			_nucType = value;
		}
	}

	public FeatureManager(IEventAggregator events, IModuleManager moduleManager, ISdkManager sdkManager)
	{
		_sdkManager = sdkManager;
		_events = events;
		_moduleManager = moduleManager;
		NucType = PlatformDetail.GetNucType();
	}

	public void LoadFeatures()
	{
		GetSupportedFeatures();
		if (_supported == null || _supported.Count == 0)
		{
			ErrorHandler.PublishError("FeaturesNotFound", ErrorType.FEATURES_NOT_FOUND, critical: true, msgIsKey: true);
			_events.GetEvent<ApplicationCloseEvent>().Publish(payload: false);
		}
		else
		{
			_events.GetEvent<ContentRegionsEvent>().Publish(GetContentRegions());
			LoadFeatureModules();
		}
	}

	private List<Tuple<int, List<string>>> GetContentRegions()
	{
		List<Tuple<int, List<string>>> list = new List<Tuple<int, List<string>>>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		List<Feature> list5 = (from f in _supported
			orderby f.PriorityLevel, f.Index
			select f).ToList();
		for (int num = 0; num < list5.Count; num++)
		{
			if (num <= 0 || list5[num].PriorityLevel != list5[num - 1].PriorityLevel || list5[num].Index != list5[num - 1].Index)
			{
				switch (list5[num].PriorityLevel)
				{
				case 0:
					list2.Add("ContentRegion" + list5[num].PriorityLevel + list5[num].Index);
					break;
				case 1:
					list3.Add("ContentRegion" + list5[num].PriorityLevel + list5[num].Index);
					break;
				case 2:
					list4.Add("ContentRegion" + list5[num].PriorityLevel + list5[num].Index);
					break;
				}
			}
		}
		if (list2.Count + list3.Count + list4.Count < 4 && list4.Count == 1)
		{
			list3.Add("ContentRegion" + 1 + list3.Count);
			for (int num2 = 0; num2 < list5.Count; num2++)
			{
				if (list5[num2].PriorityLevel == 2)
				{
					list5[num2].PriorityLevel = 1;
					list5[num2].Index = list3.Count - 1;
				}
			}
			list4.Clear();
		}
		list.Add(new Tuple<int, List<string>>(0, list2));
		list.Add(new Tuple<int, List<string>>(1, list3));
		list.Add(new Tuple<int, List<string>>(2, list4));
		return list;
	}

	private void GetSupportedFeatures()
	{
		_supported = _sdkManager.SupportedFeatures;
		RemoveFeaturesNotYetSupportedInNss();
	}

	private void RemoveFeaturesNotYetSupportedInNss()
	{
		_supported.RemoveAll((Feature x) => x.Type == FeatureType.DisplaySettings && !x.SupportedInterfaces.Contains(FeatureInterfaceType.OdmSdk));
		_supported.RemoveAll((Feature x) => x.Type == FeatureType.NucWakeSleepSettings);
		_supported.RemoveAll((Feature x) => x.Type == FeatureType.Updates);
		if (_supported.Count <= 5)
		{
			Feature feature = _supported.Find((Feature x) => x.Type == FeatureType.SystemMonitor);
			if (feature != null && feature.PriorityLevel == 0)
			{
				feature.PriorityLevel = 2;
			}
		}
	}

	private void LoadFeatureModules()
	{
		foreach (Feature item in _supported)
		{
			switch (item.Type)
			{
			case FeatureType.UserPresence:
				if (SysInfo.CheckSysBiosCode("EB") || SysInfo.CheckSysBiosCode("BC"))
				{
					_moduleManager.LoadModule("UserPresenceMgrModule");
				}
				else if (SysInfo.CheckSysBiosCode("HB"))
				{
					_moduleManager.LoadModule("UserPresenceCstMgrModule");
				}
				else
				{
					_moduleManager.LoadModule("UserPresenceCstMgrModule");
				}
				break;
			case FeatureType.DisplaySettings:
				if (_sdkManager.Sdk != SdkType.Intel && _sdkManager.Sdk == SdkType.Uniwill)
				{
					_moduleManager.LoadModule("MobileDisplayModeMgrModule");
				}
				break;
			case FeatureType.KeyWinLock:
			case FeatureType.KeyFnLock:
			case FeatureType.KeyNumPad:
			case FeatureType.KeyOSD:
				_moduleManager.LoadModule("MobileKeyboardSettingsMgrModule");
				_moduleManager.LoadModule("MobileBatterySettingsMgrModule");
				break;
			case FeatureType.KeyboardLedIndividualKeyControl:
				_moduleManager.LoadModule("MobileKeyboardLedMgrModule");
				break;
			case FeatureType.LightbarSingleColor:
				_moduleManager.LoadModule("MobileLightBarLedMgrModule");
				break;
			case FeatureType.CpuPowerMode:
			case FeatureType.dGpuPowerMode:
			case FeatureType.FanPowerMode:
				if (_sdkManager.Sdk == SdkType.Intel)
				{
					_moduleManager.LoadModule("SystemPerformanceMgrModule");
				}
				else if (_sdkManager.Sdk == SdkType.Uniwill)
				{
					_moduleManager.LoadModule("MobilePerformanceMgrModule");
				}
				break;
			case FeatureType.SystemMonitor:
				_moduleManager.LoadModule("SystemMonitorMgrModule");
				break;
			case FeatureType.LightbarMultiColor:
				if (_sdkManager.Sdk == SdkType.Intel && NucType != NucType.Serpent)
				{
					_moduleManager.LoadModule("AlexaLightBarMgrModule");
				}
				break;
			case FeatureType.ChassisLed:
				_moduleManager.LoadModule("NucLedMgrModule");
				break;
			}
		}
		_events.GetEvent<FeaturesEvent>().Publish(_supported);
	}

	public List<Feature> GetFeatureList()
	{
		return _supported;
	}
}
