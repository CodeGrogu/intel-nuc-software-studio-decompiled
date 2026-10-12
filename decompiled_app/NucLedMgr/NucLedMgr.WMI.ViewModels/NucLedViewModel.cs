using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using NucLedMgr.Interfaces;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Events;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NucLedMgr.WMI.ViewModels;

public class NucLedViewModel : BindableBase
{
	private readonly IEventAggregator _events;

	private readonly ISettingsManager _settingsMgr;

	private readonly IColorManager _colorMgr;

	private string _currentProfileName = string.Empty;

	private bool _open;

	private bool _processComplete = true;

	private bool _initialization = true;

	private LedProfile _newProfileBase;

	private LedProfile _selectedProfile;

	private bool _profilesSelected;

	private bool _smallLoadScreen;

	private string _newProfileName;

	private DelegateCommand<Led> _selectedLedChanged;

	private DelegateCommand _openEditor;

	private DelegateCommand _closeEditor;

	private DelegateCommand _addProfile;

	private DelegateCommand _selectItem;

	private DelegateCommand _updateSelection;

	private DelegateCommand _deleteSelectedProfiles;

	private DelegateCommand _reset;

	private DelegateCommand<LedType?> _LedSelected;

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

	public bool On
	{
		get
		{
			return !_settingsMgr.Disabled;
		}
		set
		{
			ProcessComplete = false;
			Task.Run(() =>
			{
				_settingsMgr.Disabled = !value;
			}).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public bool ProcessComplete
	{
		get
		{
			return _processComplete;
		}
		set
		{
			SetProperty(ref _processComplete, value, "ProcessComplete");
		}
	}

	public bool Initialization
	{
		get
		{
			return _initialization;
		}
		set
		{
			SetProperty(ref _initialization, value, "Initialization");
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

	public ObservableCollection<Led> Leds
	{
		get
		{
			return _settingsMgr.CurrentSettings;
		}
		set
		{
			_settingsMgr.CurrentSettings = value;
			_settingsMgr.CurrentProfile.Leds.Clear();
			_settingsMgr.CurrentProfile.Leds.AddRange(value);
			RaisePropertyChanged("Leds");
		}
	}

	public Led SelectedLed
	{
		get
		{
			return _settingsMgr.SelectedSetting;
		}
		set
		{
			_settingsMgr.SelectedSetting = value;
			RaisePropertyChanged("SelectedLed");
		}
	}

	public ObservableCollection<LedProfile> Profiles => _settingsMgr.Profiles;

	public LedProfile SelectedProfile
	{
		get
		{
			return _selectedProfile;
		}
		set
		{
			if (value != null)
			{
				SetProperty(ref _selectedProfile, value, "SelectedProfile");
				if (IsSettingsSet)
				{
					_settingsMgr.CurrentProfile = value;
				}
				RaisePropertyChanged("SelectedProfile");
				RaisePropertyChanged("Leds");
			}
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

	public NucType NucType => _settingsMgr.NucType;

	public Led PowerButton
	{
		get
		{
			return _settingsMgr.PowerButton;
		}
		set
		{
		}
	}

	public Led Front1
	{
		get
		{
			return _settingsMgr.Front1;
		}
		set
		{
		}
	}

	public Led Front2
	{
		get
		{
			return _settingsMgr.Front2;
		}
		set
		{
		}
	}

	public Led Front3
	{
		get
		{
			return _settingsMgr.Front3;
		}
		set
		{
		}
	}

	public Led Skull
	{
		get
		{
			return _settingsMgr.Skull;
		}
		set
		{
		}
	}

	public Led Eyes
	{
		get
		{
			return _settingsMgr.Eyes;
		}
		set
		{
		}
	}

	public Led Hdd
	{
		get
		{
			return _settingsMgr.Hdd;
		}
		set
		{
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

	public bool LedsAvailable { get; set; }

	private bool IsSettingsSet { get; set; }

	public bool ShowIntelLogo { get; private set; }

	public BitmapSource AppLogo { get; private set; }

	public DelegateCommand<Led> SelectedLedChanged
	{
		get
		{
			return _selectedLedChanged;
		}
		set
		{
			SetProperty(ref _selectedLedChanged, value, "SelectedLedChanged");
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

	public DelegateCommand SelectItem
	{
		get
		{
			return _selectItem;
		}
		set
		{
			SetProperty(ref _selectItem, value, "SelectItem");
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

	public DelegateCommand<LedType?> LedSelected
	{
		get
		{
			return _LedSelected;
		}
		set
		{
			SetProperty(ref _LedSelected, value, "LedSelected");
		}
	}

	public ICommand WindowClosing { get; private set; }

	public NucLedViewModel(IEventAggregator events, ISettingsManager settingsMgr, IColorManager colorMgr, ICustomResourceManager resourceManager)
	{
		NucLedViewModel nucLedViewModel = this;
		LedsAvailable = false;
		_events = events;
		_settingsMgr = settingsMgr;
		_colorMgr = colorMgr;
		_openEditor = new DelegateCommand(OpenedProfileEditorWindow);
		_closeEditor = new DelegateCommand(ClosedProfileEditorWindow);
		_open = false;
		_smallLoadScreen = false;
		IsSettingsSet = true;
		KeyValuePair<AppLogoType, BitmapSource> keyValuePair = resourceManager.AppLogo();
		if (keyValuePair.Key == AppLogoType.Default)
		{
			ShowIntelLogo = true;
		}
		else
		{
			ShowIntelLogo = false;
			if (keyValuePair.Key == AppLogoType.Custom)
			{
				AppLogo = keyValuePair.Value;
			}
		}
		_addProfile = new DelegateCommand(AddProfiles);
		_updateSelection = new DelegateCommand(Update);
		_deleteSelectedProfiles = new DelegateCommand(DeleteProfiles);
		_selectItem = new DelegateCommand(ItemSelected);
		_selectedLedChanged = new DelegateCommand<Led>(UpdateSelectedLed);
		_LedSelected = new DelegateCommand<LedType?>(NewZone);
		_events.GetEvent<LedsLoadedEvent>().Subscribe(LoadingScreenVisible, ThreadOption.UIThread);
		_reset = new DelegateCommand(ResetToDefaults);
		_events.GetEvent<ProcessingCompleteEvent>().Subscribe(EnableToggle);
		_selectedProfile = settingsMgr.CurrentProfile;
		Task.Run(() =>
		{
			nucLedViewModel.NewProfileBase = nucLedViewModel.SelectedProfile;
			nucLedViewModel.Leds = new ObservableCollection<Led>(nucLedViewModel.Leds.OrderBy((Led Led) => Led.LedType));
			nucLedViewModel.SelectedLed = nucLedViewModel.Leds.First();
			events.GetEvent<AllLedsEnabledEvent>().Subscribe(nucLedViewModel.EnableEvent);
			events.GetEvent<LedColorBehaviorChangeEvent>().Subscribe(nucLedViewModel.UpdateUiPattern);
			events.GetEvent<UpdateUIEvent>().Subscribe(nucLedViewModel.RefreshUI);
			events.GetEvent<ProfileChangedEvent>().Subscribe(nucLedViewModel.ChangeProfile);
			nucLedViewModel.LedsAvailable = true;
			nucLedViewModel.RefreshUI();
		});
	}

	private void EnableToggle(bool completed)
	{
		ProcessComplete = completed;
		RefreshUI();
	}

	private void LoadingScreenVisible(bool b)
	{
		LedsAvailable = b;
		RaisePropertyChanged("LedsAvailable");
		RefreshUI();
	}

	private void ResetToDefaults()
	{
		_settingsMgr.Reset();
		RaisePropertyChanged("Leds");
	}

	private void RefreshUI()
	{
		RaisePropertyChanged("PowerButton");
		RaisePropertyChanged("Skull");
		RaisePropertyChanged("Eyes");
		RaisePropertyChanged("Front1");
		RaisePropertyChanged("Front2");
		RaisePropertyChanged("Front3");
		RaisePropertyChanged("HDD");
		RaisePropertyChanged("LedsAvailable");
		RaisePropertyChanged("SelectedLed");
		RaisePropertyChanged("Leds");
		RaisePropertyChanged("On");
		RaisePropertyChanged("SelectedProfile");
		RaisePropertyChanged("Profiles");
		RaisePropertyChanged("ProcessComplete");
	}

	private void UpdateUiPattern(Led led)
	{
		led.RefreshUi();
		switch (led.LedType)
		{
		case LedType.PowerButton:
			RaisePropertyChanged("PowerButton");
			break;
		case LedType.Hdd:
			RaisePropertyChanged("HDD");
			break;
		case LedType.Front1:
			RaisePropertyChanged("Front1");
			break;
		case LedType.Front2:
			RaisePropertyChanged("Front2");
			break;
		case LedType.Front3:
			RaisePropertyChanged("Front3");
			break;
		case LedType.Skull:
			RaisePropertyChanged("Skull");
			break;
		case LedType.Eyes:
			RaisePropertyChanged("Eyes");
			break;
		}
		_settingsMgr.SaveProfile(_settingsMgr.CurrentProfile, setAsCurrentProfile: true);
		RaisePropertyChanged("SelectedLed");
		RaisePropertyChanged("On");
		RaisePropertyChanged("AlexaEnable");
	}

	private void ChangeProfile(LedProfile newProf)
	{
		IsSettingsSet = false;
		SelectedProfile = newProf;
		RaisePropertyChanged("SelectedProfile");
		RaisePropertyChanged("Leds");
		IsSettingsSet = true;
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
		IsSettingsSet = false;
		foreach (LedProfile profile in Profiles)
		{
			profile.Selected = false;
			profile.TextboxSelected = false;
		}
		Task.Run(() =>
		{
			List<LedProfile> list = Profiles.ToList();
			LedProfile selectedProfile = SelectedProfile;
			_settingsMgr.RemoveAllProfileFiles().Wait();
			Application.Current.Dispatcher.Invoke(() =>
			{
				_settingsMgr.Profiles.Clear();
			});
			foreach (LedProfile item in list)
			{
				if (item == selectedProfile)
				{
					_settingsMgr.SaveProfile(item, setAsCurrentProfile: true).Wait();
				}
				else
				{
					_settingsMgr.SaveProfile(item, setAsCurrentProfile: false).Wait();
				}
			}
			SelectedProfile = selectedProfile;
			RaisePropertyChanged("SelectedProfile");
			RaisePropertyChanged("Profiles");
			RefreshUI();
			IsSettingsSet = true;
		});
	}

	private void AddProfiles()
	{
		if (NewProfileBase == null)
		{
			return;
		}
		NewProfileName = NewProfileName.Trim();
		int num = (from x in _settingsMgr.Profiles
			group x by x.Name).Count((IGrouping<string, LedProfile> x) => x.Skip(1).Any());
		if (_settingsMgr.Profiles.Any((LedProfile profile) => string.Equals(profile.Name, NewProfileName, StringComparison.OrdinalIgnoreCase)) || num > 0)
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
		if (_settingsMgr.Profiles.Count((LedProfile a) => a.Selected) == 1)
		{
			Update();
			NewProfileBase = newProfileBase;
		}
		IsSettingsSet = true;
		SmallLoadScreen = true;
		_settingsMgr.ProcessJsonFile(NewProfileBase.Name, NewProfileName);
		SelectedProfile = _settingsMgr.GetProfileByName(NewProfileName);
		_settingsMgr.SaveProfile(SelectedProfile, setAsCurrentProfile: true);
		Application.Current.Dispatcher.Invoke(() =>
		{
			Leds = new ObservableCollection<Led>(Leds.OrderBy((Led Led) => Led.LedType));
			SmallLoadScreen = false;
		});
		RaisePropertyChanged("SelectedProfile");
	}

	private void ItemSelected()
	{
	}

	private void NewZone(LedType? SelectedLed)
	{
		if (!SelectedLed.HasValue || !SelectedLed.HasValue)
		{
			return;
		}
		switch (SelectedLed.GetValueOrDefault())
		{
		case LedType.PowerButton:
			if (PowerButton != null)
			{
				UpdateSelectedLed(Leds.ElementAt(Leds.IndexOf(PowerButton)));
			}
			break;
		case LedType.Front1:
			if (Front1 != null)
			{
				UpdateSelectedLed(Leds.ElementAt(Leds.IndexOf(Front1)));
			}
			break;
		case LedType.Front2:
			if (Front2 != null)
			{
				UpdateSelectedLed(Leds.ElementAt(Leds.IndexOf(Front2)));
			}
			break;
		case LedType.Front3:
			if (Front3 != null)
			{
				UpdateSelectedLed(Leds.ElementAt(Leds.IndexOf(Front3)));
			}
			break;
		case LedType.Skull:
			if (Skull != null)
			{
				UpdateSelectedLed(Leds.ElementAt(Leds.IndexOf(Skull)));
			}
			break;
		case LedType.Eyes:
			if (Eyes != null)
			{
				UpdateSelectedLed(Leds.ElementAt(Leds.IndexOf(Eyes)));
			}
			break;
		case LedType.Hdd:
			if (Hdd != null)
			{
				UpdateSelectedLed(Leds.ElementAt(Leds.IndexOf(Hdd)));
			}
			break;
		}
	}

	private void EnableEvent(bool enabled)
	{
		On = enabled;
		RaisePropertyChanged("On");
	}

	private void UpdateSelectedLed(Led newSelection)
	{
		SelectedLed = newSelection;
		RaisePropertyChanged("SelectedLed");
	}

	private void Update()
	{
		ValidateProfileName();
		int num = _settingsMgr.Profiles.Count((LedProfile a) => a.Selected);
		for (int num2 = 0; num2 < _settingsMgr.Profiles.Count; num2++)
		{
			LedProfile ledProfile = _settingsMgr.Profiles[num2];
			if (num == 1 && _settingsMgr.Profiles[num2].Selected)
			{
				_settingsMgr.Profiles.RemoveAt(num2);
				ledProfile.TextboxSelected = true;
				_currentProfileName = ledProfile.Name;
				_settingsMgr.Profiles.Insert(num2, ledProfile);
				ProfilesSelected = true;
				return;
			}
			_settingsMgr.Profiles.RemoveAt(num2);
			ledProfile.TextboxSelected = false;
			_settingsMgr.Profiles.Insert(num2, ledProfile);
			if (num == 0)
			{
				ProfilesSelected = false;
			}
		}
		UpdateJsonFile();
	}

	private void UpdateJsonFile()
	{
		List<string> savedProfileNames = _settingsMgr.GetSavedProfileNames();
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
				_settingsMgr.RemoveProfiles(names);
			}
			for (int i = 0; i < Profiles.Count(); i++)
			{
				if (!savedProfileNames.Contains(Profiles[i].Name))
				{
					_settingsMgr.SaveProfile(Profiles[i], setAsCurrentProfile: false).Wait();
				}
			}
		});
	}

	private void ValidateProfileName()
	{
		int num = (from x in _settingsMgr.Profiles
			group x by x.Name into x
			where x.Skip(1).Any()
			select x).Count();
		int num2 = _settingsMgr.Profiles.Where((LedProfile x) => string.IsNullOrWhiteSpace(x.Name)).Count();
		LedProfile ledProfile = null;
		if (num != 0)
		{
			ledProfile = _settingsMgr.Profiles.Where((LedProfile a) => a.TextboxSelected).FirstOrDefault();
			int index = _settingsMgr.Profiles.IndexOf(ledProfile);
			_settingsMgr.Profiles.Remove(ledProfile);
			ledProfile.Name = _currentProfileName;
			_settingsMgr.Profiles.Insert(index, ledProfile);
			ErrorHandler.PublishError("DuplicateProfileError", ErrorType.UNKNOWN, critical: true, msgIsKey: true);
			_events.GetEvent<ApplicationCloseEvent>().Publish(payload: false);
		}
		else if (num2 > 0)
		{
			ledProfile = _settingsMgr.Profiles.Where((LedProfile x) => string.IsNullOrWhiteSpace(x.Name)).FirstOrDefault();
			int index2 = _settingsMgr.Profiles.IndexOf(ledProfile);
			_settingsMgr.Profiles.Remove(ledProfile);
			ledProfile.Name = _currentProfileName;
			_settingsMgr.Profiles.Insert(index2, ledProfile);
		}
	}

	private void DeleteProfiles()
	{
		List<string> list = new List<string>();
		foreach (LedProfile profile in Profiles)
		{
			if (profile.Selected)
			{
				list.Add(profile.Name);
			}
		}
		_settingsMgr.RemoveProfiles(list);
		RaisePropertyChanged("SelectedProfile");
		ProfilesSelected = false;
	}
}
