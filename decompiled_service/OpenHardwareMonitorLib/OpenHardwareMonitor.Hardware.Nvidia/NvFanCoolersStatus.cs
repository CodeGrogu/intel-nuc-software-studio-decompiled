using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvFanCoolersStatus
{
	public uint Version;

	public uint Count;

	public ulong Reserved1;

	public ulong Reserved2;

	public ulong Reserved3;

	public ulong Reserved4;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	internal NvFanCoolersStatusItem[] Items;
}
