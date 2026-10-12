using System.Runtime.InteropServices;

namespace PerformanceTuningLibrary;

public struct FanProfileWmi
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 90)]
	public byte[] Data;
}
