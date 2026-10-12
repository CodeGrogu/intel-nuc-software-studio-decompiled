using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace OpenHardwareMonitor.Hardware.CPU;

internal sealed class AMD10CPU : AMDCPU
{
	private readonly Sensor coreTemperature;

	private readonly Sensor[] coreClocks;

	private readonly Sensor busClock;

	private const uint PERF_CTL_0 = 3221291008u;

	private const uint PERF_CTR_0 = 3221291012u;

	private const uint HWCR = 3221291029u;

	private const uint P_STATE_0 = 3221291108u;

	private const uint COFVID_STATUS = 3221291121u;

	private const byte MISCELLANEOUS_CONTROL_FUNCTION = 3;

	private const ushort FAMILY_10H_MISCELLANEOUS_CONTROL_DEVICE_ID = 4611;

	private const ushort FAMILY_11H_MISCELLANEOUS_CONTROL_DEVICE_ID = 4867;

	private const ushort FAMILY_12H_MISCELLANEOUS_CONTROL_DEVICE_ID = 5891;

	private const ushort FAMILY_14H_MISCELLANEOUS_CONTROL_DEVICE_ID = 5891;

	private const ushort FAMILY_15H_MODEL_00_MISC_CONTROL_DEVICE_ID = 5635;

	private const ushort FAMILY_15H_MODEL_10_MISC_CONTROL_DEVICE_ID = 5123;

	private const ushort FAMILY_15H_MODEL_30_MISC_CONTROL_DEVICE_ID = 5149;

	private const ushort FAMILY_15H_MODEL_60_MISC_CONTROL_DEVICE_ID = 5491;

	private const ushort FAMILY_15H_MODEL_70_MISC_CONTROL_DEVICE_ID = 5555;

	private const ushort FAMILY_16H_MODEL_00_MISC_CONTROL_DEVICE_ID = 5427;

	private const ushort FAMILY_16H_MODEL_30_MISC_CONTROL_DEVICE_ID = 5507;

	private const uint REPORTED_TEMPERATURE_CONTROL_REGISTER = 164u;

	private const uint CLOCK_POWER_TIMING_CONTROL_0_REGISTER = 212u;

	private const uint SMU_REPORTED_TEMP_CONTROL_REGISTER = 3625979044u;

	private readonly bool hasSmuTemperatureRegister;

	private readonly uint miscellaneousControlAddress;

	private readonly ushort miscellaneousControlDeviceId;

	private readonly FileStream temperatureStream;

	private readonly double timeStampCounterMultiplier;

	private readonly bool corePerformanceBoostSupport;

