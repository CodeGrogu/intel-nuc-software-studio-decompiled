namespace PerformanceTuningLibrary;

public enum SecondaryInputSourceConfiguration : byte
{
	NoControlled,
	CPUTemperature,
	PCHTemperature,
	Sensor1Temperature,
	Sensor2Temperature,
	Sensor3Temperature,
	dGpuTemperature
}
