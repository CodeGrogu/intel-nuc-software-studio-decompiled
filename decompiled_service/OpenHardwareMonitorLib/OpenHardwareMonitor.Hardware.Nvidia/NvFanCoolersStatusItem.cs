using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NvFanCoolersStatusItem
{
	public uint Type;

	public uint CurrentRpm;

	public uint CurrentMinLevel;

	public uint CurrentMaxLevel;

	public uint CurrentLevel;

	public uint Reserved1;

	public uint Reserved2;

	public uint Reserved3;

	public uint Reserved4;

	public uint Reserved5;

	public uint Reserved6;

	public uint Reserved7;

	public uint Reserved8;
}
