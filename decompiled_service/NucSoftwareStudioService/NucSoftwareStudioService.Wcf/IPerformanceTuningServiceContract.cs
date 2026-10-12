using System.Collections.Generic;
using System.ServiceModel;
using PerformanceTuningLibrary;

namespace NucSoftwareStudioService.Wcf;

[ServiceContract]
internal interface IPerformanceTuningServiceContract
{
	[OperationContract]
	bool? GetPowerLevelControlSupported();

	[OperationContract]
	PowerSetting GetCurrentPowerSettings();

	[OperationContract]
	List<PowerProfile> GetSupportedPowerProfiles();

	[OperationContract]
	bool? IsFanEcSupported();

	[OperationContract]
	bool? IsFanProfilesSupported();

	[OperationContract]
	FanProfile GetFanProfileData(string fanProfileName);

	[OperationContract]
	List<string> GetSupportedFanModes();

	[OperationContract]
	string GetCurrentBiosFanModeName();

	[OperationContract]
	bool SetFanMode(string FanMode);

	[OperationContract]
	bool SetCustomPowerSetting(PowerSetting PowerSetting);

	[OperationContract]
	bool ApplyBiosPowerProfile(string PowerProfileName);

	[OperationContract]
	List<string> SupportedFanTypes();

	[OperationContract]
	bool SetFanCustom(FanControl.AllFanSettings FanSettings);

	[OperationContract]
	bool SetFanOffConfiguration(string FanMode, bool isFanOff);

	[OperationContract]
	FanProfile23 GetFanProfileData23(string fanProfileName);

	[OperationContract]
	bool SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes);

	[OperationContract]
	List<FanProfileRN> GetFanProfileDataRN();

	[OperationContract]
	bool SetFanModeRN(string FanMode, string FanType, List<FanProfileRN> fanProfiles);
}
