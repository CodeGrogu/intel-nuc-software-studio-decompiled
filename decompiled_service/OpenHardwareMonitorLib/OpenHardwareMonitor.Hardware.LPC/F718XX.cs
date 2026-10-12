using System;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.LPC;

internal class F718XX : ISuperIO
{
	private readonly ushort address;

	private readonly Chip chip;

	private readonly float?[] voltages;

	private readonly float?[] temperatures;

	private readonly float?[] fans;

	private readonly float?[] controls;

	private const byte ADDRESS_REGISTER_OFFSET = 5;

	private const byte DATA_REGISTER_OFFSET = 6;

	private const byte PWM_VALUES_OFFSET = 45;

	private const byte VOLTAGE_BASE_REG = 32;

	private const byte TEMPERATURE_CONFIG_REG = 105;

	private const byte TEMPERATURE_BASE_REG = 112;

	private readonly byte[] FAN_TACHOMETER_REG = new byte[4] { 160, 176, 192, 208 };

	private readonly byte[] FAN_PWM_REG = new byte[4] { 163, 179, 195, 211 };

	private bool[] restoreDefaultFanPwmControlRequired = new bool[4];

	private byte[] initialFanPwmControl = new byte[4];

	public Chip Chip => chip;

	public float?[] Voltages => voltages;

	public float?[] Temperatures => temperatures;

	public float?[] Fans => fans;

	public float?[] Controls => controls;

	private byte ReadByte(byte register)
	{
		Ring0.WriteIoPort((ushort)(address + 5), register);
		return Ring0.ReadIoPort((ushort)(address + 6));
	}

	private void WriteByte(byte register, byte value)
	{
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

	private void SaveDefaultFanPwmControl(int index)
	{
		if (!restoreDefaultFanPwmControlRequired[index])
		{
			initialFanPwmControl[index] = ReadByte(FAN_PWM_REG[index]);
			restoreDefaultFanPwmControlRequired[index] = true;
		}
	}

	private void RestoreDefaultFanPwmControl(int index)
	{
		if (restoreDefaultFanPwmControlRequired[index])
		{
			WriteByte(FAN_PWM_REG[index], initialFanPwmControl[index]);
			restoreDefaultFanPwmControlRequired[index] = false;
		}
	}

	public void SetControl(int index, byte? value)
	{
		if (index < 0 || index >= controls.Length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (Ring0.WaitIsaBusMutex(10))
		{
			if (value.HasValue)
			{
				SaveDefaultFanPwmControl(index);
				WriteByte(FAN_PWM_REG[index], value.Value);
			}
			else
			{
				RestoreDefaultFanPwmControl(index);
			}
			Ring0.ReleaseIsaBusMutex();
		}
	}

	public F718XX(Chip chip, ushort address)
	{
		this.address = address;
		this.chip = chip;
		voltages = new float?[(chip == Chip.F71858) ? 3 : 9];
		temperatures = new float?[(chip == Chip.F71808E) ? 2 : 3];
		fans = new float?[(chip == Chip.F71882 || chip == Chip.F71858) ? 4 : 3];
		controls = new float?[(chip == Chip.F71878AD) ? 3 : 0];
	}

	public string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("LPC " + GetType().Name);
		stringBuilder.AppendLine();
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
		for (int i = 0; i <= 15; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append((i << 4).ToString("X2", CultureInfo.InvariantCulture));
			stringBuilder.Append("  ");
			for (int j = 0; j <= 15; j++)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(ReadByte((byte)((i << 4) | j)).ToString("X2", CultureInfo.InvariantCulture));
			}
			stringBuilder.AppendLine();
		}
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
			if (chip == Chip.F71808E && i == 6)
			{
				voltages[i] = 0f;
				continue;
			}
			int num = ReadByte((byte)(32 + i));
			voltages[i] = 0.008f * (float)num;
		}
		for (int j = 0; j < temperatures.Length; j++)
		{
			if (chip == Chip.F71858)
			{
				int num2 = 3 & ReadByte(105);
				int num3 = ReadByte((byte)(112 + 2 * j));
				int num4 = ReadByte((byte)(112 + 2 * j + 1));
				if (num3 != 187 && num3 != 204)
				{
					int num5 = 0;
					switch (num2)
					{
					case 0:
						num5 = 0;
						break;
					case 1:
						num5 = 0;
						break;
					case 2:
						num5 = (num3 & 0x80) << 8;
						break;
					case 3:
						num5 = (num4 & 1) << 15;
						break;
					}
					num5 |= num3 << 7;
					num5 |= (num4 & 0xE0) >> 1;
					short num6 = (short)(num5 & 0xFFF0);
					temperatures[j] = (float)num6 / 128f;
				}
				else
				{
					temperatures[j] = null;
				}
			}
			else
			{
				sbyte b = (sbyte)ReadByte((byte)(112 + 2 * (j + 1)));
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
		for (int k = 0; k < fans.Length; k++)
		{
			int num7 = ReadByte(FAN_TACHOMETER_REG[k]) << 8;
			num7 |= ReadByte((byte)(FAN_TACHOMETER_REG[k] + 1));
			if (num7 > 0)
			{
				fans[k] = ((num7 < 4095) ? (1500000f / (float)num7) : 0f);
			}
			else
			{
				fans[k] = null;
			}
		}
		for (int l = 0; l < controls.Length; l++)
		{
			controls[l] = (float)(int)ReadByte((byte)(45 + l)) * 100f / 255f;
		}
		Ring0.ReleaseIsaBusMutex();
	}
}
