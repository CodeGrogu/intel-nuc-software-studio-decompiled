using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Events;
using MobileKeyboardLedMgr.Interfaces;
using MobileKeyboardLedMgr.Models.KingCounty;
using MobileKeyboardLedMgr.Models.Profiles;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;
using UniwillSdkClient.Events;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models;

public class KeyboardProfileManager : NavBaseSummaryViewModel, IKeyboardProfileManager, INavigationAware
{
	private readonly ILoggerFacade _logger;

	private readonly IEventAggregator _events;

	private readonly IKeyboardPatternManager _patternMgr;

	private readonly IKeyboardColorManager _colorMgr;

	private readonly IKeyGroupsFileManager _keyGroupsFileManager;

	private readonly ISettingsFileManager _settingsFileManager;

	private int _acIdleTimer;

	private int _dcIdleTimer;

	private bool _editInPaintMode;

	private bool _editKeyGroup;

	private KeyGroup _keyGroupToEdit;

	private KeyGroup _keyGroupToRemove;

	private bool _open;

	private bool _enableSave;

	private bool _smallLoadScreen;

	private bool _errorMessage;

	private bool _projectAuroraInstalled;

	private bool _projectAuroraEnabled;

	private string _selectedLedControl;

	private bool _profilesSelected;

	private ObservableCollection<KeyboardProfile> _profiles;

	private KeyboardProfile _selectedProfile;

	private RGBKeyboardEffect _selectedEffect;

	private ColorPickerColors _selectedColor;

	private ObservableCollection<ColorPickerColors> _selectedColors;

	private ObservableCollection<KeyGroup> _keyGroups;

	private KeyboardProfile _newProfileBase;

	private string _newProfileName;

	public int BrightnessMin => 0;

	public int BrightnessMax => 100;

	public int DefaultBrightness => 100;

	public int SpeedMin => 0;

	public int SpeedMax => 9;

	public int IdleTimerMin => 0;

	public int IdleTimerMax => 13;

	public int IdleTimerInterval => 1;

	public int AcIdleTimer
	{
		get
		{
			return _acIdleTimer;
		}
		set
		{
			_acIdleTimer = value;
			_selectedProfile.AcIdleTimer = value;
			EnableSave = true;
			RaisePropertyChanged("AcIdleTimer");
			Refresh();
		}
	}

	public int DcIdleTimer
	{
		get
		{
			return _dcIdleTimer;
		}
		set
		{
			_dcIdleTimer = value;
			_selectedProfile.DcIdleTimer = value;
			EnableSave = true;
			RaisePropertyChanged("DcIdleTimer");
			Refresh();
		}
	}

	public bool On
	{
		get
		{
			return _patternMgr.On;
		}
		set
		{
			_patternMgr.On = value;
			EnableSave = true;
			RaisePropertyChanged("On");
			Refresh();
		}
	}

	public bool EditInPaintMode
	{
		get
		{
			return _editInPaintMode;
		}
		set
		{
			SetProperty(ref _editInPaintMode, value, "EditInPaintMode");
		}
	}

	public bool EditKeyGroup
	{
		get
		{
			return _editKeyGroup;
		}
		set
		{
			SetProperty(ref _editKeyGroup, value, "EditKeyGroup");
		}
	}

	public KeyGroup KeyGroupToEdit
	{
		get
		{
			return _keyGroupToEdit;
		}
		set
		{
			SetProperty(ref _keyGroupToEdit, value, "KeyGroupToEdit");
		}
	}

	public KeyGroup KeyGroupToRemove
	{
		get
		{
			return _keyGroupToRemove;
		}
		set
		{
			SetProperty(ref _keyGroupToRemove, value, "KeyGroupToRemove");
		}
	}

	private KeyGroup _originalKeyGroupBeingModified { get; set; }

