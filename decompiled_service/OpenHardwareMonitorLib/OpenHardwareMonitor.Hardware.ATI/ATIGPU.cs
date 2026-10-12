using System;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.ATI;

internal sealed class ATIGPU : Hardware
{
	private readonly int adapterIndex;

	private readonly int busNumber;

	private readonly int deviceNumber;

	private readonly Sensor temperatureCore;

	private readonly Sensor temperatureMemory;

	private readonly Sensor temperatureVrmCore;

	private readonly Sensor temperatureVrmMemory;

	private readonly Sensor temperatureVrmMemory0;

	private readonly Sensor temperatureVrmMemory1;

	private readonly Sensor temperatureLiquid;

	private readonly Sensor temperaturePlx;

	private readonly Sensor temperatureHotSpot;

	private readonly Sensor temperatureVrmSoc;

	private readonly Sensor powerCore;

	private readonly Sensor powerPpt;

	private readonly Sensor powerSocket;

	private readonly Sensor powerTotal;

	private readonly Sensor powerSoc;

	private readonly Sensor fan;

	private readonly Sensor coreClock;

	private readonly Sensor memoryClock;

	private readonly Sensor socClock;

	private readonly Sensor coreVoltage;

	private readonly Sensor memoryVoltage;

	private readonly Sensor socVoltage;

	private readonly Sensor coreLoad;

	private readonly Sensor memoryLoad;

	private readonly Sensor controlSensor;

	private readonly Control fanControl;

	private IntPtr context;

	private readonly int overdriveVersion;

	public int BusNumber => busNumber;

	public int DeviceNumber => deviceNumber;

	public override HardwareType HardwareType => HardwareType.GpuAti;

	public ATIGPU(string name, int adapterIndex, int busNumber, int deviceNumber, IntPtr context, ISettings settings)
		: base(name, new Identifier("atigpu", adapterIndex.ToString(CultureInfo.InvariantCulture)), settings)
	{
		this.adapterIndex = adapterIndex;
		this.busNumber = busNumber;
		this.deviceNumber = deviceNumber;
		this.context = context;
		if (ADL.ADL_Overdrive_Caps(adapterIndex, out var _, out var _, out overdriveVersion) != ADLStatus.OK)
		{
			overdriveVersion = -1;
		}
		temperatureCore = new Sensor("GPU Core", 0, SensorType.Temperature, this, settings);
		temperatureMemory = new Sensor("GPU Memory", 1, SensorType.Temperature, this, settings);
		temperatureVrmCore = new Sensor("GPU VRM Core", 2, SensorType.Temperature, this, settings);
		temperatureVrmMemory = new Sensor("GPU VRM Memory", 3, SensorType.Temperature, this, settings);
		temperatureVrmMemory0 = new Sensor("GPU VRM Memory #1", 4, SensorType.Temperature, this, settings);
		temperatureVrmMemory1 = new Sensor("GPU VRM Memory #2", 5, SensorType.Temperature, this, settings);
		temperatureVrmSoc = new Sensor("GPU VRM SOC", 6, SensorType.Temperature, this, settings);
		temperatureLiquid = new Sensor("GPU Liquid", 7, SensorType.Temperature, this, settings);
		temperaturePlx = new Sensor("GPU PLX", 8, SensorType.Temperature, this, settings);
		temperatureHotSpot = new Sensor("GPU Hot Spot", 9, SensorType.Temperature, this, settings);
		powerTotal = new Sensor("GPU Total", 0, SensorType.Power, this, settings);
		powerCore = new Sensor("GPU Core", 1, SensorType.Power, this, settings);
		powerPpt = new Sensor("GPU PPT", 2, SensorType.Power, this, settings);
		powerSocket = new Sensor("GPU Socket", 3, SensorType.Power, this, settings);
		powerSoc = new Sensor("GPU SOC", 4, SensorType.Power, this, settings);
		fan = new Sensor("GPU Fan", 0, SensorType.Fan, this, settings);
		coreClock = new Sensor("GPU Core", 0, SensorType.Clock, this, settings);
		memoryClock = new Sensor("GPU Memory", 1, SensorType.Clock, this, settings);
		socClock = new Sensor("GPU SOC", 2, SensorType.Clock, this, settings);
		coreVoltage = new Sensor("GPU Core", 0, SensorType.Voltage, this, settings);
		memoryVoltage = new Sensor("GPU Memory", 1, SensorType.Voltage, this, settings);
		socVoltage = new Sensor("GPU SOC", 2, SensorType.Voltage, this, settings);
		coreLoad = new Sensor("GPU Core", 0, SensorType.Load, this, settings);
		memoryLoad = new Sensor("GPU Memory", 1, SensorType.Load, this, settings);
		controlSensor = new Sensor("GPU Fan", 0, SensorType.Control, this, settings);
		ADLFanSpeedInfo fanSpeedInfo = default;
		if (ADL.ADL_Overdrive5_FanSpeedInfo_Get(adapterIndex, 0, ref fanSpeedInfo) != ADLStatus.OK)
		{
			fanSpeedInfo.MaxPercent = 100;
			fanSpeedInfo.MinPercent = 0;
		}
		fanControl = new Control(controlSensor, settings, fanSpeedInfo.MinPercent, fanSpeedInfo.MaxPercent);
		fanControl.ControlModeChanged += ControlModeChanged;
		fanControl.SoftwareControlValueChanged += SoftwareControlValueChanged;
		ControlModeChanged(fanControl);
		controlSensor.Control = fanControl;
		Update();
	}

