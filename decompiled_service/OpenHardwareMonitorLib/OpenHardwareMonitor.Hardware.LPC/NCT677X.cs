using System;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.LPC;

internal class NCT677X : ISuperIO
{
	private enum SourceNCT6771F : byte
	{
		SYSTIN = 1,
		CPUTIN,
		AUXTIN,
		SMBUSMASTER,
		PECI_0,
		PECI_1,
		PECI_2,
		PECI_3,
		PECI_4,
		PECI_5,
		PECI_6,
		PECI_7,
		PCH_CHIP_CPU_MAX_TEMP,
		PCH_CHIP_TEMP,
		PCH_CPU_TEMP,
		PCH_MCH_TEMP,
		PCH_DIM0_TEMP,
		PCH_DIM1_TEMP,
		PCH_DIM2_TEMP,
		PCH_DIM3_TEMP
	}

	private enum SourceNCT6776F : byte
	{
		SYSTIN = 1,
		CPUTIN,
		AUXTIN,
		SMBUSMASTER_0,
		SMBUSMASTER_1,
		SMBUSMASTER_2,
		SMBUSMASTER_3,
		SMBUSMASTER_4,
		SMBUSMASTER_5,
		SMBUSMASTER_6,
		SMBUSMASTER_7,
		PECI_0,
		PECI_1,
		PCH_CHIP_CPU_MAX_TEMP,
		PCH_CHIP_TEMP,
		PCH_CPU_TEMP,
		PCH_MCH_TEMP,
		PCH_DIM0_TEMP,
		PCH_DIM1_TEMP,
		PCH_DIM2_TEMP,
		PCH_DIM3_TEMP,
		BYTE_TEMP
	}

	private enum SourceNCT67XXD : byte
	{
		SYSTIN = 1,
		CPUTIN = 2,
		AUXTIN0 = 3,
		AUXTIN1 = 4,
		AUXTIN2 = 5,
		AUXTIN3 = 6,
		SMBUSMASTER_0 = 8,
		SMBUSMASTER_1 = 9,
		SMBUSMASTER_2 = 10,
		SMBUSMASTER_3 = 11,
		SMBUSMASTER_4 = 12,
		SMBUSMASTER_5 = 13,
		SMBUSMASTER_6 = 14,
		SMBUSMASTER_7 = 15,
		PECI_0 = 16,
		PECI_1 = 17,
		PCH_CHIP_CPU_MAX_TEMP = 18,
		PCH_CHIP_TEMP = 19,
		PCH_CPU_TEMP = 20,
		PCH_MCH_TEMP = 21,
		PCH_DIM0_TEMP = 22,
		PCH_DIM1_TEMP = 23,
		PCH_DIM2_TEMP = 24,
		PCH_DIM3_TEMP = 25,
		BYTE_TEMP = 26
	}

	private enum SourceNCT610X : byte
	{
		SYSTIN = 1,
		CPUTIN,
		AUXTIN,
		SMBUSMASTER_0,
		SMBUSMASTER_1,
		SMBUSMASTER_2,
		SMBUSMASTER_3,
		SMBUSMASTER_4,
		SMBUSMASTER_5,
		SMBUSMASTER_6,
		SMBUSMASTER_7,
		PECI_0,
		PECI_1,
		PCH_CHIP_CPU_MAX_TEMP,
		PCH_CHIP_TEMP,
		PCH_CPU_TEMP,
		PCH_MCH_TEMP,
		PCH_DIM0_TEMP,
		PCH_DIM1_TEMP,
		PCH_DIM2_TEMP,
		PCH_DIM3_TEMP,
		BYTE_TEMP
	}

	private readonly ushort port;

	private readonly byte revision;

	private readonly Chip chip;

	private readonly LPCPort lpcPort;

	private readonly bool isNuvotonVendor;

	private readonly float?[] voltages = new float?[0];

	private readonly float?[] temperatures = new float?[0];

	private readonly float?[] fans = new float?[0];

	private readonly float?[] controls = new float?[0];

