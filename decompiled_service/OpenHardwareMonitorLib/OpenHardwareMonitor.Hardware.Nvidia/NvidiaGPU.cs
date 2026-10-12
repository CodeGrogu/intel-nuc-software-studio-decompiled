using System;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.Nvidia;

internal class NvidiaGPU : Hardware
{
	private readonly int adapterIndex;

	private readonly NvPhysicalGpuHandle handle;

	private readonly NvDisplayHandle? displayHandle;

	private readonly NVML.NvmlDevice? device;

	private readonly Sensor[] temperatures;

	private readonly Sensor fan;

	private readonly Sensor[] clocks;

	private readonly Sensor[] loads;

	private readonly Sensor control;

	private readonly Sensor memoryLoad;

	private readonly Sensor memoryUsed;

	private readonly Sensor memoryFree;

	private readonly Sensor memoryAvail;

	private readonly Sensor power;

	private readonly Sensor pcieThroughputRx;

	private readonly Sensor pcieThroughputTx;

	private readonly Control fanControl;

	public override HardwareType HardwareType => HardwareType.GpuNvidia;

	public NvidiaGPU(int adapterIndex, NvPhysicalGpuHandle handle, NvDisplayHandle? displayHandle, ISettings settings)
		: base(GetName(handle), new Identifier("nvidiagpu", adapterIndex.ToString(CultureInfo.InvariantCulture)), settings)
	{
		this.adapterIndex = adapterIndex;
		this.handle = handle;
		this.displayHandle = displayHandle;
		NvGPUThermalSettings thermalSettings = GetThermalSettings();
		temperatures = new Sensor[thermalSettings.Count];
		for (int i = 0; i < temperatures.Length; i++)
		{
			NvThermalTarget target = thermalSettings.Sensor[i].Target;
			temperatures[i] = new Sensor(target switch
			{
				NvThermalTarget.BOARD => "GPU Board", 
				NvThermalTarget.GPU => "GPU Core", 
				NvThermalTarget.MEMORY => "GPU Memory", 
				NvThermalTarget.POWER_SUPPLY => "GPU Power Supply", 
				NvThermalTarget.UNKNOWN => "GPU Unknown", 
				_ => "GPU", 
			}, i, SensorType.Temperature, this, new ParameterDescription[0], settings);
			ActivateSensor(temperatures[i]);
		}
		fan = new Sensor("GPU", 0, SensorType.Fan, this, settings);
		clocks = new Sensor[3];
		clocks[0] = new Sensor("GPU Core", 0, SensorType.Clock, this, settings);
		clocks[1] = new Sensor("GPU Memory", 1, SensorType.Clock, this, settings);
		clocks[2] = new Sensor("GPU Shader", 2, SensorType.Clock, this, settings);
		for (int j = 0; j < clocks.Length; j++)
		{
			ActivateSensor(clocks[j]);
		}
		loads = new Sensor[4];
		loads[0] = new Sensor("GPU Core", 0, SensorType.Load, this, settings);
		loads[1] = new Sensor("GPU Frame Buffer", 1, SensorType.Load, this, settings);
		loads[2] = new Sensor("GPU Video Engine", 2, SensorType.Load, this, settings);
		loads[3] = new Sensor("GPU Bus Interface", 3, SensorType.Load, this, settings);
		memoryLoad = new Sensor("GPU Memory", 4, SensorType.Load, this, settings);
		memoryFree = new Sensor("GPU Memory Free", 1, SensorType.SmallData, this, settings);
		memoryUsed = new Sensor("GPU Memory Used", 2, SensorType.SmallData, this, settings);
		memoryAvail = new Sensor("GPU Memory Total", 3, SensorType.SmallData, this, settings);
		control = new Sensor("GPU Fan", 0, SensorType.Control, this, settings);
		NvGPUCoolerSettings coolerSettings = GetCoolerSettings();
		if (coolerSettings.Count != 0)
		{
			fanControl = new Control(control, settings, coolerSettings.Cooler[0].DefaultMin, coolerSettings.Cooler[0].DefaultMax);
			fanControl.ControlModeChanged += ControlModeChanged;
			fanControl.SoftwareControlValueChanged += SoftwareControlValueChanged;
			ControlModeChanged(fanControl);
			control.Control = fanControl;
		}
		if (NVML.IsInitialized && NVAPI.NvAPI_GPU_GetBusId != null && NVAPI.NvAPI_GPU_GetBusId(handle, out var busId) == NvStatus.OK && NVML.NvmlDeviceGetHandleByPciBusId != null && NVML.NvmlDeviceGetHandleByPciBusId("0000:" + busId.ToString("X2") + ":00.0", out var value) == NVML.NvmlReturn.Success)
		{
			device = value;
			power = new Sensor("GPU Power", 0, SensorType.Power, this, settings);
			pcieThroughputRx = new Sensor("GPU PCIE Rx", 0, SensorType.Throughput, this, settings);
			pcieThroughputTx = new Sensor("GPU PCIE Tx", 1, SensorType.Throughput, this, settings);
		}
		Update();
	}