	private void SoftwareControlValueChanged(IControl control)
	{
		if (control.ControlMode == ControlMode.Software)
		{
			ADLFanSpeedValue fanSpeedValue = new ADLFanSpeedValue
			{
				SpeedType = 1,
				Flags = 1,
				FanSpeed = (int)control.SoftwareValue
			};
			ADL.ADL_Overdrive5_FanSpeed_Set(adapterIndex, 0, ref fanSpeedValue);
		}
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
		ADL.ADL_Overdrive5_FanSpeedToDefault_Set(adapterIndex, 0);
	}

	private void GetODNTemperature(ADLODNTemperatureType type, Sensor sensor)
	{
		if (ADL.ADL2_OverdriveN_Temperature_Get(context, adapterIndex, type, out var temperature) == ADLStatus.OK)
		{
			sensor.Value = 0.001f * (float)temperature;
			ActivateSensor(sensor);
		}
		else
		{
			sensor.Value = null;
		}
	}

	private void GetOD6Power(ADLODNCurrentPowerType type, Sensor sensor)
	{
		if (ADL.ADL2_Overdrive6_CurrentPower_Get(context, adapterIndex, type, out var currentValue) == ADLStatus.OK)
		{
			sensor.Value = (float)currentValue * (1f / 255f);
			ActivateSensor(sensor);
		}
		else
		{
			sensor.Value = null;
		}
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("AMD GPU");
		stringBuilder.AppendLine();
		stringBuilder.Append("AdapterIndex: ");
		stringBuilder.AppendLine(adapterIndex.ToString(CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Overdrive Caps");
		stringBuilder.AppendLine();
		try
		{
			ADLStatus aDLStatus = ADL.ADL_Overdrive_Caps(adapterIndex, out var supported, out var enabled, out var version);
			stringBuilder.Append(" Status: ");
			stringBuilder.AppendLine(aDLStatus.ToString());
			stringBuilder.Append(" Supported: ");
			stringBuilder.AppendLine(supported.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(" Enabled: ");
			stringBuilder.AppendLine(enabled.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(" Version: ");
			stringBuilder.AppendLine(version.ToString(CultureInfo.InvariantCulture));
		}
		catch (Exception ex)
		{
			stringBuilder.AppendLine(" Status: " + ex.Message);
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Overdrive5 Parameters");
		stringBuilder.AppendLine();
		try
		{
			ADLStatus aDLStatus2 = ADL.ADL_Overdrive5_ODParameters_Get(adapterIndex, out var parameters);
			stringBuilder.Append(" Status: ");
			stringBuilder.AppendLine(aDLStatus2.ToString());
			stringBuilder.AppendFormat(" NumberOfPerformanceLevels: {0}{1}", parameters.NumberOfPerformanceLevels, Environment.NewLine);
			stringBuilder.AppendFormat(" ActivityReportingSupported: {0}{1}", parameters.ActivityReportingSupported, Environment.NewLine);
			stringBuilder.AppendFormat(" DiscretePerformanceLevels: {0}{1}", parameters.DiscretePerformanceLevels, Environment.NewLine);
			stringBuilder.AppendFormat(" EngineClock.Min: {0}{1}", parameters.EngineClock.Min, Environment.NewLine);
			stringBuilder.AppendFormat(" EngineClock.Max: {0}{1}", parameters.EngineClock.Max, Environment.NewLine);
			stringBuilder.AppendFormat(" EngineClock.Step: {0}{1}", parameters.EngineClock.Step, Environment.NewLine);
			stringBuilder.AppendFormat(" MemoryClock.Min: {0}{1}", parameters.MemoryClock.Min, Environment.NewLine);
			stringBuilder.AppendFormat(" MemoryClock.Max: {0}{1}", parameters.MemoryClock.Max, Environment.NewLine);
			stringBuilder.AppendFormat(" MemoryClock.Step: {0}{1}", parameters.MemoryClock.Step, Environment.NewLine);
			stringBuilder.AppendFormat(" Vddc.Min: {0}{1}", parameters.Vddc.Min, Environment.NewLine);
			stringBuilder.AppendFormat(" Vddc.Max: {0}{1}", parameters.Vddc.Max, Environment.NewLine);
			stringBuilder.AppendFormat(" Vddc.Step: {0}{1}", parameters.Vddc.Step, Environment.NewLine);
		}
		catch (Exception ex2)
		{
			stringBuilder.AppendLine(" Status: " + ex2.Message);
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Overdrive5 Temperature");
		stringBuilder.AppendLine();
		try
		{
			ADLTemperature temperature = default;
			ADLStatus aDLStatus3 = ADL.ADL_Overdrive5_Temperature_Get(adapterIndex, 0, ref temperature);
			stringBuilder.Append(" Status: ");
			stringBuilder.AppendLine(aDLStatus3.ToString());
			stringBuilder.AppendFormat(" Value: {0}{1}", 0.001f * (float)temperature.Temperature, Environment.NewLine);
		}
		catch (Exception ex3)
		{
			stringBuilder.AppendLine(" Status: " + ex3.Message);
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Overdrive5 FanSpeed");
		stringBuilder.AppendLine();
		try
		{
			ADLFanSpeedValue fanSpeedValue = new ADLFanSpeedValue
			{
				SpeedType = 2
			};
			ADLStatus aDLStatus4 = ADL.ADL_Overdrive5_FanSpeed_Get(adapterIndex, 0, ref fanSpeedValue);
			stringBuilder.Append(" Status RPM: ");
			stringBuilder.AppendLine(aDLStatus4.ToString());
			stringBuilder.AppendFormat(" Value RPM: {0}{1}", fanSpeedValue.FanSpeed, Environment.NewLine);
			fanSpeedValue.SpeedType = 1;
			aDLStatus4 = ADL.ADL_Overdrive5_FanSpeed_Get(adapterIndex, 0, ref fanSpeedValue);
			stringBuilder.Append(" Status Percent: ");
			stringBuilder.AppendLine(aDLStatus4.ToString());
			stringBuilder.AppendFormat(" Value Percent: {0}{1}", fanSpeedValue.FanSpeed, Environment.NewLine);
		}
		catch (Exception ex4)
		{
			stringBuilder.AppendLine(" Status: " + ex4.Message);
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Overdrive5 CurrentActivity");
		stringBuilder.AppendLine();
		try
		{
			ADLPMActivity activity = default;
			ADLStatus aDLStatus5 = ADL.ADL_Overdrive5_CurrentActivity_Get(adapterIndex, ref activity);
			stringBuilder.Append(" Status: ");
			stringBuilder.AppendLine(aDLStatus5.ToString());
			stringBuilder.AppendFormat(" EngineClock: {0}{1}", 0.01f * (float)activity.EngineClock, Environment.NewLine);
			stringBuilder.AppendFormat(" MemoryClock: {0}{1}", 0.01f * (float)activity.MemoryClock, Environment.NewLine);
			stringBuilder.AppendFormat(" Vddc: {0}{1}", 0.001f * (float)activity.Vddc, Environment.NewLine);
			stringBuilder.AppendFormat(" ActivityPercent: {0}{1}", activity.ActivityPercent, Environment.NewLine);
			stringBuilder.AppendFormat(" CurrentPerformanceLevel: {0}{1}", activity.CurrentPerformanceLevel, Environment.NewLine);
			stringBuilder.AppendFormat(" CurrentBusSpeed: {0}{1}", activity.CurrentBusSpeed, Environment.NewLine);
			stringBuilder.AppendFormat(" CurrentBusLanes: {0}{1}", activity.CurrentBusLanes, Environment.NewLine);
			stringBuilder.AppendFormat(" MaximumBusLanes: {0}{1}", activity.MaximumBusLanes, Environment.NewLine);
		}
		catch (Exception ex5)
		{
			stringBuilder.AppendLine(" Status: " + ex5.Message);
		}
		stringBuilder.AppendLine();
		if (context != IntPtr.Zero)
		{
			stringBuilder.AppendLine("Overdrive6 CurrentPower");
			stringBuilder.AppendLine();
			try
			{
				for (int i = 0; i < 4; i++)
				{
					ADLODNCurrentPowerType aDLODNCurrentPowerType = (ADLODNCurrentPowerType)i;
					string arg = aDLODNCurrentPowerType.ToString();
					ADLStatus aDLStatus6 = ADL.ADL2_Overdrive6_CurrentPower_Get(context, adapterIndex, (ADLODNCurrentPowerType)i, out var currentValue);
					if (aDLStatus6 == ADLStatus.OK)
					{
						stringBuilder.AppendFormat(" Power[{0}].Value: {1}{2}", arg, (float)currentValue * (1f / 255f), Environment.NewLine);
					}
					else
					{
						stringBuilder.AppendFormat(" Power[{0}].Status: {1}{2}", arg, aDLStatus6.ToString(), Environment.NewLine);
					}
				}
			}
			catch (EntryPointNotFoundException)
			{
				stringBuilder.AppendLine(" Status: Entry point not found");
			}
			catch (Exception ex7)
			{
				stringBuilder.AppendLine(" Status: " + ex7.Message);
			}
			stringBuilder.AppendLine();
		}
		if (context != IntPtr.Zero)
		{
			stringBuilder.AppendLine("OverdriveN Temperature");
			stringBuilder.AppendLine();
			try
			{
				for (int j = 1; j < 8; j++)
				{
					ADLODNTemperatureType aDLODNTemperatureType = (ADLODNTemperatureType)j;
					string arg2 = aDLODNTemperatureType.ToString();
					ADLStatus aDLStatus7 = ADL.ADL2_OverdriveN_Temperature_Get(context, adapterIndex, (ADLODNTemperatureType)j, out var temperature2);
					if (aDLStatus7 == ADLStatus.OK)
					{
						stringBuilder.AppendFormat(" Temperature[{0}].Value: {1}{2}", arg2, 0.001f * (float)temperature2, Environment.NewLine);
					}
					else
					{
						stringBuilder.AppendFormat(" Temperature[{0}].Status: {1}{2}", arg2, aDLStatus7.ToString(), Environment.NewLine);
					}
				}
			}
			catch (EntryPointNotFoundException)
			{
				stringBuilder.AppendLine(" Status: Entry point not found");
			}
			catch (Exception ex9)
			{
				stringBuilder.AppendLine(" Status: " + ex9.Message);
			}
			stringBuilder.AppendLine();
		}
		if (context != IntPtr.Zero)
		{
			stringBuilder.AppendLine("OverdriveN Performance Status");
			stringBuilder.AppendLine();
			try
			{
				ADLStatus aDLStatus8 = ADL.ADL2_OverdriveN_PerformanceStatus_Get(context, adapterIndex, out var performanceStatus);
				stringBuilder.Append(" Status: ");
				stringBuilder.AppendLine(aDLStatus8.ToString());
				stringBuilder.AppendFormat(" CoreClock: {0}{1}", performanceStatus.CoreClock, Environment.NewLine);
				stringBuilder.AppendFormat(" MemoryClock: {0}{1}", performanceStatus.MemoryClock, Environment.NewLine);
				stringBuilder.AppendFormat(" DCEFClock: {0}{1}", performanceStatus.DCEFClock, Environment.NewLine);
				stringBuilder.AppendFormat(" GFXClock: {0}{1}", performanceStatus.GFXClock, Environment.NewLine);
				stringBuilder.AppendFormat(" UVDClock: {0}{1}", performanceStatus.UVDClock, Environment.NewLine);
				stringBuilder.AppendFormat(" VCEClock: {0}{1}", performanceStatus.VCEClock, Environment.NewLine);
				stringBuilder.AppendFormat(" GPUActivityPercent: {0}{1}", performanceStatus.GPUActivityPercent, Environment.NewLine);
				stringBuilder.AppendFormat(" CurrentCorePerformanceLevel: {0}{1}", performanceStatus.CurrentCorePerformanceLevel, Environment.NewLine);
				stringBuilder.AppendFormat(" CurrentMemoryPerformanceLevel: {0}{1}", performanceStatus.CurrentMemoryPerformanceLevel, Environment.NewLine);
				stringBuilder.AppendFormat(" CurrentDCEFPerformanceLevel: {0}{1}", performanceStatus.CurrentDCEFPerformanceLevel, Environment.NewLine);
				stringBuilder.AppendFormat(" CurrentGFXPerformanceLevel: {0}{1}", performanceStatus.CurrentGFXPerformanceLevel, Environment.NewLine);
				stringBuilder.AppendFormat(" UVDPerformanceLevel: {0}{1}", performanceStatus.UVDPerformanceLevel, Environment.NewLine);
				stringBuilder.AppendFormat(" VCEPerformanceLevel: {0}{1}", performanceStatus.VCEPerformanceLevel, Environment.NewLine);
				stringBuilder.AppendFormat(" CurrentBusSpeed: {0}{1}", performanceStatus.CurrentBusSpeed, Environment.NewLine);
				stringBuilder.AppendFormat(" CurrentBusLanes: {0}{1}", performanceStatus.CurrentBusLanes, Environment.NewLine);
				stringBuilder.AppendFormat(" MaximumBusLanes: {0}{1}", performanceStatus.MaximumBusLanes, Environment.NewLine);
				stringBuilder.AppendFormat(" VDDC: {0}{1}", performanceStatus.VDDC, Environment.NewLine);
				stringBuilder.AppendFormat(" VDDCI: {0}{1}", performanceStatus.VDDCI, Environment.NewLine);
			}
			catch (EntryPointNotFoundException)
			{
				stringBuilder.AppendLine(" Status: Entry point not found");
			}
			catch (Exception ex11)
			{
				stringBuilder.AppendLine(" Status: " + ex11.Message);
			}
			stringBuilder.AppendLine();
		}
		if (context != IntPtr.Zero)
		{
			stringBuilder.AppendLine("Performance Metrics");
			stringBuilder.AppendLine();
			try
			{
				ADLStatus aDLStatus9 = ADL.ADL2_New_QueryPMLogData_Get(context, adapterIndex, out var dataOutput);
				if (aDLStatus9 == ADLStatus.OK)
				{
					for (int k = 0; k < dataOutput.Sensors.Length; k++)
					{
						if (dataOutput.Sensors[k].Supported)
						{
							ADLSensorType aDLSensorType = (ADLSensorType)k;
							string arg3 = aDLSensorType.ToString();
							stringBuilder.AppendFormat(" Sensor[{0}].Value: {1}{2}", arg3, dataOutput.Sensors[k].Value, Environment.NewLine);
						}
					}
				}
				else
				{
					stringBuilder.Append(" Status: ");
					stringBuilder.AppendLine(aDLStatus9.ToString());
				}
			}
			catch (EntryPointNotFoundException)
			{
				stringBuilder.AppendLine(" Status: Entry point not found");
			}
			catch (Exception ex13)
			{
				stringBuilder.AppendLine(" Status: " + ex13.Message);
			}
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	private void GetPMLog(ADLPMLogDataOutput data, ADLSensorType sensorType, Sensor sensor, float factor = 1f)
	{
		if ((int)sensorType < data.Sensors.Length && data.Sensors[(int)sensorType].Supported)
		{
			sensor.Value = (float)data.Sensors[(int)sensorType].Value * factor;
			ActivateSensor(sensor);
		}
	}

	public override void Update()
	{
		if (context != IntPtr.Zero && overdriveVersion >= 8 && ADL.ADL2_New_QueryPMLogData_Get(context, adapterIndex, out var dataOutput) == ADLStatus.OK)
		{
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_EDGE, temperatureCore);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_MEM, temperatureMemory);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_VRVDDC, temperatureVrmCore);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_VRMVDD, temperatureVrmMemory);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_VRMVDD0, temperatureVrmMemory0);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_VRMVDD1, temperatureVrmMemory1);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_VRSOC, temperatureVrmSoc);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_LIQUID, temperatureLiquid);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_PLX, temperaturePlx);
			GetPMLog(dataOutput, ADLSensorType.TEMPERATURE_HOTSPOT, temperatureHotSpot);
			GetPMLog(dataOutput, ADLSensorType.GFX_POWER, powerCore);
			GetPMLog(dataOutput, ADLSensorType.ASIC_POWER, powerTotal);
			GetPMLog(dataOutput, ADLSensorType.SOC_POWER, powerSoc);
			GetPMLog(dataOutput, ADLSensorType.FAN_RPM, fan);
			GetPMLog(dataOutput, ADLSensorType.CLK_GFXCLK, coreClock);
			GetPMLog(dataOutput, ADLSensorType.CLK_MEMCLK, memoryClock);
			GetPMLog(dataOutput, ADLSensorType.CLK_SOCCLK, socClock);
			GetPMLog(dataOutput, ADLSensorType.GFX_VOLTAGE, coreVoltage, 0.001f);
			GetPMLog(dataOutput, ADLSensorType.MEM_VOLTAGE, memoryVoltage, 0.001f);
			GetPMLog(dataOutput, ADLSensorType.SOC_VOLTAGE, socVoltage, 0.001f);
			GetPMLog(dataOutput, ADLSensorType.INFO_ACTIVITY_GFX, coreLoad);
			GetPMLog(dataOutput, ADLSensorType.INFO_ACTIVITY_MEM, memoryLoad);
			GetPMLog(dataOutput, ADLSensorType.FAN_PERCENTAGE, controlSensor);
			return;
		}
		if (context != IntPtr.Zero && overdriveVersion >= 7)
		{
			GetODNTemperature(ADLODNTemperatureType.CORE, temperatureCore);
			GetODNTemperature(ADLODNTemperatureType.MEMORY, temperatureMemory);
			GetODNTemperature(ADLODNTemperatureType.VRM_CORE, temperatureVrmCore);
			GetODNTemperature(ADLODNTemperatureType.VRM_MEMORY, temperatureVrmMemory);
			GetODNTemperature(ADLODNTemperatureType.LIQUID, temperatureLiquid);
			GetODNTemperature(ADLODNTemperatureType.PLX, temperaturePlx);
			GetODNTemperature(ADLODNTemperatureType.HOTSPOT, temperatureHotSpot);
		}
		else
		{
			ADLTemperature temperature = default;
			if (ADL.ADL_Overdrive5_Temperature_Get(adapterIndex, 0, ref temperature) == ADLStatus.OK)
			{
				temperatureCore.Value = 0.001f * (float)temperature.Temperature;
				ActivateSensor(temperatureCore);
			}
			else
			{
				temperatureCore.Value = null;
			}
		}
		if (context != IntPtr.Zero && overdriveVersion >= 6)
		{
			GetOD6Power(ADLODNCurrentPowerType.TOTAL_POWER, powerTotal);
			GetOD6Power(ADLODNCurrentPowerType.CHIP_POWER, powerCore);
			GetOD6Power(ADLODNCurrentPowerType.PPT_POWER, powerPpt);
			GetOD6Power(ADLODNCurrentPowerType.SOCKET_POWER, powerSocket);
		}
		ADLFanSpeedValue fanSpeedValue = new ADLFanSpeedValue
		{
			SpeedType = 2
		};
		if (ADL.ADL_Overdrive5_FanSpeed_Get(adapterIndex, 0, ref fanSpeedValue) == ADLStatus.OK)
		{
			fan.Value = fanSpeedValue.FanSpeed;
			ActivateSensor(fan);
		}
		else
		{
			fan.Value = null;
		}
		fanSpeedValue = new ADLFanSpeedValue
		{
			SpeedType = 1
		};
		if (ADL.ADL_Overdrive5_FanSpeed_Get(adapterIndex, 0, ref fanSpeedValue) == ADLStatus.OK)
		{
			controlSensor.Value = fanSpeedValue.FanSpeed;
			ActivateSensor(controlSensor);
		}
		else
		{
			controlSensor.Value = null;
		}
		ADLPMActivity activity = default;
		if (ADL.ADL_Overdrive5_CurrentActivity_Get(adapterIndex, ref activity) == ADLStatus.OK)
		{
			if (activity.EngineClock > 0)
			{
				coreClock.Value = 0.01f * (float)activity.EngineClock;
				ActivateSensor(coreClock);
			}
			else
			{
				coreClock.Value = null;
			}
			if (activity.MemoryClock > 0)
			{
				memoryClock.Value = 0.01f * (float)activity.MemoryClock;
				ActivateSensor(memoryClock);
			}
			else
			{
				memoryClock.Value = null;
			}
			if (activity.Vddc > 0)
			{
				coreVoltage.Value = 0.001f * (float)activity.Vddc;
				ActivateSensor(coreVoltage);
			}
			else
			{
				coreVoltage.Value = null;
			}
			if (activity.ActivityPercent >= 0 && activity.ActivityPercent <= 100)
			{
				coreLoad.Value = activity.ActivityPercent;
				ActivateSensor(coreLoad);
			}
			else
			{
				coreLoad.Value = null;
			}
		}
		else
		{
			coreClock.Value = null;
			memoryClock.Value = null;
			coreVoltage.Value = null;
			coreLoad.Value = null;
		}
	}

	public override void Close()
	{
		fanControl.ControlModeChanged -= ControlModeChanged;
		fanControl.SoftwareControlValueChanged -= SoftwareControlValueChanged;
		if (fanControl.ControlMode != ControlMode.Undefined)
		{
			SetDefaultFanSpeed();
		}
		base.Close();
	}
}
