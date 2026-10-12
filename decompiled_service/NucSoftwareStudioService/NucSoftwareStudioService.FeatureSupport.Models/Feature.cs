using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NucSoftwareStudioService.FeatureSupport.Models;

[DataContract]
public class Feature
{
	[DataMember]
	public FeatureType Type { get; set; }

	[DataMember]
	public List<FeatureInterfaceType> SupportedInterfaces { get; set; }

	[DataMember]
	public int PriorityLevel { get; set; }

	[DataMember]
	public int Index { get; set; }

	public Feature()
	{
	}

	public Feature(FeatureType type, List<FeatureInterfaceType> interfaces, int priority, int index)
	{
		Type = type;
		SupportedInterfaces = interfaces;
		PriorityLevel = priority;
		Index = index;
	}
}
