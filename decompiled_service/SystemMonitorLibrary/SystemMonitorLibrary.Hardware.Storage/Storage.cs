using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;

namespace SystemMonitorLibrary.Hardware.Storage;

internal class Storage
{
	public List<Dictionary<string, string>> GetStorageInformation()
	{
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk"))
		{
			foreach (ManagementObject item2 in managementObjectSearcher.Get().OfType<ManagementObject>())
			{
				if ((uint)item2["DriveType"] == 3)
				{
					ulong num = (ulong)item2["FreeSpace"];
					ulong num2 = (ulong)item2["Size"];
					Dictionary<string, string> item = new Dictionary<string, string>
					{
						{
							"Name",
							item2["Name"].ToString() + " " + item2["VolumeName"].ToString()
						},
						{
							"LogicalDiskName",
							item2["Name"].ToString()
						},
						{
							"LogicalDiskVolumeName",
							item2["VolumeName"].ToString()
						},
						{
							"FreeSpace",
							Math.Round((double)num / StorageConst.BYTES_TO_GIBIBYTE, 2).ToString(CultureInfo.InvariantCulture)
						},
						{
							"TotalSize",
							Math.Round((double)num2 / StorageConst.BYTES_TO_GIBIBYTE, 2).ToString(CultureInfo.InvariantCulture)
						},
						{
							"Usage",
							Math.Round(100.0 - (double)num / StorageConst.BYTES_TO_GIBIBYTE / ((double)num2 / StorageConst.BYTES_TO_GIBIBYTE) * 100.0, 2).ToString(CultureInfo.InvariantCulture)
						}
					};
					list.Add(item);
				}
			}
		}
		using ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher("\\\\.\\root\\microsoft\\windows\\storage", "SELECT * FROM MSFT_PhysicalDisk");
		foreach (ManagementObject item3 in managementObjectSearcher2.Get().OfType<ManagementObject>())
		{
			if (item3["BusType"].ToString().Equals("USB"))
			{
				continue;
			}
			foreach (Dictionary<string, string> item4 in list)
			{
				string name = Enum.GetName(typeof(StorageInterfaceEnums.MediaType), item3["MediaType"]);
				string name2 = Enum.GetName(typeof(StorageInterfaceEnums.BusType), item3["BusType"]);
				if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(name2) && !item4.ContainsKey("InterfaceType"))
				{
					item4.Add("InterfaceType", name + "-" + name2);
				}
			}
		}
		return list;
	}
}
