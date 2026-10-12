using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Threading;
using OpenHardwareMonitor.Hardware;
using PerformanceTuningLibrary;

namespace SystemMonitorLibrary.Hardware.GPU;

internal class Gpu
{
	private readonly Computer _computer;

	private double previousCurrentFrequency;

	private double previousMemoryFrequency;

	private double previousdGPUMemory;

	private double CurrentFrequency;

	private double MemoryFrequency;

	private double dGPUMemory;

	private double initialValue;

	private double latestValue;

	private double finalValue;

	private string iGPUname = GPUData.GetiGPUName();

	private double iGPUValue;

	public Gpu(Computer computer)
	{
		_computer = computer;
	}

	public Dictionary<string, string> GetIntegratedGpuInformation()
	{
		Dictionary<string, string> objDictionary = new Dictionary<string, string>();
		if (FanControl.CurrentEcVersion == ECVersion.ECNUC23)
		{
			if (string.IsNullOrWhiteSpace(iGPUname))
			{
				iGPUname = GPUData.GetiGPUName();
			}
			if (!string.IsNullOrWhiteSpace(iGPUname))
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "Name", iGPUname);
			}
		}
		else
		{
			using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
			foreach (ManagementObject item in managementObjectSearcher.Get().OfType<ManagementObject>())
			{
				string text = item["Name"].ToString();
				if (text.Contains("Intel(R)") && text.Contains("Graphics"))
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Name", text);
				}
			}
		}
		if (FanControl.CurrentEcVersion == ECVersion.ECNUC21 || FanControl.CurrentEcVersion == ECVersion.ECNUC22)
		{
			using ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine");
			ManagementObjectCollection source = managementObjectSearcher2.Get();
			HashSet<string> hashSet = new HashSet<string>();
			List<ManagementObject> list = new List<ManagementObject>();
			foreach (ManagementObject item2 in from x in source.OfType<ManagementObject>()
				where x.Path.RelativePath.Contains("engtype_3D")
				select x)
			{
				list.Add(item2);
				hashSet.Add(item2.Path.RelativePath.Split('_')[7]);
			}
			string iGpuId = hashSet.Min();
			List<ulong> list2 = new List<ulong>();
			foreach (ManagementObject item3 in list.Where((ManagementObject x) => x.Path.RelativePath.Contains(iGpuId)))
			{
				list2.Add((ulong)item3.Properties["UtilizationPercentage"].Value);
			}
			ulong num = list2.Aggregate((ulong a, ulong b) => a + b);
			if (num > 100)
			{
				num = 100uL;
			}
			if (num == 0)
			{
				num = 1uL;
			}
			Helper.DictionaryCheckAssign(ref objDictionary, "Usage", num.ToString());
		}
		else
		{
			iGPUValue = GPUData.IGPUUsage();
			iGPUValue = Math.Round(iGPUValue);
			if (iGPUValue > 100.0)
			{
				iGPUValue = 100.0;
			}
			if (iGPUValue < 1.0)
			{
				iGPUValue = 1.0;
			}
			Helper.DictionaryCheckAssign(ref objDictionary, "Usage", iGPUValue.ToString());
		}
		foreach (IHardware item4 in _computer.Hardware.Where((IHardware hardware) => hardware.HardwareType == HardwareType.CPU))
		{
			item4.Update();
			ISensor[] sensors = item4.Sensors;
			foreach (ISensor sensor in sensors)
			{
				if (sensor.SensorType == SensorType.Temperature && sensor.Name == "CPU Package")
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", Math.Round(sensor.Value.Value, 2).ToString(CultureInfo.InvariantCulture));
				}
			}
		}
		if (!objDictionary.ContainsKey("Temperature") && objDictionary.Count > 0)
		{
			if (FanControl.CurrentEcVersion == ECVersion.ECNUC22 || FanControl.CurrentEcVersion == ECVersion.ECNUC23)
			{
				if (ReadWriteControl.isDataFound)
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", FanControl.GetIntegratedGPUTemprature().ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", string.Empty);
				}
			}
			else
			{
				Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", string.Empty);
			}
		}
		return objDictionary;
	}

	public Dictionary<string, string> GetDiscreteGpuInformation()
	{
		Dictionary<string, string> objDictionary = new Dictionary<string, string>();
		foreach (IHardware item in _computer.Hardware.Where((IHardware hardware) => hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAti))
		{
			item.Update();
			Helper.DictionaryCheckAssign(ref objDictionary, "Name", string.Join(" ", item.Name.Split(' ').Distinct()));
			ISensor[] sensors = item.Sensors;
			foreach (ISensor sensor in sensors)
			{
				if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Usage", Math.Round(sensor.Value.Value, 2).ToString(CultureInfo.InvariantCulture));
				}
				else if (sensor.SensorType == SensorType.Temperature && sensor.Name == "GPU Core")
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", Math.Round(sensor.Value.Value, 2).ToString(CultureInfo.InvariantCulture));
				}
				else if (sensor.SensorType == SensorType.Clock && sensor.Name == "GPU Core")
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "CoreClockSpeed", Math.Round(sensor.Value.Value, 2).ToString(CultureInfo.InvariantCulture));
				}
				else if (sensor.SensorType == SensorType.Clock && sensor.Name == "GPU Memory")
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "MemoryClockSpeed", Math.Round(sensor.Value.Value, 2).ToString(CultureInfo.InvariantCulture));
				}
				else if (sensor.SensorType == SensorType.SmallData && sensor.Name == "GPU Memory Total")
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Memory", (sensor.Value / 1024f).ToString());
				}
				else
				{
					if (objDictionary.ContainsKey("Temperature") || objDictionary.Count <= 0)
					{
						continue;
					}
					if (FanControl.CurrentEcVersion == ECVersion.ECNUC22 || FanControl.CurrentEcVersion == ECVersion.ECNUC23)
					{
						int discreteGPUTemprature = FanControl.GetDiscreteGPUTemprature();
						if (ReadWriteControl.isDataFound)
						{
							Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", discreteGPUTemprature.ToString(CultureInfo.InvariantCulture));
						}
						else
						{
							Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", string.Empty);
						}
					}
					else
					{
						Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", string.Empty);
					}
				}
			}
		}
		if (objDictionary.Count == 0 && FanControl.CurrentEcVersion == ECVersion.ECNUC23)
		{
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
			{
				foreach (ManagementObject item2 in managementObjectSearcher.Get().OfType<ManagementObject>())
				{
					string text = item2["Name"].ToString();
					if (text != iGPUname)
					{
						Helper.DictionaryCheckAssign(ref objDictionary, "Name", text);
					}
				}
			}
			if (objDictionary.Count > 0)
			{
				initialValue = GPUData.GPUUsage();
				Thread.Sleep(1000);
				latestValue = GPUData.GPUUsage();
				finalValue = Math.Round((latestValue - initialValue) / 1.0 * 100.0);
				if (finalValue < 0.0)
				{
					finalValue = 0.0;
				}
				else if (finalValue > 100.0)
				{
					finalValue = 100.0;
				}
				Helper.DictionaryCheckAssign(ref objDictionary, "Usage", Math.Round(finalValue).ToString(CultureInfo.InvariantCulture));
				if (!objDictionary.ContainsKey("Temperature") && objDictionary.Count > 0)
				{
					if (FanControl.CurrentEcVersion == ECVersion.ECNUC22 || FanControl.CurrentEcVersion == ECVersion.ECNUC23)
					{
						int discreteGPUTemprature2 = FanControl.GetDiscreteGPUTemprature();
						if (ReadWriteControl.isDataFound)
						{
							Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", discreteGPUTemprature2.ToString(CultureInfo.InvariantCulture));
						}
						else
						{
							Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", string.Empty);
						}
					}
					else
					{
						Helper.DictionaryCheckAssign(ref objDictionary, "Temperature", string.Empty);
					}
				}
				Thread.Sleep(200);
				CurrentFrequency = GPUData.CurrentFrequency();
				Thread.Sleep(200);
				MemoryFrequency = GPUData.MemoryFrequency();
				Thread.Sleep(200);
				dGPUMemory = GPUData.TotalMemoryUsed();
				if (CurrentFrequency > 0.0)
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "CoreClockSpeed", CurrentFrequency.ToString(CultureInfo.InvariantCulture));
					previousCurrentFrequency = CurrentFrequency;
				}
				else
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "CoreClockSpeed", previousCurrentFrequency.ToString(CultureInfo.InvariantCulture));
				}
				if (MemoryFrequency > 0.0)
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "MemoryClockSpeed", (MemoryFrequency / 2.0).ToString(CultureInfo.InvariantCulture));
					previousMemoryFrequency = MemoryFrequency;
				}
				else if (previousMemoryFrequency != 0.0)
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "MemoryClockSpeed", (previousMemoryFrequency / 2.0).ToString(CultureInfo.InvariantCulture));
				}
				if (dGPUMemory > 0.0)
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Memory", (dGPUMemory / 1073741824.0).ToString(CultureInfo.InvariantCulture));
					previousdGPUMemory = dGPUMemory;
				}
				else if (previousdGPUMemory != 0.0)
				{
					Helper.DictionaryCheckAssign(ref objDictionary, "Memory", (previousdGPUMemory / 1073741824.0).ToString(CultureInfo.InvariantCulture));
				}
			}
		}
		return objDictionary;
	}
}
