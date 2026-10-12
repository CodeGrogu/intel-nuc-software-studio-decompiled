using System.Collections.Generic;
using Infrastructure.Models.FeatureMgr;

namespace FeatureMgr.Interfaces;

public interface IFeatureManager
{
	void LoadFeatures();

	List<Feature> GetFeatureList();
}
