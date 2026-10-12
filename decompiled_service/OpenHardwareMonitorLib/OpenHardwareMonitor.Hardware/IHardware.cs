namespace OpenHardwareMonitor.Hardware;

public interface IHardware : IElement
{
	string Name { get; set; }

	Identifier Identifier { get; }

	HardwareType HardwareType { get; }

	IHardware[] SubHardware { get; }

	IHardware Parent { get; }

	ISensor[] Sensors { get; }

	event SensorEventHandler SensorAdded;

	event SensorEventHandler SensorRemoved;

	string GetReport();

	void Update();
}
