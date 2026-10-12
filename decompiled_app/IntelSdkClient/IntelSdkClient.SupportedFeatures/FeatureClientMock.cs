using System.Collections.Generic;
using System.Linq;
using System.Management;
using IntelSdkClient.AppSettingsService;

namespace IntelSdkClient.SupportedFeatures;

public static class FeatureClientMock
{
	private static List<Feature> GeneralNuc()
	{
		List<Feature> list = new List<Feature>();
		list.Add(new Feature
		{
			Type = FeatureType.ChassisLed,
			SupportedInterfaces = new FeatureInterfaceType[1] { FeatureInterfaceType.Nss },
			PriorityLevel = 2,
			Index = 2
		});
		return list;
	}

	private static List<Feature> MonsterCove()
	{
		return new List<Feature>();
	}

	private static List<Feature> BishopCounty()
	{
		List<Feature> list = new List<Feature>();
		list.Add(new Feature
		{
			Type = FeatureType.LightbarMultiColor,
			SupportedInterfaces = new FeatureInterfaceType[2]
			{
				FeatureInterfaceType.Alexa,
				FeatureInterfaceType.Usb
			},
			PriorityLevel = 1,
			Index = 1
		});
		list.Add(new Feature
		{
			Type = FeatureType.SystemMonitor,
			SupportedInterfaces = new FeatureInterfaceType[1] { FeatureInterfaceType.Nss },
			PriorityLevel = 1,
			Index = 2
		});
		list.Add(new Feature
		{
			Type = FeatureType.UserPresence,
			SupportedInterfaces = new FeatureInterfaceType[1] { FeatureInterfaceType.Nss },
			PriorityLevel = 1,
			Index = 0
		});
		return list;
	}

	private static List<Feature> CamdenCounty()
	{
		List<Feature> list = new List<Feature>();
		list.Add(new Feature
		{
			Type = FeatureType.LightbarMultiColor,
			SupportedInterfaces = new FeatureInterfaceType[2]
			{
				FeatureInterfaceType.Alexa,
				FeatureInterfaceType.Wmi
			},
			PriorityLevel = 1,
			Index = 1
		});
		list.Add(new Feature
		{
			Type = FeatureType.SystemMonitor,
			SupportedInterfaces = new FeatureInterfaceType[1] { FeatureInterfaceType.Nss },
			PriorityLevel = 1,
			Index = 2
		});
		list.Add(new Feature
		{
			Type = FeatureType.UserPresence,
			SupportedInterfaces = new FeatureInterfaceType[1] { FeatureInterfaceType.Nss },
			PriorityLevel = 1,
			Index = 0
		});
		return list;
	}

	public static List<Feature> GetSupportedFeatures()
	{
		string text = null;
		try
		{
			using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MS_SystemInformation");
			foreach (ManagementObject item in managementObjectSearcher.Get().OfType<ManagementObject>())
			{
				text = item["SystemProductName"].ToString();
			}
		}
		catch
		{
			return GeneralNuc();
		}
		if (text == null)
		{
			return GeneralNuc();
		}
		if (text.Contains("NUC"))
		{
			return GeneralNuc();
		}
		if (text.Contains("BC"))
		{
			return BishopCounty();
		}
		if (text.Contains("CC"))
		{
			return CamdenCounty();
		}
		if (text.Contains("MC"))
		{
			return MonsterCove();
		}
		return GeneralNuc();
	}
}
