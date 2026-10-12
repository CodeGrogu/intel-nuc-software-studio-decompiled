using System.Runtime.Serialization;

namespace CustomizationMgr.Models;

[DataContract]
[KnownType(typeof(CustomSetting))]
public class CustomSetting
{
	[DataMember]
	public string TypeName { get; }

	public CustomSetting(string typeName)
	{
		TypeName = typeName;
	}
}
