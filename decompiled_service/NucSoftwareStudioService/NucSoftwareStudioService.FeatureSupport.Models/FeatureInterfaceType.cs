using System.Runtime.Serialization;

namespace NucSoftwareStudioService.FeatureSupport.Models;

[DataContract]
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
