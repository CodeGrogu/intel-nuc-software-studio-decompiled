using System.Runtime.InteropServices;

namespace PerformanceTuningLibrary;

public struct FanProfileEC
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 94)]
	public byte[] Data;
}
