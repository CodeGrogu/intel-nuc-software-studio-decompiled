namespace SystemMonitorLibrary.Hardware.Processor;

internal static class ProcessorConst
{
	public const string CPU_TOTAL = "CPU Total";

	public const string CPU_CORE = "CPU Core";

	public const string NAME_KEY = "Name";

	public const string CURRENT_USAGE_KEY = "CurrentUsage";

	public const string CURRENT_TEMPERATURE_KEY = "CurrentTemperature";

	public const string CURRENT_CLOCK_SPEED_KEY = "CurrentClockSpeed";

	public const string MAX_CLOCK_SPEED_KEY = "MaxClockSpeed";

	public const string PROCESSORY_QUERY = "SELECT * FROM Win32_Processor";
}
