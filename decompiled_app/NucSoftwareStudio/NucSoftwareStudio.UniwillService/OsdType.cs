using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.UniwillService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "OsdType", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public enum OsdType
{
	[EnumMember]
	PerformanceMode,
	[EnumMember]
	AirplaneMode,
	[EnumMember]
	TouchpadOn,
	[EnumMember]
	FnLock,
	[EnumMember]
	NumPad,
	[EnumMember]
	CapsLock,
	[EnumMember]
	WindowsKeyLock,
	[EnumMember]
	PowerAdapter,
	[EnumMember]
	MicrophoneMute,
	[EnumMember]
	RGBKeyboardBrightness,
	[EnumMember]
	DisableTouchpadToggle
}
