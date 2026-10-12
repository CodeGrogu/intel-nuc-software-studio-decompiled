using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvGPUCoolerLevels
{
	public uint Version;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
	public NvLevel[] Levels;
}
