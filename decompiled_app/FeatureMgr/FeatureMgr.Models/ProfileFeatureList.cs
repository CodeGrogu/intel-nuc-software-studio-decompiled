using System.Collections.Generic;
using System.Runtime.Serialization;
using Infrastructure.Models;

namespace FeatureMgr.Models;

[DataContract]
public class ProfileFeatureList
{
	[DataMember]
	public string ProfileName { get; set; }

	[DataMember]
	public List<FeatureSettings> FeatureSettings { get; set; } = new List<FeatureSettings>();

	public ProfileFeatureList()
	{
	}

	public ProfileFeatureList(string profileName)
	{
		ProfileName = profileName;
	}

	public void AddFeatureSettings(FeatureSettings settings)
	{
		int num = FeatureSettings.FindIndex((FeatureSettings f) => f.Type == settings.Type);
		if (num == -1)
		{
			FeatureSettings.Add(settings);
		}
		else
		{
			FeatureSettings[num] = settings;
		}
	}
}
