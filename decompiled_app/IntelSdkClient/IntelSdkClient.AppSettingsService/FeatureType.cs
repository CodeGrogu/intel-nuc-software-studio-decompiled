using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace IntelSdkClient.AppSettingsService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FeatureType", Namespace = "http://schemas.datacontract.org/2004/07/NucSoftwareStudioService.FeatureSupport.Models")]
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
	dGpuPowerMode,
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
	UserPresence
}