	private const uint ADDRESS_REGISTER_OFFSET = 5u;

	private const uint DATA_REGISTER_OFFSET = 6u;

	private const byte BANK_SELECT_REGISTER = 78;

	private const ushort NUVOTON_VENDOR_ID = 23715;

	private readonly ushort VENDOR_ID_HIGH_REGISTER;

	private readonly ushort VENDOR_ID_LOW_REGISTER;

	private readonly ushort[] FAN_PWM_OUT_REG;

	private readonly ushort[] FAN_PWM_COMMAND_REG;

	private readonly ushort[] FAN_CONTROL_MODE_REG;

	private readonly ushort[] fanRpmBaseRegister;

	private readonly int minFanRPM;

	private readonly ushort[] fanCountRegister;

	private readonly int maxFanCount;

	private readonly int minFanCount;

	private bool[] restoreDefaultFanControlRequired = new bool[7];

	private byte[] initialFanControlMode = new byte[7];

	private byte[] initialFanPwmCommand = new byte[7];

	private readonly ushort[] voltageRegisters;

	private readonly ushort voltageVBatRegister;

	private readonly ushort vBatMonitorControlRegister;

	private readonly byte[] temperaturesSource;

	private readonly ushort[] temperatureRegister;

	private readonly ushort[] temperatureHalfRegister;

	private readonly int[] temperatureHalfBit;

	private readonly ushort[] temperatureSourceRegister;

	private readonly ushort?[] alternateTemperatureRegister;

	public Chip Chip => chip;

	public float?[] Voltages => voltages;

	public float?[] Temperatures => temperatures;

	public float?[] Fans => fans;

	public float?[] Controls => controls;

	private byte ReadByte(ushort address)
	{
		byte value = (byte)(address >> 8);
		byte value2 = (byte)(address & 0xFF);
		Ring0.WriteIoPort((uint)(port + 5), 78);
		Ring0.WriteIoPort((uint)(port + 6), value);
		Ring0.WriteIoPort((uint)(port + 5), value2);
		return Ring0.ReadIoPort((uint)(port + 6));
	}

	private void WriteByte(ushort address, byte value)
	{
		byte value2 = (byte)(address >> 8);
		byte value3 = (byte)(address & 0xFF);
		Ring0.WriteIoPort((uint)(port + 5), 78);
		Ring0.WriteIoPort((uint)(port + 6), value2);
		Ring0.WriteIoPort((uint)(port + 5), value3);
		Ring0.WriteIoPort((uint)(port + 6), value);
	}

