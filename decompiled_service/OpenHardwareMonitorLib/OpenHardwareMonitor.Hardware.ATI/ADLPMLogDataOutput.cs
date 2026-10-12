using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.ATI;

internal struct ADLPMLogDataOutput
{
	public int Size;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	public ADLSingleSensorData[] Sensors;
}
