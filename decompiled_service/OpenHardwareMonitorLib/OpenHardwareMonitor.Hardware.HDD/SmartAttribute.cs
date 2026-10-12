using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware.HDD;

internal class SmartAttribute
{
	public delegate float RawValueConversion(byte[] rawValue, byte value, IReadOnlyArray<IParameter> parameters);

	private RawValueConversion rawValueConversion;

	public byte Identifier { get; private set; }

	public string Name { get; private set; }

	public SensorType? SensorType { get; private set; }

	public int SensorChannel { get; private set; }

	public string SensorName { get; private set; }

	public bool DefaultHiddenSensor { get; private set; }

	public ParameterDescription[] ParameterDescriptions { get; private set; }

	public bool HasRawValueConversion => rawValueConversion != null;

	public SmartAttribute(byte identifier, string name)
		: this(identifier, name, null, null, 0, null)
	{
	}

	public SmartAttribute(byte identifier, string name, RawValueConversion rawValueConversion)
		: this(identifier, name, rawValueConversion, null, 0, null)
	{
	}

	public SmartAttribute(byte identifier, string name, RawValueConversion rawValueConversion, SensorType? sensorType, int sensorChannel, string sensorName, bool defaultHiddenSensor = false, ParameterDescription[] parameterDescriptions = null)
	{
		Identifier = identifier;
		Name = name;
		this.rawValueConversion = rawValueConversion;
		SensorType = sensorType;
		SensorChannel = sensorChannel;
		SensorName = sensorName;
		DefaultHiddenSensor = defaultHiddenSensor;
		ParameterDescriptions = parameterDescriptions;
	}

	public float ConvertValue(DriveAttributeValue value, IReadOnlyArray<IParameter> parameters)
	{
		if (rawValueConversion == null)
		{
			return (int)value.AttrValue;
		}
		return rawValueConversion(value.RawValue, value.AttrValue, parameters);
	}
}
