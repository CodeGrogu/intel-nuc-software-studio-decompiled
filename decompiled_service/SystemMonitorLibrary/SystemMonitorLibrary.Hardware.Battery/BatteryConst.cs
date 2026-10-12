namespace SystemMonitorLibrary.Hardware.Battery;

internal static class BatteryConst
{
	public const string ESTIMATED_CHARGE_REMAINING_KEY = "EstimatedChargeRemaining";

	public const string ESTIMATED_RUN_TIME_KEY = "EstimatedRunTime";

	public const string BATTERY_QUERY = "SELECT * FROM Win32_Battery";

	public const string BATTERY_STATUS_KEY = "BatteryStatus";

	public const string PLUGGED_IN_VALUE = "Plugged in";

	public const string MINUTES_VALUE = "minutes";

	public const string BATTERY_SUPPORTED_KEY = "IsBatterySupported";
}