	private static string GetName(NvPhysicalGpuHandle handle)
	{
		if (NVAPI.NvAPI_GPU_GetFullName(handle, out var text) == NvStatus.OK)
		{
			return "NVIDIA " + text.Trim();
		}
		return "NVIDIA";
	}

	private NvGPUThermalSettings GetThermalSettings()
	{
		NvGPUThermalSettings gpuThermalSettings = new NvGPUThermalSettings
		{
			Version = NVAPI.GPU_THERMAL_SETTINGS_VER,
			Count = 3u,
			Sensor = new NvSensor[3]
		};
		if (NVAPI.NvAPI_GPU_GetThermalSettings == null || NVAPI.NvAPI_GPU_GetThermalSettings(handle, 15, ref gpuThermalSettings) != NvStatus.OK)
		{
			gpuThermalSettings.Count = 0u;
		}
		return gpuThermalSettings;
	}

	private NvGPUCoolerSettings GetCoolerSettings()
	{
		NvGPUCoolerSettings gpuCoolerSettings = new NvGPUCoolerSettings
		{
			Version = NVAPI.GPU_COOLER_SETTINGS_VER,
			Cooler = new NvCooler[20]
		};
		if (NVAPI.NvAPI_GPU_GetCoolerSettings == null || NVAPI.NvAPI_GPU_GetCoolerSettings(handle, 0, ref gpuCoolerSettings) != NvStatus.OK)
		{
			gpuCoolerSettings.Count = 0u;
		}
		return gpuCoolerSettings;
	}

	private NvFanCoolersStatus GetFanCoolersStatus()
	{
		NvFanCoolersStatus fanCoolersStatus = new NvFanCoolersStatus
		{
			Version = NVAPI.GPU_FAN_COOLERS_STATUS_VER,
			Items = new NvFanCoolersStatusItem[32]
		};
		if (NVAPI.NvAPI_GPU_ClientFanCoolersGetStatus == null || NVAPI.NvAPI_GPU_ClientFanCoolersGetStatus(handle, ref fanCoolersStatus) != NvStatus.OK)
		{
			fanCoolersStatus.Count = 0u;
		}
		return fanCoolersStatus;
	}

	private uint[] GetClocks()
	{
		NvClocks nvClocks = new NvClocks
		{
			Version = NVAPI.GPU_CLOCKS_VER,
			Clock = new uint[288]
		};
		if (NVAPI.NvAPI_GPU_GetAllClocks != null && NVAPI.NvAPI_GPU_GetAllClocks(handle, ref nvClocks) == NvStatus.OK)
		{
			return nvClocks.Clock;
		}
		return null;
	}

