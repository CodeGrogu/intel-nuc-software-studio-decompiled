using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvUtilizationDomainEx
{
	public bool Present;

	public int Percentage;
}
