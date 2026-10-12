using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.ATI;

internal struct ADLAdapterInfo
{
	public int Size;

	public int AdapterIndex;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string UDID;

	public int BusNumber;

	public int DeviceNumber;

	public int FunctionNumber;

	public int VendorID;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string AdapterName;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string DisplayName;

	public int Present;

	public int Exist;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string DriverPath;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string DriverPathExt;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string PNPString;

	public int OSDisplayIndex;
}
