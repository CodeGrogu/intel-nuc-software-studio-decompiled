using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using Prism.Events;
using Prism.Mvvm;

namespace PeripheralSettingsMgr.Models;

public class ScheduledSettingsData : BindableBase
{
	private const string POWERSHELL = "powershell.exe";

	private const string SLEEP_ARGUMENTS = "-command Add-Type -AssemblyName System.Windows.Forms; $PowerState = [System.Windows.Forms.PowerState]::Suspend; $Force = $false; $DisableWake = $false; [System.Windows.Forms.Application]::SetSuspendState($PowerState, $Force, $DisableWake);";

	private const string WAKE_ARGUMENTS = "/c \"exit\"";

	private const string CMD = "cmd.exe";

	private const string USER_ID = "SYSTEM";

	private const string WAKE = "Wake";

	private const string SLEEP = "Sleep";

	private const string WAKE_DESCRIPTION = "Will wake at user specified time if device is asleep";

	private const string SLEEP_DESCRIPTION = "Will sleep at user specified time if device is not in use";

	private const string NSS_TASK_MANAGER = "NSSTaskManager";

	private const string NSS_TASK_MANAGER_DESCRIPTION = "Ensures deletion of NSS tasks in case of uninstall";

	private static readonly string LOG_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

	private static readonly string TASK_MGR_ARG_1 = "/c \"where /R \"" + LOG_PATH + "\" \"NssLog.txt\" && exit || SCHTASKS /DELETE /TN Sleep /F\"";

	private static readonly string TASK_MGR_ARG_2 = "/c \"where /R \"" + LOG_PATH + "\" \"NssLog.txt\" && exit || SCHTASKS /DELETE /TN Wake /F\"";

	private static readonly string TASK_MGR_ARG_3 = "/c \"where /R \"" + LOG_PATH + "\" \"NssLog.txt\" && exit || SCHTASKS /DELETE /TN NSSTaskManager /F\"";

	private readonly string _taskName;

	private readonly string _taskDescription;

	private RepeatType _selectedRepeat;

	private SleepWakeType _sleepWake;

	private string _selectedHour;

	private string _selectedMinute;

	private bool _on;

	private string _wakeSleepStr;

	private DateTime _selectedDateTime;

	private const RepeatType DEFAULT_REPEAT = RepeatType.None;

	private readonly TaskDefinition _scheduledSetting;

	private readonly TaskService _taskService;

	private int _sleepInactivity;

	private bool _sleep;

	public RepeatType SelectedRepeat
	{
		get
		{
			return _selectedRepeat;
		}
		set
		{
			SetProperty(ref _selectedRepeat, value, "SelectedRepeat");
			scheduleTask();
		}
	}

	public SleepWakeType SleepWake
	{
		get
		{
			return _sleepWake;
		}
		set
		{
			SetProperty(ref _sleepWake, value, "SleepWake");
		}
	}

	public string SelectedHour
	{
		get
		{
			return _selectedHour;
		}
		set
		{
			SetProperty(ref _selectedHour, value, "SelectedHour");
			UpdateDateTime();
		}
	}

	public string SelectedMinute
	{
		get
		{
			return _selectedMinute;
		}
		set
		{
			SetProperty(ref _selectedMinute, value, "SelectedMinute");
			UpdateDateTime();
		}
	}

	public DateTime SelectedDateTime
	{
		get
		{
			return _selectedDateTime;
		}
		set
		{
			SetProperty(ref _selectedDateTime, value, "SelectedDateTime");
			scheduleTask();
		}
	}

	public bool On
	{
		get
		{
			return _on;
		}
		set
		{
			SetProperty(ref _on, value, "On");
			if (On)
			{
				scheduleTask();
			}
			else
			{
				DeleteTask();
			}
		}
	}

	public string WakeSleepStr
	{
		get
		{
			return _wakeSleepStr;
		}
		set
		{
			SetProperty(ref _wakeSleepStr, value, "WakeSleepStr");
		}
	}

	public ObservableCollection<RepeatType> RepeatTypes { get; } = new ObservableCollection<RepeatType>
	{
		RepeatType.None,
		RepeatType.Daily,
		RepeatType.Weekly,
		RepeatType.Monthly
	};

