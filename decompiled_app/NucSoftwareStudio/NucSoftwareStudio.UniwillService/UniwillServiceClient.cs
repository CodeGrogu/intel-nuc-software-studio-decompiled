using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace NucSoftwareStudio.UniwillService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class UniwillServiceClient : DuplexClientBase<IUniwillService>, IUniwillService
{
	public UniwillServiceClient(InstanceContext callbackInstance)
		: base(callbackInstance)
	{
	}

	public UniwillServiceClient(InstanceContext callbackInstance, string endpointConfigurationName)
		: base(callbackInstance, endpointConfigurationName)
	{
	}

	public UniwillServiceClient(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)
		: base(callbackInstance, endpointConfigurationName, remoteAddress)
	{
	}

	public UniwillServiceClient(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(callbackInstance, endpointConfigurationName, remoteAddress)
	{
	}

	public UniwillServiceClient(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
		: base(callbackInstance, binding, remoteAddress)
	{
	}

	public Feature[] GetSupportedFeatures()
	{
		return Channel.GetSupportedFeatures();
	}

	public Task<Feature[]> GetSupportedFeaturesAsync()
	{
		return Channel.GetSupportedFeaturesAsync();
	}

	public void SetIdleTimerValue(int Seconds, bool Ac)
	{
		Channel.SetIdleTimerValue(Seconds, Ac);
	}

	public Task SetIdleTimerValueAsync(int Seconds, bool Ac)
	{
		return Channel.SetIdleTimerValueAsync(Seconds, Ac);
	}

	public Tuple<int, int> GetIdleTimerValue()
	{
		return Channel.GetIdleTimerValue();
	}

	public Task<Tuple<int, int>> GetIdleTimerValueAsync()
	{
		return Channel.GetIdleTimerValueAsync();
	}

	public void SetLightBarIdleTimerValue(int Seconds, bool Ac)
	{
		Channel.SetLightBarIdleTimerValue(Seconds, Ac);
	}

	public Task SetLightBarIdleTimerValueAsync(int Seconds, bool Ac)
	{
		return Channel.SetLightBarIdleTimerValueAsync(Seconds, Ac);
	}

	public Tuple<int, int> GetLightBarIdleTimerValue()
	{
		return Channel.GetLightBarIdleTimerValue();
	}

	public Task<Tuple<int, int>> GetLightBarIdleTimerValueAsync()
	{
		return Channel.GetLightBarIdleTimerValueAsync();
	}

	public void EnableKeyboardLeds(bool Enable)
	{
		Channel.EnableKeyboardLeds(Enable);
	}

	public Task EnableKeyboardLedsAsync(bool Enable)
	{
		return Channel.EnableKeyboardLedsAsync(Enable);
	}

	public void EnableProjectAurora(bool Enable)
	{
		Channel.EnableProjectAurora(Enable);
	}

	public Task EnableProjectAuroraAsync(bool Enable)
	{
		return Channel.EnableProjectAuroraAsync(Enable);
	}

	public bool GetProjectAuroraState()
	{
		return Channel.GetProjectAuroraState();
	}

	public Task<bool> GetProjectAuroraStateAsync()
	{
		return Channel.GetProjectAuroraStateAsync();
	}

	public bool GetProjectAuroraInstalled()
	{
		return Channel.GetProjectAuroraInstalled();
	}

	public Task<bool> GetProjectAuroraInstalledAsync()
	{
		return Channel.GetProjectAuroraInstalledAsync();
	}

	public void SetKeyboardBrightness(int Brightness, bool Ac)
	{
		Channel.SetKeyboardBrightness(Brightness, Ac);
	}

	public Task SetKeyboardBrightnessAsync(int Brightness, bool Ac)
	{
		return Channel.SetKeyboardBrightnessAsync(Brightness, Ac);
	}

	public Tuple<int, int> GetKeyboardBrightness()
	{
		return Channel.GetKeyboardBrightness();
	}

	public Task<Tuple<int, int>> GetKeyboardBrightnessAsync()
	{
		return Channel.GetKeyboardBrightnessAsync();
	}

	public void ApplyKeyboardLedProfile(KeyboardLedProfile Profile)
	{
		Channel.ApplyKeyboardLedProfile(Profile);
	}

	public Task ApplyKeyboardLedProfileAsync(KeyboardLedProfile Profile)
	{
		return Channel.ApplyKeyboardLedProfileAsync(Profile);
	}

	public KeyboardLedProfile GetCurrentKeyboardLedProfile()
	{
		return Channel.GetCurrentKeyboardLedProfile();
	}

	public Task<KeyboardLedProfile> GetCurrentKeyboardLedProfileAsync()
	{
		return Channel.GetCurrentKeyboardLedProfileAsync();
	}

	public RGBKeyboardCapabilities GetSupportedCapabilities()
	{
		return Channel.GetSupportedCapabilities();
	}

	public Task<RGBKeyboardCapabilities> GetSupportedCapabilitiesAsync()
	{
		return Channel.GetSupportedCapabilitiesAsync();
	}

	public void SetLightbarProfile(LightbarProfile Profile)
	{
		Channel.SetLightbarProfile(Profile);
	}

	public Task SetLightbarProfileAsync(LightbarProfile Profile)
	{
		return Channel.SetLightbarProfileAsync(Profile);
	}

	public LightbarProfile GetCurrentLightbarProfile()
	{
		return Channel.GetCurrentLightbarProfile();
	}

	public Task<LightbarProfile> GetCurrentLightbarProfileAsync()
	{
		return Channel.GetCurrentLightbarProfileAsync();
	}

	public void EnableKey(KeyState State)
	{
		Channel.EnableKey(State);
	}

	public Task EnableKeyAsync(KeyState State)
	{
		return Channel.EnableKeyAsync(State);
	}

	public KeyState[] GetCurrentKeyStates()
	{
		return Channel.GetCurrentKeyStates();
	}

	public Task<KeyState[]> GetCurrentKeyStatesAsync()
	{
		return Channel.GetCurrentKeyStatesAsync();
	}

	public PerformanceProfile[] GetDefaultProfiles()
	{
		return Channel.GetDefaultProfiles();
	}

	public Task<PerformanceProfile[]> GetDefaultProfilesAsync()
	{
		return Channel.GetDefaultProfilesAsync();
	}

	public ProfileIndex GetDefaultProfile()
	{
		return Channel.GetDefaultProfile();
	}

	public Task<ProfileIndex> GetDefaultProfileAsync()
	{
		return Channel.GetDefaultProfileAsync();
	}

	public PerformanceProfile[] GetCurrentProfiles()
	{
		return Channel.GetCurrentProfiles();
	}

	public Task<PerformanceProfile[]> GetCurrentProfilesAsync()
	{
		return Channel.GetCurrentProfilesAsync();
	}

	public PerformanceState GetCurrentState()
	{
		return Channel.GetCurrentState();
	}

	public Task<PerformanceState> GetCurrentStateAsync()
	{
		return Channel.GetCurrentStateAsync();
	}

	public void ApplyProfile(ProfileIndex Index)
	{
		Channel.ApplyProfile(Index);
	}

	public Task ApplyProfileAsync(ProfileIndex Index)
	{
		return Channel.ApplyProfileAsync(Index);
	}

	public void UpdateProfile(ProfileIndex Index, PerformanceMode Ac, PerformanceMode Dc)
	{
		Channel.UpdateProfile(Index, Ac, Dc);
	}

	public Task UpdateProfileAsync(ProfileIndex Index, PerformanceMode Ac, PerformanceMode Dc)
	{
		return Channel.UpdateProfileAsync(Index, Ac, Dc);
	}

	public void ApplyBenchmarkMode(bool On)
	{
		Channel.ApplyBenchmarkMode(On);
	}

	public Task ApplyBenchmarkModeAsync(bool On)
	{
		return Channel.ApplyBenchmarkModeAsync(On);
	}

	public void DisablePassiveCoolingMode(bool Disable)
	{
		Channel.DisablePassiveCoolingMode(Disable);
	}

	public Task DisablePassiveCoolingModeAsync(bool Disable)
	{
		return Channel.DisablePassiveCoolingModeAsync(Disable);
	}

	public FanCurveSetting GetFanCurve(int FanMode, FanType TableType, bool AC)
	{
		return Channel.GetFanCurve(FanMode, TableType, AC);
	}

	public Task<FanCurveSetting> GetFanCurveAsync(int FanMode, FanType TableType, bool AC)
	{
		return Channel.GetFanCurveAsync(FanMode, TableType, AC);
	}

	public void SetFanTable(int FanMode, FanType TableType, bool AC, FanTable Table)
	{
		Channel.SetFanTable(FanMode, TableType, AC, Table);
	}

	public Task SetFanTableAsync(int FanMode, FanType TableType, bool AC, FanTable Table)
	{
		return Channel.SetFanTableAsync(FanMode, TableType, AC, Table);
	}

	public FanCurveSetting RestoreFanCurve(int FanMode, FanType TableType, bool AC)
	{
		return Channel.RestoreFanCurve(FanMode, TableType, AC);
	}

	public Task<FanCurveSetting> RestoreFanCurveAsync(int FanMode, FanType TableType, bool AC)
	{
		return Channel.RestoreFanCurveAsync(FanMode, TableType, AC);
	}

	public int GetBatteryChargingLevel()
	{
		return Channel.GetBatteryChargingLevel();
	}

	public Task<int> GetBatteryChargingLevelAsync()
	{
		return Channel.GetBatteryChargingLevelAsync();
	}

	public void SetBatteryChargingLevel(int Level)
	{
		Channel.SetBatteryChargingLevel(Level);
	}

	public Task SetBatteryChargingLevelAsync(int Level)
	{
		return Channel.SetBatteryChargingLevelAsync(Level);
	}

	public bool EnableOnScreenDisplay(bool Enable)
	{
		return Channel.EnableOnScreenDisplay(Enable);
	}

	public Task<bool> EnableOnScreenDisplayAsync(bool Enable)
	{
		return Channel.EnableOnScreenDisplayAsync(Enable);
	}

	public bool EnableDisplay(bool Enable)
	{
		return Channel.EnableDisplay(Enable);
	}

	public Task<bool> EnableDisplayAsync(bool Enable)
	{
		return Channel.EnableDisplayAsync(Enable);
	}

	public bool EnableDisplayModeManagement(bool Enable)
	{
		return Channel.EnableDisplayModeManagement(Enable);
	}

	public Task<bool> EnableDisplayModeManagementAsync(bool Enable)
	{
		return Channel.EnableDisplayModeManagementAsync(Enable);
	}

	public DisplayMode[] GetSavedDisplayModes()
	{
		return Channel.GetSavedDisplayModes();
	}

	public Task<DisplayMode[]> GetSavedDisplayModesAsync()
	{
		return Channel.GetSavedDisplayModesAsync();
	}

	public void GetCurrentDisplayMode()
	{
		Channel.GetCurrentDisplayMode();
	}

	public Task GetCurrentDisplayModeAsync()
	{
		return Channel.GetCurrentDisplayModeAsync();
	}

	public void SetDisplayMode(int Index)
	{
		Channel.SetDisplayMode(Index);
	}

	public Task SetDisplayModeAsync(int Index)
	{
		return Channel.SetDisplayModeAsync(Index);
	}

	public void UpdateDisplayMode(DisplayMode Mode)
	{
		Channel.UpdateDisplayMode(Mode);
	}

	public Task UpdateDisplayModeAsync(DisplayMode Mode)
	{
		return Channel.UpdateDisplayModeAsync(Mode);
	}

	public void RecoverDisplayMode(int Index)
	{
		Channel.RecoverDisplayMode(Index);
	}

	public Task RecoverDisplayModeAsync(int Index)
	{
		return Channel.RecoverDisplayModeAsync(Index);
	}

	public bool SysMonitorSubscribe(SysMonType[] Types)
	{
		return Channel.SysMonitorSubscribe(Types);
	}

	public Task<bool> SysMonitorSubscribeAsync(SysMonType[] Types)
	{
		return Channel.SysMonitorSubscribeAsync(Types);
	}

	public bool StopMonitoring()
	{
		return Channel.StopMonitoring();
	}

	public Task<bool> StopMonitoringAsync()
	{
		return Channel.StopMonitoringAsync();
	}
}
