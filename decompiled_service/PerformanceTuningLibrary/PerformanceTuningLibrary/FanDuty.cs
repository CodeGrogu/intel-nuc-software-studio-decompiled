using System.Runtime.InteropServices;

namespace PerformanceTuningLibrary;

public struct FanDuty
{
	[MarshalAs(UnmanagedType.U1)]
	public byte nucEcSection;

	public byte fanNumber;

	public byte fanDuty;
}
