using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using OpenHardwareMonitor.Hardware;
using PerformanceTuningLibrary;

namespace SystemMonitorLibrary.Hardware.Processor;

internal class Processor
{
	private readonly Computer _computer;

	public Processor(Computer computer)
	{
		_computer = computer;
	}

	public Dictionary<string, string> GetProcessorInformation()
	{
		Dictionary<string, string> objDictionary = new Dictionary<string, string>();
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
		{
			foreach (ManagementObject item in managementObjectSearcher.Get().OfType<ManagementObject>())
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "Name", (string)item["Name"]);
			}
		}
		foreach (IHardware item2 in _computer.Hardware.Where((IHardware hardware) => hardware.HardwareType == HardwareType.CPU))
		{
			item2.Update();
			List<float?> list = new List<float?>();
			List<float?> list2 = new List<float?>();
			List<float?> list3 = new List<float?>();
			ISensor[] sensors = item2.Sensors;
			foreach (ISensor sensor in sensors)
			{
				if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "CurrentUsage", Math.Round(sensor.Value.Value, 2).ToString(CultureInfo.InvariantCulture));
				}
				else if (sensor.SensorType == SensorType.Clock && sensor.Name.Contains("CPU Core"))
				{
					list.Add(sensor.Value);
					list2.Add(sensor.Max);
				}
				else if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("CPU Core"))
				{
					list3.Add(sensor.Value);
				}
			}
			if (list.Count > 0)
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "CurrentClockSpeed", Math.Round(list.Average().Value, 2).ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "CurrentClockSpeed", string.Empty);
			}
			if (list2.Count > 0)
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "MaxClockSpeed", Math.Round(list2.Average().Value, 2).ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "MaxClockSpeed", string.Empty);
			}
			if (list3.Count > 0)
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "CurrentTemperature", Math.Round(list3.Average().Value, 2).ToString(CultureInfo.InvariantCulture));
			}
			else if (FanControl.CurrentEcVersion == ECVersion.ECNUC22 || FanControl.CurrentEcVersion == ECVersion.ECNUC23)
			{
				int cPUTemprature = FanControl.GetCPUTemprature();
				if (ReadWriteControl.isDataFound)
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "CurrentTemperature", cPUTemprature.ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "CurrentTemperature", string.Empty);
				}
			}
			else
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "CurrentTemperature", string.Empty);
			}
		}
		return objDictionary;
	}
}
