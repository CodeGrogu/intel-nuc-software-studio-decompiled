using System;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.LPC;

internal class IT87XX : ISuperIO
{
	private readonly ushort address;

	private readonly Chip chip;

	private readonly byte version;

	private readonly ushort gpioAddress;

	private readonly int gpioCount;

	private readonly ushort addressReg;

	private readonly ushort dataReg;

	private readonly float?[] voltages = new float?[0];

	private readonly float?[] temperatures = new float?[0];

	private readonly float?[] fans = new float?[0];

	private readonly float?[] controls = new float?[0];

	private readonly float voltageGain;

	private readonly bool has16bitFanCounter;

	private const byte ITE_VENDOR_ID = 144;

	private const byte ADDRESS_REGISTER_OFFSET = 5;

	private const byte DATA_REGISTER_OFFSET = 6;

	private const byte CONFIGURATION_REGISTER = 0;

	private const byte TEMPERATURE_BASE_REG = 41;

	private const byte VENDOR_ID_REGISTER = 88;

	private const byte FAN_TACHOMETER_DIVISOR_REGISTER = 11;

	private readonly byte[] FAN_TACHOMETER_REG = new byte[5] { 13, 14, 15, 128, 130 };

	private readonly byte[] FAN_TACHOMETER_EXT_REG = new byte[5] { 24, 25, 26, 129, 131 };

	private const byte VOLTAGE_BASE_REG = 32;

	private const byte FAN_MAIN_CTRL_REG = 19;

	private readonly byte[] FAN_PWM_CTRL_REG;

	private readonly byte[] FAN_PWM_CTRL_EXT_REG = new byte[5] { 99, 107, 115, 123, 163 };

	private bool[] restoreDefaultFanPwmControlRequired = new bool[5];

	private bool[] initialFanOutputModeEnabled = new bool[3];

	private byte[] initialFanPwmControl = new byte[5];

	private byte[] initialFanPwmControlExt = new byte[5];

	public Chip Chip => chip;

	public float?[] Voltages => voltages;

	public float?[] Temperatures => temperatures;

	public float?[] Fans => fans;

	public float?[] Controls => controls;

	private byte ReadByte(byte register, out bool valid)
	{
		Ring0.WriteIoPort(addressReg, register);
		byte result = Ring0.ReadIoPort(dataReg);
		if (chip == Chip.IT8688E)
		{
			valid = true;
			return result;
		}
		valid = register == Ring0.ReadIoPort(addressReg);
		return result;
	}

	private bool WriteByte(byte register, byte value)
	{
		Ring0.WriteIoPort(addressReg, register);
		Ring0.WriteIoPort(dataReg, value);
		return register == Ring0.ReadIoPort(addressReg);
	}

	public byte? ReadGPIO(int index)
	{
		if (index >= gpioCount)
		{
			return null;
		}
		return Ring0.ReadIoPort((ushort)(gpioAddress + index));
	}

	public void WriteGPIO(int index, byte value)
	{
		if (index < gpioCount)
		{
			Ring0.WriteIoPort((ushort)(gpioAddress + index), value);
		}
	}

	private void SaveDefaultFanPwmControl(int index)
	{
		if (!restoreDefaultFanPwmControlRequired[index])
		{
			initialFanPwmControl[index] = ReadByte(FAN_PWM_CTRL_REG[index], out var valid);
			if (index < 3)
			{
				initialFanOutputModeEnabled[index] = (ReadByte(19, out valid) & (1 << index)) > 0;
			}
			if (chip == Chip.IT8721F || chip == Chip.IT8665E || chip == Chip.IT8686E || chip == Chip.IT8688E || chip == Chip.IT879XE)
			{
				initialFanPwmControlExt[index] = ReadByte(FAN_PWM_CTRL_EXT_REG[index], out valid);
			}
			restoreDefaultFanPwmControlRequired[index] = true;
		}
	}

	private void RestoreDefaultFanPwmControl(int index)
	{
		if (!restoreDefaultFanPwmControlRequired[index])
		{
			return;
		}
		WriteByte(FAN_PWM_CTRL_REG[index], initialFanPwmControl[index]);
		if (index < 3)
		{
			byte b = ReadByte(19, out var _);
			if ((b & (1 << index)) > 0 != initialFanOutputModeEnabled[index])
			{
				WriteByte(19, (byte)(b ^ (1 << index)));
			}
		}
		if (chip == Chip.IT8721F || chip == Chip.IT8665E || chip == Chip.IT8686E || chip == Chip.IT8688E || chip == Chip.IT879XE)
		{
			WriteByte(FAN_PWM_CTRL_EXT_REG[index], initialFanPwmControlExt[index]);
		}
		restoreDefaultFanPwmControlRequired[index] = false;
	}

