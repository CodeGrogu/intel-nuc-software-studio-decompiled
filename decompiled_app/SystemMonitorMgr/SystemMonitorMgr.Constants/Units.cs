using Infrastructure.Converters;

namespace SystemMonitorMgr.Constants;

internal static class Units
{
	public static readonly string MEGAHERTZ = ResourceDictionaryConverter.GetString("Megahertz");

	public static readonly string GIGABYTE = ResourceDictionaryConverter.GetString("Gigabyte");

	public static readonly string GIBIBYTE = ResourceDictionaryConverter.GetString("Gibibyte");

	public static readonly string CELSIUS = ResourceDictionaryConverter.GetString("Celsius");

	public static readonly string PERCENT = ResourceDictionaryConverter.GetString("PercentSymbol");
}
