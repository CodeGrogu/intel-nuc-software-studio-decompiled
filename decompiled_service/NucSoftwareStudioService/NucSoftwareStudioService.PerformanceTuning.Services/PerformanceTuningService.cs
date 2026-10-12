using System;
using System.Collections.Generic;
using System.ServiceModel;
using NucSoftwareStudioService.Wcf;
using PerformanceTuningLibrary;

namespace NucSoftwareStudioService.PerformanceTuning.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
internal class PerformanceTuningService : IPerformanceTuningServiceContract
{
	public bool? GetPowerLevelControlSupported()
	{
		try
		{
			return PowerLevelControl.PowerLevelControlSupported;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public PowerSetting GetCurrentPowerSettings()
	{
		try
		{
			return PowerLevelControl.GetCurrentPowerSettings();
		}
		catch (Exception)
		{
			return default;
		}
	}

	public List<PowerProfile> GetSupportedPowerProfiles()
	{
		try
		{
			return PowerLevelControl.GetSupportedPowerProfiles();
		}
		catch (Exception)
		{
			return new List<PowerProfile>();
		}
	}

	public bool? IsFanEcSupported()
	{
		try
		{
			return FanControl.FanEcSupported;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool? IsFanProfilesSupported()
	{
		try
		{
			return FanControl.FanProfilesSupported;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public FanProfile GetFanProfileData(string fanProfileName)
	{
		try
		{
			return FanControl.GetSelectedFanProfile(fanProfileName);
		}
		catch (Exception)
		{
			return default;
		}
	}

	public FanProfile23 GetFanProfileData23(string fanProfileName)
	{
		try
		{
			return FanControl.GetSelectedFanProfile23(fanProfileName);
		}
		catch (Exception)
		{
			return default;
		}
	}

	public List<FanProfileRN> GetFanProfileDataRN()
	{
		try
		{
			return FanControl.GetSelectedFanProfileRN();
		}
		catch (Exception)
		{
			return new List<FanProfileRN>();
		}
	}

	public List<string> GetSupportedFanModes()
	{
		try
		{
			return FanControl.SupportedFanModes;
		}
		catch (Exception)
		{
			return new List<string>();
		}
	}

	public string GetCurrentBiosFanModeName()
	{
		try
		{
			return FanControl.GetCurrentBiosFanModeName();
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public bool SetFanMode(string FanMode)
	{
		try
		{
			return FanControl.SetFanMode(FanMode);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetFanModeRN(string FanMode, string FanType, List<FanProfileRN> fanProfiles)
	{
		try
		{
			return FanControl.SetFanModeRN(FanMode, FanType, fanProfiles);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetFanOffConfiguration(string FanMode, bool isFanOff)
	{
		try
		{
			return FanControl.SetFanOffConfiguration(FanMode, isFanOff);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetCustomPowerSetting(PowerSetting powerSetting)
	{
		try
		{
			return PowerLevelControl.SetCustomPowerSetting(powerSetting);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool ApplyBiosPowerProfile(string powerProfileName)
	{
		try
		{
			return PowerLevelControl.ApplyBiosPowerProfile(powerProfileName);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public List<string> SupportedFanTypes()
	{
		try
		{
			return FanControl.SupportedFanTypes();
		}
		catch (Exception)
		{
			return new List<string>();
		}
	}

	public bool SetFanCustom(FanControl.AllFanSettings FanSettings)
	{
		try
		{
			return FanControl.SetFanCustom(FanSettings);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes)
	{
		try
		{
			return FanControl.SetFanDutyCycleFixedMode(selectedFanMode, FanBytes);
		}
		catch (Exception)
		{
			return false;
		}
	}
}
