using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using NucLedMgr.Arduino.Enums;
using NucLedMgr.Arduino.Events;
using NucLedMgr.Arduino.Interfaces;
using NucLedMgr.Arduino.Models;
using NucLedMgr.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NucLedMgr.Arduino.ViewModels;

public class ArduinoLedViewModel : BindableBase
{
	private readonly IEventAggregator _events;

	private readonly IArduinoPatternManager _patternMgr;

	private string _currentProfileName = string.Empty;

	private bool _profilesSelected;

	private string _newProfileName;

	private bool _open;

	private bool _smallLoadScreen;

	private LedProfile _newProfileBase;

	private DelegateCommand<ArduinoLedData> _selectedZoneChanged;

	private DelegateCommand _reset;

	private DelegateCommand _refresh;

	private DelegateCommand<MonsterCoveZone?> _zoneSelected;

	private DelegateCommand _openEditor;

	private DelegateCommand _closeEditor;

	private DelegateCommand _addProfile;

	private DelegateCommand _deleteSelectedProfiles;

	private DelegateCommand _updateSelection;

	public bool On
	{
		get
		{
			return _patternMgr.On;
		}
		set
		{
			_patternMgr.On = value;
		}
	}

	public ObservableCollection<LedProfile> Profiles => _patternMgr.Profiles;

	public LedProfile SelectedProfile
	{
		get
		{
			return _patternMgr.CurrentProfile;
		}
		set
		{
			_patternMgr.CurrentProfile = value;
			RaisePropertyChanged("SelectedProfile");
		}
	}

	public bool ProfilesSelected
	{
		get
		{
			return _profilesSelected;
		}
		set
		{
			SetProperty(ref _profilesSelected, value, "ProfilesSelected");
		}
	}

	public string NewProfileName
	{
		get
		{
			return _newProfileName;
		}
		set
		{
			SetProperty(ref _newProfileName, value, "NewProfileName");
		}
	}

	public bool Open
	{
		get
		{
			return _open;
		}
		set
		{
			SetProperty(ref _open, value, "Open");
		}
	}

	public ArduinoLedData SelectedZone
	{
		get
		{
			return _patternMgr.SelectedZone;
		}
		set
		{
			_patternMgr.SelectedZone = value;
		}
	}

	public ObservableCollection<ArduinoLedData> ArduinoZones => _patternMgr.ArduinoZones;

	public bool LedsAvailable { get; set; }

	public bool SmallLoadScreen
	{
		get
		{
			return _smallLoadScreen;
		}
		set
		{
			SetProperty(ref _smallLoadScreen, value, "SmallLoadScreen");
		}
	}

	public LedProfile NewProfileBase
	{
		get
		{
			return _newProfileBase;
		}
		set
		{
			SetProperty(ref _newProfileBase, value, "NewProfileBase");
		}
	}

	public ArduinoLedData PowerButton { get; set; }

	public ArduinoLedData FrontPanel { get; set; }

	public ArduinoLedData FrontConnectors { get; set; }

	public ArduinoLedData LeftSide { get; set; }

	public ArduinoLedData RightSide { get; set; }

	public DelegateCommand<ArduinoLedData> SelectedZoneChanged
	{
		get
		{
			return _selectedZoneChanged;
		}
		set
		{
			SetProperty(ref _selectedZoneChanged, value, "SelectedZoneChanged");
		}
	}

	public DelegateCommand Reset
	{
		get
		{
			return _reset;
		}
		set
		{
			SetProperty(ref _reset, value, "Reset");
		}
	}

	public DelegateCommand Refresh
	{
		get
		{
			return _refresh;
		}
		set
		{
			SetProperty(ref _refresh, value, "Refresh");
		}
	}

	public DelegateCommand<MonsterCoveZone?> ZoneSelected
	{
		get
		{
			return _zoneSelected;
		}
		set
		{
			SetProperty(ref _zoneSelected, value, "ZoneSelected");
		}
	}

