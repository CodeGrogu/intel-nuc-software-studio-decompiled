using System.Collections.ObjectModel;
using Infrastructure.Converters;
using PeripheralSettingsMgr.Interfaces;
using Prism.Events;

namespace PeripheralSettingsMgr.Models;

public class ScheduledSettingsManager : IScheduledSettingsManager
{
	private const SleepWakeType _scheduledWake = SleepWakeType.Wake;

	private const SleepWakeType _scheduledSleep = SleepWakeType.Sleep;

	public ObservableCollection<ScheduledSettingsData> ScheduledSettings { get; set; }

	public ScheduledSettingsManager(IEventAggregator events)
	{
		ScheduledSettings = new ObservableCollection<ScheduledSettingsData>
		{
			new ScheduledSettingsData(events, SleepWakeType.Sleep, ResourceDictionaryConverter.GetString("ScheduledSleepString"), on: false),
			new ScheduledSettingsData(events, SleepWakeType.Wake, ResourceDictionaryConverter.GetString("ScheduledWakeString"), on: false)
		};
	}
}
