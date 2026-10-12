namespace OpenHardwareMonitor.Hardware.LPC;

internal class LPCPort
{
	private readonly ushort registerPort;

	private readonly ushort valuePort;

	private const byte DEVCIE_SELECT_REGISTER = 7;

	private const byte CONFIGURATION_CONTROL_REGISTER = 2;

	private const byte NUVOTON_HARDWARE_MONITOR_IO_SPACE_LOCK = 40;

	public ushort RegisterPort => registerPort;

	public ushort ValuePort => valuePort;

	public LPCPort(ushort registerPort, ushort valuePort)
	{
		this.registerPort = registerPort;
		this.valuePort = valuePort;
	}

	public byte ReadByte(byte register)
	{
		Ring0.WriteIoPort(registerPort, register);
		return Ring0.ReadIoPort(valuePort);
	}

	public void WriteByte(byte register, byte value)
	{
		Ring0.WriteIoPort(registerPort, register);
		Ring0.WriteIoPort(valuePort, value);
	}

	public ushort ReadWord(byte register)
	{
		return (ushort)((ReadByte(register) << 8) | ReadByte((byte)(register + 1)));
	}

	public void Select(byte logicalDeviceNumber)
	{
		Ring0.WriteIoPort(registerPort, 7);
		Ring0.WriteIoPort(valuePort, logicalDeviceNumber);
	}

	public void WinbondNuvotonFintekEnter()
	{
		Ring0.WriteIoPort(registerPort, 135);
		Ring0.WriteIoPort(registerPort, 135);
	}

	public void WinbondNuvotonFintekExit()
	{
		Ring0.WriteIoPort(registerPort, 170);
	}

	public void NuvotonDisableIOSpaceLock()
	{
		byte b = ReadByte(40);
		if ((b & 0x10) > 0)
		{
			WriteByte(40, (byte)(b & -17));
		}
	}

	public void IT87Enter()
	{
		Ring0.WriteIoPort(registerPort, 135);
		Ring0.WriteIoPort(registerPort, 1);
		Ring0.WriteIoPort(registerPort, 85);
		if (registerPort == 78)
		{
			Ring0.WriteIoPort(registerPort, 170);
		}
		else
		{
			Ring0.WriteIoPort(registerPort, 85);
		}
	}

	public void IT87Exit()
	{
		if (registerPort != 78)
		{
			Ring0.WriteIoPort(registerPort, 2);
			Ring0.WriteIoPort(valuePort, 2);
		}
	}

	public void SMSCEnter()
	{
		Ring0.WriteIoPort(registerPort, 85);
	}

	public void SMSCExit()
	{
		Ring0.WriteIoPort(registerPort, 170);
	}
}