	public void SetControl(int index, byte? value)
	{
		if (index < 0 || index >= controls.Length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (!Ring0.WaitIsaBusMutex(10))
		{
			return;
		}
		if (value.HasValue)
		{
			SaveDefaultFanPwmControl(index);
			if (index < 3 && !initialFanOutputModeEnabled[index])
			{
				WriteByte(19, (byte)(ReadByte(19, out var _) | (1 << index)));
			}
			if (chip == Chip.IT8721F || chip == Chip.IT8665E || chip == Chip.IT8686E || chip == Chip.IT8688E || chip == Chip.IT879XE)
			{
				WriteByte(FAN_PWM_CTRL_REG[index], (byte)(initialFanPwmControl[index] & 0x7F));
				WriteByte(FAN_PWM_CTRL_EXT_REG[index], value.Value);
			}
			else
			{
				WriteByte(FAN_PWM_CTRL_REG[index], (byte)(value.Value >> 1));
			}
		}
		else
		{
			RestoreDefaultFanPwmControl(index);
		}
		Ring0.ReleaseIsaBusMutex();
	}

	public IT87XX(Chip chip, ushort address, ushort gpioAddress, byte version)
	{
		this.address = address;
		this.chip = chip;
		this.version = version;
		addressReg = (ushort)(address + 5);
		dataReg = (ushort)(address + 6);
		this.gpioAddress = gpioAddress;
		byte b = ReadByte(88, out var valid);
		if (valid && b == 144 && ((ReadByte(0, out valid) & 0x10) != 0 || chip == Chip.IT8655E || chip == Chip.IT8665E) && valid)
		{
			if (chip == Chip.IT8665E)
			{
				FAN_PWM_CTRL_REG = new byte[5] { 21, 22, 23, 30, 31 };
			}
			else
			{
				FAN_PWM_CTRL_REG = new byte[5] { 21, 22, 23, 127, 167 };
			}
			switch (chip)
			{
			case Chip.IT8665E:
			case Chip.IT8686E:
			case Chip.IT8688E:
				voltages = new float?[9];
				temperatures = new float?[6];
				fans = new float?[5];
				controls = new float?[5];
				break;
			case Chip.IT8655E:
				voltages = new float?[9];
				temperatures = new float?[6];
				fans = new float?[3];
				break;
			case Chip.IT879XE:
				voltages = new float?[9];
				temperatures = new float?[3];
				fans = new float?[3];
				controls = new float?[3];
				break;
			case Chip.IT8705F:
				voltages = new float?[9];
				temperatures = new float?[3];
				fans = new float?[3];
				controls = new float?[3];
				break;
			default:
				voltages = new float?[9];
				temperatures = new float?[3];
				fans = new float?[5];
				controls = new float?[3];
				break;
			}
			switch (chip)
			{
			case Chip.IT8620E:
			case Chip.IT8628E:
			case Chip.IT8686E:
			case Chip.IT8688E:
			case Chip.IT8721F:
			case Chip.IT8728F:
			case Chip.IT8771E:
			case Chip.IT8772E:
				voltageGain = 0.012f;
				break;
			case Chip.IT8655E:
			case Chip.IT8665E:
			case Chip.IT879XE:
				voltageGain = 0.011f;
				break;
			default:
				voltageGain = 0.016f;
				break;
			}
			if ((chip == Chip.IT8705F && version < 3) || (chip == Chip.IT8712F && version < 8))
			{
				has16bitFanCounter = false;
			}
			else
			{
				has16bitFanCounter = true;
			}
			switch (chip)
			{
			case Chip.IT8712F:
			case Chip.IT8716F:
			case Chip.IT8718F:
			case Chip.IT8726F:
				gpioCount = 5;
				break;
			case Chip.IT8720F:
			case Chip.IT8721F:
				gpioCount = 8;
				break;
			default:
				gpioCount = 0;
				break;
			}
		}
	}

	public string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("LPC " + GetType().Name);
		stringBuilder.AppendLine();
		stringBuilder.Append("Chip ID: 0x");
		stringBuilder.AppendLine(chip.ToString("X"));
		stringBuilder.Append("Chip Version: 0x");
		stringBuilder.AppendLine(version.ToString("X", CultureInfo.InvariantCulture));
		stringBuilder.Append("Base Address: 0x");
		stringBuilder.AppendLine(address.ToString("X4", CultureInfo.InvariantCulture));
		stringBuilder.Append("GPIO Address: 0x");
		stringBuilder.AppendLine(gpioAddress.ToString("X4", CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		if (!Ring0.WaitIsaBusMutex(100))
		{
			return stringBuilder.ToString();
		}
		stringBuilder.AppendLine("Environment Controller Registers");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("      00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F");
		stringBuilder.AppendLine();
		for (int i = 0; i <= 10; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append((i << 4).ToString("X2", CultureInfo.InvariantCulture));
			stringBuilder.Append("  ");
			for (int j = 0; j <= 15; j++)
			{
				stringBuilder.Append(" ");
				byte b = ReadByte((byte)((i << 4) | j), out var valid);
				stringBuilder.Append(valid ? b.ToString("X2", CultureInfo.InvariantCulture) : "??");
			}
			stringBuilder.AppendLine();
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("GPIO Registers");
		stringBuilder.AppendLine();
		for (int k = 0; k < gpioCount; k++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append(ReadGPIO(k).Value.ToString("X2", CultureInfo.InvariantCulture));
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		Ring0.ReleaseIsaBusMutex();
		return stringBuilder.ToString();
	}

	public void Update()
	{
		if (!Ring0.WaitIsaBusMutex(10))
		{
			return;
		}
		for (int i = 0; i < voltages.Length; i++)
		{
			float num = voltageGain * (float)(int)ReadByte((byte)(32 + i), out var valid);
			if (valid)
			{
				if (num > 0f)
				{
					voltages[i] = num;
				}
				else
				{
					voltages[i] = null;
				}
			}
		}
		for (int j = 0; j < temperatures.Length; j++)
		{
			sbyte b = (sbyte)ReadByte((byte)(41 + j), out var valid2);
			if (valid2)
			{
				if (b < sbyte.MaxValue && b > 0)
				{
					temperatures[j] = b;
				}
				else
				{
					temperatures[j] = null;
				}
			}
		}
		if (has16bitFanCounter)
		{
			for (int k = 0; k < fans.Length; k++)
			{
				int num2 = ReadByte(FAN_TACHOMETER_REG[k], out var valid3);
				if (!valid3)
				{
					continue;
				}
				num2 |= ReadByte(FAN_TACHOMETER_EXT_REG[k], out valid3) << 8;
				if (valid3)
				{
					if (num2 > 63)
					{
						fans[k] = ((num2 < 65535) ? (1350000f / (float)(num2 * 2)) : 0f);
					}
					else
					{
						fans[k] = null;
					}
				}
			}
		}
		else
		{
			for (int l = 0; l < fans.Length; l++)
			{
				int num3 = ReadByte(FAN_TACHOMETER_REG[l], out var valid4);
				if (!valid4)
				{
					continue;
				}
				int num4 = 2;
				if (l < 2)
				{
					int num5 = ReadByte(11, out valid4);
					if (!valid4)
					{
						continue;
					}
					num4 = 1 << ((num5 >> 3 * l) & 7);
				}
				if (num3 > 0)
				{
					fans[l] = ((num3 < 255) ? (1350000f / (float)(num3 * num4)) : 0f);
				}
				else
				{
					fans[l] = null;
				}
			}
		}
		for (int m = 0; m < controls.Length; m++)
		{
			byte b2 = ReadByte(FAN_PWM_CTRL_REG[m], out var valid5);
			if (!valid5)
			{
				continue;
			}
			if ((b2 & 0x80) > 0)
			{
				controls[m] = null;
			}
			else if (chip == Chip.IT8721F || chip == Chip.IT8665E || chip == Chip.IT8686E || chip == Chip.IT8688E || chip == Chip.IT879XE)
			{
				b2 = ReadByte(FAN_PWM_CTRL_EXT_REG[m], out valid5);
				if (valid5)
				{
					controls[m] = (float)Math.Round((float)(int)b2 * 100f / 255f);
				}
			}
			else
			{
				controls[m] = (float)Math.Round((float)(b2 & 0x7F) * 100f / 127f);
			}
		}
		Ring0.ReleaseIsaBusMutex();
	}
}
