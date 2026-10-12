using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SecondaryInputSourceConfiguration", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public enum SecondaryInputSourceConfiguration : byte
{
	[EnumMember]
	NoControlled,
	[EnumMember]
	CPUTemperature,
	[EnumMember]
	PCHTemperature,
	[EnumMember]
	Sensor1Temperature,
	[EnumMember]
	Sensor2Temperature,
	[EnumMember]
	Sensor3Temperature,
	[EnumMember]
	dGpuTemperature
}
