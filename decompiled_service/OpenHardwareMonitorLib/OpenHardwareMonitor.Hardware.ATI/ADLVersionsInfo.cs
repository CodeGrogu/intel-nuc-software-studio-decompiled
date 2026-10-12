using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.ATI;

internal struct ADLVersionsInfo
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string DriverVersion;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string CatalystVersion;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string CatalystWebLink;
}
