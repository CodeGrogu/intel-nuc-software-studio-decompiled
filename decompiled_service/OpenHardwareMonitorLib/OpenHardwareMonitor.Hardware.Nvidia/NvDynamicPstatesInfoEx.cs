using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvDynamicPstatesInfoEx
{
	public uint Version;

	public uint Flags;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	public NvUtilizationDomainEx[] UtilizationDomains;
}
