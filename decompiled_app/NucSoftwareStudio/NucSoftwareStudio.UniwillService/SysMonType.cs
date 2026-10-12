using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.UniwillService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SysMonType", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public enum SysMonType
{
	[EnumMember]
	Cpu,
	[EnumMember]
	Memory,
	[EnumMember]
	dGpu,
	[EnumMember]
	iGpu,
	[EnumMember]
	Network,
	[EnumMember]
	Battery,
	[EnumMember]
	HwFuelGauge,
	[EnumMember]
	Storage,
	[EnumMember]
	Fan,
	[EnumMember]
	SystemInfo
}
