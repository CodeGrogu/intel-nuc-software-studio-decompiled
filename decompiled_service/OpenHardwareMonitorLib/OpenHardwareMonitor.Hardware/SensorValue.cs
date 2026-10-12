using System;

namespace OpenHardwareMonitor.Hardware;

public struct SensorValue(float value, DateTime time)
{
	private readonly float value = value;

	private readonly DateTime time = time;

	public float Value => value;

	public DateTime Time => time;
}
