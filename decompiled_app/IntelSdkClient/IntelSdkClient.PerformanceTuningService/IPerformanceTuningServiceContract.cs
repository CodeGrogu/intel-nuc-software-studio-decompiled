using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntelSdkClient.PerformanceTuningService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "PerformanceTuningService.IPerformanceTuningServiceContract")]
public interface IPerformanceTuningServiceContract
{
	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetPowerLevelControlSupported", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetPowerLevelControlSupportedResponse")]
	bool? GetPowerLevelControlSupported();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetPowerLevelControlSupported", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetPowerLevelControlSupportedResponse")]
	Task<bool?> GetPowerLevelControlSupportedAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetCurrentPowerSettings", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetCurrentPowerSettingsResponse")]
	PowerSetting GetCurrentPowerSettings();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetCurrentPowerSettings", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetCurrentPowerSettingsResponse")]
	Task<PowerSetting> GetCurrentPowerSettingsAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetSupportedPowerProfiles", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetSupportedPowerProfilesResponse")]
	PowerProfile[] GetSupportedPowerProfiles();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetSupportedPowerProfiles", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetSupportedPowerProfilesResponse")]
	Task<PowerProfile[]> GetSupportedPowerProfilesAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/IsFanEcSupported", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/IsFanEcSupportedResponse")]
	bool? IsFanEcSupported();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/IsFanEcSupported", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/IsFanEcSupportedResponse")]
	Task<bool?> IsFanEcSupportedAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/IsFanProfilesSupported", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/IsFanProfilesSupportedResponse")]
	bool? IsFanProfilesSupported();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/IsFanProfilesSupported", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/IsFanProfilesSupportedResponse")]
	Task<bool?> IsFanProfilesSupportedAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileData", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileDataResponse")]
	FanProfile GetFanProfileData(string fanProfileName);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileData", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileDataResponse")]
	Task<FanProfile> GetFanProfileDataAsync(string fanProfileName);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetSupportedFanModes", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetSupportedFanModesResponse")]
	string[] GetSupportedFanModes();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetSupportedFanModes", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetSupportedFanModesResponse")]
	Task<string[]> GetSupportedFanModesAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetCurrentBiosFanModeName", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetCurrentBiosFanModeNameResponse")]
	string GetCurrentBiosFanModeName();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetCurrentBiosFanModeName", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetCurrentBiosFanModeNameResponse")]
	Task<string> GetCurrentBiosFanModeNameAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanMode", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanModeResponse")]
	bool SetFanMode(string FanMode);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanMode", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanModeResponse")]
	Task<bool> SetFanModeAsync(string FanMode);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetCustomPowerSetting", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetCustomPowerSettingResponse")]
	bool SetCustomPowerSetting(PowerSetting PowerSetting);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetCustomPowerSetting", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetCustomPowerSettingResponse")]
	Task<bool> SetCustomPowerSettingAsync(PowerSetting PowerSetting);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/ApplyBiosPowerProfile", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/ApplyBiosPowerProfileResponse")]
	bool ApplyBiosPowerProfile(string PowerProfileName);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/ApplyBiosPowerProfile", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/ApplyBiosPowerProfileResponse")]
	Task<bool> ApplyBiosPowerProfileAsync(string PowerProfileName);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SupportedFanTypes", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SupportedFanTypesResponse")]
	string[] SupportedFanTypes();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SupportedFanTypes", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SupportedFanTypesResponse")]
	Task<string[]> SupportedFanTypesAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanCustom", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanCustomResponse")]
	bool SetFanCustom(FanControlAllFanSettings FanSettings);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanCustom", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanCustomResponse")]
	Task<bool> SetFanCustomAsync(FanControlAllFanSettings FanSettings);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanOffConfiguration", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanOffConfigurationResponse")]
	bool SetFanOffConfiguration(string FanMode, bool isFanOff);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanOffConfiguration", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanOffConfigurationResponse")]
	Task<bool> SetFanOffConfigurationAsync(string FanMode, bool isFanOff);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileData23", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileData23Response")]
	FanProfile23 GetFanProfileData23(string fanProfileName);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileData23", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileData23Response")]
	Task<FanProfile23> GetFanProfileData23Async(string fanProfileName);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanDutyCycleFixedMode", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanDutyCycleFixedModeResponse")]
	bool SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanDutyCycleFixedMode", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanDutyCycleFixedModeResponse")]
	Task<bool> SetFanDutyCycleFixedModeAsync(string selectedFanMode, byte[] FanBytes);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileDataRN", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileDataRNResponse")]
	FanProfileRN[] GetFanProfileDataRN();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileDataRN", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/GetFanProfileDataRNResponse")]
	Task<FanProfileRN[]> GetFanProfileDataRNAsync();

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanModeRN", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanModeRNResponse")]
	bool SetFanModeRN(string FanMode, string FanType, FanProfileRN[] fanProfiles);

	[OperationContract(Action = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanModeRN", ReplyAction = "http://tempuri.org/IPerformanceTuningServiceContract/SetFanModeRNResponse")]
	Task<bool> SetFanModeRNAsync(string FanMode, string FanType, FanProfileRN[] fanProfiles);
}
