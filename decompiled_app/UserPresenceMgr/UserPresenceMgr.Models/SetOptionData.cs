using ContextSensingClient;

namespace UserPresenceMgr.Models;

public class SetOptionData
{
	public FeatureType FeatureType { get; }

	public FeatureProperty FeatureProperty { get; }

	public object Data { get; }

	public SetOptionData(FeatureType type, FeatureProperty property, object data)
	{
		FeatureType = type;
		FeatureProperty = property;
		Data = data;
	}
}
