using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvClocks
{
	public uint Version;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 288)]
	public uint[] Clock;
}
