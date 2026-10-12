namespace SystemMonitorLibrary.Hardware.Gpu;

internal static class GpuConst
{
	public const string GPU_CORE = "GPU Core";

	public const string GPU_MEMORY = "GPU Memory";

	public const string GPU_MEMORY_TOTAL = "GPU Memory Total";

	public const string PERF_MON_ENG_3D = "engtype_3D";

	public const int PERF_MON_GPU_ID_INDEX = 7;

	public const string PERF_MON_UTILIZATION_PERCENTAGE = "UtilizationPercentage";

	public const string CPU_PACKAGE = "CPU Package";

	public const string NAME_KEY = "Name";

	public const string USAGE_KEY = "Usage";

	public const string TEMPERATURE_KEY = "Temperature";

	public const string CORE_CLOCK_SPEED_KEY = "CoreClockSpeed";

	public const string MEMORY_CLOCK_SPEED_KEY = "MemoryClockSpeed";

	public const string MEMORY_KEY = "Memory";

	public const string IGPU_NAME_QUERY = "SELECT * FROM Win32_VideoController";

	public const string IGPU_USAGE_QUERY = "SELECT * FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine";

	public const string INTEL_KEY = "Intel(R)";

	public const string INTEL_KEY_IRIS = "Intel(R) Iris(R)";

	public const string GRAPHICS_KEY = "Graphics";

	public const int BIT_CONVERSION_VALUE = 1073741824;
}
