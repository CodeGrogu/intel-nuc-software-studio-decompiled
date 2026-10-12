using System;
using System.Collections.Generic;
using System.IO;
using FeatureMgr.Interfaces;
using Infrastructure.Converters;
using Infrastructure.Events;
using Infrastructure.Models;
using Prism.Events;
using Prism.Logging;

namespace FeatureMgr.Models;

public class ProfileManager : IProfileManager
{
	private readonly ILoggerFacade _logger;

	private readonly IUserSettingsManager _userSettings;

	private readonly IEventAggregator _events;

	private static readonly string _profilesDirectory = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "Profiles\\");

	private readonly List<Type> types = new List<Type>();

	private readonly List<ProfileFeatureList> _profiles = new List<ProfileFeatureList>();

	public ProfileFeatureList SelectedProfile { get; private set; }

	public ProfileManager(IEventAggregator events, ILoggerFacade logger, IUserSettingsManager userSettings)
	{
		_logger = logger;
		_userSettings = userSettings;
		_events = events;
		Directory.CreateDirectory(_profilesDirectory);
		_events.GetEvent<FeatureProfileTypeEvent>().Subscribe(AddKnownTypes);
		_events.GetEvent<SaveFeatureSettingsEvent>().Subscribe(AddSettings);
	}

	public List<string> GetProfileNames()
	{
		List<string> list = new List<string>();
		foreach (ProfileFeatureList profile in _profiles)
		{
			list.Add(profile.ProfileName);
		}
		return list;
	}

	public void AddNewProfile(string ProfileName)
	{
		if (GetProfileByName(ProfileName) == null)
		{
			ProfileFeatureList profileFeatureList = new ProfileFeatureList(ProfileName);
			_profiles.Add(profileFeatureList);
			NssJsonSerializer.Serialize(profileFeatureList, Path.Combine(_profilesDirectory, ProfileName + ".json"));
		}
	}

	public bool SelectProfileByName(string ProfileName)
	{
		ProfileFeatureList profileByName = GetProfileByName(ProfileName);
		if (profileByName != null)
		{
			SelectedProfileChanged(profileByName);
			return true;
		}
		return false;
	}

	public void LoadSavedProfiles()
	{
		GetSavedProfiles();
		if (_profiles.Count == 0)
		{
			string profileName = ResourceDictionaryConverter.GetString("DefaultProfileName");
			AddNewProfile(profileName);
			SelectProfileByName(profileName);
		}
		else
		{
			GetSelectedProfile();
		}
	}

	private void GetSelectedProfile()
	{
		string selectedProfileName = _userSettings.GetSelectedProfileName();
		ProfileFeatureList profileByName = GetProfileByName(selectedProfileName);
		if (profileByName != null)
		{
			SelectedProfileChanged(profileByName);
		}
	}

	private void SelectedProfileChanged(ProfileFeatureList NewSelectedProfile)
	{
		SelectedProfile = NewSelectedProfile;
		_userSettings.SetSelectedProfileName(SelectedProfile.ProfileName);
		foreach (FeatureSettings featureSetting in SelectedProfile.FeatureSettings)
		{
			_events.GetEvent<ApplyFeatureSettingsEvent>().Publish(featureSetting);
		}
	}

	private void RemoveProfile(string Name)
	{
		int num = _profiles.FindIndex((ProfileFeatureList f) => f.ProfileName == Name);
		if (num != -1)
		{
			_profiles.RemoveAt(num);
			File.Delete(Path.Combine(_profilesDirectory, Name + ".json"));
		}
	}

	private ProfileFeatureList GetProfileByName(string Name)
	{
		int num = _profiles.FindIndex((ProfileFeatureList f) => f.ProfileName == Name);
		if (num != -1)
		{
			return _profiles[num];
		}
		return null;
	}

	private void GetSavedProfiles()
	{
		string[] files = Directory.GetFiles(_profilesDirectory);
		foreach (string path in files)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			if (GetProfileByName(fileNameWithoutExtension) == null)
			{
				ProfileFeatureList item = NssJsonSerializer.Deserialize<ProfileFeatureList>(path, types);
				_profiles.Add(item);
			}
		}
	}

	private void AddKnownTypes(Type t)
	{
		types.Add(t);
	}

	private void AddSettings(FeatureSettings settings)
	{
		if (SelectedProfile != null)
		{
			SelectedProfile.AddFeatureSettings(settings);
		}
		else
		{
			SelectedProfile = new ProfileFeatureList("default");
			SelectedProfile.AddFeatureSettings(settings);
		}
		NssJsonSerializer.Serialize(SelectedProfile, Path.Combine(_profilesDirectory, SelectedProfile.ProfileName + ".json"));
		foreach (FeatureSettings featureSetting in SelectedProfile.FeatureSettings)
		{
			_events.GetEvent<ApplyFeatureSettingsEvent>().Publish(featureSetting);
		}
	}
}
