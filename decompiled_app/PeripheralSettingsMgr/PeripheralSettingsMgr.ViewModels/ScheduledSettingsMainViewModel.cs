using System.Collections.ObjectModel;
using PeripheralSettingsMgr.Interfaces;
using PeripheralSettingsMgr.Models;

namespace PeripheralSettingsMgr.ViewModels;

public class ScheduledSettingsMainViewModel
{
	private readonly IScheduledSettingsManager _settingsMgr;

	public ObservableCollection<ScheduledSettingsData> ScheduledSettings { get; set; }

	public ScheduledSettingsMainViewModel(IScheduledSettingsManager settingsMgr)
	{
		_settingsMgr = settingsMgr;
		ScheduledSettings = _settingsMgr.ScheduledSettings;
	}
}
