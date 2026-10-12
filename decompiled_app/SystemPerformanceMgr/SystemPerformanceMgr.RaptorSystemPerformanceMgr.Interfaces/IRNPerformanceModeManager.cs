using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using IntelSdkClient.PerformanceTuningService;

namespace SystemPerformanceMgr.RaptorSystemPerformanceMgr.Interfaces;

internal interface IRNPerformanceModeManager
{
	bool PowerModesSupported { get; }

	uint Max_PL1 { get; set; }

	uint Max_PL2 { get; set; }

	uint Max_Tau { get; set; }

	int Temp_Min { get; set; }

	int Temp_Max { get; set; }

	int Duty_Min { get; set; }

	int Duty_Max { get; set; }

	List<FanProfileRN> FanProfiles { get; set; }

	int FixedDutyCycle { get; set; }

	PowerProfile CurrentPowerMode { get; set; }

	ObservableCollection<PowerProfile> SupportedCpuModes { get; set; }

	PowerSetting CurrentBiosPowerSetting { get; }

	bool FanModesSupported { get; }

	string CurrentFanMode { get; }

	List<string> SupportedFanModes { get; }

	List<string> SupportedFanTypes { get; }

	FanSettingRN[] FanSettings { get; }

	PowerProfile Custom { get; set; }

	DoubleCollection TauValues { get; }

	string PerformanceDriverError { get; }

	bool SetPowerMode(PowerProfile NewMode);

	void GetCurrentBiosPowerSetting();

	bool SetFanMode(string NewMode, string FanType, FanProfileRN fanProfile);

	FanProfile23 GetFanProfileData23(string currentFanMode);

	bool SetFanCustomMode(string NewMode, FanSettingRN[] fanSettings, string FanType);

	bool SetFanOffConfiguration(string selectedFanMode, bool isFanOff);

	List<FanProfileRN> GetFanProfileDataRN();

	void SaveFanProfile(List<FanProfileRN> fanProfile);

	void SetFansToCoolMode();
}
