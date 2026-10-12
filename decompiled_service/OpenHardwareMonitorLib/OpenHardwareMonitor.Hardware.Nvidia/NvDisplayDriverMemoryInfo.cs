using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvDisplayDriverMemoryInfo
{
	public uint Version;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	public uint[] Values;
}