	public ObservableCollection<string> Hours { get; } = new ObservableCollection<string>
	{
		"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
		"20", "21", "22", "23"
	};

	public ObservableCollection<string> Minutes { get; } = new ObservableCollection<string>
	{
		"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
		"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
		"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
		"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
		"50", "51", "52", "53", "54", "55", "56", "57", "58", "59"
	};

	public int SleepInactivity
	{
		get
		{
			return _sleepInactivity;
		}
		set
		{
			SetProperty(ref _sleepInactivity, value, "SleepInactivity");
			scheduleTask();
		}
	}

	public bool Sleep
	{
		get
		{
			return _sleep;
		}
		set
		{
			SetProperty(ref _sleep, value, "Sleep");
		}
	}

	public ScheduledSettingsData(IEventAggregator events, SleepWakeType sleepWakeType, string label, bool on)
	{
		_sleepWake = sleepWakeType;
		if (_sleepWake == SleepWakeType.Sleep)
		{
			_taskName = "Sleep";
			_taskDescription = "Will sleep at user specified time if device is not in use";
			_sleep = true;
		}
		if (_sleepWake == SleepWakeType.Wake)
		{
			_taskName = "Wake";
			_taskDescription = "Will wake at user specified time if device is asleep";
			_sleep = false;
		}
		_wakeSleepStr = label;
		_taskService = new TaskService();
		if (_taskService.RootFolder.Tasks.Any((Task task2) => task2.Name == _taskName))
		{
			Task task = _taskService.FindTask(_taskName);
			_scheduledSetting = task.Definition;
			SetUI();
		}
		else
		{
			_scheduledSetting = _taskService.NewTask();
			_scheduledSetting.RegistrationInfo.Description = _taskDescription;
			_scheduledSetting.Principal.LogonType = TaskLogonType.InteractiveToken;
			_selectedDateTime = DateTime.Today;
			_selectedHour = "00";
			_selectedMinute = "00";
			_selectedRepeat = RepeatType.None;
			_on = on;
		}
		CreateNSSTaskMgr();
	}

	private void CreateNSSTaskMgr()
	{
		if (!_taskService.RootFolder.Tasks.Any((Task task) => task.Name == "NSSTaskManager"))
		{
			TaskDefinition taskDefinition = _taskService.NewTask();
			taskDefinition.RegistrationInfo.Description = "Ensures deletion of NSS tasks in case of uninstall";
			taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;
			taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
			taskDefinition.Settings.StartWhenAvailable = true;
			DailyTrigger unboundTrigger = new DailyTrigger(1)
			{
				StartBoundary = DateTime.Now
			};
			taskDefinition.Triggers.Add(unboundTrigger);
			taskDefinition.Actions.Add(new ExecAction("cmd.exe", TASK_MGR_ARG_1));
			taskDefinition.Actions.Add(new ExecAction("cmd.exe", TASK_MGR_ARG_2));
			taskDefinition.Actions.Add(new ExecAction("cmd.exe", TASK_MGR_ARG_3));
			_taskService.RootFolder.RegisterTaskDefinition("NSSTaskManager", taskDefinition, TaskCreation.CreateOrUpdate, "SYSTEM", null, TaskLogonType.ServiceAccount);
		}
	}

	private void scheduleTask()
	{
		DeleteTask();
		CreateTrigger();
		CreateTask();
	}

	private void SetUI()
	{
		Trigger trigger = _scheduledSetting.Triggers.FirstOrDefault();
		SelectedDateTime = trigger.StartBoundary;
		string text = SelectedDateTime.Hour.ToString();
		if (text.Length == 1)
		{
			text = "0" + text;
		}
		string text2 = SelectedDateTime.Minute.ToString();
		if (text2.Length == 1)
		{
			text2 = "0" + text2;
		}
		SelectedHour = text;
		SelectedMinute = text2;
		switch (trigger.TriggerType)
		{
		case TaskTriggerType.Time:
			SelectedRepeat = RepeatType.None;
			break;
		case TaskTriggerType.Daily:
			SelectedRepeat = RepeatType.Daily;
			break;
		case TaskTriggerType.Weekly:
			SelectedRepeat = RepeatType.Weekly;
			break;
		case TaskTriggerType.Monthly:
			SelectedRepeat = RepeatType.Monthly;
			break;
		}
		On = true;
	}

