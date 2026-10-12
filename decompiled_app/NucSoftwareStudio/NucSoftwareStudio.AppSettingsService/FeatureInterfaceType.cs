using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.AppSettingsService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FeatureInterfaceType", Namespace = "http://schemas.datacontract.org/2004/07/NucSoftwareStudioService.FeatureSupport.Models")]
public enum FeatureInterfaceType
{
	[EnumMember]
	Error = -1,
	[EnumMember]
	Wmi,
	[EnumMember]
	Mmio,
	[EnumMember]
	Arduino,
	[EnumMember]
	Usb,
	[EnumMember]
	OdmSdk,
	[EnumMember]
	Nss,
	[EnumMember]
	Alexa
}
