using Infrastructure.Converters;

namespace MobilePerformanceMgr.Models;

public static class MobilePerformanceTuningConstants
{
	public const string MOBILE_PERFORMANCE_VIEW = "MobilePerformanceView";

	public const string MOBILE_PERFORMANCE_SUMMARY_VIEW = "MobilePerformanceSummaryView";

	public const int PERFORMANCE_MODE = 1;

	public const int BALANCED_STANDARD_MODE = 2;

	public const int BATTERY_SAVER_QUIET_MODE = 3;

	public const bool BENCHMARK_MODE_DEFAULT = false;

	public const bool PASSIVE_COOLING_MODE_DEFAULT = true;

	public static readonly string PERFORMANCE_CARD_TITLE_STRING = ResourceDictionaryConverter.GetString("PerformanceCardTitleString");

	public static readonly string PERFORMANCE_STRING = ResourceDictionaryConverter.GetString("PerformanceString");

	public static readonly string BENCHMARK_STRING = ResourceDictionaryConverter.GetString("Benchmark");

	public static readonly string BATTERY_SAVER_STRING = ResourceDictionaryConverter.GetString("BatterySaverString");

	public static readonly string BALANCED_STRING = ResourceDictionaryConverter.GetString("Balanced");

	public static readonly string QUIET_STRING = ResourceDictionaryConverter.GetString("Quiet");

	public static readonly string STANDARD_STRING = ResourceDictionaryConverter.GetString("StandardString");

	public const string PERFORMANCE_TRAY_APP_KEY = "SwitchToPerformanceString";

	public const string BALANCED_TRAY_APP_KEY = "SwitchToBalancedString";

	public const string BATTERY_SAVER_TRAY_APP_KEY = "SwitchToBatterySaverString";
}
