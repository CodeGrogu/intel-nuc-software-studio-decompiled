namespace OpenHardwareMonitor.Hardware.Nvidia;

internal enum NvThermalController
{
	NONE = 0,
	GPU_INTERNAL = 1,
	ADM1032 = 2,
	MAX6649 = 3,
	MAX1617 = 4,
	LM99 = 5,
	LM89 = 6,
	LM64 = 7,
	ADT7473 = 8,
	SBMAX6649 = 9,
	VBIOSEVT = 10,
	OS = 11,
	UNKNOWN = -1
}
