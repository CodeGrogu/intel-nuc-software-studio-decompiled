using System.Runtime.InteropServices;

namespace PerformanceTuningLibrary;

public struct FanProfileWmi23
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
	public byte[] Data;
}
