using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;

namespace PeripheralSettingsMgr.Models.DisplayModes;

public class DisplayModeProfile : BindableBase
{
	public delegate bool NameValidatorDelegate(string Name);

	private readonly NameValidatorDelegate _nameValidator;

	private bool _selected;

	private DelegateCommand _selectItem;

	private string _name;

	private const string HIDE_GRID_BORDER = "0,0,0,0";

	private const string DISPLAY_GRID_BORDER = "1,1,1,1";

	public bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			SetProperty(ref _selected, value, "Selected");
		}
	}

	[JsonIgnore]
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

	[JsonIgnore]
	public string OldName { get; private set; }

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			if (_nameValidator == null || _nameValidator(value))
			{
				OldName = Name;
				SetProperty(ref _name, value, "Name");
			}
		}
	}

	public bool Default { get; set; }

	public ObservableCollection<MonitorDisplayModeProfile> Monitors { get; set; }

	public int NumMonitorColumns
	{
		get
		{
			if (Monitors.Count >= 2)
			{
				return 2;
			}
			return 1;
		}
		set
		{
		}
	}

	public string DetermineBorder
	{
		get
		{
			if (Monitors.Count <= 1)
			{
				return "0,0,0,0";
			}
			return "1,1,1,1";
		}
	}

	public DisplayModeProfile(List<MonitorSetting> monitors, NameValidatorDelegate nameValidator, string name = "", bool defaultMode = false)
	{
		_nameValidator = nameValidator;
		_name = name;
		Default = defaultMode;
		_selectItem = new DelegateCommand(() =>
		{
			Selected = true;
		});
		Monitors = new ObservableCollection<MonitorDisplayModeProfile>();
		foreach (MonitorSetting monitor in monitors)
		{
			Monitors.Add(new MonitorDisplayModeProfile(monitor));
		}
	}

	[JsonConstructor]
	public DisplayModeProfile(bool selected, string name, bool Default, ObservableCollection<MonitorDisplayModeProfile> monitors)
	{
		_selected = selected;
		_name = name;
		this.Default = Default;
		Monitors = monitors;
	}

	public DisplayModeProfile(DisplayModeProfile BaseProfile, string Name)
	{
		_nameValidator = BaseProfile._nameValidator;
		_name = Name;
		Default = false;
		_selectItem = new DelegateCommand(() =>
		{
			Selected = true;
		});
		Monitors = new ObservableCollection<MonitorDisplayModeProfile>();
		foreach (MonitorDisplayModeProfile monitor in BaseProfile.Monitors)
		{
			Monitors.Add(new MonitorDisplayModeProfile(monitor));
		}
	}

	public void UpdateMonitors(List<MonitorSetting> monitors)
	{
		if (Monitors.Count == 0)
		{
			foreach (MonitorSetting m in monitors)
			{
				Application.Current.Dispatcher.Invoke(() =>
				{
					Monitors.Add(new MonitorDisplayModeProfile(m));
				});
			}
			return;
		}
		foreach (MonitorSetting m2 in monitors)
		{
			MonitorDisplayModeProfile monitorDisplayModeProfile = Monitors.FirstOrDefault((MonitorDisplayModeProfile x) => x.MonitorName == m2.Name);
			if (monitorDisplayModeProfile != null)
			{
				monitorDisplayModeProfile.AcSettings.UpdateDisplayDelegate(m2.SetDisplaySetting);
				monitorDisplayModeProfile.DcSettings.UpdateDisplayDelegate(m2.SetDisplaySetting);
			}
		}
	}

	public void ApplySettings(bool Ac)
	{
		foreach (MonitorDisplayModeProfile monitor in Monitors)
		{
			if (Ac)
			{
				monitor.AcSettings.ApplySettings();
			}
			else
			{
				monitor.DcSettings.ApplySettings();
			}
		}
	}
}
