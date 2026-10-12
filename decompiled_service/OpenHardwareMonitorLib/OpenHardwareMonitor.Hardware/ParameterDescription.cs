namespace OpenHardwareMonitor.Hardware;

internal struct ParameterDescription(string name, string description, float defaultValue)
{
	private readonly string name = name;

	private readonly string description = description;

	private readonly float defaultValue = defaultValue;

	public string Name => name;

	public string Description => description;

	public float DefaultValue => defaultValue;
}
