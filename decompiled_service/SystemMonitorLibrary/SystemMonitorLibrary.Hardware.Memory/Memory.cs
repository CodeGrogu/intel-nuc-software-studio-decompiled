using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenHardwareMonitor.Hardware;

namespace SystemMonitorLibrary.Hardware.Memory;

internal class Memory
{
	private readonly Computer _computer;

	public Memory(Computer computer)
	{
		_computer = computer;
	}

	public Dictionary<string, string> GetMemoryInformation()
	{
		Dictionary<string, string> objDictionary = new Dictionary<string, string>();
		double num = 0.0;
		double num2 = 0.0;
		foreach (IHardware item in _computer.Hardware.Where((IHardware hardware) => hardware.HardwareType == HardwareType.RAM))
		{
			item.Update();
			ISensor[] sensors = item.Sensors;
			foreach (ISensor sensor in sensors)
			{
				if (sensor.SensorType == SensorType.Load && sensor.Name == "Memory")
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Usage", Math.Round(sensor.Value.Value, 2).ToString(CultureInfo.InvariantCulture));
				}
				else if (sensor.SensorType == SensorType.Data && sensor.Name == "Available Memory")
				{
					num = Math.Round(sensor.Value.Value, 2);
					Helper.DictionaryCheckAssign(ref objDictionary, "AvailableMemory", num.ToString(CultureInfo.InvariantCulture));
				}
				else if (sensor.SensorType == SensorType.Data && sensor.Name == "Used Memory")
				{
					num2 = Math.Round(sensor.Value.Value, 2);
					Helper.DictionaryCheckAssign(ref objDictionary, "UsedMemory", num2.ToString(CultureInfo.InvariantCulture));
				}
			}
		}
		Helper.DictionaryCheckAssign(ref objDictionary, "TotalMemory", Math.Round(num + num2, 2).ToString(CultureInfo.InvariantCulture));
		return objDictionary;
	}
}
