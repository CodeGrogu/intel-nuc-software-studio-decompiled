using System.CodeDom.Compiler;
using System.ServiceModel;

namespace NucSoftwareStudio.UniwillService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IUniwillServiceCallback
{
	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/IdleTimerNotification")]
	void IdleTimerNotification(int AcSeconds, int DcSeconds);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/LightBarIdleTimerNotification")]
	void LightBarIdleTimerNotification(int AcSeconds, int DcSeconds);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/EnableKeyboardLedsNotification")]
	void EnableKeyboardLedsNotification(bool Enable);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/ProjectAuroraStateNotification")]
	void ProjectAuroraStateNotification(bool Enable);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/CurrentKeyboardLedProfileNotification")]
	void CurrentKeyboardLedProfileNotification(KeyboardLedProfile CurrentProfile);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/KeyboardBrightnessNotification")]
	void KeyboardBrightnessNotification(int Ac, int Dc);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/CurrentLightbarProfileNotification")]
	void CurrentLightbarProfileNotification(LightbarProfile CurrentProfile);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/KeyStateNotification")]
	void KeyStateNotification(KeyState State);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/CurrentProfile")]
	void CurrentProfile(ProfileIndex Index);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/ProfileSetting")]
	void ProfileSetting(PerformanceProfile Profile);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/BenchmarkMode")]
	void BenchmarkMode(bool On);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/PassiveCoolingMode")]
	void PassiveCoolingMode(bool Disabled);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/UpdateFanCurveSetting")]
	void UpdateFanCurveSetting(FanCurveSetting Setting);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/OsdNotification")]
	void OsdNotification(OsdType Type, int Status);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/DisplayModeCallback")]
	void DisplayModeCallback(DisplayMode Mode, bool Current);

	[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IUniwillService/SysMonitorNotification")]
	void SysMonitorNotification(SystemMonitorData Data);
}
