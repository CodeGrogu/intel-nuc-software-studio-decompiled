using System.Collections.ObjectModel;
using Newtonsoft.Json;
using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr.Models;

[JsonObject(Title = "MobilePerformanceTuningSettings")]
public class MobilePerformanceTuningCustomSettings
{
	private static readonly ObservableCollection<PerformanceProfile> KING_COUNTY_DEFAULT_CURRENT_PROFILES = new ObservableCollection<PerformanceProfile>
	{
		new PerformanceProfile
		{
			Ac = new PerformanceMode
			{
				CpuMode = 1,
				DGpuMode = 1,
				FanMode = 1
			},
			Dc = new PerformanceMode
			{
				CpuMode = 1,
				DGpuMode = 1,
				FanMode = 1
			},
			Index = ProfileIndex.Performance
		},
		new PerformanceProfile
		{
			Ac = new PerformanceMode
			{
				CpuMode = 2,
				DGpuMode = 2,
				FanMode = 2
			},
			Dc = new PerformanceMode
			{
				CpuMode = 2,
				DGpuMode = 2,
				FanMode = 2
			},
			Index = ProfileIndex.Balanced
		},
		new PerformanceProfile
		{
			Ac = new PerformanceMode
			{
				CpuMode = 3,
				DGpuMode = 3,
				FanMode = 3
			},
			Dc = new PerformanceMode
			{
				CpuMode = 3,
				DGpuMode = 3,
				FanMode = 3
			},
			Index = ProfileIndex.BatterySaver
		}
	};

	[JsonProperty]
	public bool PassiveCoolingMode { get; set; }

	[JsonProperty]
	public bool BenchmarkMode { get; set; }

	[JsonProperty]
	public ObservableCollection<PerformanceProfile> CurrentProfiles { get; set; }

	[JsonProperty]
	public int SelectedProfileIndex { get; set; }

	[JsonConstructor]
	public MobilePerformanceTuningCustomSettings(bool passiveCoolingMode, bool benchmarkMode, ObservableCollection<PerformanceProfile> currrentProfiles, int selectedProfileIndex)
	{
		PassiveCoolingMode = passiveCoolingMode;
		BenchmarkMode = benchmarkMode;
		CurrentProfiles = currrentProfiles;
		SelectedProfileIndex = selectedProfileIndex;
	}

	public static MobilePerformanceTuningCustomSettings GetDefaultSettingsKingCounty()
	{
		return new MobilePerformanceTuningCustomSettings(passiveCoolingMode: true, benchmarkMode: false, KING_COUNTY_DEFAULT_CURRENT_PROFILES, 2);
	}
}
