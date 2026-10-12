using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace OpenHardwareMonitor.Hardware.LPC;

internal class LPCIO
{
	private readonly List<ISuperIO> superIOs = new List<ISuperIO>();

	private readonly StringBuilder report = new StringBuilder();

	private readonly ushort[] REGISTER_PORTS = new ushort[2] { 46, 78 };

	private readonly ushort[] VALUE_PORTS = new ushort[2] { 47, 79 };

	private const byte CHIP_ID_REGISTER = 32;

	private const byte CHIP_REVISION_REGISTER = 33;

	private const byte BASE_ADDRESS_REGISTER = 96;

	private const byte FINTEK_VENDOR_ID_REGISTER = 35;

	private const ushort FINTEK_VENDOR_ID = 6452;

	private const byte WINBOND_NUVOTON_HARDWARE_MONITOR_LDN = 11;

	private const byte F71858_HARDWARE_MONITOR_LDN = 2;

	private const byte FINTEK_HARDWARE_MONITOR_LDN = 4;

	private const byte IT87_ENVIRONMENT_CONTROLLER_LDN = 4;

	private const byte IT8705_GPIO_LDN = 5;

	private const byte IT87XX_GPIO_LDN = 7;

	private const byte IT87_CHIP_VERSION_REGISTER = 34;

	public ISuperIO[] SuperIO => superIOs.ToArray();

	private void ReportUnknownChip(LPCPort port, string type, int chip)
	{
		report.Append("Chip ID: Unknown ");
		report.Append(type);
		report.Append(" with ID 0x");
		report.Append(chip.ToString("X", CultureInfo.InvariantCulture));
		report.Append(" at 0x");
		report.Append(port.RegisterPort.ToString("X", CultureInfo.InvariantCulture));
		report.Append("/0x");
		report.AppendLine(port.ValuePort.ToString("X", CultureInfo.InvariantCulture));
		report.AppendLine();
	}

