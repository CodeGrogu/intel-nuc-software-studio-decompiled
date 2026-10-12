using System.Collections.ObjectModel;
using PeripheralSettingsMgr.Models;

namespace PeripheralSettingsMgr.Interfaces;

public interface IScheduledSettingsManager
{
	ObservableCollection<ScheduledSettingsData> ScheduledSettings { get; set; }
}
