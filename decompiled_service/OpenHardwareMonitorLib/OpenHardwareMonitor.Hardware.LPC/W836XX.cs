using System;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.LPC;

internal class W836XX : ISuperIO
{
	private readonly ushort address;

	private readonly byte revision;

	private readonly Chip chip;

	private readonly float?[] voltages = new float?[0];

	private readonly float?[] temperatures = new float?[0];

	private readonly float?[] fans = new float?[0];

	private readonly float?[] controls = new float?[0];

	private readonly bool[] peciTemperature = new bool[0];

	private readonly byte[] voltageRegister = new byte[0];

	private readonly byte[] voltageBank = new byte[0];

	private readonly float voltageGain = 0.008f;

	private const ushort WINBOND_VENDOR_ID = 23715;

	private const byte HIGH_BYTE = 128;

	private const byte ADDRESS_REGISTER_OFFSET = 5;

	private const byte DATA_REGISTER_OFFSET = 6;

	private const byte VOLTAGE_VBAT_REG = 81;

	private const byte BANK_SELECT_REGISTER = 78;

	private const byte VENDOR_ID_REGISTER = 79;

	private const byte TEMPERATURE_SOURCE_SELECT_REG = 73;

	private readonly byte[] TEMPERATURE_REG = new byte[3] { 80, 80, 39 };

	private readonly byte[] TEMPERATURE_BANK = new byte[3] { 1, 2, 0 };

	private readonly byte[] FAN_TACHO_REG = new byte[5] { 40, 41, 42, 63, 83 };

	private readonly byte[] FAN_TACHO_BANK = new byte[5] { 0, 0, 0, 0, 5 };

	private readonly byte[] FAN_BIT_REG = new byte[5] { 71, 75, 76, 89, 93 };

	private readonly byte[] FAN_DIV_BIT0 = new byte[5] { 36, 38, 30, 8, 10 };

	private readonly byte[] FAN_DIV_BIT1 = new byte[5] { 37, 39, 31, 9, 11 };

	private readonly byte[] FAN_DIV_BIT2 = new byte[5] { 5, 6, 7, 23, 15 };

	public Chip Chip => chip;

	public float?[] Voltages => voltages;

	public float?[] Temperatures => temperatures;

	public float?[] Fans => fans;

	public float?[] Controls => controls;

	private byte ReadByte(byte bank, byte register)
	{
		Ring0.WriteIoPort((ushort)(address + 5), 78);
		Ring0.WriteIoPort((ushort)(address + 6), bank);
		Ring0.WriteIoPort((ushort)(address + 5), register);
		return Ring0.ReadIoPort((ushort)(address + 6));
	}

	private void WriteByte(byte bank, byte register, byte value)
	{
		Ring0.WriteIoPort((ushort)(address + 5), 78);
		Ring0.WriteIoPort((ushort)(address + 6), bank);
		Ring0.WriteIoPort((ushort)(address + 5), register);
		Ring0.WriteIoPort((ushort)(address + 6), value);
	}

	public byte? ReadGPIO(int index)
	{
		return null;
	}

	public void WriteGPIO(int index, byte value)
	{
	}

	public void SetControl(int index, byte? value)
	{
	}

	public W836XX(Chip chip, byte revision, ushort address)
	{
		this.address = address;
		this.revision = revision;
		this.chip = chip;
		if (IsWinbondVendor())
		{
			temperatures = new float?[3];
			peciTemperature = new bool[3];
			switch (chip)
			{
			case Chip.W83667HG:
			case Chip.W83667HGB:
			{
				byte b2 = ReadByte(0, 73);
				peciTemperature[0] = (b2 & 4) != 0;
				peciTemperature[1] = (b2 & 0x40) != 0;
				peciTemperature[2] = false;
				break;
			}
			case Chip.W83627DHG:
			case Chip.W83627DHGP:
			{
				byte b = ReadByte(0, 73);
				peciTemperature[0] = (b & 7) != 0;
				peciTemperature[1] = (b & 0x70) != 0;
				peciTemperature[2] = false;
				break;
			}
			default:
				peciTemperature[0] = false;
				peciTemperature[1] = false;
				peciTemperature[2] = false;
				break;
			}
			switch (chip)
			{
			case Chip.W83627EHF:
				voltages = new float?[10];
				voltageRegister = new byte[10] { 32, 33, 34, 35, 36, 37, 38, 80, 81, 82 };
				voltageBank = new byte[10] { 0, 0, 0, 0, 0, 0, 0, 5, 5, 5 };
				voltageGain = 0.008f;
				fans = new float?[5];
				break;
			case Chip.W83627DHG:
			case Chip.W83667HG:
			case Chip.W83627DHGP:
			case Chip.W83667HGB:
				voltages = new float?[9];
				voltageRegister = new byte[9] { 32, 33, 34, 35, 36, 37, 38, 80, 81 };
				voltageBank = new byte[9] { 0, 0, 0, 0, 0, 0, 0, 5, 5 };
				voltageGain = 0.008f;
				fans = new float?[5];
				break;
			case Chip.W83627HF:
			case Chip.W83627THF:
			case Chip.W83687THF:
				voltages = new float?[7];
				voltageRegister = new byte[7] { 32, 33, 34, 35, 36, 80, 81 };
				voltageBank = new byte[7] { 0, 0, 0, 0, 0, 5, 5 };
				voltageGain = 0.016f;
				fans = new float?[3];
				break;
			}
		}
	}

