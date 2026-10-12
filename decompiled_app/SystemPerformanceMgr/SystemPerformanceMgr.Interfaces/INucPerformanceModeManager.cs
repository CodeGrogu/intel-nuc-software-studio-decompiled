using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using FeatureMgr.Enums;
using IntelSdkClient.PerformanceTuningService;

namespace SystemPerformanceMgr.Interfaces;

public interface INucPerformanceModeManager
{
	bool PowerModesSupported { get; }

	uint Max_PL1 { get; set; }

	uint Max_PL2 { get; set; }

	uint Max_Tau { get; set; }

	int Temp_Min { get; set; }

	int Temp_Max { get; set; }

	int Duty_Min { get; set; }

	int Duty_Max { get; set; }

	int FixedDutyCycle { get; set; }

	PowerProfile CurrentPowerMode { get; set; }

	ObservableCollection<PowerProfile> SupportedCpuModes { get; set; }

	PowerSetting CurrentBiosPowerSetting { get; }

	bool FanModesSupported { get; }

	string CurrentFanMode { get; }

	List<string> SupportedFanModes { get; }

	List<string> SupportedFanTypes { get; }

	FanSetting[] FanSettings { get; }

	PowerProfile Custom { get; set; }

	DoubleCollection TauValues { get; }

	NucType NucType { get; }

	string PerformanceDriverError { get; }

	bool SetPowerMode(PowerProfile NewMode);

	bool SetFanMode(string NewMode);

	FanProfile GetFanProfileData(string currentFanMode);

	FanProfile23 GetFanProfileData23(string currentFanMode);

	bool SetFanCustomMode(FanSetting[] fanSettings);

	bool SetFanOffConfiguration(string selectedFanMode, bool isFanOff);

	void SaveFanOffCapability(string selectedFanMode, bool isFanOff);

	void SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes);
}