	public bool OnAndNotDefault
	{
		get
		{
			if (!SelectedProfile.Default)
			{
				return On;
			}
			return false;
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

	public bool EnableSave
	{
		get
		{
			return _enableSave;
		}
		set
		{
			SetProperty(ref _enableSave, value, "EnableSave");
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

	public bool ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			SetProperty(ref _errorMessage, value, "ErrorMessage");
		}
	}

	public bool ProjectAuroraInstalled
	{
		get
		{
			return _projectAuroraInstalled;
		}
		set
		{
			SetProperty(ref _projectAuroraInstalled, value, "ProjectAuroraInstalled");
			SelectedLedControl = LedControls[0];
		}
	}

	public bool ProjectAuroraEnabled
	{
		get
		{
			return _projectAuroraEnabled;
		}
		set
		{
			SetProperty(ref _projectAuroraEnabled, value, "ProjectAuroraEnabled");
			_patternMgr.ProjectAuroraEnabled = value;
		}
	}

	public List<string> LedControls { get; } = new List<string>
	{
		KeyboardLedConstants.ENABLE_LEDS,
		KeyboardLedConstants.DISABLE_LEDS,
		KeyboardLedConstants.USE_PROJECT_AURORA
	};

	public string SelectedLedControl
	{
		get
		{
			return _selectedLedControl;
		}
		set
		{
			SetProperty(ref _selectedLedControl, value, "SelectedLedControl");
			if (value.Equals(KeyboardLedConstants.ENABLE_LEDS))
			{
				ProjectAuroraEnabled = false;
				On = true;
			}
			else if (value.Equals(KeyboardLedConstants.DISABLE_LEDS))
			{
				ProjectAuroraEnabled = false;
				On = false;
			}
			else if (value.Equals(KeyboardLedConstants.USE_PROJECT_AURORA))
			{
				ProjectAuroraEnabled = true;
				On = false;
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

	public ObservableCollection<KeyboardProfile> Profiles
	{
		get
		{
			return _profiles;
		}
		set
		{
			SetProperty(ref _profiles, value, "Profiles");
		}
	}

	public KeyboardProfile SelectedProfile
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
				ApplyProfile(value.Name);
				_selectedEffect = _selectedProfile.Pattern.SelectedEffect;
				_selectedColors = _selectedProfile.Pattern.SelectedColors;
				_selectedColor = _selectedColors[0];
			}
			EnableSave = false;
			RaisePropertyChanged("EnableSave");
			Refresh();
		}
	}

	public List<KeyboardPattern> DefaultPatterns => _patternMgr.DefaultPatterns;

	public List<RGBKeyboardEffect> Effects => _patternMgr.PresetPatterns.Effects;

	public RGBKeyboardEffect SelectedEffect
	{
		get
		{
			return _selectedEffect;
		}
		set
		{
			SetProperty(ref _selectedEffect, value, "SelectedEffect");
			ApplyPattern(value);
			SelectedColors = SelectedProfile.Pattern.SelectedColors;
			SelectedColor = SelectedColors[0];
			EnableSave = true;
			RaisePropertyChanged("SelectedEffect");
			Refresh();
		}
	}

	public List<RGBKeyboardDirection> Directions
	{
		get
		{
			return SelectedProfile.Pattern.Directions;
		}
		set
		{
			SelectedProfile.Pattern.Directions = value;
			RaisePropertyChanged("Directions");
		}
	}

	public RGBKeyboardDirection SelectedDirection
	{
		get
		{
			return SelectedProfile.Pattern.SelectedDirection;
		}
		set
		{
			SelectedProfile.Pattern.SelectedDirection = value;
			SelectedProfile.Pattern.UpdatePresetPatternKeysWithColors();
			_patternMgr.SelectedPattern.SelectedDirection = value;
			_patternMgr.ApplySelectedPattern();
			EnableSave = true;
			RaisePropertyChanged("SelectedDirection");
			Refresh();
		}
	}

	public ObservableCollection<System.Windows.Media.Color> StandardColors => _colorMgr.StandardColors;

	public ObservableCollection<System.Windows.Media.Color> RecentColors => _colorMgr.RecentColors;

	public ColorPickerColors SelectedColor
	{
		get
		{
			return _selectedColor;
		}
		set
		{
			SetProperty(ref _selectedColor, value, "SelectedColor");
			CurrentColor = SelectedColor.SelectedColor;
		}
	}

	public System.Windows.Media.Color CurrentColor
	{
		get
		{
			return _colorMgr.CurrentColor;
		}
		set
		{
			_colorMgr.CurrentColor = value;
			KeyboardUiData.SelectedColor = value;
			if (KeyGroupToEdit != null)
			{
				KeyGroupToEdit.SelectedColor = value;
			}
			foreach (KeyGroup keyGroup in KeyGroups)
			{
				keyGroup.SelectedColor = value;
			}
			_colorMgr.SaveRecentColors(RecentColors);
			EnableSave = true;
			RaisePropertyChanged("CurrentColor");
			RaisePropertyChanged("SelectedColors");
			RaisePropertyChanged("EnableSave");
		}
	}

	public ObservableCollection<ColorPickerColors> SelectedColors
	{
		get
		{
			return _selectedColors;
		}
		set
		{
			SetProperty(ref _selectedColors, value, "SelectedColors");
		}
	}

	public int ACBrightness
	{
		get
		{
			return _selectedProfile.Pattern.ACBrightness;
		}
		set
		{
			if (_selectedProfile != null)
			{
				if (value < BrightnessMin)
				{
					value = BrightnessMin;
				}
				else if (value > BrightnessMax)
				{
					value = BrightnessMax;
				}
				_selectedProfile.Pattern.ACBrightness = value;
				_patternMgr.SelectedPattern.ACBrightness = value;
				EnableSave = true;
				RaisePropertyChanged("ACBrightness");
				RaisePropertyChanged("EnableSave");
				Refresh();
			}
		}
	}

	public int DCBrightness
	{
		get
		{
			return _selectedProfile.Pattern.DCBrightness;
		}
		set
		{
			if (_selectedProfile != null)
			{
				if (value < BrightnessMin)
				{
					value = BrightnessMin;
				}
				else if (value > BrightnessMax)
				{
					value = BrightnessMax;
				}
				_selectedProfile.Pattern.DCBrightness = value;
				_patternMgr.SelectedPattern.DCBrightness = value;
				EnableSave = true;
				RaisePropertyChanged("DCBrightness");
				RaisePropertyChanged("EnableSave");
				Refresh();
			}
		}
	}

	public KeyboardUiData KeyboardUiData => _patternMgr.KeyboardUiData;

	public ObservableCollection<KeyGroup> KeyGroups
	{
		get
		{
			return _keyGroups;
		}
		set
		{
			SetProperty(ref _keyGroups, value, "KeyGroups");
		}
	}

	public int Speed
	{
		get
		{
			return _selectedProfile.Pattern.Speed;
		}
		set
		{
			if (_selectedProfile != null)
			{
				_selectedProfile.Pattern.Speed = value;
				_patternMgr.SelectedPattern.Speed = value;
			}
			EnableSave = true;
			RaisePropertyChanged("Speed");
			RaisePropertyChanged("EnableSave");
			Refresh();
		}
	}

	public KeyboardProfile NewProfileBase
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

	public DelegateCommand UndoPaintChanges { get; }

	public DelegateCommand DiscardPaintChanges { get; }

	public DelegateCommand<KeyGroup> OpenKeyGroupsEditor { get; }

	public DelegateCommand CloseKeyGroupsEditor { get; }

	public DelegateCommand SaveKeyGroup { get; }

	public DelegateCommand<KeyGroup> ConfirmDeleteKeyGroup { get; }

	public DelegateCommand DeleteConfirmedKeyGroup { get; }

	public DelegateCommand<KeyGroup> DeleteKeyGroup { get; }

	public DelegateCommand CancelDeleteKeyGroup { get; }

	public DelegateCommand CloseInfo { get; }

	public DelegateCommand OpenInfo { get; }

	public DelegateCommand OpenPaintModeEditor { get; }

	public DelegateCommand ClosePaintModeEditor { get; }

	public DelegateCommand DiscardChanges { get; }

	public DelegateCommand UpdateIdleTimer { get; }

	public DelegateCommand UpdateBrightness { get; }

	public DelegateCommand SendToSDK { get; }

	public DelegateCommand<ColorPickerColors> SwitchColor { get; }

	public DelegateCommand<System.Windows.Media.Color?> SelectColor { get; }

	public DelegateCommand OpenColorPicker { get; }

	public DelegateCommand<KeyValuePair<int, System.Windows.Media.Color>?> ColorSelectionChanged { get; }

	public DelegateCommand AddColorToColorList { get; }

	public DelegateCommand SaveProfileButtonClick { get; }

	public DelegateCommand OpenEditorButtonClick { get; }

	public DelegateCommand CloseEditor { get; }

	public DelegateCommand AddProfile { get; }

	public DelegateCommand UpdateSelection { get; }

	public DelegateCommand SelectItem { get; }

	public DelegateCommand DeleteSelectedProfiles { get; }

	public DelegateCommand RestoreDefaults { get; }

	public DelegateCommand LaunchAurora { get; }

	public KeyboardProfileManager(ILoggerFacade logger, IEventAggregator events, IRegionManager regionMgr, IKeyboardPatternManager patternMgr, IKeyboardColorManager colorMgr, IKeyGroupsFileManager keyGroupsFileManager, ICustomResourceManager resourceManager, ISettingsFileManager settingsFileManager)
		: base(regionMgr, resourceManager, "KeyboardLedSummaryCardTitleString", "KeyboardView", IconType.Keyboard)
	{
		_logger = logger;
		_events = events;
		_patternMgr = patternMgr;
		_errorMessage = patternMgr.ErrorMessage;
		_colorMgr = colorMgr;
		_keyGroupsFileManager = keyGroupsFileManager;
		_settingsFileManager = settingsFileManager;
		_profiles = new ObservableCollection<KeyboardProfile>();
		if (!ErrorMessage)
		{
			if (!Directory.Exists(KeyboardLedConstants.PROFILES_DIRECTORY))
			{
				Directory.CreateDirectory(KeyboardLedConstants.PROFILES_DIRECTORY);
			}
			GetSavedProfiles();
			if (GetProfileByName(KeyboardLedConstants.DEFAULT_NAME) == null)
			{
				CreateNewDefaultProfile();
			}
			if (GetProfileByName(KeyboardLedConstants.CUSTOM_NAME) == null)
			{
				CreateNewDefaultCustomProfile();
			}
			if (SelectedProfile == null)
			{
				if (_profiles.Count == 2 && GetProfileByName(KeyboardLedConstants.DEFAULT_NAME) != null && GetProfileByName(KeyboardLedConstants.CUSTOM_NAME) != null)
				{
					ApplyProfile(KeyboardLedConstants.CUSTOM_NAME);
				}
				else
				{
					ApplyProfile(Profiles[0].Name);
				}
			}
			_events.GetEvent<KeyboardStatusUpdateEvent>().Subscribe(Refresh);
			_events.GetEvent<EnableSaveEvent>().Subscribe(EnableSaveEventHandler);
			_events.GetEvent<ToggleColorEvent>().Subscribe(ToggleColorOnOff);
			_events.GetEvent<UpdateSelectedColorsEvent>().Subscribe(UpdateColorBindings);
			_events.GetEvent<NavigateToHomeEvent>().Subscribe(OnNavigateToHome);
			_events.GetEvent<KeyboardBrightnessEvent>().Subscribe(OnBrightnessChange);
			_events.GetEvent<ProjectAuroraEvent>().Subscribe(OnProjectAuroraUpdate);
			UndoPaintChanges = new DelegateCommand(Undo);
			DiscardPaintChanges = new DelegateCommand(Discard);
			OpenKeyGroupsEditor = new DelegateCommand<KeyGroup>(OpenKeyGroupEditorWindow);
			CloseKeyGroupsEditor = new DelegateCommand(CloseKeyGroupEditorWindow);
			SaveKeyGroup = new DelegateCommand(SaveKeyGroupAndCloseEditorWindow);
			ConfirmDeleteKeyGroup = new DelegateCommand<KeyGroup>(ConfirmRemoveKeyGroup);
			DeleteConfirmedKeyGroup = new DelegateCommand(RemoveConfirmedKeyGroup);
			DeleteKeyGroup = new DelegateCommand<KeyGroup>(RemoveKeyGroup);
			CancelDeleteKeyGroup = new DelegateCommand(CancelRemoveKeyGroup);
			OpenInfo = new DelegateCommand(OpenInfoWindow);
			CloseInfo = new DelegateCommand(CloseInfoWindow);
			OpenPaintModeEditor = new DelegateCommand(OpenPaintModeEditorWindow);
			ClosePaintModeEditor = new DelegateCommand(ClosePaintModeEditorWindow);
			DiscardChanges = new DelegateCommand(ResetToLastSave);
			UpdateIdleTimer = new DelegateCommand(SendIdleTimerToSDK);
			UpdateBrightness = new DelegateCommand(SendBrightnessToSDK);
			SendToSDK = new DelegateCommand(UpdateSdk);
			SwitchColor = new DelegateCommand<ColorPickerColors>(UpdateSelectedColor);
			ColorSelectionChanged = new DelegateCommand<KeyValuePair<int, System.Windows.Media.Color>?>(UpdateColorList);
			SelectColor = new DelegateCommand<System.Windows.Media.Color?>(UpdateColor);
			OpenColorPicker = new DelegateCommand(SelectFromColorPicker);
			SaveProfileButtonClick = new DelegateCommand(SaveProfile);
			OpenEditorButtonClick = new DelegateCommand(OpenedProfileEditorWindow);
			CloseEditor = new DelegateCommand(ClosedProfileEditorWindow);
			AddProfile = new DelegateCommand(AddProfiles);
			UpdateSelection = new DelegateCommand(UpdateSelectedProfile);
			SelectItem = new DelegateCommand(ItemSelected);
			DeleteSelectedProfiles = new DelegateCommand(DeleteProfiles);
			RestoreDefaults = new DelegateCommand(ReturnToDefaults);
			LaunchAurora = new DelegateCommand(OpenProjectAuroraApp);
			_selectedEffect = _selectedProfile.Pattern.SelectedEffect;
			_profilesSelected = false;
			_selectedColors = SelectedProfile.Pattern.SelectedColors;
			_selectedColor = SelectedColors[0];
			OnProjectAuroraUpdate();
			_keyGroups = new ObservableCollection<KeyGroup>(_keyGroupsFileManager.ReadSavedKeyGroups(_selectedColor.SelectedColor));
			ResetNewProfileBase();
			_enableSave = false;
			KeyboardUiData.Editable = false;
		}
		else
		{
			OpenDetailView = null;
		}
	}

	private void OpenProjectAuroraApp()
	{
		string path = Path.Combine(Directory.GetParent(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]))?.FullName, "NssOnStartupLauncher\\");
		try
		{
			Process process = new Process();
			process.StartInfo.FileName = Path.Combine(path, "NssOnStartupLauncher.exe");
			process.StartInfo.Arguments = "aurora";
			process.StartInfo.UseShellExecute = true;
			process.Start();
		}
		catch (Exception)
		{
		}
	}

