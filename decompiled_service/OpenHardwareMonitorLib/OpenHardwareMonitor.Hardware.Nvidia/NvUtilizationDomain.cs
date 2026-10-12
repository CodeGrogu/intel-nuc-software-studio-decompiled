using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvUtilizationDomain
{
	public bool Present;

	public int Percentage;

	public ulong Reserved;
}
