using System.Runtime.Serialization;

namespace NucSoftwareStudioService.FeatureSupport.Models;

[DataContract]
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
