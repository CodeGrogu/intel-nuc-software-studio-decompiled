namespace OpenHardwareMonitor.Hardware.CPU;

internal abstract class AMDCPU : GenericCPU
{
	private const byte PCI_BUS = 0;

	private const byte PCI_BASE_DEVICE = 24;

	private const byte DEVICE_VENDOR_ID_REGISTER = 0;

	private const ushort AMD_VENDOR_ID = 4130;

	public AMDCPU(int processorIndex, CPUID[][] cpuid, ISettings settings)
		: base(processorIndex, cpuid, settings)
	{
	}

	protected uint GetPciAddress(byte function, ushort deviceId)
	{
		uint pciAddress = Ring0.GetPciAddress(0, (byte)(24 + processorIndex), function);
		if (!Ring0.ReadPciConfig(pciAddress, 0u, out var value))
		{
			return uint.MaxValue;
		}
		if (value != ((deviceId << 16) | 0x1022))
		{
			return uint.MaxValue;
		}
		return pciAddress;
	}
}
