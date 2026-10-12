namespace OpenHardwareMonitor.Hardware.Nvidia;

internal enum NvThermalTarget
{
	NONE = 0,
	GPU = 1,
	MEMORY = 2,
	POWER_SUPPLY = 4,
	BOARD = 8,
	ALL = 15,
	UNKNOWN = -1
}
