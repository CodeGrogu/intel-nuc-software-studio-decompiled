namespace OpenHardwareMonitor.Hardware;

public interface IComputer : IElement
{
	IHardware[] Hardware { get; }

	bool MainboardEnabled { get; }

	bool CPUEnabled { get; }

	bool RAMEnabled { get; }

	bool GPUEnabled { get; }

	bool FanControllerEnabled { get; }

	bool HDDEnabled { get; }

	event HardwareEventHandler HardwareAdded;

	event HardwareEventHandler HardwareRemoved;

	string GetReport();
}
