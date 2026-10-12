using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace SystemMonitorLibrary.Hardware.Battery;

internal class Battery
{
	public Dictionary<string, string> GetBatteryInformation()
	{
		Dictionary<string, string> objDictionary = new Dictionary<string, string>();
		bool flag = false;
		ushort num = 0;
		uint num2 = 0u;
		bool flag2 = false;
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery"))
		{
			foreach (ManagementObject item in managementObjectSearcher.Get().OfType<ManagementObject>())
			{
				foreach (PropertyData property in item.Properties)
				{
					switch (property.Name)
					{
					case "BatteryStatus":
						flag = IsBatteryPluggedIn((ushort)property.Value);
						flag2 = true;
						break;
					case "EstimatedChargeRemaining":
						num = (ushort)property.Value;
						flag2 = true;
						break;
					case "EstimatedRunTime":
						num2 = (uint)property.Value;
						flag2 = true;
						break;
					}
				}
			}
		}
		if (flag2)
		{
			if (flag)
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "EstimatedRunTime", "Plugged in");
			}
			else
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "EstimatedRunTime", num2 + " minutes");
			}
			Helper.DictionaryCheckAssign(ref objDictionary, "EstimatedChargeRemaining", num.ToString());
			Helper.DictionaryCheckAssign(ref objDictionary, "IsBatterySupported", flag2.ToString());
		}
		else
		{
			Helper.DictionaryCheckAssign(ref objDictionary, "IsBatterySupported", flag2.ToString());
		}
		return objDictionary;
	}

	private bool IsBatteryPluggedIn(ushort Status)
	{
		switch (Status)
		{
		case 1:
		case 10:
		case 11:
			return false;
		case 2:
		case 3:
		case 6:
		case 7:
		case 8:
		case 9:
			return true;
		default:
			return false;
		}
	}
}