	public AMD10CPU(int processorIndex, CPUID[][] cpuid, ISettings settings)
		: base(processorIndex, cpuid, settings)
	{
		coreTemperature = new Sensor("Core" + ((coreCount > 1) ? (" #1 - #" + coreCount) : ""), 0, SensorType.Temperature, this, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset.", 0f)
		}, settings);
		switch (family)
		{
		case 16u:
			miscellaneousControlDeviceId = 4611;
			break;
		case 17u:
			miscellaneousControlDeviceId = 4867;
			break;
		case 18u:
			miscellaneousControlDeviceId = 5891;
			break;
		case 20u:
			miscellaneousControlDeviceId = 5891;
			break;
		case 21u:
			switch (model & 0xF0)
			{
			case 0u:
				miscellaneousControlDeviceId = 5635;
				break;
			case 16u:
				miscellaneousControlDeviceId = 5123;
				break;
			case 48u:
				miscellaneousControlDeviceId = 5149;
				break;
			case 96u:
				miscellaneousControlDeviceId = 5491;
				hasSmuTemperatureRegister = true;
				break;
			case 112u:
				miscellaneousControlDeviceId = 5555;
				hasSmuTemperatureRegister = true;
				break;
			default:
				miscellaneousControlDeviceId = 0;
				break;
			}
			break;
		case 22u:
			switch (model & 0xF0)
			{
			case 0u:
				miscellaneousControlDeviceId = 5427;
				break;
			case 48u:
				miscellaneousControlDeviceId = 5507;
				break;
			default:
				miscellaneousControlDeviceId = 0;
				break;
			}
			break;
		default:
			miscellaneousControlDeviceId = 0;
			break;
		}
		miscellaneousControlAddress = GetPciAddress(3, miscellaneousControlDeviceId);
		busClock = new Sensor("Bus Speed", 0, SensorType.Clock, this, settings);
		coreClocks = new Sensor[coreCount];
		for (int i = 0; i < coreClocks.Length; i++)
		{
			coreClocks[i] = new Sensor(CoreString(i), i + 1, SensorType.Clock, this, settings);
			if (HasTimeStampCounter)
			{
				ActivateSensor(coreClocks[i]);
			}
		}
		corePerformanceBoostSupport = (cpuid[0][0].ExtData[7, 3] & 0x200) != 0;
		GroupAffinity affinity = ThreadAffinity.Set(cpuid[0][0].Affinity);
		Ring0.Rdmsr(3221291029u, out var eax, out var edx);
		if (corePerformanceBoostSupport)
		{
			Ring0.Wrmsr(3221291029u, eax | 0x2000000, edx);
		}
		Ring0.Rdmsr(3221291008u, out var eax2, out var edx2);
		Ring0.Rdmsr(3221291012u, out var eax3, out var edx3);
		timeStampCounterMultiplier = estimateTimeStampCounterMultiplier();
		Ring0.Wrmsr(3221291008u, eax2, edx2);
		Ring0.Wrmsr(3221291012u, eax3, edx3);
		if (corePerformanceBoostSupport)
		{
			Ring0.Wrmsr(3221291029u, eax, edx);
		}
		ThreadAffinity.Set(affinity);
		temperatureStream = null;
		if (OperatingSystem.IsUnix)
		{
			string[] directories = Directory.GetDirectories("/sys/class/hwmon/");
			foreach (string text in directories)
			{
				string text2 = null;
				try
				{
					using StreamReader streamReader = new StreamReader(text + "/device/name");
					text2 = streamReader.ReadLine();
				}
				catch (IOException)
				{
				}
				if (text2 != null && text2 == "k10temp")
				{
					temperatureStream = new FileStream(text + "/device/temp1_input", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				}
			}
		}
		Update();
	}

	private double estimateTimeStampCounterMultiplier()
	{
		estimateTimeStampCounterMultiplier(0.0);
		estimateTimeStampCounterMultiplier(0.0);
		List<double> list = new List<double>(3);
		for (int i = 0; i < 3; i++)
		{
			list.Add(estimateTimeStampCounterMultiplier(0.025));
		}
		list.Sort();
		return list[1];
	}

	private double estimateTimeStampCounterMultiplier(double timeWindow)
	{
		Ring0.Wrmsr(3221291008u, 4391030u, 0u);
		Ring0.Wrmsr(3221291012u, 0u, 0u);
		long num = (long)(timeWindow * (double)Stopwatch.Frequency);
		long num2 = Stopwatch.GetTimestamp() + (long)Math.Ceiling(0.001 * (double)num);
		long num3 = num2 + num;
		while (Stopwatch.GetTimestamp() < num2)
		{
		}
		Ring0.Rdmsr(3221291012u, out var eax, out var edx);
		while (Stopwatch.GetTimestamp() < num3)
		{
		}
		Ring0.Rdmsr(3221291012u, out var eax2, out var edx2);
		Ring0.Rdmsr(3221291121u, out var eax3, out var _);
		double coreMultiplier = GetCoreMultiplier(eax3);
		ulong num4 = ((ulong)edx << 32) | eax;
		ulong num5 = ((ulong)edx2 << 32) | eax2;
		double num6 = 1E-06 * ((double)(num5 - num4) * (double)Stopwatch.Frequency) / (double)(num3 - num2) / coreMultiplier;
		return 0.25 * Math.Round(4.0 * TimeStampCounterFrequency / num6);
	}