	private bool IsWinbondVendor()
	{
		return (ushort)((ReadByte(128, 79) << 8) | ReadByte(0, 79)) == 23715;
	}

	private static ulong SetBit(ulong target, int bit, int value)
	{
		if ((value & 1) != value)
		{
			throw new ArgumentException("Value must be one bit only.");
		}
		if (bit < 0 || bit > 63)
		{
			throw new ArgumentException("Bit out of range.");
		}
		ulong num = (ulong)(1L << bit);
		if (value <= 0)
		{
			return target & ~num;
		}
		return target | num;
	}

	public void Update()
	{
		if (!Ring0.WaitIsaBusMutex(10))
		{
			return;
		}
		for (int i = 0; i < voltages.Length; i++)
		{
			if (voltageRegister[i] != 81)
			{
				float num2;
				if ((chip == Chip.W83627HF || chip == Chip.W83627THF || chip == Chip.W83687THF) && i == 0)
				{
					byte b = ReadByte(0, 24);
					int num = ReadByte(voltageBank[i], voltageRegister[i]);
					num2 = (((b & 1) != 0) ? (0.00488f * (float)num + 0.69f) : (0.016f * (float)num));
				}
				else
				{
					int num3 = ReadByte(voltageBank[i], voltageRegister[i]);
					num2 = voltageGain * (float)num3;
				}
				if (num2 > 0f)
				{
					voltages[i] = num2;
				}
				else
				{
					voltages[i] = null;
				}
			}
			else if ((ReadByte(0, 93) & 1) > 0)
			{
				voltages[i] = voltageGain * (float)(int)ReadByte(5, 81);
			}
			else
			{
				voltages[i] = null;
			}
		}
		for (int j = 0; j < temperatures.Length; j++)
		{
			int num4 = (sbyte)ReadByte(TEMPERATURE_BANK[j], TEMPERATURE_REG[j]) << 1;
			if (TEMPERATURE_BANK[j] > 0)
			{
				num4 |= ReadByte(TEMPERATURE_BANK[j], (byte)(TEMPERATURE_REG[j] + 1)) >> 7;
			}
			float num5 = (float)num4 / 2f;
			if (num5 <= 125f && num5 >= -55f && !peciTemperature[j])
			{
				temperatures[j] = num5;
			}
			else
			{
				temperatures[j] = null;
			}
		}
		ulong num6 = 0uL;
		for (int k = 0; k < FAN_BIT_REG.Length; k++)
		{
			num6 = (num6 << 8) | ReadByte(0, FAN_BIT_REG[k]);
		}
		ulong num7 = num6;
		for (int l = 0; l < fans.Length; l++)
		{
			int num8 = ReadByte(FAN_TACHO_BANK[l], FAN_TACHO_REG[l]);
			int num9 = (int)((((num6 >> (int)FAN_DIV_BIT2[l]) & 1) << 2) | (((num6 >> (int)FAN_DIV_BIT1[l]) & 1) << 1) | ((num6 >> (int)FAN_DIV_BIT0[l]) & 1));
			int num10 = 1 << num9;
			float value = ((num8 < 255) ? (1350000f / (float)(num8 * num10)) : 0f);
			fans[l] = value;
			if (num8 > 192 && num9 < 7)
			{
				num9++;
			}
			if (num8 < 96 && num9 > 0)
			{
				num9--;
			}
			num7 = SetBit(num7, FAN_DIV_BIT2[l], (num9 >> 2) & 1);
			num7 = SetBit(num7, FAN_DIV_BIT1[l], (num9 >> 1) & 1);
			num7 = SetBit(num7, FAN_DIV_BIT0[l], num9 & 1);
		}
		for (int num11 = FAN_BIT_REG.Length - 1; num11 >= 0; num11--)
		{
			byte b2 = (byte)(num6 & 0xFF);
			byte b3 = (byte)(num7 & 0xFF);
			num6 >>= 8;
			num7 >>= 8;
			if (b2 != b3)
			{
				WriteByte(0, FAN_BIT_REG[num11], b3);
			}
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
		stringBuilder.AppendLine(address.ToString("X4", CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		if (!Ring0.WaitIsaBusMutex(100))
		{
			return stringBuilder.ToString();
		}
		stringBuilder.AppendLine("Hardware Monitor Registers");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("      00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F");
		stringBuilder.AppendLine();
		for (int i = 0; i <= 7; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append((i << 4).ToString("X2", CultureInfo.InvariantCulture));
			stringBuilder.Append("  ");
			for (int j = 0; j <= 15; j++)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(ReadByte(0, (byte)((i << 4) | j)).ToString("X2", CultureInfo.InvariantCulture));
			}
			stringBuilder.AppendLine();
		}
		for (int k = 1; k <= 15; k++)
		{
			stringBuilder.AppendLine("Bank " + k);
			for (int l = 5; l < 6; l++)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append((l << 4).ToString("X2", CultureInfo.InvariantCulture));
				stringBuilder.Append("  ");
				for (int m = 0; m <= 15; m++)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(ReadByte((byte)k, (byte)((l << 4) | m)).ToString("X2", CultureInfo.InvariantCulture));
				}
				stringBuilder.AppendLine();
			}
		}
		stringBuilder.AppendLine();
		Ring0.ReleaseIsaBusMutex();
		return stringBuilder.ToString();
	}
}
