using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NucLedMgr.ENE.Models;

namespace NucLedMgr.ENE.Interfaces;

public interface IEnePatternManager
{
	bool On { get; set; }

	string SerialPortError { get; }

	LedProfile CurrentProfile { get; set; }

	ObservableCollection<LedProfile> Profiles { get; }

	EneLedData SelectedZone { get; set; }

	ObservableCollection<EneLedData> EneZones { get; }

	void Reset();

	void GetCurrentPattern();

	void EnableAll(bool Enable);

	Task SaveProfile(LedProfile profile, bool setAsCurrentProfile);

	Task RemoveAllProfileFiles();

	void ProcessJsonFile(string fileName, string newName = null);

	LedProfile GetProfileByName(string name);

	void RemoveProfiles(List<string> names);

	List<string> GetSavedProfileNames();
}
