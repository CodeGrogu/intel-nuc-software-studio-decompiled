namespace OpenHardwareMonitor.Hardware.LPC;

internal interface ISuperIO
{
	Chip Chip { get; }

	float?[] Voltages { get; }

	float?[] Temperatures { get; }

	float?[] Fans { get; }

	float?[] Controls { get; }

	void SetControl(int index, byte? value);

	byte? ReadGPIO(int index);

	void WriteGPIO(int index, byte value);

	string GetReport();

	void Update();
}
