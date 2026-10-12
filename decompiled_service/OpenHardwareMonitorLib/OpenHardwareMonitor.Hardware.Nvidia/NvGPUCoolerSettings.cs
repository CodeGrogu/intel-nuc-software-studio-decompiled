using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvGPUCoolerSettings
{
	public uint Version;

	public uint Count;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
	public NvCooler[] Cooler;
}
