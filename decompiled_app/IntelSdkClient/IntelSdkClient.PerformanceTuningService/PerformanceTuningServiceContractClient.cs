using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace IntelSdkClient.PerformanceTuningService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class PerformanceTuningServiceContractClient : ClientBase<IPerformanceTuningServiceContract>, IPerformanceTuningServiceContract
{
	public PerformanceTuningServiceContractClient()
	{
	}

	public PerformanceTuningServiceContractClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public PerformanceTuningServiceContractClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public PerformanceTuningServiceContractClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public PerformanceTuningServiceContractClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public bool? GetPowerLevelControlSupported()
	{
		return Channel.GetPowerLevelControlSupported();
	}

	public Task<bool?> GetPowerLevelControlSupportedAsync()
	{
		return Channel.GetPowerLevelControlSupportedAsync();
	}

	public PowerSetting GetCurrentPowerSettings()
	{
		return Channel.GetCurrentPowerSettings();
	}

	public Task<PowerSetting> GetCurrentPowerSettingsAsync()
	{
		return Channel.GetCurrentPowerSettingsAsync();
	}

	public PowerProfile[] GetSupportedPowerProfiles()
	{
		return Channel.GetSupportedPowerProfiles();
	}

	public Task<PowerProfile[]> GetSupportedPowerProfilesAsync()
	{
		return Channel.GetSupportedPowerProfilesAsync();
	}

	public bool? IsFanEcSupported()
	{
		return Channel.IsFanEcSupported();
	}

	public Task<bool?> IsFanEcSupportedAsync()
	{
		return Channel.IsFanEcSupportedAsync();
	}

	public bool? IsFanProfilesSupported()
	{
		return Channel.IsFanProfilesSupported();
	}

	public Task<bool?> IsFanProfilesSupportedAsync()
	{
		return Channel.IsFanProfilesSupportedAsync();
	}

	public FanProfile GetFanProfileData(string fanProfileName)
	{
		return Channel.GetFanProfileData(fanProfileName);
	}

	public Task<FanProfile> GetFanProfileDataAsync(string fanProfileName)
	{
		return Channel.GetFanProfileDataAsync(fanProfileName);
	}

	public string[] GetSupportedFanModes()
	{
		return Channel.GetSupportedFanModes();
	}

	public Task<string[]> GetSupportedFanModesAsync()
	{
		return Channel.GetSupportedFanModesAsync();
	}

	public string GetCurrentBiosFanModeName()
	{
		return Channel.GetCurrentBiosFanModeName();
	}

	public Task<string> GetCurrentBiosFanModeNameAsync()
	{
		return Channel.GetCurrentBiosFanModeNameAsync();
	}

	public bool SetFanMode(string FanMode)
	{
		return Channel.SetFanMode(FanMode);
	}

	public Task<bool> SetFanModeAsync(string FanMode)
	{
		return Channel.SetFanModeAsync(FanMode);
	}

	public bool SetCustomPowerSetting(PowerSetting PowerSetting)
	{
		return Channel.SetCustomPowerSetting(PowerSetting);
	}

	public Task<bool> SetCustomPowerSettingAsync(PowerSetting PowerSetting)
	{
		return Channel.SetCustomPowerSettingAsync(PowerSetting);
	}

	public bool ApplyBiosPowerProfile(string PowerProfileName)
	{
		return Channel.ApplyBiosPowerProfile(PowerProfileName);
	}

	public Task<bool> ApplyBiosPowerProfileAsync(string PowerProfileName)
	{
		return Channel.ApplyBiosPowerProfileAsync(PowerProfileName);
	}

	public string[] SupportedFanTypes()
	{
		return Channel.SupportedFanTypes();
	}

	public Task<string[]> SupportedFanTypesAsync()
	{
		return Channel.SupportedFanTypesAsync();
	}

	public bool SetFanCustom(FanControlAllFanSettings FanSettings)
	{
		return Channel.SetFanCustom(FanSettings);
	}

	public Task<bool> SetFanCustomAsync(FanControlAllFanSettings FanSettings)
	{
		return Channel.SetFanCustomAsync(FanSettings);
	}

	public bool SetFanOffConfiguration(string FanMode, bool isFanOff)
	{
		return Channel.SetFanOffConfiguration(FanMode, isFanOff);
	}

	public Task<bool> SetFanOffConfigurationAsync(string FanMode, bool isFanOff)
	{
		return Channel.SetFanOffConfigurationAsync(FanMode, isFanOff);
	}

	public FanProfile23 GetFanProfileData23(string fanProfileName)
	{
		return Channel.GetFanProfileData23(fanProfileName);
	}

	public Task<FanProfile23> GetFanProfileData23Async(string fanProfileName)
	{
		return Channel.GetFanProfileData23Async(fanProfileName);
	}

	public bool SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes)
	{
		return Channel.SetFanDutyCycleFixedMode(selectedFanMode, FanBytes);
	}

	public Task<bool> SetFanDutyCycleFixedModeAsync(string selectedFanMode, byte[] FanBytes)
	{
		return Channel.SetFanDutyCycleFixedModeAsync(selectedFanMode, FanBytes);
	}

	public FanProfileRN[] GetFanProfileDataRN()
	{
		return Channel.GetFanProfileDataRN();
	}

	public Task<FanProfileRN[]> GetFanProfileDataRNAsync()
	{
		return Channel.GetFanProfileDataRNAsync();
	}

	public bool SetFanModeRN(string FanMode, string FanType, FanProfileRN[] fanProfiles)
	{
		return Channel.SetFanModeRN(FanMode, FanType, fanProfiles);
	}

	public Task<bool> SetFanModeRNAsync(string FanMode, string FanType, FanProfileRN[] fanProfiles)
	{
		return Channel.SetFanModeRNAsync(FanMode, FanType, fanProfiles);
	}
}
