using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.TBalancer;

internal struct FT_DEVICE_INFO_NODE
{
	public uint Flags;

	public FT_DEVICE Type;

	public uint ID;

	public uint LocId;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
	public string SerialNumber;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
	public string Description;

	public FT_HANDLE Handle;
}
