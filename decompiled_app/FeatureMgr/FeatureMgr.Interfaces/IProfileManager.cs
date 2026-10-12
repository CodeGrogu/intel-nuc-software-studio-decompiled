using System.Collections.Generic;
using FeatureMgr.Models;

namespace FeatureMgr.Interfaces;

public interface IProfileManager
{
	ProfileFeatureList SelectedProfile { get; }

	List<string> GetProfileNames();

	bool SelectProfileByName(string ProfileName);

	void AddNewProfile(string ProfileName);

	void LoadSavedProfiles();
}
