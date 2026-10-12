using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using SystemMonitorLibrary.Hardware;
using SystemMonitorLibrary.Hardware.Battery;
using SystemMonitorLibrary.Hardware.GPU;
using SystemMonitorLibrary.Hardware.Memory;
using SystemMonitorLibrary.Hardware.Processor;
using SystemMonitorLibrary.Hardware.Storage;
using SystemMonitorLibrary.Hardware.SystemInformation;
using Infrastructure.Logging;
using OpenHardwareMonitor.Hardware;
using PerformanceTuningLibrary;

namespace SystemMonitorLibrary.Client;

public class SystemMonitorLibraryClient
{
	public delegate void SystemMonitorDataCallback(List<FanSpeed> FanData, SystemMonitorData SystemData);

	private SystemInformation _system;

	private Battery _battery;

	private Processor _processor;

	private Gpu _gpu;

	private Memory _memory;

	private Storage _storage;

	private int iGPUCounter;

	private int dGPUCounter;

	private Dictionary<string, string> _systemInformation;

	private Dictionary<string, string> _batteryInformation;

	private Dictionary<string, string> _processorInformation;

	private Dictionary<string, string> _integratedGpuInformation;

	private Dictionary<string, string> _discreteGpuInformation;

	private Dictionary<string, string> _memoryInformation;

	private List<Dictionary<string, string>> _storageInformation;

	private readonly NssServiceLogger _logger;

	private Timer _timer;

	private int _timerInterval;

	private static readonly int DEFAULT_MONITOR_UPDATE_INTERVAL = (int)TimeSpan.FromSeconds(2.0).TotalMilliseconds;

	public SystemMonitorDataCallback UpdateDataCallback;

	private const string SYSTEM_MONITOR_INTERVAL_CONFIG_KEY = "SystemMonitorInterval";

	public SystemMonitorLibraryClient(NssServiceLogger logger)
	{
		_logger = logger;
		Computer computer = new Computer
		{
			GPUEnabled = true,
			CPUEnabled = true,
			RAMEnabled = true,
			HDDEnabled = true,
			MainboardEnabled = true,
			FanControllerEnabled = true
		};
		computer.Open();
		Task.Run(() =>
		{
			IHardware[] hardware = computer.Hardware;
			for (int i = 0; i < hardware.Length; i++)
			{
				hardware[i].Update();
			}
			_system = new SystemInformation();
			_battery = new Battery();
			_processor = new Processor(computer);
			_gpu = new Gpu(computer);
			_memory = new Memory(computer);
			_storage = new Storage();
			int result = -1;
			if (ConfigurationManager.AppSettings["SystemMonitorInterval"] != null)
			{
				int.TryParse(ConfigurationManager.AppSettings["SystemMonitorInterval"], out result);
			}
			_timerInterval = ((result > 0) ? ((int)TimeSpan.FromSeconds(result).TotalMilliseconds) : DEFAULT_MONITOR_UPDATE_INTERVAL);
		}).ContinueWith(async (Task _) => await GetDataAsync().ConfigureAwait(continueOnCapturedContext: false));
	}

	public void Subscribe(bool Subscribe, SystemMonitorDataCallback NewCallback = null)
	{
		if (Subscribe)
		{
			UpdateDataCallback = NewCallback;
			SetTimer();
		}
		else
		{
			UpdateDataCallback = null;
			_timer.Enabled = false;
		}
	}

	private void SetTimer()
	{
		if (_timer == null)
		{
			_timer = new Timer
			{
				Interval = _timerInterval,
				AutoReset = true
			};
			_timer.Elapsed += MonitorTimer_Tick;
		}
		_timer.Enabled = true;
	}

	private void MonitorTimer_Tick(object sender, EventArgs e)
	{
		Task.Run(async () =>
		{
			SystemMonitorData systemData = await GetDataAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<FanSpeed> fanData = GetFanData();
			UpdateDataCallback?.Invoke(fanData, systemData);
		});
	}

	private List<FanSpeed> GetFanData()
	{
		return FanControl.GetFanSpeed();
	}

