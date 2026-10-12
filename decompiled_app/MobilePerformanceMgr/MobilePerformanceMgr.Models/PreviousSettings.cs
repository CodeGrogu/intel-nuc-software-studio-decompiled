using Newtonsoft.Json;

namespace MobilePerformanceMgr.Models;

[JsonObject]
public class PreviousSettings
{
	public bool Benchmark { get; set; }

	public bool PassiveCooling { get; set; }

	public int ProfileIndex { get; set; }

	public PreviousSettings(bool benchmark, bool passiveCooling, int profileIndex)
	{
		Benchmark = benchmark;
		PassiveCooling = passiveCooling;
		ProfileIndex = profileIndex;
	}

	[JsonConstructor]
	public PreviousSettings()
	{
	}
}
