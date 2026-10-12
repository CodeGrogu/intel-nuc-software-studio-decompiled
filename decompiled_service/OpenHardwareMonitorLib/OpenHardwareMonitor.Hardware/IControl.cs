namespace OpenHardwareMonitor.Hardware;

public interface IControl
{
	Identifier Identifier { get; }

	ControlMode ControlMode { get; }

	float SoftwareValue { get; }

	float MinSoftwareValue { get; }

	float MaxSoftwareValue { get; }

	void SetDefault();

	void SetSoftware(float value);
}
