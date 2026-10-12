using System.Runtime.InteropServices;

namespace PerformanceTuningLibrary;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct SpDeviceInterfaceDetailData
{
	public uint cbSize;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string DevicePath;
}
