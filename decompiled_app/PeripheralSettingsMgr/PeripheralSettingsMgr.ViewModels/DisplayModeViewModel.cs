using System.Collections.Generic;
using System.Collections.ObjectModel;
using PeripheralSettingsMgr.Interfaces;
using PeripheralSettingsMgr.Models.DisplayModes;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PeripheralSettingsMgr.ViewModels;

public class DisplayModeViewModel : BindableBase, INavigationAware
{
	private const string CURRENT_PROFILE_PROPERTY_NAME = "CurrentProfile";

	private readonly IDisplaySettingsManager _settingsMgr;

	private bool _editing;

	private bool _profilesSelected;

	private string _newProfileName;

	private DisplayModeProfile _newProfileBase;

	private DelegateCommand _editProfiles;

	private DelegateCommand _deleteSelectedProfiles;

	private DelegateCommand _closeEditor;

	private DelegateCommand _addProfile;

	private DelegateCommand _updateSelection;

	private DelegateCommand _reset;

	private DelegateCommand _resetDefault;

	public bool Editing
	{
		get
		{
			return _editing;
		}
		set
		{
			SetProperty(ref _editing, value, "Editing");
		}
	}

	public bool AcOnly
	{
		get
		{
			return _settingsMgr.AcOnly;
		}
		set
		{
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

	public ObservableCollection<DisplayModeProfile> Profiles => _settingsMgr.Profiles;

	public DisplayModeProfile CurrentProfile
	{
		get
		{
			return _settingsMgr.CurrentProfile;
		}
		set
		{
			_settingsMgr.CurrentProfile = value;
			RaisePropertyChanged("CurrentProfile");
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

	public DisplayModeProfile NewProfileBase
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

	public DelegateCommand EditProfiles
	{
		get
		{
			return _editProfiles;
		}
		set
		{
			SetProperty(ref _editProfiles, value, "EditProfiles");
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

	public DelegateCommand ResetDefault
	{
		get
		{
			return _resetDefault;
		}
		set
		{
			SetProperty(ref _resetDefault, value, "ResetDefault");
		}
	}

	public DisplayModeViewModel(IDisplaySettingsManager settingsMgr)
	{
		_settingsMgr = settingsMgr;
		_editProfiles = new DelegateCommand(() =>
		{
			Editing = !_editing;
		});
		_deleteSelectedProfiles = new DelegateCommand(DeleteProfiles);
		_closeEditor = new DelegateCommand(() =>
		{
			Editing = false;
			foreach (DisplayModeProfile profile in Profiles)
			{
				profile.Selected = false;
			}
			NewProfileName = "";
			NewProfileBase = _settingsMgr.GetDefaultProfile();
			ProfilesSelected = false;
		});
		_addProfile = new DelegateCommand(Add);
		_updateSelection = new DelegateCommand(Update);
		_reset = new DelegateCommand(() =>
		{
			CurrentProfile = _settingsMgr.ResetSettings();
			RaisePropertyChanged("CurrentProfile");
		});
		_resetDefault = new DelegateCommand(() =>
		{
			CurrentProfile = _settingsMgr.ResetDefaultSettings();
			RaisePropertyChanged("CurrentProfile");
		});
	}

	private void Add()
	{
		_settingsMgr.AddProfile(NewProfileName, NewProfileBase);
		NewProfileName = "";
		NewProfileBase = _settingsMgr.GetDefaultProfile();
	}

	private void Update()
	{
		foreach (DisplayModeProfile profile in Profiles)
		{
			if (profile.Selected)
			{
				ProfilesSelected = true;
				return;
			}
		}
		ProfilesSelected = false;
	}

	private void DeleteProfiles()
	{
		List<string> list = new List<string>();
		foreach (DisplayModeProfile profile in Profiles)
		{
			if (profile.Selected && !profile.Default)
			{
				list.Add(profile.Name);
			}
		}
		_settingsMgr.RemoveProfiles(list);
		RaisePropertyChanged("CurrentProfile");
	}

	public void OnNavigatedTo(NavigationContext navigationContext)
	{
		NewProfileName = "";
		NewProfileBase = _settingsMgr.GetDefaultProfile();
		Editing = false;
		RaisePropertyChanged("CurrentProfile");
	}

	public bool IsNavigationTarget(NavigationContext navigationContext)
	{
		return true;
	}

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
		Editing = false;
		NewProfileName = "";
		NewProfileBase = _settingsMgr.GetDefaultProfile();
	}
}
