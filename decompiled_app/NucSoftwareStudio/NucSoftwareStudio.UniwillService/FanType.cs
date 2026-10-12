using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.UniwillService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanType", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public enum FanType
{
	[EnumMember]
	Cpu,
	[EnumMember]
	Gpu
}
