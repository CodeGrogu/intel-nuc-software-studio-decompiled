using System.Runtime.InteropServices;

namespace PerformanceTuningLibrary;

public struct FanSpeedStruct
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
	public byte[] Data;
}
