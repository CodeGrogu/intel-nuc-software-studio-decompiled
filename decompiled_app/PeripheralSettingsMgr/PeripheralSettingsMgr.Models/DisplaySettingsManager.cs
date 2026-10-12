using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Infrastructure.Converters;
using Infrastructure.Models;
using Microsoft.Win32;
using PeripheralSettingsMgr.Events;
using PeripheralSettingsMgr.Interfaces;
using PeripheralSettingsMgr.Models.DisplayModes;
using Prism.Events;
using Prism.Logging;

namespace PeripheralSettingsMgr.Models;

public class DisplaySettingsManager : IDisplaySettingsManager
{
	private static readonly string PROFILES_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "DisplayModeProfiles.json");

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private bool _ac;

	public DisplayModeProfile _default;

	private DisplayModeProfile _current;

	private DisplayModeProfile _resetSettings;

	private List<MonitorSetting> _monitors = new List<MonitorSetting>();

	public ObservableCollection<DisplayModeProfile> Profiles { get; private set; }

	public bool AcOnly { get; private set; }

	public DisplayModeProfile LaunchSettings { get; private set; }

	public DisplayModeProfile CurrentProfile
	{
		get
		{
			return _current;
		}
		set
		{
			_current = value;
			if (_current != null)
			{
				_resetSettings = new DisplayModeProfile(_current, _current.Name);
				_current.ApplySettings(_ac);
				_current.Selected = true;
			}
		}
	}

	public DisplaySettingsManager(IEventAggregator events, ILoggerFacade logger)
	{
		_logger = logger;
		_events = events;
		_events.GetEvent<MonitorStatusQueryCompleteEvent>().Subscribe(UpdateMonitorGroup);
		new MonitorGroup(_logger, _events);
		MonitorGroup.DisplayStatusChanged += OnDisplayStatusChanged;
		Profiles = GetSavedDisplayModes();
		if (Profiles == null || Profiles.Count == 0)
		{
			string profile_string = ResourceDictionaryConverter.GetString("ProfileString");
			DisplayModeProfile item = new DisplayModeProfile(_monitors, NameValidator, profile_string);
			Profiles = new ObservableCollection<DisplayModeProfile> { item };
			CurrentProfile = Profiles.Single((DisplayModeProfile x) => x.Name == profile_string);
		}
		else
		{
			CurrentProfile = Profiles.FirstOrDefault((DisplayModeProfile x) => x.Selected);
		}
		if (_monitors.Count > 0)
		{
			foreach (DisplayModeProfile profile in Profiles)
			{
				profile.UpdateMonitors(_monitors);
			}
		}
		if (_default == null)
		{
			_default = new DisplayModeProfile(_monitors, NameValidator, ResourceDictionaryConverter.GetString("DefaultProfileString"), defaultMode: true);
		}
		if (SystemInformation.PowerStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
		{
			AcOnly = true;
			_ac = true;
		}
		else
		{
			_ac = SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online;
			SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
		}
	}

	private void UpdateMonitorGroup(MonitorGroup monitorGroup)
	{
		if (Profiles != null)
		{
			foreach (DisplayModeProfile profile in Profiles)
			{
				profile.UpdateMonitors(monitorGroup.Monitors);
			}
		}
		if (_default != null)
		{
			_default.UpdateMonitors(monitorGroup.Monitors);
		}
		if (_resetSettings != null)
		{
			_resetSettings.UpdateMonitors(monitorGroup.Monitors);
		}
		LaunchSettings = new DisplayModeProfile(monitorGroup.Monitors, null, "", defaultMode: true);
		_monitors = monitorGroup.Monitors;
	}

	private void OnDisplayStatusChanged()
	{
	}

	private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
	{
		if (e.Mode == PowerModes.StatusChange)
		{
			if (SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online)
			{
				_ac = true;
			}
			else
			{
				_ac = false;
			}
			CurrentProfile?.ApplySettings(_ac);
		}
	}

	private bool NameValidator(string Name)
	{
		return GetProfileByName(Name) == null;
	}

	public DisplayModeProfile ResetSettings()
	{
		return UpdateProfiles(new DisplayModeProfile(_resetSettings, _resetSettings.Name));
	}

	public DisplayModeProfile ResetDefaultSettings()
	{
		return UpdateProfiles(new DisplayModeProfile(_default, CurrentProfile.Name));
	}

	public DisplayModeProfile UpdateProfiles(DisplayModeProfile newProfile)
	{
		for (int i = 0; i < Profiles.Count; i++)
		{
			if (Profiles[i].Name.Equals(newProfile.Name))
			{
				Profiles[i] = newProfile;
			}
		}
		CurrentProfile = newProfile;
		CurrentProfile.ApplySettings(_ac);
		return CurrentProfile;
	}

	public DisplayModeProfile GetProfileByName(string Name)
	{
		if (Profiles == null)
		{
			return null;
		}
		return Profiles.FirstOrDefault((DisplayModeProfile f) => f.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
	}

	public bool UpdateProfileName(string OldName, string NewName)
	{
		if (GetProfileByName(NewName) != null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(OldName))
		{
			Profiles.Add(new DisplayModeProfile(_monitors, NameValidator, NewName));
		}
		else
		{
			DisplayModeProfile profileByName = GetProfileByName(OldName);
			if (profileByName == null)
			{
				return false;
			}
			profileByName.Name = NewName;
		}
		return true;
	}

	public DisplayModeProfile GetDefaultProfile()
	{
		if (_default == null)
		{
			return Profiles.First();
		}
		return _default;
	}

	private ObservableCollection<DisplayModeProfile> GetSavedDisplayModes()
	{
		return NssJsonSerializer.Deserialize<ObservableCollection<DisplayModeProfile>>(PROFILES_PATH, new List<Type> { typeof(ObservableCollection<DisplayModeProfile>) });
	}

	private void SaveDisplayModes()
	{
		NssJsonSerializer.Serialize(Profiles, PROFILES_PATH);
	}

	public DisplayModeProfile UpdateCurrentProfile(DisplayModeProfile mode)
	{
		if (mode == null)
		{
			return CurrentProfile;
		}
		DisplayModeProfile displayModeProfile = Profiles.FirstOrDefault((DisplayModeProfile x) => x.Name == mode.Name);
		if (displayModeProfile == null)
		{
			return CurrentProfile;
		}
		CurrentProfile = displayModeProfile;
		return CurrentProfile;
	}

	public void RemoveProfiles(List<string> Names)
	{
		foreach (string n in Names)
		{
			DisplayModeProfile displayModeProfile = Profiles.FirstOrDefault((DisplayModeProfile x) => x.Name.Equals(n));
			if (displayModeProfile == null || displayModeProfile.Default)
			{
				return;
			}
			Profiles.Remove(displayModeProfile);
		}
		if (CurrentProfile == null)
		{
			CurrentProfile = GetDefaultProfile();
		}
	}

	public void AddProfile(string Name, DisplayModeProfile BaseProfile)
	{
		if (GetProfileByName(Name) == null)
		{
			DisplayModeProfile item = ((BaseProfile != null) ? new DisplayModeProfile(BaseProfile, Name) : new DisplayModeProfile(_monitors, NameValidator, Name));
			Profiles.Add(item);
		}
	}

	~DisplaySettingsManager()
	{
		SaveDisplayModes();
	}
}