	private bool DetectWinbondFintek(LPCPort port)
	{
		port.WinbondNuvotonFintekEnter();
		byte logicalDeviceNumber = 0;
		byte b = port.ReadByte(32);
		byte b2 = port.ReadByte(33);
		Chip chip = Chip.Unknown;
		switch (b)
		{
		case 5:
			switch (b2)
			{
			case 7:
				chip = Chip.F71858;
				logicalDeviceNumber = 2;
				break;
			case 65:
				chip = Chip.F71882;
				logicalDeviceNumber = 4;
				break;
			}
			break;
		case 6:
			if (b2 == 1)
			{
				chip = Chip.F71862;
				logicalDeviceNumber = 4;
			}
			break;
		case 7:
			if (b2 == 35)
			{
				chip = Chip.F71889F;
				logicalDeviceNumber = 4;
			}
			break;
		case 8:
			if (b2 == 20)
			{
				chip = Chip.F71869;
				logicalDeviceNumber = 4;
			}
			break;
		case 9:
			switch (b2)
			{
			case 1:
				chip = Chip.F71808E;
				logicalDeviceNumber = 4;
				break;
			case 9:
				chip = Chip.F71889ED;
				logicalDeviceNumber = 4;
				break;
			}
			break;
		case 16:
			switch (b2)
			{
			case 5:
				chip = Chip.F71889AD;
				logicalDeviceNumber = 4;
				break;
			case 7:
				chip = Chip.F71869A;
				logicalDeviceNumber = 4;
				break;
			}
			break;
		case 17:
			if (b2 == 6)
			{
				chip = Chip.F71878AD;
				logicalDeviceNumber = 4;
			}
			break;
		case 82:
			if (b2 == 23 || b2 == 58 || b2 == 65)
			{
				chip = Chip.W83627HF;
				logicalDeviceNumber = 11;
			}
			break;
		case 130:
			if ((b2 & 0xF0) == 128)
			{
				chip = Chip.W83627THF;
				logicalDeviceNumber = 11;
			}
			break;
		case 133:
			if (b2 == 65)
			{
				chip = Chip.W83687THF;
				logicalDeviceNumber = 11;
			}
			break;
		case 136:
		{
			int num = b2 & 0xF0;
			if (num == 80 || num == 96)
			{
				chip = Chip.W83627EHF;
				logicalDeviceNumber = 11;
			}
			break;
		}
		case 160:
			if ((b2 & 0xF0) == 32)
			{
				chip = Chip.W83627DHG;
				logicalDeviceNumber = 11;
			}
			break;
		case 165:
			if ((b2 & 0xF0) == 16)
			{
				chip = Chip.W83667HG;
				logicalDeviceNumber = 11;
			}
			break;
		case 176:
			if ((b2 & 0xF0) == 112)
			{
				chip = Chip.W83627DHGP;
				logicalDeviceNumber = 11;
			}
			break;
		case 179:
			if ((b2 & 0xF0) == 80)
			{
				chip = Chip.W83667HGB;
				logicalDeviceNumber = 11;
			}
			break;
		case 180:
			if ((b2 & 0xF0) == 112)
			{
				chip = Chip.NCT6771F;
				logicalDeviceNumber = 11;
			}
			break;
		case 195:
			if ((b2 & 0xF0) == 48)
			{
				chip = Chip.NCT6776F;
				logicalDeviceNumber = 11;
			}
			break;
		case 196:
			if ((b2 & 0xF0) == 80)
			{
				chip = Chip.NCT610X;
				logicalDeviceNumber = 11;
			}
			break;
		case 197:
			if ((b2 & 0xF0) == 96)
			{
				chip = Chip.NCT6779D;
				logicalDeviceNumber = 11;
			}
			break;
		case 200:
			if (b2 == 3)
			{
				chip = Chip.NCT6791D;
				logicalDeviceNumber = 11;
			}
			break;
		case 201:
			switch (b2)
			{
			case 17:
				chip = Chip.NCT6792D;
				logicalDeviceNumber = 11;
				break;
			case 19:
				chip = Chip.NCT6792DA;
				logicalDeviceNumber = 11;
				break;
			}
			break;
		case 209:
			if (b2 == 33)
			{
				chip = Chip.NCT6793D;
				logicalDeviceNumber = 11;
			}
			break;
		case 211:
			if (b2 == 82)
			{
				chip = Chip.NCT6795D;
				logicalDeviceNumber = 11;
			}
			break;
		case 212:
			switch (b2)
			{
			case 35:
				chip = Chip.NCT6796D;
				logicalDeviceNumber = 11;
				break;
			case 42:
				chip = Chip.NCT6796DR;
				logicalDeviceNumber = 11;
				break;
			case 43:
				chip = Chip.NCT6798D;
				logicalDeviceNumber = 11;
				break;
			case 81:
				chip = Chip.NCT6797D;
				logicalDeviceNumber = 11;
				break;
			}
			break;
		}
		if (chip == Chip.Unknown)
		{
			if (b != 0 && b != byte.MaxValue)
			{
				port.WinbondNuvotonFintekExit();
				ReportUnknownChip(port, "Winbond / Nuvoton / Fintek", (b << 8) | b2);
			}
			return false;
		}
		port.Select(logicalDeviceNumber);
		ushort num2 = port.ReadWord(96);
		Thread.Sleep(1);
		ushort num3 = port.ReadWord(96);
		ushort num4 = port.ReadWord(35);
		if (num2 == num3 && (chip == Chip.NCT6791D || chip == Chip.NCT6792D || chip == Chip.NCT6792DA || chip == Chip.NCT6793D || chip == Chip.NCT6795D || chip == Chip.NCT6796D || chip == Chip.NCT6796DR || chip == Chip.NCT6797D || chip == Chip.NCT6798D))
		{
			port.NuvotonDisableIOSpaceLock();
		}
		port.WinbondNuvotonFintekExit();
		if (num2 != num3)
		{
			report.Append("Chip ID: 0x");
			report.AppendLine(chip.ToString("X"));
			report.Append("Chip revision: 0x");
			report.AppendLine(b2.ToString("X", CultureInfo.InvariantCulture));
			report.AppendLine("Error: Address verification failed");
			report.AppendLine();
			return false;
		}
		if ((num2 & 7) == 5)
		{
			num2 &= 0xFFF8;
		}
		if (num2 < 256 || (num2 & 0xF007) != 0)
		{
			report.Append("Chip ID: 0x");
			report.AppendLine(chip.ToString("X"));
			report.Append("Chip revision: 0x");
			report.AppendLine(b2.ToString("X", CultureInfo.InvariantCulture));
			report.Append("Error: Invalid address 0x");
			report.AppendLine(num2.ToString("X", CultureInfo.InvariantCulture));
			report.AppendLine();
			return false;
		}
		switch (chip)
		{
		case Chip.W83627HF:
		case Chip.W83627THF:
		case Chip.W83687THF:
		case Chip.W83627EHF:
		case Chip.W83627DHG:
		case Chip.W83667HG:
		case Chip.W83627DHGP:
		case Chip.W83667HGB:
			superIOs.Add(new W836XX(chip, b2, num2));
			break;
		case Chip.NCT6771F:
		case Chip.NCT6776F:
		case Chip.NCT610X:
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
			superIOs.Add(new NCT677X(chip, b2, num2, port));
			break;
		case Chip.F71858:
		case Chip.F71882:
		case Chip.F71862:
		case Chip.F71889F:
		case Chip.F71869:
		case Chip.F71808E:
		case Chip.F71889ED:
		case Chip.F71889AD:
		case Chip.F71869A:
		case Chip.F71878AD:
			if (num4 != 6452)
			{
				report.Append("Chip ID: 0x");
				report.AppendLine(chip.ToString("X"));
				report.Append("Chip revision: 0x");
				report.AppendLine(b2.ToString("X", CultureInfo.InvariantCulture));
				report.Append("Error: Invalid vendor ID 0x");
				report.AppendLine(num4.ToString("X", CultureInfo.InvariantCulture));
				report.AppendLine();
				return false;
			}
			superIOs.Add(new F718XX(chip, num2));
			break;
		}
		return true;
	}

