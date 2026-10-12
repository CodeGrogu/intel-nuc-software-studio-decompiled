using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace IntelSdkClient.SystemMonitorService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanType", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public enum FanType : byte
{
	[EnumMember]
	NotConnected,
	[EnumMember]
	CPUFan,
	[EnumMember]
	dGPUFan,
	[EnumMember]
	SystemFan1,
	[EnumMember]
	SystemFan2,
	[EnumMember]
	LiquidCoolerFanPump,
	[EnumMember]
	Unsupported,
	[EnumMember]
	CPUFan2
}
