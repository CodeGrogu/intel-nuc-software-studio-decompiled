using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models;
using MobileKeyboardLedMgr.Models.Profiles;
using Prism.Commands;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Interfaces;

public interface IKeyboardProfileManager
{
	ObservableCollection<KeyboardProfile> Profiles { get; set; }

	ObservableCollection<KeyGroup> KeyGroups { get; set; }

	ObservableCollection<Color> StandardColors { get; }

	ObservableCollection<Color> RecentColors { get; }

	ObservableCollection<ColorPickerColors> SelectedColors { get; }

	List<RGBKeyboardDirection> Directions { get; set; }

	List<KeyboardPattern> DefaultPatterns { get; }

	List<RGBKeyboardEffect> Effects { get; }

	List<string> LedControls { get; }

	KeyboardProfile SelectedProfile { get; set; }

	KeyboardProfile NewProfileBase { get; set; }

	KeyboardUiData KeyboardUiData { get; }

	KeyGroup KeyGroupToEdit { get; set; }

	KeyGroup KeyGroupToRemove { get; set; }

	RGBKeyboardEffect SelectedEffect { get; set; }

	RGBKeyboardDirection SelectedDirection { get; set; }

	ColorPickerColors SelectedColor { get; set; }

	Color CurrentColor { get; set; }

	int ACBrightness { get; set; }

	int DCBrightness { get; set; }

	int Speed { get; set; }

	int BrightnessMin { get; }

	int BrightnessMax { get; }

	int DefaultBrightness { get; }

	int IdleTimerMin { get; }

	int IdleTimerMax { get; }

	int IdleTimerInterval { get; }

	int AcIdleTimer { get; set; }

	int DcIdleTimer { get; set; }

	int SpeedMin { get; }

	int SpeedMax { get; }

	bool EditInPaintMode { get; set; }

	bool EditKeyGroup { get; set; }

	bool ProjectAuroraInstalled { get; set; }

	bool ProjectAuroraEnabled { get; set; }

	bool On { get; set; }

	bool OnAndNotDefault { get; }

	bool Open { get; set; }

	bool ProfilesSelected { get; set; }

	bool SmallLoadScreen { get; set; }

	string NewProfileName { get; set; }

	string SelectedLedControl { get; set; }

	DelegateCommand UndoPaintChanges { get; }

	DelegateCommand DiscardPaintChanges { get; }

	DelegateCommand<ColorPickerColors> SwitchColor { get; }

	DelegateCommand RestoreDefaults { get; }

	DelegateCommand<KeyGroup> OpenKeyGroupsEditor { get; }

	DelegateCommand CloseKeyGroupsEditor { get; }

	DelegateCommand<KeyGroup> ConfirmDeleteKeyGroup { get; }

	DelegateCommand DeleteConfirmedKeyGroup { get; }

	DelegateCommand<KeyGroup> DeleteKeyGroup { get; }

	DelegateCommand CancelDeleteKeyGroup { get; }

	DelegateCommand OpenInfo { get; }

	DelegateCommand CloseInfo { get; }

	DelegateCommand OpenPaintModeEditor { get; }

	DelegateCommand ClosePaintModeEditor { get; }

	DelegateCommand DiscardChanges { get; }

	DelegateCommand UpdateIdleTimer { get; }

	DelegateCommand UpdateBrightness { get; }

	DelegateCommand SendToSDK { get; }

	DelegateCommand OpenColorPicker { get; }

	DelegateCommand<KeyValuePair<int, Color>?> ColorSelectionChanged { get; }

	DelegateCommand AddColorToColorList { get; }

	DelegateCommand SaveProfileButtonClick { get; }

	DelegateCommand OpenEditorButtonClick { get; }

	DelegateCommand CloseEditor { get; }

	DelegateCommand AddProfile { get; }

	DelegateCommand UpdateSelection { get; }

	DelegateCommand SelectItem { get; }

	DelegateCommand DeleteSelectedProfiles { get; }

	void ApplyProfile(string name);

	void SaveProfilesAsJson(KeyboardProfile profile, bool isCurrent, string newName = null);

	void RemoveProfiles(List<string> names);

	void RemoveAllProfileFiles();

	void ApplyPattern(RGBKeyboardEffect effect);

	KeyboardProfile GetProfileByName(string searchName);
}
