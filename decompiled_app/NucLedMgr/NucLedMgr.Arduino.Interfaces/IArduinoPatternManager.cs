using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FeatureMgr.Enums;
using NucLedMgr.Arduino.Models;

namespace NucLedMgr.Arduino.Interfaces;

public interface IArduinoPatternManager
{
	bool On { get; set; }

	string SerialPortError { get; }

	NucType NucType { get; }

	LedProfile CurrentProfile { get; set; }

	ObservableCollection<LedProfile> Profiles { get; }

	ArduinoLedData SelectedZone { get; set; }

	ObservableCollection<ArduinoLedData> ArduinoZones { get; }

	void Reset();

	void Refresh();

	void EnableAll(bool Enable);

	void ApplyPrevious();

	Task SaveProfile(LedProfile profile, bool setAsCurrentProfile);

	Task RemoveAllProfileFiles();

	void ProcessJsonFile(string fileName, string newName = null);

	LedProfile GetProfileByName(string name);

	void RemoveProfiles(List<string> names);

	List<string> GetSavedProfileNames();
}
