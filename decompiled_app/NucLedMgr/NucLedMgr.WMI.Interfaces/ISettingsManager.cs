using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FeatureMgr.Enums;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Interfaces;

public interface ISettingsManager
{
	Led SelectedSetting { get; set; }

	ObservableCollection<Led> CurrentSettings { get; set; }

	LedProfile CurrentProfile { get; set; }

	ObservableCollection<LedProfile> Profiles { get; }

	bool LedsLoaded { get; }

	NucType NucType { get; }

	Led PowerButton { get; }

	Led Front1 { get; }

	Led Front2 { get; }

	Led Front3 { get; }

	Led Skull { get; }

	Led Eyes { get; }

	Led Hdd { get; }

	bool Disabled { get; set; }

	Task SaveProfile(LedProfile profile, bool setAsCurrentProfile);

	void RemoveProfiles(List<string> names);

	Task RemoveAllProfileFiles();

	LedProfile GetProfileByName(string name);

	void Reset();

	void ResetPanther();

	void DisableAll();

	void ApplyPrevious();

	void ProcessJsonFile(string fileName, string newName = null);

	List<string> GetSavedProfileNames();
}
