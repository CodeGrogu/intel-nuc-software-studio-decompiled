using System.Collections.Generic;

namespace UniwillSdkClient.SupportedFeatures;

public class Feature
{
	public FeatureType Type { get; set; }

	public List<FeatureInterfaceType> SupportedInterfaces { get; set; }

	public int PriorityLevel { get; set; }

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
