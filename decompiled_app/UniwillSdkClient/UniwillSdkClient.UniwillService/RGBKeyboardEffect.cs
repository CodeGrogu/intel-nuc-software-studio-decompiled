using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RGBKeyboardEffect", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public enum RGBKeyboardEffect
{
	[EnumMember]
	Monocolor,
	[EnumMember]
	Breathing,
	[EnumMember]
	Wave,
	[EnumMember]
	Reactive,
	[EnumMember]
	Rainbow,
	[EnumMember]
	Ripple,
	[EnumMember]
	Raindrop,
	[EnumMember]
	Marquee,
	[EnumMember]
	Aurora,
	[EnumMember]
	Spark,
	[EnumMember]
	Music,
	[EnumMember]
	UserMode,
	[EnumMember]
	GamingMode
}
