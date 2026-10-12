using System.Collections.Generic;
using FeatureMgr.Enums;
using Infrastructure.Models.FeatureMgr;

namespace FeatureMgr.Interfaces;

public interface ISdkManager
{
	SdkType Sdk { get; }

	List<Feature> SupportedFeatures { get; }
}
