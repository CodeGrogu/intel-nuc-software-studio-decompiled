using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace SystemMonitorLibrary.Hardware.SystemInformation;

internal class SystemInformation
{
	public Dictionary<string, string> GetSystemInformation()
	{
		Dictionary<string, string> objDictionary = new Dictionary<string, string>();
		using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MS_SystemInformation");
		foreach (ManagementObject item in managementObjectSearcher.Get().OfType<ManagementObject>())
		{
			Helper.DictionaryCheckAssign(ref objDictionary, "Manufacturer", item["SystemManufacturer"].ToString());
			Helper.DictionaryCheckAssign(ref objDictionary, "ProductName", item["SystemProductName"].ToString());
			Helper.DictionaryCheckAssign(ref objDictionary, "BiosVersion", item["BIOSVersion"].ToString());
			Helper.DictionaryCheckAssign(ref objDictionary, "ECFirmwareVersion", item["ECFirmwareMajorRelease"].ToString() + "." + item["ECFirmwareMinorRelease"].ToString());
			Helper.DictionaryCheckAssign(ref objDictionary, "SystemSKU", item["SystemSKU"].ToString());
			Helper.DictionaryCheckAssign(ref objDictionary, "SystemVersion", item["SystemVersion"].ToString());
			Helper.DictionaryCheckAssign(ref objDictionary, "BaseBoardVersion", item["BaseBoardVersion"].ToString());
		}
		return objDictionary;
	}
}
