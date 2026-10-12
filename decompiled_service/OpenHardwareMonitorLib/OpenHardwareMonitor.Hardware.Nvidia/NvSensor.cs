using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvSensor
{
	public NvThermalController Controller;

	public uint DefaultMinTemp;

	public uint DefaultMaxTemp;

	public uint CurrentTemp;

	public NvThermalTarget Target;
}