	private void OnProjectAuroraUpdate()
	{
		_projectAuroraInstalled = _patternMgr.ProjectAuroraInstalled;
		_projectAuroraEnabled = _patternMgr.ProjectAuroraEnabled;
		if (_projectAuroraInstalled)
		{
			if (_projectAuroraEnabled)
			{
				SelectedLedControl = KeyboardLedConstants.USE_PROJECT_AURORA;
				ObservableCollection<KeyColorData> keys = MonocolorPattern.Generate(_patternMgr.KeyboardType, DefaultColors.OFF_COLOR_UI, enabled: false);
				KeyboardUiData.ApplyPattern(keys);
				KeyboardUiData.DisableAll();
			}
			else if (_patternMgr.On)
			{
				SelectedLedControl = KeyboardLedConstants.ENABLE_LEDS;
			}
			else
			{
				SelectedLedControl = KeyboardLedConstants.DISABLE_LEDS;
			}
		}
		RaisePropertyChanged("ProjectAuroraInstalled");
		RaisePropertyChanged("ProjectAuroraEnabled");
	}

	private void CreateNewDefaultProfile()
	{
		PresetPatternsKC presetPatternsKC = new PresetPatternsKC(_colorMgr, _events, _settingsFileManager, _patternMgr.KeyboardType);
		string targetPatternId = "Monocolor";
		int acIdleTimer = DefaultSettings.IDLE_TIMER_AC_VAL;
		int dcIdleTimer = DefaultSettings.IDLE_TIMER_DC_VAL;
		if (_settingsFileManager?.CustomDefaultSettings?.DefaultEffect != null)
		{
			targetPatternId = _settingsFileManager.CustomDefaultSettings.DefaultEffect;
			acIdleTimer = KeyboardIdleTimer.ConvertSecondsToIdleTImer(_settingsFileManager.CustomDefaultSettings.DefaultIdleTimerAc);
			dcIdleTimer = KeyboardIdleTimer.ConvertSecondsToIdleTImer(_settingsFileManager.CustomDefaultSettings.DefaultIdleTimerDc);
		}
		KeyboardProfile item = new KeyboardProfile(KeyboardLedConstants.DEFAULT_NAME, presetPatternsKC.Patterns.Find((KeyboardPattern p) => p.Id.Equals(targetPatternId)), acIdleTimer, dcIdleTimer, isEnabled: true)
		{
			Default = true
		};
		Profiles.Add(item);
	}

