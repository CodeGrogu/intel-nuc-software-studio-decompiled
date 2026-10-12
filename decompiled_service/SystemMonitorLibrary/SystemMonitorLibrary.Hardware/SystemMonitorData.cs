using System.Collections.Generic;

namespace SystemMonitorLibrary.Hardware;

public class SystemMonitorData
{
	public Dictionary<string, string> SystemInformation { get; set; }

	public Dictionary<string, string> BatteryInformation { get; set; }

	public Dictionary<string, string> ProcessorInformation { get; set; }

	public Dictionary<string, string> IntegratedGpuInformation { get; set; }

	public Dictionary<string, string> DiscreteGpuInformation { get; set; }

	public Dictionary<string, string> MemoryInformation { get; set; }

	public List<Dictionary<string, string>> StorageInformation { get; set; }
}
