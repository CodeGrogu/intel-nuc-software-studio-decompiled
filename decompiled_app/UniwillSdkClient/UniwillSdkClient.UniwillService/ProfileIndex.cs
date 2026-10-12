using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ProfileIndex", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public enum ProfileIndex
{
	[EnumMember]
	Performance = 1,
	[EnumMember]
	Balanced,
	[EnumMember]
	BatterySaver
}
