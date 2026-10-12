using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvDisplayDriverVersion
{
	public uint Version;

	public uint DriverVersion;

	public uint BldChangeListNum;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
	public string BuildBranch;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
	public string Adapter;
}
