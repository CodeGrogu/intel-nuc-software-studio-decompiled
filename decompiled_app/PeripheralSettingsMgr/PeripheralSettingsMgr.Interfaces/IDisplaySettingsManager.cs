using System.Collections.Generic;
using System.Collections.ObjectModel;
using PeripheralSettingsMgr.Models.DisplayModes;

namespace PeripheralSettingsMgr.Interfaces;

public interface IDisplaySettingsManager
{
	ObservableCollection<DisplayModeProfile> Profiles { get; }

	DisplayModeProfile CurrentProfile { get; set; }

	bool AcOnly { get; }

	bool UpdateProfileName(string OldName, string NewName);

	DisplayModeProfile GetDefaultProfile();

	DisplayModeProfile UpdateCurrentProfile(DisplayModeProfile mode);

	void RemoveProfiles(List<string> Names);

	void AddProfile(string Name, DisplayModeProfile BaseProfile);

	DisplayModeProfile ResetSettings();

	DisplayModeProfile ResetDefaultSettings();

	DisplayModeProfile UpdateProfiles(DisplayModeProfile newProfile);
}
