using System.ServiceModel;
using IntelSdkClient.PerformanceTuningService;

namespace IntelSdkClient.Proxy;

public class PerformanceTuningServiceProxy : ClientBase<IPerformanceTuningServiceContract>
{
	public bool? GetPowerLevelControlSupported()
	{
		return Channel.GetPowerLevelControlSupported();
	}

	public PowerSetting GetCurrentPowerSettings()
	{
		return Channel.GetCurrentPowerSettings();
	}

	public PowerProfile[] GetSupportedPowerProfiles()
	{
		return Channel.GetSupportedPowerProfiles();
	}

	public bool? IsFanEcSupported()
	{
		return Channel.IsFanEcSupported();
	}

	public bool? IsFanProfilesSupported()
	{
		return Channel.IsFanProfilesSupported();
	}

	public string[] GetSupportedFanModes()
	{
		return Channel.GetSupportedFanModes();
	}

	public string GetCurrentBiosFanModeName()
	{
		return Channel.GetCurrentBiosFanModeName();
	}

	public bool SetFanMode(string FanMode)
	{
		return Channel.SetFanMode(FanMode);
	}

	public bool SetCustomPowerSetting(PowerSetting powerSetting)
	{
		return Channel.SetCustomPowerSetting(powerSetting);
	}

	public bool ApplyBiosPowerProfile(string powerSettingName)
	{
		return Channel.ApplyBiosPowerProfile(powerSettingName);
	}

	public FanProfile GetFanProfileData(string currentFanMode)
	{
		return Channel.GetFanProfileData(currentFanMode);
	}
}
