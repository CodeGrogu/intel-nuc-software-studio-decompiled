using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RGBKeyboardDirection", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public enum RGBKeyboardDirection
{
	[EnumMember]
	None,
	[EnumMember]
	Right,
	[EnumMember]
	Left,
	[EnumMember]
	Up,
	[EnumMember]
	Down,
	[EnumMember]
	Auto,
	[EnumMember]
	PressAnyKey
}