	public DelegateCommand OpenEditor
	{
		get
		{
			return _openEditor;
		}
		set
		{
			SetProperty(ref _openEditor, value, "OpenEditor");
		}
	}

	public DelegateCommand CloseEditor
	{
		get
		{
			return _closeEditor;
		}
		set
		{
			SetProperty(ref _closeEditor, value, "CloseEditor");
		}
	}

	public DelegateCommand AddProfile
	{
		get
		{
			return _addProfile;
		}
		set
		{
			SetProperty(ref _addProfile, value, "AddProfile");
		}
	}

	public DelegateCommand DeleteSelectedProfiles
	{
		get
		{
			return _deleteSelectedProfiles;
		}
		set
		{
			SetProperty(ref _deleteSelectedProfiles, value, "DeleteSelectedProfiles");
		}
	}

	public DelegateCommand UpdateSelection
	{
		get
		{
			return _updateSelection;
		}
		set
		{
			SetProperty(ref _updateSelection, value, "UpdateSelection");
		}
	}

	public ArduinoLedViewModel(IEventAggregator events, IArduinoPatternManager patternMgr, IColorManager colorMgr)
	{
		_events = events;
		_events.GetEvent<RefreshArduinoUiEvent>().Subscribe(RefreshUi);
		_events.GetEvent<ArduinoZoneColorPatternChangeEvent>().Subscribe(UpdateUiPattern);
		_events.GetEvent<LedAvailable>().Subscribe(IsLedAvailable);
		_patternMgr = patternMgr;
		_selectedZoneChanged = new DelegateCommand<ArduinoLedData>(UpdateSelectedZone);
		_zoneSelected = new DelegateCommand<MonsterCoveZone?>(NewZone);
		_reset = new DelegateCommand(_patternMgr.Reset);
		_refresh = new DelegateCommand(_patternMgr.Refresh);
		_openEditor = new DelegateCommand(OpenedProfileEditorWindow);
		_closeEditor = new DelegateCommand(ClosedProfileEditorWindow);
		_addProfile = new DelegateCommand(AddProfiles);
		_deleteSelectedProfiles = new DelegateCommand(DeleteProfiles);
		_updateSelection = new DelegateCommand(Update);
		PowerButton = ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.Error);
		FrontPanel = ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.Skull);
		FrontConnectors = ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.BottomFront);
		LeftSide = ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.BottomLeft);
		RightSide = ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == MonsterCoveZone.BottomRight);
		NewProfileBase = SelectedProfile;
		LedsAvailable = true;
		RefreshUi(Enabled: true);
	}

	private void IsLedAvailable(bool LedAvailable)
	{
		LedsAvailable = LedAvailable;
		RaisePropertyChanged("LedsAvailable");
	}

	private void OpenedProfileEditorWindow()
	{
		ProfilesSelected = false;
		NewProfileName = "";
		Open = true;
	}

	private void ClosedProfileEditorWindow()
	{
		ValidateProfileName();
		ProfilesSelected = false;
		NewProfileName = "";
		Open = false;
		foreach (LedProfile profile in Profiles)
		{
			profile.Selected = false;
			profile.TextboxSelected = false;
		}
		Task.Run(() =>
		{
			List<LedProfile> list = Profiles.ToList();
			LedProfile selectedProfile = SelectedProfile;
			_patternMgr.RemoveAllProfileFiles().Wait();
			Application.Current.Dispatcher.Invoke(() =>
			{
				_patternMgr.Profiles.Clear();
			});
			foreach (LedProfile item in list)
			{
				if (item == selectedProfile)
				{
					_patternMgr.SaveProfile(item, setAsCurrentProfile: true).Wait();
				}
				else
				{
					_patternMgr.SaveProfile(item, setAsCurrentProfile: false).Wait();
				}
			}
			_patternMgr.CurrentProfile = selectedProfile;
			RaisePropertyChanged("SelectedProfile");
			RaisePropertyChanged("Profiles");
			RefreshUi(Enabled: true);
		});
	}

	private void AddProfiles()
	{
		if (NewProfileBase == null)
		{
			return;
		}
		NewProfileName = NewProfileName.Trim();
		int num = (from x in _patternMgr.Profiles
			group x by x.Name).Count((IGrouping<string, LedProfile> x) => x.Skip(1).Any());
		if (_patternMgr.Profiles.Any((LedProfile profile) => string.Equals(profile.Name, NewProfileName, StringComparison.OrdinalIgnoreCase)) || num > 0)
		{
			ErrorHandler.PublishError("DuplicateProfileError", ErrorType.UNKNOWN, critical: true, msgIsKey: true);
			_events.GetEvent<ApplicationCloseEvent>().Publish(payload: false);
			return;
		}
		if (NewProfileName.Length > 15)
		{
			NewProfileName = NewProfileName.Substring(0, 15);
		}
		LedProfile newProfileBase = NewProfileBase;
		if (_patternMgr.Profiles.Count((LedProfile a) => a.Selected) == 1)
		{
			RefreshSelection();
			Update();
			NewProfileBase = newProfileBase;
		}
		SmallLoadScreen = true;
		Task.Run(() =>
		{
			_patternMgr.ProcessJsonFile(NewProfileBase.Name, NewProfileName);
			_patternMgr.CurrentProfile = _patternMgr.GetProfileByName(NewProfileName);
			_patternMgr.SaveProfile(_patternMgr.CurrentProfile, setAsCurrentProfile: true);
			Application.Current.Dispatcher.Invoke(() =>
			{
				SmallLoadScreen = false;
			});
			RaisePropertyChanged("SelectedProfile");
		});
	}

	private void DeleteProfiles()
	{
		Task.Run(() =>
		{
			List<string> list = new List<string>();
			foreach (LedProfile profile in Profiles)
			{
				if (profile.Selected)
				{
					list.Add(profile.Name);
				}
			}
			_patternMgr.RemoveProfiles(list);
			RaisePropertyChanged("SelectedProfile");
			ProfilesSelected = false;
		});
	}

	private void Update()
	{
		ValidateProfileName();
		int num = _patternMgr.Profiles.Count((LedProfile a) => a.Selected);
		for (int num2 = 0; num2 < _patternMgr.Profiles.Count; num2++)
		{
			LedProfile ledProfile = _patternMgr.Profiles[num2];
			if (num == 1 && _patternMgr.Profiles[num2].Selected)
			{
				_patternMgr.Profiles.RemoveAt(num2);
				ledProfile.TextboxSelected = true;
				_currentProfileName = ledProfile.Name;
				_patternMgr.Profiles.Insert(num2, ledProfile);
				ProfilesSelected = true;
				return;
			}
			_patternMgr.Profiles.RemoveAt(num2);
			ledProfile.TextboxSelected = false;
			_patternMgr.Profiles.Insert(num2, ledProfile);
			if (num == 0)
			{
				ProfilesSelected = false;
			}
		}
		UpdateJsonFile();
	}

	private void UpdateJsonFile()
	{
		List<string> savedProfileNames = _patternMgr.GetSavedProfileNames();
		List<string> updatedProfileNames = Profiles.Select((LedProfile a) => a.Name).ToList();
		List<string> names = new List<string>();
		Task.Run(() =>
		{
			foreach (string item in savedProfileNames)
			{
				if (!updatedProfileNames.Contains(item))
				{
					names.Add(item);
				}
			}
			if (names.Count >= 1)
			{
				_patternMgr.RemoveProfiles(names);
			}
			for (int i = 0; i < Profiles.Count(); i++)
			{
				if (!savedProfileNames.Contains(Profiles[i].Name))
				{
					_patternMgr.SaveProfile(Profiles[i], setAsCurrentProfile: false).Wait();
				}
			}
		});
	}

	private void ValidateProfileName()
	{
		int num = (from x in _patternMgr.Profiles
			group x by x.Name into x
			where x.Skip(1).Any()
			select x).Count();
		int num2 = _patternMgr.Profiles.Where((LedProfile x) => string.IsNullOrWhiteSpace(x.Name)).Count();
		LedProfile ledProfile = null;
		if (num != 0)
		{
			ledProfile = _patternMgr.Profiles.Where((LedProfile a) => a.TextboxSelected).FirstOrDefault();
			int index = _patternMgr.Profiles.IndexOf(ledProfile);
			_patternMgr.Profiles.Remove(ledProfile);
			ledProfile.Name = _currentProfileName;
			_patternMgr.Profiles.Insert(index, ledProfile);
			ErrorHandler.PublishError("DuplicateProfileError", ErrorType.UNKNOWN, critical: true, msgIsKey: true);
			_events.GetEvent<ApplicationCloseEvent>().Publish(payload: false);
		}
		else if (num2 > 0)
		{
			ledProfile = _patternMgr.Profiles.Where((LedProfile x) => string.IsNullOrWhiteSpace(x.Name)).FirstOrDefault();
			int index2 = _patternMgr.Profiles.IndexOf(ledProfile);
			_patternMgr.Profiles.Remove(ledProfile);
			ledProfile.Name = _currentProfileName;
			_patternMgr.Profiles.Insert(index2, ledProfile);
		}
	}

	private void RefreshSelection()
	{
		for (int i = 0; i < _patternMgr.Profiles.Count; i++)
		{
			LedProfile ledProfile = _patternMgr.Profiles[i];
			if (_patternMgr.Profiles[i].Selected)
			{
				_patternMgr.Profiles.RemoveAt(i);
				ledProfile.TextboxSelected = false;
				ledProfile.Selected = false;
				_patternMgr.Profiles.Insert(i, ledProfile);
				ProfilesSelected = false;
			}
		}
	}

	private void RefreshUi(bool Enabled)
	{
		RaisePropertyChanged("On");
		RaisePropertyChanged("ArduinoZones");
		RaisePropertyChanged("FrontPanel");
		RaisePropertyChanged("FrontConnectors");
		RaisePropertyChanged("LeftSide");
		RaisePropertyChanged("RightSide");
		RaisePropertyChanged("SelectedProfile");
	}

	private void UpdateUiPattern(MonsterCoveZone Zone)
	{
		switch (Zone)
		{
		case MonsterCoveZone.Skull:
			RaisePropertyChanged("FrontPanel");
			break;
		case MonsterCoveZone.BottomRight:
			RaisePropertyChanged("RightSide");
			break;
		case MonsterCoveZone.BottomLeft:
			RaisePropertyChanged("LeftSide");
			break;
		case MonsterCoveZone.BottomFront:
			RaisePropertyChanged("FrontConnectors");
			break;
		}
	}

	private void NewZone(MonsterCoveZone? SelectedZone)
	{
		if (SelectedZone.HasValue && SelectedZone.HasValue)
		{
			switch (SelectedZone.GetValueOrDefault())
			{
			case MonsterCoveZone.Skull:
				UpdateSelectedZone(ArduinoZones[0]);
				break;
			case MonsterCoveZone.BottomRight:
				UpdateSelectedZone(ArduinoZones[2]);
				break;
			case MonsterCoveZone.BottomLeft:
				UpdateSelectedZone(ArduinoZones[3]);
				break;
			case MonsterCoveZone.BottomFront:
				UpdateSelectedZone(ArduinoZones[1]);
				break;
			}
		}
	}

	private void UpdateSelectedZone(ArduinoLedData newSelection)
	{
		SelectedZone = newSelection;
		RaisePropertyChanged("SelectedZone");
	}
}
