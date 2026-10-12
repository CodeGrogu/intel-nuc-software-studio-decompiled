using System.Collections.Generic;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware;

public interface ISensor : IElement
{
	IHardware Hardware { get; }

	SensorType SensorType { get; }

	Identifier Identifier { get; }

	string Name { get; set; }

	int Index { get; }

	bool IsDefaultHidden { get; }

	IReadOnlyArray<IParameter> Parameters { get; }

	float? Value { get; }

	float? Min { get; }

	float? Max { get; }

	IEnumerable<SensorValue> Values { get; }

	IControl Control { get; }

	void ResetMin();

	void ResetMax();
}