	private DaysOfTheWeek getDaysOfTheWeek()
	{
		return SelectedDateTime.DayOfWeek switch
		{
			DayOfWeek.Monday => DaysOfTheWeek.Monday, 
			DayOfWeek.Tuesday => DaysOfTheWeek.Tuesday, 
			DayOfWeek.Wednesday => DaysOfTheWeek.Wednesday, 
			DayOfWeek.Thursday => DaysOfTheWeek.Thursday, 
			DayOfWeek.Friday => DaysOfTheWeek.Friday, 
			DayOfWeek.Saturday => DaysOfTheWeek.Saturday, 
			_ => DaysOfTheWeek.Sunday, 
		};
	}

	private void UpdateDateTime()
	{
		TimeSpan timeSpan = new TimeSpan(Convert.ToInt16(SelectedHour), Convert.ToInt16(SelectedMinute), 0);
		SelectedDateTime = SelectedDateTime.Date + timeSpan;
	}

	private void CreateTrigger()
	{
		if (_scheduledSetting.Triggers.Count == 1)
		{
			_scheduledSetting.Triggers.RemoveAt(0);
		}
		switch (SelectedRepeat)
		{
		case RepeatType.None:
		{
			TimeTrigger unboundTrigger4 = new TimeTrigger
			{
				StartBoundary = SelectedDateTime
			};
			_scheduledSetting.Triggers.Add(unboundTrigger4);
			break;
		}
		case RepeatType.Daily:
		{
			DailyTrigger unboundTrigger3 = new DailyTrigger(1)
			{
				StartBoundary = SelectedDateTime
			};
			_scheduledSetting.Triggers.Add(unboundTrigger3);
			break;
		}
		case RepeatType.Weekly:
		{
			WeeklyTrigger unboundTrigger2 = new WeeklyTrigger(getDaysOfTheWeek(), 1)
			{
				StartBoundary = SelectedDateTime
			};
			_scheduledSetting.Triggers.Add(unboundTrigger2);
			break;
		}
		case RepeatType.Monthly:
		{
			MonthlyTrigger unboundTrigger = new MonthlyTrigger(SelectedDateTime.Day)
			{
				StartBoundary = SelectedDateTime
			};
			_scheduledSetting.Triggers.Add(unboundTrigger);
			break;
		}
		}
	}

	private void CreateTask()
	{
		string path;
		string arguments;
		switch (SleepWake)
		{
		default:
			return;
		case SleepWakeType.Sleep:
			path = "powershell.exe";
			arguments = "-command Add-Type -AssemblyName System.Windows.Forms; $PowerState = [System.Windows.Forms.PowerState]::Suspend; $Force = $false; $DisableWake = $false; [System.Windows.Forms.Application]::SetSuspendState($PowerState, $Force, $DisableWake);";
			if (_sleepInactivity > 0)
			{
				_scheduledSetting.Settings.IdleSettings.IdleDuration = new TimeSpan(0, _sleepInactivity, 0);
				_scheduledSetting.Settings.RunOnlyIfIdle = true;
			}
			else
			{
				_scheduledSetting.Settings.RunOnlyIfIdle = false;
			}
			break;
		case SleepWakeType.Wake:
			path = "cmd.exe";
			arguments = "/c \"exit\"";
			_scheduledSetting.Principal.RunLevel = TaskRunLevel.Highest;
			_scheduledSetting.Principal.UserId = "SYSTEM";
			_scheduledSetting.Settings.StopIfGoingOnBatteries = false;
			_scheduledSetting.Settings.WakeToRun = true;
			_scheduledSetting.Settings.AllowDemandStart = true;
			break;
		}
		if (_scheduledSetting.Actions.Count == 1)
		{
			_scheduledSetting.Actions.RemoveAt(0);
		}
		_scheduledSetting.Actions.Add(new ExecAction(path, arguments));
		_taskService.RootFolder.RegisterTaskDefinition(_taskName, _scheduledSetting, TaskCreation.CreateOrUpdate, "SYSTEM", null, TaskLogonType.ServiceAccount);
	}

	private void DeleteTask()
	{
		if (_taskName != null && _taskService.FindTask(_taskName) != null)
		{
			_taskService.RootFolder.DeleteTask(_taskName);
		}
	}
}
