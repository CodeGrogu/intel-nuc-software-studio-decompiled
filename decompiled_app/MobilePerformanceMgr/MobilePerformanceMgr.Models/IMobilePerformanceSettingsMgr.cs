using System.Collections.ObjectModel;
using Prism.Commands;
using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr.Models;

public interface IMobilePerformanceSettingsMgr
{
	PerformanceProfile SelectedProfile { get; set; }

	DelegateCommand OpenDetailView { get; set; }

	ObservableCollection<PerformanceProfile> CurrentProfiles { get; set; }

	bool PassiveCoolingMode { get; set; }

	bool BenchmarkMode { get; set; }

	bool ErrorMessage { get; set; }

	void UpdateProfile(PerformanceProfile prof);
}
