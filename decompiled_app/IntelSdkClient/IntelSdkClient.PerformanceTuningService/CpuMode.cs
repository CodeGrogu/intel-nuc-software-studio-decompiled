using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CpuMode", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public enum CpuMode
{
	[EnumMember]
	Unknown,
	[EnumMember]
	Performance,
	[EnumMember]
	Balanced,
	[EnumMember]
	BatterySaver
}
