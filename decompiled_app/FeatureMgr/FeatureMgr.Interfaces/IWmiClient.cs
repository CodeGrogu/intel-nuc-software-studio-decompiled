using System.Collections.Generic;
using Infrastructure.Models.FeatureMgr;

namespace FeatureMgr.Interfaces;

public interface IWmiClient
{
	List<Feature> GetSupportedFeatures();
}