	public override void Update()
	{
		NvGPUThermalSettings thermalSettings = GetThermalSettings();
		Sensor[] array = temperatures;
		foreach (Sensor sensor in array)
		{
			sensor.Value = thermalSettings.Sensor[sensor.Index].CurrentTemp;
		}
		bool flag = false;
		if (NVAPI.NvAPI_GPU_GetTachReading != null && NVAPI.NvAPI_GPU_GetTachReading(handle, out var value) == NvStatus.OK)
		{
			fan.Value = value;
			ActivateSensor(fan);
			flag = true;
		}
		uint[] array2 = GetClocks();
		if (array2 != null)
		{
			clocks[1].Value = 0.001f * (float)array2[8];
			if (array2[30] != 0)
			{
				clocks[0].Value = 0.0005f * (float)array2[30];
				clocks[2].Value = 0.001f * (float)array2[30];
			}
			else
			{
				clocks[0].Value = 0.001f * (float)array2[0];
				clocks[2].Value = 0.001f * (float)array2[14];
			}
		}
		NvDynamicPstatesInfoEx dynamicPstatesInfoEx = new NvDynamicPstatesInfoEx
		{
			Version = NVAPI.GPU_DYNAMIC_PSTATES_INFO_EX_VER,
			UtilizationDomains = new NvUtilizationDomainEx[8]
		};
		if (NVAPI.NvAPI_GPU_GetDynamicPstatesInfoEx != null && NVAPI.NvAPI_GPU_GetDynamicPstatesInfoEx(handle, ref dynamicPstatesInfoEx) == NvStatus.OK)
		{
			for (int j = 0; j < loads.Length; j++)
			{
				if (dynamicPstatesInfoEx.UtilizationDomains[j].Present)
				{
					loads[j].Value = dynamicPstatesInfoEx.UtilizationDomains[j].Percentage;
					ActivateSensor(loads[j]);
				}
			}
		}
		else
		{
			NvDynamicPstatesInfo dynamicPstatesInfo = new NvDynamicPstatesInfo
			{
				Version = NVAPI.GPU_DYNAMIC_PSTATES_INFO_VER,
				UtilizationDomains = new NvUtilizationDomain[8]
			};
			if (NVAPI.NvAPI_GPU_GetDynamicPstatesInfo != null && NVAPI.NvAPI_GPU_GetDynamicPstatesInfo(handle, ref dynamicPstatesInfo) == NvStatus.OK)
			{
				for (int k = 0; k < loads.Length; k++)
				{
					if (dynamicPstatesInfo.UtilizationDomains[k].Present)
					{
						loads[k].Value = dynamicPstatesInfo.UtilizationDomains[k].Percentage;
						ActivateSensor(loads[k]);
					}
				}
			}
		}
		NvGPUCoolerSettings coolerSettings = GetCoolerSettings();
		bool flag2 = false;
		if (coolerSettings.Count != 0)
		{
			control.Value = coolerSettings.Cooler[0].CurrentLevel;
			ActivateSensor(control);
			flag2 = true;
		}
		if (!flag || !flag2)
		{
			NvFanCoolersStatus fanCoolersStatus = GetFanCoolersStatus();
			if (fanCoolersStatus.Count != 0)
			{
				if (!flag2)
				{
					control.Value = fanCoolersStatus.Items[0].CurrentLevel;
					ActivateSensor(control);
					flag2 = true;
				}
				if (!flag)
				{
					fan.Value = fanCoolersStatus.Items[0].CurrentRpm;
					ActivateSensor(fan);
					flag = true;
				}
			}
		}
		NvDisplayDriverMemoryInfo memoryInfo = new NvDisplayDriverMemoryInfo
		{
			Version = NVAPI.DISPLAY_DRIVER_MEMORY_INFO_VER,
			Values = new uint[5]
		};
		if (NVAPI.NvAPI_GetDisplayDriverMemoryInfo != null && displayHandle.HasValue && NVAPI.NvAPI_GetDisplayDriverMemoryInfo(displayHandle.Value, ref memoryInfo) == NvStatus.OK)
		{
			uint num = memoryInfo.Values[0];
			uint num2 = memoryInfo.Values[4];
			float num3 = Math.Max(num - num2, 0u);
			memoryFree.Value = (float)num2 / 1024f;
			memoryAvail.Value = (float)num / 1024f;
			memoryUsed.Value = num3 / 1024f;
			memoryLoad.Value = 100f * num3 / (float)num;
			ActivateSensor(memoryAvail);
			ActivateSensor(memoryUsed);
			ActivateSensor(memoryFree);
			ActivateSensor(memoryLoad);
		}
		if (power != null && NVML.NvmlDeviceGetPowerUsage(device.Value, out var num4) == NVML.NvmlReturn.Success)
		{
			power.Value = (float)num4 * 0.001f;
			ActivateSensor(power);
		}
		if (pcieThroughputRx != null && NVML.NvmlDeviceGetPcieThroughput(device.Value, NVML.NvmlPcieUtilCounter.RxBytes, out var value2) == NVML.NvmlReturn.Success)
		{
			pcieThroughputRx.Value = (float)value2 * (1f / 1024f);
			ActivateSensor(pcieThroughputRx);
		}
		if (pcieThroughputTx != null && NVML.NvmlDeviceGetPcieThroughput(device.Value, NVML.NvmlPcieUtilCounter.TxBytes, out var value3) == NVML.NvmlReturn.Success)
		{
			pcieThroughputTx.Value = (float)value3 * (1f / 1024f);
			ActivateSensor(pcieThroughputTx);
		}
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Nvidia GPU");
		stringBuilder.AppendLine();
		stringBuilder.AppendFormat("Name: {0}{1}", name, Environment.NewLine);
		stringBuilder.AppendFormat("Index: {0}{1}", adapterIndex, Environment.NewLine);
		if (displayHandle.HasValue && NVAPI.NvAPI_GetDisplayDriverVersion != null)
		{
			NvDisplayDriverVersion displayDriverVersion = new NvDisplayDriverVersion
			{
				Version = NVAPI.DISPLAY_DRIVER_VERSION_VER
			};
			if (NVAPI.NvAPI_GetDisplayDriverVersion(displayHandle.Value, ref displayDriverVersion) == NvStatus.OK)
			{
				stringBuilder.Append("Driver Version: ");
				stringBuilder.Append(displayDriverVersion.DriverVersion / 100);
				stringBuilder.Append(".");
				stringBuilder.Append((displayDriverVersion.DriverVersion % 100).ToString("00", CultureInfo.InvariantCulture));
				stringBuilder.AppendLine();
				stringBuilder.Append("Driver Branch: ");
				stringBuilder.AppendLine(displayDriverVersion.BuildBranch);
			}
		}
		stringBuilder.AppendLine();
		if (NVAPI.NvAPI_GPU_GetPCIIdentifiers != null && NVAPI.NvAPI_GPU_GetPCIIdentifiers(handle, out var deviceId, out var subSystemId, out var revisionId, out var extDeviceId) == NvStatus.OK)
		{
			stringBuilder.Append("DeviceID: 0x");
			stringBuilder.AppendLine(deviceId.ToString("X", CultureInfo.InvariantCulture));
			stringBuilder.Append("SubSystemID: 0x");
			stringBuilder.AppendLine(subSystemId.ToString("X", CultureInfo.InvariantCulture));
			stringBuilder.Append("RevisionID: 0x");
			stringBuilder.AppendLine(revisionId.ToString("X", CultureInfo.InvariantCulture));
			stringBuilder.Append("ExtDeviceID: 0x");
			stringBuilder.AppendLine(extDeviceId.ToString("X", CultureInfo.InvariantCulture));
			stringBuilder.AppendLine();
		}
		if (NVAPI.NvAPI_GPU_GetThermalSettings != null)
		{
			NvGPUThermalSettings gpuThermalSettings = new NvGPUThermalSettings
			{
				Version = NVAPI.GPU_THERMAL_SETTINGS_VER,
				Count = 3u,
				Sensor = new NvSensor[3]
			};
			NvStatus nvStatus = NVAPI.NvAPI_GPU_GetThermalSettings(handle, 15, ref gpuThermalSettings);
			stringBuilder.AppendLine("Thermal Settings");
			stringBuilder.AppendLine();
			if (nvStatus == NvStatus.OK)
			{
				for (int i = 0; i < gpuThermalSettings.Count; i++)
				{
					stringBuilder.AppendFormat(" Sensor[{0}].Controller: {1}{2}", i, gpuThermalSettings.Sensor[i].Controller, Environment.NewLine);
					stringBuilder.AppendFormat(" Sensor[{0}].DefaultMinTemp: {1}{2}", i, gpuThermalSettings.Sensor[i].DefaultMinTemp, Environment.NewLine);
					stringBuilder.AppendFormat(" Sensor[{0}].DefaultMaxTemp: {1}{2}", i, gpuThermalSettings.Sensor[i].DefaultMaxTemp, Environment.NewLine);
					stringBuilder.AppendFormat(" Sensor[{0}].CurrentTemp: {1}{2}", i, gpuThermalSettings.Sensor[i].CurrentTemp, Environment.NewLine);
					stringBuilder.AppendFormat(" Sensor[{0}].Target: {1}{2}", i, gpuThermalSettings.Sensor[i].Target, Environment.NewLine);
				}
			}
			else
			{
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(nvStatus.ToString());
			}
			stringBuilder.AppendLine();
		}
		if (NVAPI.NvAPI_GPU_GetAllClocks != null)
		{
			NvClocks nvClocks = new NvClocks
			{
				Version = NVAPI.GPU_CLOCKS_VER,
				Clock = new uint[288]
			};
			NvStatus nvStatus2 = NVAPI.NvAPI_GPU_GetAllClocks(handle, ref nvClocks);
			stringBuilder.AppendLine("Clocks");
			stringBuilder.AppendLine();
			if (nvStatus2 == NvStatus.OK)
			{
				for (int j = 0; j < nvClocks.Clock.Length; j++)
				{
					if (nvClocks.Clock[j] != 0)
					{
						stringBuilder.AppendFormat(" Clock[{0}]: {1}{2}", j, nvClocks.Clock[j], Environment.NewLine);
					}
				}
			}
			else
			{
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(nvStatus2.ToString());
			}
			stringBuilder.AppendLine();
		}
		if (NVAPI.NvAPI_GPU_GetTachReading != null)
		{
			NvStatus nvStatus3 = NVAPI.NvAPI_GPU_GetTachReading(handle, out var value);
			stringBuilder.AppendLine("Tachometer");
			stringBuilder.AppendLine();
			if (nvStatus3 == NvStatus.OK)
			{
				stringBuilder.AppendFormat(" Value: {0}{1}", value, Environment.NewLine);
			}
			else
			{
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(nvStatus3.ToString());
			}
			stringBuilder.AppendLine();
		}
		if (NVAPI.NvAPI_GPU_GetDynamicPstatesInfoEx != null)
		{
			NvDynamicPstatesInfoEx dynamicPstatesInfoEx = new NvDynamicPstatesInfoEx
			{
				Version = NVAPI.GPU_DYNAMIC_PSTATES_INFO_EX_VER,
				UtilizationDomains = new NvUtilizationDomainEx[8]
			};
			NvStatus nvStatus4 = NVAPI.NvAPI_GPU_GetDynamicPstatesInfoEx(handle, ref dynamicPstatesInfoEx);
			stringBuilder.AppendLine("Utilization Domains Ex");
			stringBuilder.AppendLine();
			if (nvStatus4 == NvStatus.OK)
			{
				for (int k = 0; k < dynamicPstatesInfoEx.UtilizationDomains.Length; k++)
				{
					if (dynamicPstatesInfoEx.UtilizationDomains[k].Present)
					{
						stringBuilder.AppendFormat(" Percentage[{0}]: {1}{2}", k, dynamicPstatesInfoEx.UtilizationDomains[k].Percentage, Environment.NewLine);
					}
				}
			}
			else
			{
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(nvStatus4.ToString());
			}
			stringBuilder.AppendLine();
		}
		if (NVAPI.NvAPI_GPU_GetDynamicPstatesInfo != null)
		{
			NvDynamicPstatesInfo dynamicPstatesInfo = new NvDynamicPstatesInfo
			{
				Version = NVAPI.GPU_DYNAMIC_PSTATES_INFO_VER,
				UtilizationDomains = new NvUtilizationDomain[8]
			};
			NvStatus nvStatus5 = NVAPI.NvAPI_GPU_GetDynamicPstatesInfo(handle, ref dynamicPstatesInfo);
			stringBuilder.AppendLine("Utilization Domains");
			stringBuilder.AppendLine();
			if (nvStatus5 == NvStatus.OK)
			{
				for (int l = 0; l < dynamicPstatesInfo.UtilizationDomains.Length; l++)
				{
					if (dynamicPstatesInfo.UtilizationDomains[l].Present)
					{
						stringBuilder.AppendFormat(" Percentage[{0}]: {1}{2}", l, dynamicPstatesInfo.UtilizationDomains[l].Percentage, Environment.NewLine);
					}
				}
			}
			else
			{
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(nvStatus5.ToString());
			}
			stringBuilder.AppendLine();
		}
		if (NVAPI.NvAPI_GPU_GetCoolerSettings != null)
		{
			NvGPUCoolerSettings gpuCoolerSettings = new NvGPUCoolerSettings
			{
				Version = NVAPI.GPU_COOLER_SETTINGS_VER,
				Cooler = new NvCooler[20]
			};
			NvStatus nvStatus6 = NVAPI.NvAPI_GPU_GetCoolerSettings(handle, 0, ref gpuCoolerSettings);
			stringBuilder.AppendLine("Cooler Settings");
			stringBuilder.AppendLine();
			if (nvStatus6 == NvStatus.OK)
			{
				for (int m = 0; m < gpuCoolerSettings.Count; m++)
				{
					stringBuilder.AppendFormat(" Cooler[{0}].Type: {1}{2}", m, gpuCoolerSettings.Cooler[m].Type, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].Controller: {1}{2}", m, gpuCoolerSettings.Cooler[m].Controller, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].DefaultMin: {1}{2}", m, gpuCoolerSettings.Cooler[m].DefaultMin, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].DefaultMax: {1}{2}", m, gpuCoolerSettings.Cooler[m].DefaultMax, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].CurrentMin: {1}{2}", m, gpuCoolerSettings.Cooler[m].CurrentMin, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].CurrentMax: {1}{2}", m, gpuCoolerSettings.Cooler[m].CurrentMax, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].CurrentLevel: {1}{2}", m, gpuCoolerSettings.Cooler[m].CurrentLevel, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].DefaultPolicy: {1}{2}", m, gpuCoolerSettings.Cooler[m].DefaultPolicy, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].CurrentPolicy: {1}{2}", m, gpuCoolerSettings.Cooler[m].CurrentPolicy, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].Target: {1}{2}", m, gpuCoolerSettings.Cooler[m].Target, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].ControlType: {1}{2}", m, gpuCoolerSettings.Cooler[m].ControlType, Environment.NewLine);
					stringBuilder.AppendFormat(" Cooler[{0}].Active: {1}{2}", m, gpuCoolerSettings.Cooler[m].Active, Environment.NewLine);
				}
			}
			else
			{
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(nvStatus6.ToString());
			}
			stringBuilder.AppendLine();
		}
		if (NVAPI.NvAPI_GetDisplayDriverMemoryInfo != null && displayHandle.HasValue)
		{
			NvDisplayDriverMemoryInfo memoryInfo = new NvDisplayDriverMemoryInfo
			{
				Version = NVAPI.DISPLAY_DRIVER_MEMORY_INFO_VER,
				Values = new uint[5]
			};
			NvStatus nvStatus7 = NVAPI.NvAPI_GetDisplayDriverMemoryInfo(displayHandle.Value, ref memoryInfo);
			stringBuilder.AppendLine("Memory Info");
			stringBuilder.AppendLine();
			if (nvStatus7 == NvStatus.OK)
			{
				for (int n = 0; n < memoryInfo.Values.Length; n++)
				{
					stringBuilder.AppendFormat(" Value[{0}]: {1}{2}", n, memoryInfo.Values[n], Environment.NewLine);
				}
			}
			else
			{
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(nvStatus7.ToString());
			}
			stringBuilder.AppendLine();
		}
		if (NVAPI.NvAPI_GPU_ClientFanCoolersGetStatus != null)
		{
			NvFanCoolersStatus fanCoolersStatus = new NvFanCoolersStatus
			{
				Version = NVAPI.GPU_FAN_COOLERS_STATUS_VER,
				Items = new NvFanCoolersStatusItem[32]
			};
			NvStatus nvStatus8 = NVAPI.NvAPI_GPU_ClientFanCoolersGetStatus(handle, ref fanCoolersStatus);
			stringBuilder.AppendLine("Fan Coolers Status");
			stringBuilder.AppendLine();
			if (nvStatus8 == NvStatus.OK)
			{
				for (int num = 0; num < fanCoolersStatus.Count; num++)
				{
					stringBuilder.AppendFormat(" Items[{0}].Type: {1}{2}", num, fanCoolersStatus.Items[num].Type, Environment.NewLine);
					stringBuilder.AppendFormat(" Items[{0}].CurrentRpm: {1}{2}", num, fanCoolersStatus.Items[num].CurrentRpm, Environment.NewLine);
					stringBuilder.AppendFormat(" Items[{0}].CurrentMinLevel: {1}{2}", num, fanCoolersStatus.Items[num].CurrentMinLevel, Environment.NewLine);
					stringBuilder.AppendFormat(" Items[{0}].CurrentMaxLevel: {1}{2}", num, fanCoolersStatus.Items[num].CurrentMaxLevel, Environment.NewLine);
					stringBuilder.AppendFormat(" Items[{0}].CurrentLevel: {1}{2}", num, fanCoolersStatus.Items[num].CurrentLevel, Environment.NewLine);
				}
			}
			else
			{
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(nvStatus8.ToString());
			}
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	private void SoftwareControlValueChanged(IControl control)
	{
		NvGPUCoolerLevels gpuCoolerLevels = new NvGPUCoolerLevels
		{
			Version = NVAPI.GPU_COOLER_LEVELS_VER,
			Levels = new NvLevel[20]
		};
		gpuCoolerLevels.Levels[0].Level = (int)control.SoftwareValue;
		gpuCoolerLevels.Levels[0].Policy = 1;
		NVAPI.NvAPI_GPU_SetCoolerLevels(handle, 0, ref gpuCoolerLevels);
	}

	private void ControlModeChanged(IControl control)
	{
		switch (control.ControlMode)
		{
		case ControlMode.Undefined:
			break;
		case ControlMode.Default:
			SetDefaultFanSpeed();
			break;
		case ControlMode.Software:
			SoftwareControlValueChanged(control);
			break;
		}
	}

	private void SetDefaultFanSpeed()
	{
		NvGPUCoolerLevels gpuCoolerLevels = new NvGPUCoolerLevels
		{
			Version = NVAPI.GPU_COOLER_LEVELS_VER,
			Levels = new NvLevel[20]
		};
		gpuCoolerLevels.Levels[0].Policy = 32;
		NVAPI.NvAPI_GPU_SetCoolerLevels(handle, 0, ref gpuCoolerLevels);
	}

	public override void Close()
	{
		if (fanControl != null)
		{
			fanControl.ControlModeChanged -= ControlModeChanged;
			fanControl.SoftwareControlValueChanged -= SoftwareControlValueChanged;
			if (fanControl.ControlMode != ControlMode.Undefined)
			{
				SetDefaultFanSpeed();
			}
		}
		base.Close();
	}
}
