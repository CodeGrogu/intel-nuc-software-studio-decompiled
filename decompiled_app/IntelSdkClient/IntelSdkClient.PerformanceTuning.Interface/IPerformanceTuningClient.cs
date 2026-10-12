using System.Collections.Generic;
using IntelSdkClient.PerformanceTuningService;

namespace IntelSdkClient.PerformanceTuning.Interface;

public interface IPerformanceTuningClient
{
	bool? GetPowerLevelControlSupported();

	PowerSetting GetCurrentPowerSettings();

	PowerProfile[] GetSupportedPowerProfiles();

	bool? IsFanEcSupported();

	bool? IsFanProfilesSupported();

	string[] GetSupportedFanModes();

	string GetCurrentBiosFanModeName();

	bool SetFanMode(string NewMode);

	bool SetCustomPowerSetting(PowerSetting powerSetting);

	bool ApplyBiosPowerProfile(string powerSettingName);

	FanProfile GetFanProfileData(string currentFanMode);

	FanProfile23 GetFanProfileData23(string currentFanMode);

	string[] SupportedFanTypes();

	bool SetFanCustom(FanControlAllFanSettings allFanSettings);

	bool SetFanOffConfiguration(string selectedFanMode, bool isFanOff);

	bool SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes);

	List<FanProfileRN> GetFanProfileDataRN();

	bool SetFanModeRN(string NewMode, string fanType, List<FanProfileRN> FanProfiles);
}