	private async Task<SystemMonitorData> GetDataAsync()
	{
		Task<Dictionary<string, string>> systemInformationTask = GetSystemInformationAsync();
		Task<Dictionary<string, string>> batteryInformationTask = GetBatteryInformationAsync();
		Task<Dictionary<string, string>> processorInformationTask = GetProcessorInformationAsync();
		Task<Dictionary<string, string>> integratedGpuInformationTask = GetIntegratedGpuInformationAsync();
		Task<Dictionary<string, string>> discreteGpuInformationTask = GetDiscreteGpuInformationAsync();
		Task<Dictionary<string, string>> memoryInformationTask = GetMemoryInformationAsync();
		Task<List<Dictionary<string, string>>> storageInformationTask = GetStorageInformationAsync();
		List<Task> tasks = new List<Task> { systemInformationTask, batteryInformationTask, processorInformationTask, integratedGpuInformationTask, discreteGpuInformationTask, memoryInformationTask, storageInformationTask };
		SystemMonitorData Data = new SystemMonitorData();
		while (tasks.Count > 0)
		{
			Task task = await Task.WhenAny(tasks).ConfigureAwait(continueOnCapturedContext: false);
			if (task == systemInformationTask && systemInformationTask.Result != null)
			{
				Data.SystemInformation = systemInformationTask.Result;
			}
			else if (task == batteryInformationTask && batteryInformationTask.Result != null)
			{
				Data.BatteryInformation = batteryInformationTask.Result;
			}
			else if (task == processorInformationTask && processorInformationTask.Result != null)
			{
				Data.ProcessorInformation = processorInformationTask.Result;
			}
			else if (task == integratedGpuInformationTask && integratedGpuInformationTask.Result != null)
			{
				Data.IntegratedGpuInformation = integratedGpuInformationTask.Result;
			}
			else if (task == discreteGpuInformationTask && discreteGpuInformationTask.Result != null)
			{
				Data.DiscreteGpuInformation = discreteGpuInformationTask.Result;
			}
			else if (task == memoryInformationTask && memoryInformationTask.Result != null)
			{
				Data.MemoryInformation = memoryInformationTask.Result;
			}
			else if (task == storageInformationTask && storageInformationTask.Result != null)
			{
				Data.StorageInformation = new List<Dictionary<string, string>>();
				foreach (Dictionary<string, string> item in storageInformationTask.Result)
				{
					Data.StorageInformation.Add(item);
				}
			}
			tasks.Remove(task);
		}
		return Data;
	}

	public async Task<Dictionary<string, string>> GetSystemInformationAsync()
	{
		return await Task.Run(() =>
		{
			try
			{
				_systemInformation = _system.GetSystemInformation();
				return _systemInformation;
			}
			catch (Exception ex)
			{
				_logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
				return _systemInformation;
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<Dictionary<string, string>> GetBatteryInformationAsync()
	{
		return await Task.Run(() =>
		{
			try
			{
				_batteryInformation = _battery.GetBatteryInformation();
				return _batteryInformation;
			}
			catch (Exception ex)
			{
				_logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
				return _batteryInformation;
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<Dictionary<string, string>> GetProcessorInformationAsync()
	{
		return await Task.Run(() =>
		{
			try
			{
				_processorInformation = _processor.GetProcessorInformation();
				return _processorInformation;
			}
			catch (Exception ex)
			{
				_logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
				return _processorInformation;
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<Dictionary<string, string>> GetIntegratedGpuInformationAsync()
	{
		return await Task.Run(() =>
		{
			try
			{
				if (iGPUCounter < 3)
				{
					_integratedGpuInformation = _gpu.GetIntegratedGpuInformation();
				}
				if (_integratedGpuInformation != null)
				{
					iGPUCounter = 0;
				}
				return _integratedGpuInformation;
			}
			catch (Exception ex)
			{
				_logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
				iGPUCounter++;
				return _integratedGpuInformation;
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<Dictionary<string, string>> GetDiscreteGpuInformationAsync()
	{
		return await Task.Run(() =>
		{
			try
			{
				if (dGPUCounter < 3)
				{
					_discreteGpuInformation = _gpu.GetDiscreteGpuInformation();
				}
				if (_discreteGpuInformation != null)
				{
					dGPUCounter = 0;
				}
				return _discreteGpuInformation;
			}
			catch (Exception ex)
			{
				_logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
				dGPUCounter++;
				return _discreteGpuInformation;
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<Dictionary<string, string>> GetMemoryInformationAsync()
	{
		return await Task.Run(() =>
		{
			try
			{
				_memoryInformation = _memory.GetMemoryInformation();
				return _memoryInformation;
			}
			catch (Exception ex)
			{
				_logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
				return _memoryInformation;
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<List<Dictionary<string, string>>> GetStorageInformationAsync()
	{
		return await Task.Run(() =>
		{
			try
			{
				_storageInformation = _storage.GetStorageInformation();
				return _storageInformation;
			}
			catch (Exception ex)
			{
				_logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
				return _storageInformation;
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}
}