	private void CreateNewDefaultCustomProfile()
	{
		PresetPatternsKC presetPatternsKC = new PresetPatternsKC(_colorMgr, _events, _settingsFileManager, _patternMgr.KeyboardType);
		string targetPatternId = "Monocolor";
		int acIdleTimer = DefaultSettings.IDLE_TIMER_AC_VAL;
		int dcIdleTimer = DefaultSettings.IDLE_TIMER_DC_VAL;
		if (_settingsFileManager?.CustomDefaultSettings?.DefaultEffect != null)
		{
			targetPatternId = _settingsFileManager.CustomDefaultSettings.DefaultEffect;
			acIdleTimer = KeyboardIdleTimer.ConvertSecondsToIdleTImer(_settingsFileManager.CustomDefaultSettings.DefaultIdleTimerAc);
			dcIdleTimer = KeyboardIdleTimer.ConvertSecondsToIdleTImer(_settingsFileManager.CustomDefaultSettings.DefaultIdleTimerDc);
		}
		KeyboardProfile item = new KeyboardProfile(KeyboardLedConstants.CUSTOM_NAME, presetPatternsKC.Patterns.Find((KeyboardPattern p) => p.Id.Equals(targetPatternId)), acIdleTimer, dcIdleTimer, isEnabled: true)
		{
			Default = false
		};
		Profiles.Add(item);
	}

