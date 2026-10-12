using System.Collections.Generic;
using Infrastructure.Models.FeatureMgr;

namespace FeatureMgr.Interfaces;

public interface IUserSettingsManager
{
	List<Feature> GetUserFeatures();

	string GetSelectedProfileName();

	void SetUserFeatures(List<Feature> features);

	void SetSelectedProfileName(string selectedProfile);
}
