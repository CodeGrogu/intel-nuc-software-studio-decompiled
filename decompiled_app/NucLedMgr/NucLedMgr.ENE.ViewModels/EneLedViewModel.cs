using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using NucLedMgr.ENE.Enums;
using NucLedMgr.ENE.Events;
using NucLedMgr.ENE.Interfaces;
using NucLedMgr.ENE.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NucLedMgr.ENE.ViewModels;

public class EneLedViewModel : BindableBase
{
	private readonly IEventAggregator _events;

	private readonly IEnePatternManager _patternMgr;

	public string _currentProfileName = string.Empty;

	private DelegateCommand<EneLedData> _selectedZoneChanged;

	private DelegateCommand _reset;

	private bool _smallLoadScreen;

	private LedProfile _newProfileBase;

	private bool _profilesSelected;

	private string _newProfileName;

	private bool _open;

	private DelegateCommand _openEditor;

	private DelegateCommand _closeEditor;

	private DelegateCommand _addProfile;

	private DelegateCommand _deleteSelectedProfiles;

	private DelegateCommand _updateSelection;

	public ObservableCollection<EneLedData> EneZones => _patternMgr.EneZones;

	public bool On
	{
		get
		{
			return _patternMgr.On;
		}
		set
		{
			SelectedZone.ButtonStatus = value;
			_patternMgr.On = value;
		}
	}

	public EneLedData SelectedZone
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

	public DelegateCommand<EneLedData> SelectedZoneChanged
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

	public EneLedData LogoHeader { get; set; }

	public EneLedData LeftHeader { get; set; }

	public EneLedData RightHeader { get; set; }

	public EneLedData FrontHeader { get; set; }

	public bool LedsAvailable { get; set; }

	public EneLedViewModel(IEventAggregator events, IEnePatternManager patternMgr)
	{
		_events = events;
		_patternMgr = patternMgr;
		_events.GetEvent<LedAvailable>().Subscribe(IsLedAvailable);
		LogoHeader = _patternMgr.EneZones.FirstOrDefault((EneLedData x) => x.Index == EneLedZone.LogoHeader);
		LeftHeader = _patternMgr.EneZones.FirstOrDefault((EneLedData x) => x.Index == EneLedZone.LeftHeader);
		RightHeader = _patternMgr.EneZones.FirstOrDefault((EneLedData x) => x.Index == EneLedZone.RightHeader);
		FrontHeader = _patternMgr.EneZones.FirstOrDefault((EneLedData x) => x.Index == EneLedZone.FrontHeader);
		LedsAvailable = true;
		_selectedZoneChanged = new DelegateCommand<EneLedData>(UpdateSelectedZone);
		_reset = new DelegateCommand(_patternMgr.Reset);
		_events.GetEvent<RefreshEneUiEvent>().Subscribe(RefreshUi);
		_openEditor = new DelegateCommand(OpenedProfileEditorWindow);
		_closeEditor = new DelegateCommand(ClosedProfileEditorWindow);
		_addProfile = new DelegateCommand(AddProfiles);
		_deleteSelectedProfiles = new DelegateCommand(DeleteProfiles);
		_updateSelection = new DelegateCommand(Update);
		NewProfileBase = SelectedProfile;
	}

	private void IsLedAvailable(bool ledAvailable)
	{
		LedsAvailable = ledAvailable;
		RaisePropertyChanged("LedsAvailable");
	}

	private void ClosedProfileEditorWindow()
	{
		ProfilesSelected = false;
		NewProfileName = "";
		Open = false;
		ValidateProfileName();
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
			if (ledProfile != null)
			{
				int index = _patternMgr.Profiles.IndexOf(ledProfile);
				_patternMgr.Profiles.Remove(ledProfile);
				ledProfile.Name = _currentProfileName;
				_patternMgr.Profiles.Insert(index, ledProfile);
			}
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
		Update();
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
		RaisePropertyChanged("EneZones");
		RaisePropertyChanged("LogoHeader");
		RaisePropertyChanged("FrontHeader");
		RaisePropertyChanged("LeftHeader");
		RaisePropertyChanged("RightHeader");
		RaisePropertyChanged("EneZones");
		RaisePropertyChanged("SelectedProfile");
		RaisePropertyChanged("SelectedZone");
	}

	private void OpenedProfileEditorWindow()
	{
		ProfilesSelected = false;
		NewProfileName = "";
		Open = true;
	}

	private void UpdateSelectedZone(EneLedData newSelection)
	{
		newSelection.Name = newSelection.Name.Trim();
		if (string.IsNullOrWhiteSpace(newSelection.Name))
		{
			newSelection.Name = newSelection.InitialName;
		}
		SelectedZone = newSelection;
		foreach (EneLedData eneZone in EneZones)
		{
			eneZone.Name = eneZone.Name.Trim();
			if (string.IsNullOrWhiteSpace(eneZone.Name))
			{
				eneZone.Name = eneZone.InitialName;
			}
			if (eneZone.Name.Length > 15)
			{
				eneZone.Name = eneZone.Name.Substring(0, 15);
			}
			eneZone.IsEditable = false;
		}
		UpdateSelectedProfile();
		RaisePropertyChanged("SelectedZone");
		RefreshUi(Enabled: true);
	}

	private void UpdateSelectedProfile()
	{
		foreach (EneLedData eneZone in EneZones)
		{
			SelectedProfile.ZoneNames[eneZone.Index] = eneZone.Name;
		}
		_patternMgr.SaveProfile(SelectedProfile, setAsCurrentProfile: true);
	}
}
