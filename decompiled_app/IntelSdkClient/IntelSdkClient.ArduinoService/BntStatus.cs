using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace IntelSdkClient.ArduinoService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "BntStatus", Namespace = "http://schemas.datacontract.org/2004/07/SerialDeviceAccessLibrary")]
public enum BntStatus : byte
{
	[EnumMember]
	Off = 0,
	[EnumMember]
	On = 1,
	[EnumMember]
	NotSetting = byte.MaxValue
}