	public NCT677X(Chip chip, byte revision, ushort port, LPCPort lpcPort)
	{
		this.chip = chip;
		this.revision = revision;
		this.port = port;
		this.lpcPort = lpcPort;
		if (chip == Chip.NCT610X)
		{
			VENDOR_ID_HIGH_REGISTER = 33022;
			VENDOR_ID_LOW_REGISTER = 254;
			FAN_PWM_OUT_REG = new ushort[3] { 74, 75, 76 };
			FAN_PWM_COMMAND_REG = new ushort[3] { 281, 297, 313 };
			FAN_CONTROL_MODE_REG = new ushort[3] { 275, 291, 307 };
			vBatMonitorControlRegister = 792;
		}
		else
		{
			VENDOR_ID_HIGH_REGISTER = 32847;
			VENDOR_ID_LOW_REGISTER = 79;
			FAN_PWM_OUT_REG = new ushort[7] { 1, 3, 17, 19, 21, 23, 41 };
			FAN_PWM_COMMAND_REG = new ushort[7] { 265, 521, 777, 2057, 2313, 2569, 2825 };
			FAN_CONTROL_MODE_REG = new ushort[7] { 258, 514, 770, 2050, 2306, 2562, 2818 };
			vBatMonitorControlRegister = 93;
		}
		isNuvotonVendor = IsNuvotonVendor();
		if (!isNuvotonVendor)
		{
			return;
		}
		switch (chip)
		{
		case Chip.NCT6771F:
		case Chip.NCT6776F:
			if (chip == Chip.NCT6771F)
			{
				fans = new float?[4];
				minFanRPM = 20;
				temperaturesSource = new byte[4] { 5, 2, 3, 1 };
			}
			else
			{
				fans = new float?[5];
				minFanRPM = 164;
				temperaturesSource = new byte[4] { 12, 2, 3, 1 };
			}
			fanRpmBaseRegister = new ushort[5] { 1622, 1624, 1626, 1628, 1630 };
			controls = new float?[3];
			voltages = new float?[9];
			voltageRegisters = new ushort[9] { 32, 33, 34, 35, 36, 37, 38, 1360, 1361 };
			voltageVBatRegister = 1361;
			temperatures = new float?[4];
			temperatureRegister = new ushort[9] { 39, 115, 117, 119, 336, 592, 1579, 1580, 1581 };
			temperatureHalfRegister = new ushort[9] { 0, 116, 118, 120, 337, 593, 1582, 1582, 1582 };
			temperatureHalfBit = new int[9] { -1, 7, 7, 7, 7, 7, 0, 1, 2 };
			temperatureSourceRegister = new ushort[9] { 1569, 256, 512, 768, 1570, 1571, 1572, 1573, 1574 };
			alternateTemperatureRegister = new ushort?[4];
			break;
		case Chip.NCT6779D:
		case Chip.NCT6791D:
		case Chip.NCT6792D:
		case Chip.NCT6792DA:
		case Chip.NCT6793D:
		case Chip.NCT6795D:
		case Chip.NCT6796D:
		case Chip.NCT6796DR:
		case Chip.NCT6798D:
		case Chip.NCT6797D:
			switch (chip)
			{
			case Chip.NCT6791D:
			case Chip.NCT6792D:
			case Chip.NCT6792DA:
			case Chip.NCT6793D:
			case Chip.NCT6795D:
				fans = new float?[6];
				controls = new float?[6];
				break;
			case Chip.NCT6796D:
			case Chip.NCT6796DR:
			case Chip.NCT6798D:
			case Chip.NCT6797D:
				fans = new float?[7];
				controls = new float?[7];
				break;
			default:
				fans = new float?[5];
				controls = new float?[5];
				break;
			}
			fanCountRegister = new ushort[7] { 1200, 1202, 1204, 1206, 1208, 1210, 1228 };
			maxFanCount = 8191;
			minFanCount = 21;
			voltages = new float?[15];
			voltageRegisters = new ushort[15]
			{
				1152, 1153, 1154, 1155, 1156, 1157, 1158, 1159, 1160, 1161,
				1162, 1163, 1164, 1165, 1166
			};
			voltageVBatRegister = 1160;
			temperatures = new float?[7];
			temperaturesSource = new byte[7] { 16, 2, 1, 3, 4, 5, 6 };
			temperatureRegister = new ushort[7] { 39, 115, 117, 119, 121, 123, 336 };
			temperatureHalfRegister = new ushort[7] { 0, 116, 118, 120, 122, 124, 337 };
			temperatureHalfBit = new int[7] { -1, 7, 7, 7, 7, 7, 7 };
			temperatureSourceRegister = new ushort[7] { 1569, 256, 512, 768, 2048, 2304, 1570 };
			alternateTemperatureRegister = new ushort?[7]
			{
				null,
				(ushort)1169,
				(ushort)1168,
				(ushort)1170,
				(ushort)1171,
				(ushort)1172,
				(ushort)1173
			};
			break;
		case Chip.NCT610X:
			fans = new float?[3];
			controls = new float?[3];
			fanRpmBaseRegister = new ushort[3] { 48, 50, 52 };
			minFanRPM = 164;
			voltages = new float?[9];
			voltageRegisters = new ushort[9] { 768, 769, 770, 771, 772, 773, 775, 776, 777 };
			voltageVBatRegister = 776;
			temperatures = new float?[4];
			temperaturesSource = new byte[4] { 12, 1, 2, 3 };
			temperatureRegister = new ushort[4] { 39, 24, 25, 26 };
			temperatureHalfRegister = new ushort[4] { 0, 27, 283, 539 };
			temperatureHalfBit = new int[4] { -1, 7, 7, 7 };
			temperatureSourceRegister = new ushort[4] { 1569, 256, 512, 768 };
			alternateTemperatureRegister = new ushort?[4]
			{
				null,
				(ushort)24,
				(ushort)25,
				(ushort)26
			};
			break;
		}
	}

