using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace UniwillSdkClient.UniwillService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "UniwillService.IUniwillService", CallbackContract = typeof(IUniwillServiceCallback), SessionMode = SessionMode.Required)]
public interface IUniwillService
{
	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetSupportedFeatures", ReplyAction = "http://tempuri.org/IUniwillService/GetSupportedFeaturesResponse")]
	Feature[] GetSupportedFeatures();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetSupportedFeatures", ReplyAction = "http://tempuri.org/IUniwillService/GetSupportedFeaturesResponse")]
	Task<Feature[]> GetSupportedFeaturesAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetIdleTimerValue")]
	void SetIdleTimerValue(int Seconds, bool Ac);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetIdleTimerValue")]
	Task SetIdleTimerValueAsync(int Seconds, bool Ac);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetIdleTimerValue", ReplyAction = "http://tempuri.org/IUniwillService/GetIdleTimerValueResponse")]
	Tuple<int, int> GetIdleTimerValue();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetIdleTimerValue", ReplyAction = "http://tempuri.org/IUniwillService/GetIdleTimerValueResponse")]
	Task<Tuple<int, int>> GetIdleTimerValueAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetLightBarIdleTimerValue")]
	void SetLightBarIdleTimerValue(int Seconds, bool Ac);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetLightBarIdleTimerValue")]
	Task SetLightBarIdleTimerValueAsync(int Seconds, bool Ac);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetLightBarIdleTimerValue", ReplyAction = "http://tempuri.org/IUniwillService/GetLightBarIdleTimerValueResponse")]
	Tuple<int, int> GetLightBarIdleTimerValue();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetLightBarIdleTimerValue", ReplyAction = "http://tempuri.org/IUniwillService/GetLightBarIdleTimerValueResponse")]
	Task<Tuple<int, int>> GetLightBarIdleTimerValueAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/EnableKeyboardLeds")]
	void EnableKeyboardLeds(bool Enable);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/EnableKeyboardLeds")]
	Task EnableKeyboardLedsAsync(bool Enable);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/EnableProjectAurora")]
	void EnableProjectAurora(bool Enable);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/EnableProjectAurora")]
	Task EnableProjectAuroraAsync(bool Enable);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetProjectAuroraState", ReplyAction = "http://tempuri.org/IUniwillService/GetProjectAuroraStateResponse")]
	bool GetProjectAuroraState();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetProjectAuroraState", ReplyAction = "http://tempuri.org/IUniwillService/GetProjectAuroraStateResponse")]
	Task<bool> GetProjectAuroraStateAsync();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetProjectAuroraInstalled", ReplyAction = "http://tempuri.org/IUniwillService/GetProjectAuroraInstalledResponse")]
	bool GetProjectAuroraInstalled();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetProjectAuroraInstalled", ReplyAction = "http://tempuri.org/IUniwillService/GetProjectAuroraInstalledResponse")]
	Task<bool> GetProjectAuroraInstalledAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetKeyboardBrightness")]
	void SetKeyboardBrightness(int Brightness, bool Ac);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetKeyboardBrightness")]
	Task SetKeyboardBrightnessAsync(int Brightness, bool Ac);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetKeyboardBrightness", ReplyAction = "http://tempuri.org/IUniwillService/GetKeyboardBrightnessResponse")]
	Tuple<int, int> GetKeyboardBrightness();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetKeyboardBrightness", ReplyAction = "http://tempuri.org/IUniwillService/GetKeyboardBrightnessResponse")]
	Task<Tuple<int, int>> GetKeyboardBrightnessAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/ApplyKeyboardLedProfile")]
	void ApplyKeyboardLedProfile(KeyboardLedProfile Profile);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/ApplyKeyboardLedProfile")]
	Task ApplyKeyboardLedProfileAsync(KeyboardLedProfile Profile);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentKeyboardLedProfile", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentKeyboardLedProfileResponse")]
	KeyboardLedProfile GetCurrentKeyboardLedProfile();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentKeyboardLedProfile", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentKeyboardLedProfileResponse")]
	Task<KeyboardLedProfile> GetCurrentKeyboardLedProfileAsync();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetSupportedCapabilities", ReplyAction = "http://tempuri.org/IUniwillService/GetSupportedCapabilitiesResponse")]
	RGBKeyboardCapabilities GetSupportedCapabilities();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetSupportedCapabilities", ReplyAction = "http://tempuri.org/IUniwillService/GetSupportedCapabilitiesResponse")]
	Task<RGBKeyboardCapabilities> GetSupportedCapabilitiesAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetLightbarProfile")]
	void SetLightbarProfile(LightbarProfile Profile);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetLightbarProfile")]
	Task SetLightbarProfileAsync(LightbarProfile Profile);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentLightbarProfile", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentLightbarProfileResponse")]
	LightbarProfile GetCurrentLightbarProfile();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentLightbarProfile", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentLightbarProfileResponse")]
	Task<LightbarProfile> GetCurrentLightbarProfileAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/EnableKey")]
	void EnableKey(KeyState State);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/EnableKey")]
	Task EnableKeyAsync(KeyState State);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentKeyStates", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentKeyStatesResponse")]
	KeyState[] GetCurrentKeyStates();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentKeyStates", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentKeyStatesResponse")]
	Task<KeyState[]> GetCurrentKeyStatesAsync();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetDefaultProfiles", ReplyAction = "http://tempuri.org/IUniwillService/GetDefaultProfilesResponse")]
	PerformanceProfile[] GetDefaultProfiles();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetDefaultProfiles", ReplyAction = "http://tempuri.org/IUniwillService/GetDefaultProfilesResponse")]
	Task<PerformanceProfile[]> GetDefaultProfilesAsync();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetDefaultProfile", ReplyAction = "http://tempuri.org/IUniwillService/GetDefaultProfileResponse")]
	ProfileIndex GetDefaultProfile();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetDefaultProfile", ReplyAction = "http://tempuri.org/IUniwillService/GetDefaultProfileResponse")]
	Task<ProfileIndex> GetDefaultProfileAsync();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentProfiles", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentProfilesResponse")]
	PerformanceProfile[] GetCurrentProfiles();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentProfiles", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentProfilesResponse")]
	Task<PerformanceProfile[]> GetCurrentProfilesAsync();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentState", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentStateResponse")]
	PerformanceState GetCurrentState();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetCurrentState", ReplyAction = "http://tempuri.org/IUniwillService/GetCurrentStateResponse")]
	Task<PerformanceState> GetCurrentStateAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/ApplyProfile")]
	void ApplyProfile(ProfileIndex Index);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/ApplyProfile")]
	Task ApplyProfileAsync(ProfileIndex Index);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/UpdateProfile")]
	void UpdateProfile(ProfileIndex Index, PerformanceMode Ac, PerformanceMode Dc);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/UpdateProfile")]
	Task UpdateProfileAsync(ProfileIndex Index, PerformanceMode Ac, PerformanceMode Dc);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetWhisperModeType", ReplyAction = "http://tempuri.org/IUniwillService/GetWhisperModeTypeResponse")]
	WhisperModeType GetWhisperModeType();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetWhisperModeType", ReplyAction = "http://tempuri.org/IUniwillService/GetWhisperModeTypeResponse")]
	Task<WhisperModeType> GetWhisperModeTypeAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetWhisperModeType")]
	void SetWhisperModeType(WhisperModeType Type);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetWhisperModeType")]
	Task SetWhisperModeTypeAsync(WhisperModeType Type);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetWhisperModeFps", ReplyAction = "http://tempuri.org/IUniwillService/GetWhisperModeFpsResponse")]
	int GetWhisperModeFps();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetWhisperModeFps", ReplyAction = "http://tempuri.org/IUniwillService/GetWhisperModeFpsResponse")]
	Task<int> GetWhisperModeFpsAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetWhisperModeFps")]
	void SetWhisperModeFps(int Fps);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetWhisperModeFps")]
	Task SetWhisperModeFpsAsync(int Fps);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/ApplyBenchmarkMode")]
	void ApplyBenchmarkMode(bool On);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/ApplyBenchmarkMode")]
	Task ApplyBenchmarkModeAsync(bool On);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/DisablePassiveCoolingMode")]
	void DisablePassiveCoolingMode(bool Disable);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/DisablePassiveCoolingMode")]
	Task DisablePassiveCoolingModeAsync(bool Disable);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetFanCurve", ReplyAction = "http://tempuri.org/IUniwillService/GetFanCurveResponse")]
	FanCurveSetting GetFanCurve(int FanMode, FanType TableType, bool AC);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetFanCurve", ReplyAction = "http://tempuri.org/IUniwillService/GetFanCurveResponse")]
	Task<FanCurveSetting> GetFanCurveAsync(int FanMode, FanType TableType, bool AC);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetFanTable")]
	void SetFanTable(int FanMode, FanType TableType, bool AC, FanTable Table);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetFanTable")]
	Task SetFanTableAsync(int FanMode, FanType TableType, bool AC, FanTable Table);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/RestoreFanCurve", ReplyAction = "http://tempuri.org/IUniwillService/RestoreFanCurveResponse")]
	FanCurveSetting RestoreFanCurve(int FanMode, FanType TableType, bool AC);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/RestoreFanCurve", ReplyAction = "http://tempuri.org/IUniwillService/RestoreFanCurveResponse")]
	Task<FanCurveSetting> RestoreFanCurveAsync(int FanMode, FanType TableType, bool AC);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetBatteryChargingLevel", ReplyAction = "http://tempuri.org/IUniwillService/GetBatteryChargingLevelResponse")]
	int GetBatteryChargingLevel();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetBatteryChargingLevel", ReplyAction = "http://tempuri.org/IUniwillService/GetBatteryChargingLevelResponse")]
	Task<int> GetBatteryChargingLevelAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetBatteryChargingLevel")]
	void SetBatteryChargingLevel(int Level);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetBatteryChargingLevel")]
	Task SetBatteryChargingLevelAsync(int Level);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetBatteryMode")]
	void SetBatteryMode(BatteryMode NewMode);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetBatteryMode")]
	Task SetBatteryModeAsync(BatteryMode NewMode);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetBatteryMode", ReplyAction = "http://tempuri.org/IUniwillService/GetBatteryModeResponse")]
	BatteryMode GetBatteryMode();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetBatteryMode", ReplyAction = "http://tempuri.org/IUniwillService/GetBatteryModeResponse")]
	Task<BatteryMode> GetBatteryModeAsync();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetBatteryState", ReplyAction = "http://tempuri.org/IUniwillService/GetBatteryStateResponse")]
	Tuple<bool, bool> GetBatteryState();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetBatteryState", ReplyAction = "http://tempuri.org/IUniwillService/GetBatteryStateResponse")]
	Task<Tuple<bool, bool>> GetBatteryStateAsync();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/SetDGpuTargetTemperature", ReplyAction = "http://tempuri.org/IUniwillService/SetDGpuTargetTemperatureResponse")]
	int SetDGpuTargetTemperature(int Temperature);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/SetDGpuTargetTemperature", ReplyAction = "http://tempuri.org/IUniwillService/SetDGpuTargetTemperatureResponse")]
	Task<int> SetDGpuTargetTemperatureAsync(int Temperature);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/EnableOnScreenDisplay", ReplyAction = "http://tempuri.org/IUniwillService/EnableOnScreenDisplayResponse")]
	bool EnableOnScreenDisplay(bool Enable);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/EnableOnScreenDisplay", ReplyAction = "http://tempuri.org/IUniwillService/EnableOnScreenDisplayResponse")]
	Task<bool> EnableOnScreenDisplayAsync(bool Enable);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/EnableDisplay", ReplyAction = "http://tempuri.org/IUniwillService/EnableDisplayResponse")]
	bool EnableDisplay(bool Enable);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/EnableDisplay", ReplyAction = "http://tempuri.org/IUniwillService/EnableDisplayResponse")]
	Task<bool> EnableDisplayAsync(bool Enable);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/EnableDisplayModeManagement", ReplyAction = "http://tempuri.org/IUniwillService/EnableDisplayModeManagementResponse")]
	bool EnableDisplayModeManagement(bool Enable);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/EnableDisplayModeManagement", ReplyAction = "http://tempuri.org/IUniwillService/EnableDisplayModeManagementResponse")]
	Task<bool> EnableDisplayModeManagementAsync(bool Enable);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetSavedDisplayModes", ReplyAction = "http://tempuri.org/IUniwillService/GetSavedDisplayModesResponse")]
	DisplayMode[] GetSavedDisplayModes();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/GetSavedDisplayModes", ReplyAction = "http://tempuri.org/IUniwillService/GetSavedDisplayModesResponse")]
	Task<DisplayMode[]> GetSavedDisplayModesAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/GetCurrentDisplayMode")]
	void GetCurrentDisplayMode();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/GetCurrentDisplayMode")]
	Task GetCurrentDisplayModeAsync();

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetDisplayMode")]
	void SetDisplayMode(int Index);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SetDisplayMode")]
	Task SetDisplayModeAsync(int Index);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/UpdateDisplayMode")]
	void UpdateDisplayMode(DisplayMode Mode);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/UpdateDisplayMode")]
	Task UpdateDisplayModeAsync(DisplayMode Mode);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/RecoverDisplayMode")]
	void RecoverDisplayMode(int Index);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/RecoverDisplayMode")]
	Task RecoverDisplayModeAsync(int Index);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/SysMonitorSubscribe", ReplyAction = "http://tempuri.org/IUniwillService/SysMonitorSubscribeResponse")]
	bool SysMonitorSubscribe(SysMonType[] Types);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/SysMonitorSubscribe", ReplyAction = "http://tempuri.org/IUniwillService/SysMonitorSubscribeResponse")]
	Task<bool> SysMonitorSubscribeAsync(SysMonType[] Types);

	[OperationContract(Action = "http://tempuri.org/IUniwillService/StopMonitoring", ReplyAction = "http://tempuri.org/IUniwillService/StopMonitoringResponse")]
	bool StopMonitoring();

	[OperationContract(Action = "http://tempuri.org/IUniwillService/StopMonitoring", ReplyAction = "http://tempuri.org/IUniwillService/StopMonitoringResponse")]
	Task<bool> StopMonitoringAsync();
}
