using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FeatureType", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public enum FeatureType
{
	[EnumMember]
	Error = -1,
	[EnumMember]
	Updates,
	[EnumMember]
	ChassisLed,
	[EnumMember]
	KeyboardLedSingleColor,
	[EnumMember]
	KeyboardLedIndividualKeyControl,
	[EnumMember]
	LightbarSingleColor,
	[EnumMember]
	LightbarMultiColor,
	[EnumMember]
	CpuPowerMode,
	[EnumMember]
	dGpuPoewrMode,
	[EnumMember]
	FanPowerMode,
	[EnumMember]
	SystemMonitor,
	[EnumMember]
	DisplaySettings,
	[EnumMember]
	KeyWinLock,
	[EnumMember]
	KeyFnLock,
	[EnumMember]
	KeyNumPad,
	[EnumMember]
	KeyOSD,
	[EnumMember]
	NucWakeSleepSettings,
	[EnumMember]
	UserPresence,
	[EnumMember]
	BatteryChargeLimit,
	[EnumMember]
	TrackpadEnable,
	[EnumMember]
	IntelligentCollab,
	[EnumMember]
	AudioTuning,
	[EnumMember]
	BatteryChargeRate,
	[EnumMember]
	WhisperMode,
	[EnumMember]
	UserPresenceCst
}