	private bool IsNuvotonVendor()
	{
		return ((ReadByte(VENDOR_ID_HIGH_REGISTER) << 8) | ReadByte(VENDOR_ID_LOW_REGISTER)) == 23715;
	}

	public byte? ReadGPIO(int index)
	{
		return null;
	}

	public void WriteGPIO(int index, byte value)
	{
	}

	private void SaveDefaultFanControl(int index)
	{
		if (!restoreDefaultFanControlRequired[index])
		{
			initialFanControlMode[index] = ReadByte(FAN_CONTROL_MODE_REG[index]);
			initialFanPwmCommand[index] = ReadByte(FAN_PWM_COMMAND_REG[index]);
			restoreDefaultFanControlRequired[index] = true;
		}
	}

	private void RestoreDefaultFanControl(int index)
	{
		if (restoreDefaultFanControlRequired[index])
		{
			WriteByte(FAN_CONTROL_MODE_REG[index], initialFanControlMode[index]);
			WriteByte(FAN_PWM_COMMAND_REG[index], initialFanPwmCommand[index]);
			restoreDefaultFanControlRequired[index] = false;
		}
	}

	public void SetControl(int index, byte? value)
	{
		if (!isNuvotonVendor)
		{
			return;
		}
		if (index < 0 || index >= controls.Length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (Ring0.WaitIsaBusMutex(10))
		{
			if (value.HasValue)
			{
				SaveDefaultFanControl(index);
				WriteByte(FAN_CONTROL_MODE_REG[index], 0);
				WriteByte(FAN_PWM_COMMAND_REG[index], value.Value);
			}
			else
			{
				RestoreDefaultFanControl(index);
			}
			Ring0.ReleaseIsaBusMutex();
		}
	}

	private void DisableIOSpaceLock()
	{
		if ((chip == Chip.NCT6791D || chip == Chip.NCT6792D || chip == Chip.NCT6792DA || chip == Chip.NCT6793D || chip == Chip.NCT6795D || chip == Chip.NCT6796D || chip == Chip.NCT6796DR || chip == Chip.NCT6797D || chip == Chip.NCT6798D) && !IsNuvotonVendor())
		{
			lpcPort.WinbondNuvotonFintekEnter();
			lpcPort.NuvotonDisableIOSpaceLock();
			lpcPort.WinbondNuvotonFintekExit();
		}
	}

	public void Update()
	{
		if (!isNuvotonVendor || !Ring0.WaitIsaBusMutex(10))
		{
			return;
		}
		DisableIOSpaceLock();
		for (int i = 0; i < voltages.Length; i++)
		{
			float num = 0.008f * (float)(int)ReadByte(voltageRegisters[i]);
			bool flag = num > 0f;
			if (flag && voltageRegisters[i] == voltageVBatRegister)
			{
				flag = (ReadByte(vBatMonitorControlRegister) & 1) > 0;
			}
			voltages[i] = (flag ? new float?(num) : ((float?)null));
		}
		int num2 = 0;
		for (int num3 = temperatureRegister.Length - 1; num3 >= 0; num3--)
		{
			int num4 = (sbyte)ReadByte(temperatureRegister[num3]) << 1;
			if (temperatureHalfBit[num3] > 0)
			{
				num4 |= (ReadByte(temperatureHalfRegister[num3]) >> temperatureHalfBit[num3]) & 1;
			}
			byte b = ReadByte(temperatureSourceRegister[num3]);
			num2 |= 1 << (int)b;
			float? num5 = 0.5f * (float)num4;
			if (num5 > 125f || num5 < -55f)
			{
				num5 = null;
			}
			for (int j = 0; j < temperatures.Length; j++)
			{
				if (temperaturesSource[j] == b)
				{
					temperatures[j] = num5;
				}
			}
		}
		for (int k = 0; k < alternateTemperatureRegister.Length; k++)
		{
			if (alternateTemperatureRegister[k].HasValue && (num2 & (1 << (int)temperaturesSource[k])) <= 0)
			{
				float? num6 = (sbyte)ReadByte(alternateTemperatureRegister[k].Value);
				if (num6 > 125f || num6 < -55f)
				{
					num6 = null;
				}
				temperatures[k] = num6;
			}
		}
		for (int l = 0; l < fans.Length; l++)
		{
			if (fanCountRegister != null)
			{
				byte b2 = ReadByte(fanCountRegister[l]);
				byte b3 = ReadByte((ushort)(fanCountRegister[l] + 1));
				int num7 = (b2 << 5) | (b3 & 0x1F);
				if (num7 < maxFanCount)
				{
					if (num7 >= minFanCount)
					{
						fans[l] = 1350000f / (float)num7;
					}
					else
					{
						fans[l] = null;
					}
				}
				else
				{
					fans[l] = 0f;
				}
			}
			else
			{
				byte b4 = ReadByte(fanRpmBaseRegister[l]);
				byte b5 = ReadByte((ushort)(fanRpmBaseRegister[l] + 1));
				int num8 = (b4 << 8) | b5;
				fans[l] = ((num8 > minFanRPM) ? num8 : 0);
			}
		}
		for (int m = 0; m < controls.Length; m++)
		{
			int num9 = ReadByte(FAN_PWM_OUT_REG[m]);
			controls[m] = (float)num9 / 2.55f;
		}
		Ring0.ReleaseIsaBusMutex();
	}

	public string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("LPC " + GetType().Name);
		stringBuilder.AppendLine();
		stringBuilder.Append("Chip ID: 0x");
		stringBuilder.AppendLine(chip.ToString("X"));
		stringBuilder.Append("Chip revision: 0x");
		stringBuilder.AppendLine(revision.ToString("X", CultureInfo.InvariantCulture));
		stringBuilder.Append("Base Adress: 0x");
		stringBuilder.AppendLine(port.ToString("X4", CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		if (!Ring0.WaitIsaBusMutex(100))
		{
			return stringBuilder.ToString();
		}
		ushort[] array = new ushort[100]
		{
			0, 16, 32, 48, 64, 80, 96, 112, 240, 256,
			272, 288, 304, 320, 336, 512, 528, 544, 560, 576,
			592, 608, 768, 800, 816, 832, 864, 1024, 1040, 1056,
			1088, 1104, 1120, 1152, 1168, 1200, 1216, 1264, 1280, 1360,
			1376, 1536, 1552, 1568, 1584, 1600, 1616, 1632, 1648, 1792,
			1808, 1824, 1840, 2048, 2080, 2096, 2112, 2304, 2336, 2352,
			2368, 2400, 2560, 2576, 2592, 2608, 2624, 2640, 2656, 2672,
			2816, 2832, 2848, 2864, 2896, 2912, 2928, 3072, 3088, 3104,
			3120, 3152, 3168, 3184, 3328, 3344, 3360, 3376, 3408, 3424,
			3584, 3600, 3616, 3632, 3840, 3856, 3872, 3888, 32832, 33008
		};
		stringBuilder.AppendLine("Hardware Monitor Registers");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("        00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F");
		stringBuilder.AppendLine();
		ushort[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ushort num = array2[i];
			stringBuilder.Append(" ");
			stringBuilder.Append(num.ToString("X4", CultureInfo.InvariantCulture));
			stringBuilder.Append("  ");
			for (ushort num2 = 0; num2 <= 15; num2++)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(ReadByte((ushort)(num | num2)).ToString("X2", CultureInfo.InvariantCulture));
			}
			stringBuilder.AppendLine();
		}
		stringBuilder.AppendLine();
		Ring0.ReleaseIsaBusMutex();
		return stringBuilder.ToString();
	}
}
