using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvGPUThermalSettings
{
	public uint Version;

	public uint Count;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public NvSensor[] Sensor;
}
