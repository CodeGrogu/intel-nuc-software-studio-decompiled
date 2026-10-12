using System.Runtime.InteropServices;

namespace PerformanceTuningLibrary;

public struct TempratureProfileEC
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
	public byte[] Data;
}