	private bool DetectIT87(LPCPort port)
	{
		if (port.RegisterPort != 46 && port.RegisterPort != 78)
		{
			return false;
		}
		port.IT87Enter();
		ushort num = port.ReadWord(32);
		Chip chip = num switch
		{
			34336 => Chip.IT8620E, 
			34344 => Chip.IT8628E, 
			34389 => Chip.IT8655E, 
			34405 => Chip.IT8665E, 
			34438 => Chip.IT8686E, 
			34440 => Chip.IT8688E, 
			34565 => Chip.IT8705F, 
			34578 => Chip.IT8712F, 
			34582 => Chip.IT8716F, 
			34584 => Chip.IT8718F, 
			34592 => Chip.IT8720F, 
			34593 => Chip.IT8721F, 
			34598 => Chip.IT8726F, 
			34600 => Chip.IT8728F, 
			34611 => Chip.IT879XE, 
			34673 => Chip.IT8771E, 
			34674 => Chip.IT8772E, 
			_ => Chip.Unknown, 
		};
		if (chip == Chip.Unknown)
		{
			if (num != 0 && num != ushort.MaxValue)
			{
				port.IT87Exit();
				ReportUnknownChip(port, "ITE", num);
			}
			return false;
		}
		port.Select(4);
		ushort num2 = port.ReadWord(96);
		Thread.Sleep(1);
		ushort num3 = port.ReadWord(96);
		byte version = (byte)(port.ReadByte(34) & 0xF);
		ushort num4;
		ushort num5;
		if (chip == Chip.IT8705F)
		{
			port.Select(5);
			num4 = port.ReadWord(96);
			Thread.Sleep(1);
			num5 = port.ReadWord(96);
		}
		else
		{
			port.Select(7);
			num4 = port.ReadWord(98);
			Thread.Sleep(1);
			num5 = port.ReadWord(98);
		}
		port.IT87Exit();
		if (num2 != num3 || num2 < 256 || (num2 & 0xF007) != 0)
		{
			report.Append("Chip ID: 0x");
			report.AppendLine(chip.ToString("X"));
			report.Append("Error: Invalid address 0x");
			report.AppendLine(num2.ToString("X", CultureInfo.InvariantCulture));
			report.AppendLine();
			return false;
		}
		if (num4 != num5 || num4 < 256 || (num4 & 0xF007) != 0)
		{
			report.Append("Chip ID: 0x");
			report.AppendLine(chip.ToString("X"));
			report.Append("Error: Invalid GPIO address 0x");
			report.AppendLine(num4.ToString("X", CultureInfo.InvariantCulture));
			report.AppendLine();
			return false;
		}
		superIOs.Add(new IT87XX(chip, num2, num4, version));
		return true;
	}

	private bool DetectSMSC(LPCPort port)
	{
		port.SMSCEnter();
		ushort num = port.ReadWord(32);
		if (0 == 0)
		{
			if (num != 0 && num != ushort.MaxValue)
			{
				port.SMSCExit();
				ReportUnknownChip(port, "SMSC", num);
			}
			return false;
		}
		port.SMSCExit();
		return true;
	}

	private void Detect()
	{
		for (int i = 0; i < REGISTER_PORTS.Length; i++)
		{
			LPCPort port = new LPCPort(REGISTER_PORTS[i], VALUE_PORTS[i]);
			if (!DetectWinbondFintek(port) && !DetectIT87(port))
			{
				DetectSMSC(port);
			}
		}
	}

	public LPCIO()
	{
		if (Ring0.IsOpen && Ring0.WaitIsaBusMutex(100))
		{
			Detect();
			Ring0.ReleaseIsaBusMutex();
		}
	}

	public string GetReport()
	{
		if (report.Length > 0)
		{
			return "LPCIO" + Environment.NewLine + Environment.NewLine + report;
		}
		return null;
	}
}