	protected override uint[] GetMSRs()
	{
		return new uint[5] { 3221291008u, 3221291012u, 3221291029u, 3221291108u, 3221291121u };
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetReport());
		stringBuilder.Append("Miscellaneous Control Address: 0x");
		stringBuilder.AppendLine(miscellaneousControlAddress.ToString("X", CultureInfo.InvariantCulture));
		stringBuilder.Append("Time Stamp Counter Multiplier: ");
		stringBuilder.AppendLine(timeStampCounterMultiplier.ToString(CultureInfo.InvariantCulture));
		if (family == 20)
		{
			uint value = 0u;
			Ring0.ReadPciConfig(miscellaneousControlAddress, 212u, out value);
			stringBuilder.Append("PCI Register D18F3xD4: ");
			stringBuilder.AppendLine(value.ToString("X8", CultureInfo.InvariantCulture));
		}
		stringBuilder.AppendLine();
		return stringBuilder.ToString();
	}

	private double GetCoreMultiplier(uint cofvidEax)
	{
		switch (family)
		{
		case 16u:
		case 17u:
		case 21u:
		case 22u:
		{
			uint num4 = (cofvidEax >> 6) & 7;
			uint num5 = cofvidEax & 0x1F;
			return 0.5 * (double)(num5 + 16) / (double)(1 << (int)num4);
		}
		case 18u:
		{
			uint num3 = (cofvidEax >> 4) & 0x1F;
			return (double)(num3 + 16) / (cofvidEax & 0xF) switch
			{
				0u => 1.0, 
				1u => 1.5, 
				2u => 2.0, 
				3u => 3.0, 
				4u => 4.0, 
				5u => 6.0, 
				6u => 8.0, 
				7u => 12.0, 
				8u => 16.0, 
				_ => 1.0, 
			};
		}
		case 20u:
		{
			uint num = (cofvidEax >> 4) & 0x1F;
			uint num2 = cofvidEax & 0xF;
			uint value = 0u;
			Ring0.ReadPciConfig(miscellaneousControlAddress, 212u, out value);
			return (double)((value & 0x1F) + 16) / ((double)num + (double)num2 * 0.25 + 1.0);
		}
		default:
			return 1.0;
		}
	}

	private string ReadFirstLine(Stream stream)
	{
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			stream.Seek(0L, SeekOrigin.Begin);
			int num = stream.ReadByte();
			while (num != -1 && num != 10)
			{
				stringBuilder.Append((char)num);
				num = stream.ReadByte();
			}
		}
		catch
		{
		}
		return stringBuilder.ToString();
	}

	private bool ReadSmuRegister(uint address, out uint value)
	{
		if (Ring0.WaitPciBusMutex(10))
		{
			if (!Ring0.WritePciConfig(0u, 184u, address))
			{
				value = 0u;
				Ring0.ReleasePciBusMutex();
				return false;
			}
			bool result = Ring0.ReadPciConfig(0u, 188u, out value);
			Ring0.ReleasePciBusMutex();
			return result;
		}
		value = 0u;
		return false;
	}

	public override void Update()
	{
		base.Update();
		if (temperatureStream == null)
		{
			if (miscellaneousControlAddress != uint.MaxValue)
			{
				if ((!hasSmuTemperatureRegister) ? Ring0.ReadPciConfig(miscellaneousControlAddress, 164u, out var value) : ReadSmuRegister(3625979044u, out value))
				{
					if ((family == 21 || family == 22) && (value & 0x30000) == 196608)
					{
						coreTemperature.Value = (float)((value >> 21) & 0x7FF) * 0.125f + coreTemperature.Parameters[0].Value - 49f;
					}
					else
					{
						coreTemperature.Value = (float)((value >> 21) & 0x7FF) * 0.125f + coreTemperature.Parameters[0].Value;
					}
					ActivateSensor(coreTemperature);
				}
				else
				{
					DeactivateSensor(coreTemperature);
				}
			}
		}
		else
		{
			string s = ReadFirstLine(temperatureStream);
			try
			{
				coreTemperature.Value = 0.001f * (float)long.Parse(s, CultureInfo.InvariantCulture);
				ActivateSensor(coreTemperature);
			}
			catch
			{
				DeactivateSensor(coreTemperature);
			}
		}
		if (!HasTimeStampCounter)
		{
			return;
		}
		double num = 0.0;
		for (int i = 0; i < coreClocks.Length; i++)
		{
			Thread.Sleep(1);
			if (Ring0.RdmsrTx(3221291121u, out var eax, out var _, cpuid[i][0].Affinity))
			{
				double coreMultiplier = GetCoreMultiplier(eax);
				coreClocks[i].Value = (float)(coreMultiplier * TimeStampCounterFrequency / timeStampCounterMultiplier);
				num = (float)(TimeStampCounterFrequency / timeStampCounterMultiplier);
			}
			else
			{
				coreClocks[i].Value = (float)TimeStampCounterFrequency;
			}
		}
		if (num > 0.0)
		{
			busClock.Value = (float)num;
			ActivateSensor(busClock);
		}
	}

	public override void Close()
	{
		if (temperatureStream != null)
		{
			temperatureStream.Close();
		}
		base.Close();
	}
}