	private void ResetNewProfileBase()
	{
		if (SelectedProfile == null)
		{
			ApplyProfile(KeyboardLedConstants.DEFAULT_NAME);
		}
		NewProfileBase = SelectedProfile;
	}

	private void GetSavedProfiles()
	{
		try
		{
			List<string> list = Directory.GetFiles(KeyboardLedConstants.PROFILES_DIRECTORY).ToList();
			if (list.Count <= 0)
			{
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(KeyboardLedConstants.PROFILES_DIRECTORY);
			string text = KeyboardLedConstants.PROFILES_DIRECTORY + (from f in directoryInfo.GetFiles()
				orderby f.LastWriteTime descending
				select f).First().ToString();
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			ProcessJsonFile(text);
			if (GetProfileByName(fileNameWithoutExtension) != null && SelectedProfile == null)
			{
				ApplyProfile(fileNameWithoutExtension);
			}
			list.Remove(text);
			foreach (string item in list)
			{
				ProcessJsonFile(item);
			}
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void UpdateColorBindings()
	{
		_selectedColors = SelectedProfile.Pattern.SelectedColors;
		SelectedColor = _selectedColors[_selectedColor.Index];
		RaisePropertyChanged("_selectedColors");
	}

	public void ApplyPattern(RGBKeyboardEffect effect)
	{
		KeyboardPattern keyboardPattern = _patternMgr.PresetPatterns.Patterns.Find((KeyboardPattern p) => p.SelectedEffect == effect);
		keyboardPattern.ACBrightness = SelectedProfile.Pattern.ACBrightness;
		keyboardPattern.DCBrightness = SelectedProfile.Pattern.DCBrightness;
		if (SelectedProfile.Pattern.SpeedEnabled && keyboardPattern.SpeedEnabled)
		{
			keyboardPattern.Speed = SelectedProfile.Pattern.Speed;
		}
		SelectedProfile.Pattern = keyboardPattern.DeepCopy();
		_patternMgr.SelectedPattern = SelectedProfile.Pattern;
		_patternMgr.ApplySelectedPattern();
	}

	public void ApplyProfile(string name)
	{
		_selectedProfile = Profiles.FirstOrDefault((KeyboardProfile p) => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		UpdateProfileWriteTime(name);
		_patternMgr.SelectedPattern = SelectedProfile.Pattern;
		_selectedEffect = _selectedProfile.Pattern.SelectedEffect;
		RaisePropertyChanged("SelectedEffect");
		_patternMgr.AcBrightness = ACBrightness;
		_patternMgr.DcBrightness = DCBrightness;
		_acIdleTimer = _selectedProfile.AcIdleTimer;
		_dcIdleTimer = _selectedProfile.DcIdleTimer;
		SendIdleTimerToSDK();
		if (On != _selectedProfile.IsEnabled)
		{
			On = _selectedProfile.IsEnabled;
		}
		_patternMgr.ApplySelectedPattern();
		EnableSave = false;
		RaisePropertyChanged("EnableSave");
	}

	public void SaveProfilesAsJson(KeyboardProfile profile, bool isCurrent, string newName = null)
	{
		if (newName != null)
		{
			profile.Name = newName;
		}
		try
		{
			string name = profile.Name;
			KeyboardPattern pattern = profile.Pattern;
			_profiles.Remove(profile);
			profile = new KeyboardProfile(name, pattern, profile.AcIdleTimer, profile.DcIdleTimer, On);
			if (profile.Name == KeyboardLedConstants.DEFAULT_NAME && GetProfileByName(KeyboardLedConstants.DEFAULT_NAME) == null)
			{
				profile.Default = true;
			}
			if (File.Exists(Path.Combine(KeyboardLedConstants.PROFILES_DIRECTORY, profile.Id + ".json")))
			{
				File.Delete(Path.Combine(KeyboardLedConstants.PROFILES_DIRECTORY, profile.Id + ".json"));
			}
			SerializableKeyboardProfile serializableKeyboardProfile = profile.GetSerializableKeyboardProfile();
			NssJsonSerializer.SerializeWithBinding(serializableKeyboardProfile, Path.Combine(KeyboardLedConstants.PROFILES_DIRECTORY, serializableKeyboardProfile.Name + ".json"), KeyboardLedConstants.ACCEPTED_JSON_TYPES);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			return;
		}
		if (!Profiles.Contains(GetProfileByName(profile.Name)))
		{
			Profiles.Add(profile);
		}
		if (isCurrent)
		{
			ApplyProfile(profile.Name);
		}
		EnableSave = false;
		RaisePropertyChanged("EnableSave");
	}

	private void ProcessJsonFile(string fileName, string newName = null)
	{
		string searchName = Path.GetFileNameWithoutExtension(fileName);
		if (newName != null)
		{
			fileName = KeyboardLedConstants.PROFILES_DIRECTORY + fileName + ".json";
			searchName = newName;
		}
		try
		{
			SerializableKeyboardProfile serializableKeyboardProfile = NssJsonSerializer.DeserializeWithBinding<SerializableKeyboardProfile>(fileName, KeyboardLedConstants.ACCEPTED_JSON_TYPES);
			if (serializableKeyboardProfile != null)
			{
				KeyboardProfile keyboardProfile = new KeyboardProfile(serializableKeyboardProfile.Name, SerializableKeyboardPatternToPattern(serializableKeyboardProfile.Pattern), serializableKeyboardProfile.AcIdleTimer, serializableKeyboardProfile.DcIdleTimer, serializableKeyboardProfile.IsEnabled);
				if (keyboardProfile.Name == KeyboardLedConstants.DEFAULT_NAME)
				{
					keyboardProfile.Default = true;
				}
				if (GetProfileByName(searchName) != null)
				{
					Profiles.Remove(GetProfileByName(searchName));
				}
				if (keyboardProfile != null && keyboardProfile.Pattern != null)
				{
					keyboardProfile.Pattern.StandardColors = _colorMgr.StandardColors;
					keyboardProfile.Pattern.RecentColors = _colorMgr.RecentColors;
					Profiles.Add(keyboardProfile);
				}
			}
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void RemoveAllProfileFiles()
	{
		try
		{
			foreach (string item in Directory.GetFiles(Path.GetDirectoryName(KeyboardLedConstants.PROFILES_DIRECTORY)).ToList())
			{
				File.Delete(item);
			}
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void RemoveProfiles(List<string> names)
	{
		foreach (string name in names)
		{
			KeyboardProfile profile = GetProfileByName(name);
			if (profile != null && Profiles.Count > 1)
			{
				System.Windows.Application.Current.Dispatcher.Invoke(() =>
				{
					Profiles.Remove(profile);
				});
				if (profile.Name.Equals(SelectedProfile.Name))
				{
					ApplyProfile(KeyboardLedConstants.DEFAULT_NAME);
				}
				File.Delete(KeyboardLedConstants.PROFILES_DIRECTORY + profile.Name + ".json");
			}
		}
		foreach (string name2 in names)
		{
			KeyboardProfile profileByName = GetProfileByName(name2);
			if (profileByName != null && Profiles.Count > 1)
			{
				File.Delete(KeyboardLedConstants.PROFILES_DIRECTORY + profileByName.Name + ".json");
			}
		}
	}

	public KeyboardProfile GetProfileByName(string searchName)
	{
		if (_profiles.Count == 0)
		{
			return null;
		}
		return _profiles.FirstOrDefault((KeyboardProfile p) => p.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));
	}

	private void EnableSaveEventHandler()
	{
		EnableSave = true;
		RaisePropertyChanged("EnableSave");
		Refresh();
	}

	public KeyboardPattern SerializableKeyboardPatternToPattern(SerializableKeyboardPattern serializedPattern)
	{
		PresetPatternsKC presetPatterns = new PresetPatternsKC(_colorMgr, _events, _settingsFileManager, _patternMgr.KeyboardType);
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		foreach (KeyColorData key in serializedPattern.Keys)
		{
			observableCollection.Add(key);
		}
		SortedList<int, System.Windows.Media.Color> sortedList = new SortedList<int, System.Windows.Media.Color>();
		foreach (KeyValuePair<int, System.Windows.Media.Color> availableColor in serializedPattern.AvailableColors)
		{
			sortedList.Add(availableColor.Key, availableColor.Value);
		}
		ObservableCollection<ColorPickerColors> observableCollection2 = new ObservableCollection<ColorPickerColors>();
		foreach (SerializableColorPickerColors selectedColor in serializedPattern.SelectedColors)
		{
			observableCollection2.Add(new ColorPickerColors(_events, selectedColor.Index, selectedColor.SelectedColor, _selectedEffect, _colorMgr.StandardColors, _colorMgr.RecentColors, selectedColor.Enabled, selectedColor.Enabled));
		}
		return new KeyboardPattern(_events, presetPatterns, serializedPattern.Id, serializedPattern.Name, _patternMgr.KeyboardType, observableCollection, serializedPattern.Directions, serializedPattern.SelectedDirection, serializedPattern.SelectedEffect, serializedPattern.ACBrightness, serializedPattern.DCBrightness, serializedPattern.Speed, serializedPattern.Preset, serializedPattern.FrequencySupported, sortedList, observableCollection2, _colorMgr.StandardColors, _colorMgr.RecentColors, serializedPattern.EditingEnabled, serializedPattern.TotalColorCount, serializedPattern.SpeedEnabled, serializedPattern.KeyGroupEnabled);
	}

	private void ItemSelected()
	{
	}

	private void SaveProfile()
	{
		SaveProfilesAsJson(SelectedProfile, isCurrent: true);
		EnableSave = false;
		RaisePropertyChanged("EnableSave");
		Refresh();
	}

	private void AddProfiles()
	{
		if (SelectedProfile != null)
		{
			SmallLoadScreen = true;
			if (NewProfileBase == null)
			{
				NewProfileBase = GetProfileByName(KeyboardLedConstants.DEFAULT_NAME);
			}
			KeyboardProfile profile = new KeyboardProfile(NewProfileName, NewProfileBase.Pattern.DeepCopy(), NewProfileBase.AcIdleTimer, NewProfileBase.DcIdleTimer, NewProfileBase.IsEnabled);
			SaveProfilesAsJson(profile, isCurrent: true, NewProfileName);
			SmallLoadScreen = false;
			ResetNewProfileBase();
			SelectedColors = SelectedProfile.Pattern.SelectedColors;
			SelectedColor = SelectedColors[0];
			_selectedEffect = _selectedProfile.Pattern.SelectedEffect;
			Refresh();
		}
	}

	private void UpdateSelectedProfile()
	{
		SmallLoadScreen = true;
		foreach (KeyboardProfile profile in Profiles)
		{
			if (profile.Selected)
			{
				ProfilesSelected = true;
				SmallLoadScreen = false;
				RaisePropertyChanged("Profiles");
				RaisePropertyChanged("SelectedProfile");
				return;
			}
		}
		ProfilesSelected = false;
		SmallLoadScreen = false;
		RaisePropertyChanged("Profiles");
		RaisePropertyChanged("SelectedProfile");
	}

	private void DeleteProfiles()
	{
		List<string> list = new List<string>();
		foreach (KeyboardProfile profile in Profiles)
		{
			if (profile.Selected)
			{
				list.Add(profile.Name);
			}
		}
		RemoveProfiles(list);
		ResetNewProfileBase();
		ProfilesSelected = false;
		RaisePropertyChanged("Profiles");
		RaisePropertyChanged("SelectedProfile");
		Refresh();
	}

	private void UpdateProfileWriteTime(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return;
		}
		try
		{
			string path = KeyboardLedConstants.PROFILES_DIRECTORY + name + ".json";
			if (File.Exists(path))
			{
				File.SetLastWriteTime(path, DateTime.Now);
				return;
			}
			KeyboardProfile profileByName = GetProfileByName(name);
			SaveProfilesAsJson(profileByName, isCurrent: true);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void UpdateColorList(KeyValuePair<int, System.Windows.Media.Color>? obj)
	{
		if (obj.HasValue)
		{
			SelectedProfile.Pattern.UpdateColorList(obj.Value, CurrentColor);
			ApplyProfile(SelectedProfile.Name);
			EnableSave = true;
			RaisePropertyChanged("SelectedColors");
			RaisePropertyChanged("KeyboardUiData");
			RaisePropertyChanged("EnableSave");
		}
	}

	private void UpdateColor(System.Windows.Media.Color? newColor)
	{
		if (newColor.HasValue)
		{
			CurrentColor = newColor.Value;
			EnableSave = true;
			RaisePropertyChanged("RecentColors");
			RaisePropertyChanged("EnableSave");
		}
	}

	private void SelectFromColorPicker()
	{
		ColorDialog colorDialog = new ColorDialog();
		colorDialog.FullOpen = true;
		colorDialog.Color = System.Drawing.Color.FromArgb(CurrentColor.A, CurrentColor.R, CurrentColor.G, CurrentColor.B);
		colorDialog.ShowDialog();
		System.Drawing.Color color = colorDialog.Color;
		UpdateColor(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
		EnableSave = true;
		RaisePropertyChanged("EnableSave");
	}

	private void ToggleColorOnOff(ColorPickerColors color)
	{
		if (color.Selected)
		{
			AddColorToSelectedColorsList(color);
			SelectedColors = SelectedProfile.Pattern.SelectedColors;
			SelectedColor = SelectedColors.FirstOrDefault((ColorPickerColors c) => c.Index == color.Index);
		}
		else
		{
			RemoveColorSelection(color);
			SelectedColors = SelectedProfile.Pattern.SelectedColors;
			SelectedColor = SelectedColors.FirstOrDefault((ColorPickerColors c) => c.Index != color.Index && c.Selected);
			_patternMgr.ApplySelectedPattern();
		}
		EnableSave = true;
		Refresh();
	}

	private void AddColorToSelectedColorsList(ColorPickerColors color)
	{
		CurrentColor = color.SelectedColor;
		if (SelectedProfile.Pattern.AvailableColors.Count <= SelectedProfile.Pattern.TotalColorCount)
		{
			KeyValuePair<int, System.Windows.Media.Color> value = new KeyValuePair<int, System.Windows.Media.Color>(color.Index, color.SelectedColor);
			UpdateColorList(value);
		}
	}

	private void RemoveColorSelection(ColorPickerColors color)
	{
		KeyValuePair<int, System.Windows.Media.Color> keyValuePair = SelectedProfile.Pattern.AvailableColors.FirstOrDefault((KeyValuePair<int, System.Windows.Media.Color> c) => c.Key == color.Index);
		SelectedProfile.Pattern.AvailableColors.Remove(keyValuePair.Key);
		SelectedProfile.Pattern.UpdateSelectedColors();
		KeyboardUiData.ApplyPattern(SelectedProfile.Pattern.Keys);
	}

	private void OpenedProfileEditorWindow()
	{
		_smallLoadScreen = true;
		NewProfileName = "";
		Open = true;
		_smallLoadScreen = false;
		ResetNewProfileBase();
	}

	private void ClosedProfileEditorWindow()
	{
		NewProfileName = "";
		Open = false;
		List<KeyboardProfile> list = Profiles.ToList();
		KeyboardProfile selectedProfile = SelectedProfile;
		RemoveAllProfileFiles();
		Profiles.Clear();
		foreach (KeyboardProfile item in list)
		{
			SmallLoadScreen = true;
			if (item == selectedProfile)
			{
				SaveProfilesAsJson(item, isCurrent: true);
			}
			else
			{
				SaveProfilesAsJson(item, isCurrent: false);
			}
			SmallLoadScreen = false;
		}
		ProfilesSelected = false;
		ResetNewProfileBase();
		RaisePropertyChanged("Profiles");
		RaisePropertyChanged("SelectedProfile");
		Refresh();
	}

	public void Refresh()
	{
		RaisePropertyChanged("KeyboardUiData");
		RaisePropertyChanged("Effects");
		RaisePropertyChanged("SelectedEffect");
		RaisePropertyChanged("SelectedColors");
		RaisePropertyChanged("SelectedColor");
		RaisePropertyChanged("CurrentColor");
		RaisePropertyChanged("Selected");
		RaisePropertyChanged("RecentColors");
		RaisePropertyChanged("On");
		RaisePropertyChanged("OnAndNotDefault");
		RaisePropertyChanged("ACBrightness");
		RaisePropertyChanged("DCBrightness");
		RaisePropertyChanged("AcIdleTimer");
		RaisePropertyChanged("DcIdleTimer");
		RaisePropertyChanged("Speed");
		RaisePropertyChanged("EnableSave");
		RaisePropertyChanged("Directions");
		RaisePropertyChanged("SelectedDirection");
		RaisePropertyChanged("Profiles");
		RaisePropertyChanged("SelectedProfile");
	}

	private void UpdateSdk()
	{
		_patternMgr.ApplySelectedPattern();
	}

	private void SendIdleTimerToSDK()
	{
		_patternMgr.UpdateIdleTimer(_acIdleTimer, isAc: true);
		_patternMgr.UpdateIdleTimer(_dcIdleTimer, isAc: false);
	}

	private void SendBrightnessToSDK()
	{
		_patternMgr.ApplyBrightness(isAcBrightness: true);
		_patternMgr.ApplyBrightness(isAcBrightness: false);
	}

	private void UpdateSelectedColor(ColorPickerColors color)
	{
		if (color.Enabled)
		{
			SelectedColor = SelectedColors.FirstOrDefault((ColorPickerColors c) => c.Index == color.Index);
			EnableSave = true;
			RaisePropertyChanged("SelectedColor");
			RaisePropertyChanged("SelectedColors");
		}
	}

	private void ReturnToDefaults()
	{
		PresetPatternsKC presetPatternsKC = new PresetPatternsKC(_colorMgr, _events, _settingsFileManager, _patternMgr.KeyboardType);
		KeyboardProfile profileByName = GetProfileByName(KeyboardLedConstants.DEFAULT_NAME);
		KeyboardProfile newProfile = new KeyboardProfile(SelectedProfile.Name, presetPatternsKC.Patterns.FirstOrDefault((KeyboardPattern p) => p.Name.Equals(SelectedProfile.Pattern.Name)), profileByName.AcIdleTimer, profileByName.DcIdleTimer, isEnabled: true);
		Profiles.Remove(SelectedProfile);
		Profiles.Add(newProfile);
		SelectedProfile = Profiles.FirstOrDefault((KeyboardProfile p) => p.Name == newProfile.Name);
		_selectedColors = SelectedProfile.Pattern.SelectedColors;
		_selectedColor = SelectedColors[0];
		EnableSave = true;
		Refresh();
	}

	private void OpenKeyGroupEditorWindow(KeyGroup keyGroup = null)
	{
		if (keyGroup == null)
		{
			KeyGroupToEdit = new KeyGroup(_events, _patternMgr.KeyboardType, "", _selectedColor.SelectedColor, null);
		}
		else
		{
			_originalKeyGroupBeingModified = keyGroup;
			KeyGroupToEdit = _originalKeyGroupBeingModified.DeepCopy();
		}
		KeyboardUiData.Editable = false;
		if (KeyGroupToEdit != null)
		{
			KeyGroupToEdit.KeyboardUiData.Editable = true;
		}
		EditKeyGroup = true;
	}

	private void CloseKeyGroupEditorWindow()
	{
		KeyboardUiData.Editable = true;
		if (KeyGroupToEdit != null)
		{
			KeyGroupToEdit.KeyboardUiData.Editable = false;
		}
		_originalKeyGroupBeingModified = null;
		KeyGroupToEdit = null;
		EditKeyGroup = false;
	}

	private void SaveKeyGroupAndCloseEditorWindow()
	{
		if (_originalKeyGroupBeingModified == null)
		{
			KeyGroups.Add(KeyGroupToEdit);
		}
		else
		{
			KeyGroups.Remove(_originalKeyGroupBeingModified);
			KeyGroups.Add(KeyGroupToEdit);
		}
		KeyGroups = new ObservableCollection<KeyGroup>(KeyGroups.OrderBy((KeyGroup x) => x.Name).ToList());
		_keyGroupsFileManager.SaveKeyGroups(KeyGroups.ToList());
		CloseKeyGroupEditorWindow();
	}

	private void RemoveKeyGroup(KeyGroup keyGroupToRemove)
	{
		KeyGroups.Remove(keyGroupToRemove);
		_keyGroupsFileManager.SaveKeyGroups(KeyGroups.ToList());
	}

	private void ConfirmRemoveKeyGroup(KeyGroup keyGroupToRemove)
	{
		KeyGroupToRemove = keyGroupToRemove;
	}

	private void RemoveConfirmedKeyGroup()
	{
		if (KeyGroupToRemove != null)
		{
			RemoveKeyGroup(KeyGroupToRemove);
			KeyGroupToRemove = null;
		}
	}

	private void CancelRemoveKeyGroup()
	{
		KeyGroupToRemove = null;
	}

	private void OpenInfoWindow()
	{
	}

	private void CloseInfoWindow()
	{
	}

	private void OpenPaintModeEditorWindow()
	{
	}

	private void ClosePaintModeEditorWindow()
	{
		EditInPaintMode = false;
	}

	private void ResetToLastSave()
	{
	}

	private void Undo()
	{
	}

	private void Discard()
	{
	}

	public void OnNavigatedTo(NavigationContext navigationContext)
	{
		if (navigationContext.Uri.Equals("KeyboardView"))
		{
			KeyboardUiData.Editable = SelectedEffect == RGBKeyboardEffect.UserMode;
			if (ProjectAuroraEnabled)
			{
				_selectedLedControl = KeyboardLedConstants.USE_PROJECT_AURORA;
			}
			else
			{
				_selectedLedControl = (On ? KeyboardLedConstants.ENABLE_LEDS : KeyboardLedConstants.DISABLE_LEDS);
			}
			RaisePropertyChanged("SelectedLedControl");
		}
	}

	public bool IsNavigationTarget(NavigationContext navigationContext)
	{
		return true;
	}

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
	}

	private void OnNavigateToHome()
	{
		if (KeyboardUiData.Editable)
		{
			KeyboardUiData.Editable = false;
		}
	}

	private void OnBrightnessChange((int Ac, int Dc) brightnessEvent)
	{
		if (PowerUtil.IsPluggedIn())
		{
			if (brightnessEvent.Ac != ACBrightness)
			{
				(ACBrightness, _) = brightnessEvent;
			}
		}
		else if (brightnessEvent.Dc != DCBrightness)
		{
			DCBrightness = brightnessEvent.Dc;
		}
	}
}
