using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceTuningLibrary;

public static class ReadWriteControl
{
	private const uint COMMAND_PORT = 102u;

	private const uint DATA_PORT = 98u;

	private const uint EC_OUTPUT_BUF_FULL = 1u;

	private const uint EC_INPUT_BUF_FULL = 2u;

	private const byte EC_READ = 128;

	private const byte EC_WRITE = 129;

	private const byte FAN_SPEED_HIGH_ADDR = 27;

	private const byte FAN_SPEED_LOW_ADDR = 28;

	private const int MAX_FAN_COUNT = 6;

	private static DriverControl driverControl;

	public static List<string> FanTypes { get; set; }

	public static bool isDataFound { get; set; }

	public static bool Start()
	{
		driverControl = new DriverControl();
		return driverControl.InitializeDriver();
	}

	public static List<FanSpeed> ReadFanSpeed(NucEcSection ecSection)
	{
		byte ecSection2 = (byte)((ecSection == NucEcSection.UseEcSectionBC) ? NucEcSection.UseEcSection21 : ecSection);
		if (driverControl == null)
		{
			Start();
		}
		FanSpeedStruct value = default;
		if (driverControl.GetFanSpeed(ref value, ecSection2))
		{
			if (ecSection == NucEcSection.UseEcSectionBC)
			{
				value.Data = value.Data.Skip(1).Concat(new List<byte> { 0 }).ToArray();
			}
			List<FanSpeed> list = new List<FanSpeed>();
			for (int i = 0; i < 6; i++)
			{
				FanSpeed item = new FanSpeed
				{
					name = "Fan" + (i + 1),
					type = (FanType)value.Data[i * 3],
					speed = value.Data[i * 3 + 2] + (uint)((double)(int)value.Data[i * 3 + 1] * Math.Pow(2.0, 8.0))
				};
				if (item.type != FanType.NotConnected && (int)item.type < 6)
				{
					list.Add(item);
				}
				if (item.type != FanType.NotConnected && item.type == FanType.CPUFan2)
				{
					list.Add(item);
				}
			}
			return list;
		}
		return new List<FanSpeed>();
	}

	public static List<string> GetFanType(NucEcSection ecSection)
	{
		byte ecSection2 = (byte)((ecSection == NucEcSection.UseEcSectionBC) ? NucEcSection.UseEcSection21 : ecSection);
		if (driverControl == null)
		{
			Start();
		}
		FanSpeedStruct value = default;
		if (driverControl.GetFanSpeed(ref value, ecSection2))
		{
			if (ecSection == NucEcSection.UseEcSectionBC)
			{
				value.Data = value.Data.Skip(1).Concat(new List<byte> { 0 }).ToArray();
			}
			List<FanSpeed> list = new List<FanSpeed>();
			FanTypes = new List<string>();
			for (int i = 0; i < 6; i++)
			{
				FanSpeed item = new FanSpeed
				{
					name = "Fan" + (i + 1),
					type = (FanType)value.Data[i * 3],
					speed = value.Data[i * 3 + 2] + (uint)((double)(int)value.Data[i * 3 + 1] * Math.Pow(2.0, 8.0))
				};
				if (item.type != FanType.NotConnected && (int)item.type < 6)
				{
					list.Add(item);
					FanTypes.Add(item.type.ToString());
				}
				if (item.type != FanType.NotConnected && item.type == FanType.CPUFan2)
				{
					list.Add(item);
					FanTypes.Add(item.type.ToString());
				}
			}
		}
		return FanTypes;
	}

	public static bool ControlFanDuty(byte fanNumber, byte duty, NucEcSection ecSection)
	{
		if (driverControl == null)
		{
			Start();
		}
		if (duty >= 100 || duty <= 0)
		{
			return false;
		}
		FanDuty fanSpeed = new FanDuty
		{
			fanDuty = duty,
			fanNumber = fanNumber,
			nucEcSection = (byte)ecSection
		};
		return driverControl.SetFanSpeed(fanSpeed);
	}

	public static ulong ReadPowerLimit()
	{
		if (driverControl == null)
		{
			Start();
		}
		ulong value = 0uL;
		driverControl.ReadPowerLimit(ref value);
		return value;
	}

	public static bool WritePowerLimit(ulong data)
	{
		if (driverControl == null)
		{
			Start();
		}
		return driverControl.WritePowerLimit(data);
	}

	public static FanProfileEC ReadFanProfile()
	{
		if (driverControl == null)
		{
			Start();
		}
		FanProfileEC value = new FanProfileEC
		{
			Data = new byte[94]
		};
		driverControl.ReadFanProfile(ref value);
		return value;
	}

	public static TempratureProfileEC ReadTempratureProfile()
	{
		if (driverControl == null)
		{
			Start();
		}
		TempratureProfileEC value = new TempratureProfileEC
		{
			Data = new byte[15]
		};
		isDataFound = driverControl.ReadTempratureProfile(ref value);
		return value;
	}

	public static bool WriteFanProfile21(FanProfileWmi fanProfile)
	{
		if (driverControl == null)
		{
			Start();
		}
		return driverControl.WriteFanProfileNUC21(fanProfile);
	}

	public static bool WriteFanProfile22(FanProfileWmi fanProfile)
	{
		if (driverControl == null)
		{
			Start();
		}
		return driverControl.WriteFanProfileNUC22(fanProfile);
	}

	public static bool WriteFanProfile23(FanProfileWmi23 fanProfile)
	{
		if (driverControl == null)
		{
			Start();
		}
		return driverControl.WriteFanProfileNUC23(fanProfile);
	}
}
