using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using SystemMonitorMgr.Constants;
using SystemMonitorMgr.Enums;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Interfaces;
using Infrastructure.Converters;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Utilities;
using IntelSdkClient.SystemMonitor;
using IntelSdkClient.SystemMonitorService;
using Prism.Events;
using Prism.Logging;

namespace SystemMonitorMgr.Models;

public class GenericSystemMonitor : ISystemMonitor
{
	private readonly IEventAggregator _events;

	private readonly SystemMonitorClient _monitor;

	private readonly SystemMonitorData _system;

	private readonly SystemMonitorData _battery;

	private readonly SystemMonitorData _processor;

	private readonly SystemMonitorData _integratedGpu;

	private readonly SystemMonitorData _discreteGpu;

	private readonly SystemMonitorData _memory;

	private readonly SystemMonitorData _storage0;

	private readonly SystemMonitorData _storage1;

	private readonly SystemMonitorData _storage2;

	private readonly SystemMonitorData _network;

	public ObservableCollection<SystemMonitorData> Data { get; set; }

	public ClientState Subscribed { get; set; }

	public List<FanData> FanData { get; set; }

	public string CpuName { get; set; }

	public string GpuName { get; set; }

	public GenericSystemMonitor(IEventAggregator events, ILoggerFacade logger)
	{
		GenericSystemMonitor genericSystemMonitor = this;
		_events = events;
		_events.GetEvent<SystemMonitorDataEvent>().Subscribe(PublishNewData);
		_system = new SystemMonitorData(SystemDataType.Hardware, _events.GetEvent<UsageDataEvent>());
		_battery = new SystemMonitorData(SystemDataType.Battery, _events.GetEvent<UsageDataEvent>());
		_processor = new SystemMonitorData(SystemDataType.Cpu, _events.GetEvent<UsageDataEvent>());
		_integratedGpu = new SystemMonitorData(SystemDataType.iGpu, _events.GetEvent<UsageDataEvent>());
		_discreteGpu = new SystemMonitorData(SystemDataType.dGpu, _events.GetEvent<UsageDataEvent>());
		_memory = new SystemMonitorData(SystemDataType.Memory, _events.GetEvent<UsageDataEvent>());
		_storage0 = new SystemMonitorData(SystemDataType.SSD0, _events.GetEvent<UsageDataEvent>());
		_storage1 = new SystemMonitorData(SystemDataType.SSD1, _events.GetEvent<UsageDataEvent>());
		_storage2 = new SystemMonitorData(SystemDataType.SSD2, _events.GetEvent<UsageDataEvent>());
		_network = new SystemMonitorData(SystemDataType.Network, _events.GetEvent<UsageDataEvent>());
		Data = new ObservableCollection<SystemMonitorData> { _system, _battery, _processor, _integratedGpu, _discreteGpu, _memory, _storage0, _storage1, _storage2, _network };
		_monitor = new SystemMonitorClient(_events);
		Task.Run(() =>
		{
			try
			{
				genericSystemMonitor._monitor.Client.Subscribe();
				genericSystemMonitor.Subscribed = ClientState.Success;
			}
			catch (Exception exception)
			{
				genericSystemMonitor.Subscribed = ClientState.Error;
				logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			}
			events.GetEvent<SysMonConnectionEvent>().Publish(genericSystemMonitor.Subscribed);
		});
	}

	private void PublishNewData(Tuple<FanSpeed[], IntelSdkClient.SystemMonitorService.SystemMonitorData> Data)
	{
		PublishFanData(Data.Item1);
		PopulateSystemData(Data.Item2.SystemInformation);
		PopulateBatteryData(Data.Item2.BatteryInformation);
		PopulateProcessorData(Data.Item2.ProcessorInformation);
		PopulateIntegratedGpuData(Data.Item2.IntegratedGpuInformation);
		PopulateDiscreteGpuData(Data.Item2.DiscreteGpuInformation);
		PopulateMemoryData(Data.Item2.MemoryInformation);
		PopulateStorageData(Data.Item2.StorageInformation);
		_events.GetEvent<SystemMonitorDataChangeEvent>().Publish();
	}

	private void PublishFanData(FanSpeed[] fans)
	{
		if (fans != null && fans.Length != 0)
		{
			FanData = new List<FanData>();
			if (fans != null && fans.Length == 2 && fans[0].type == FanType.CPUFan && fans[1].type == FanType.CPUFan)
			{
				fans[0].type = FanType.SystemFan1;
				fans[1].type = FanType.SystemFan2;
			}
			for (int i = 0; i < fans.Length; i++)
			{
				FanSpeed fanSpeed = fans[i];
				FanData.Add(new FanData(fanSpeed.type, (int)fanSpeed.speed));
			}
			_events.GetEvent<FanStatusEvent>().Publish(FanData);
		}
	}

	private void PopulateSystemData(Dictionary<string, string> systemInformation)
	{
		if (systemInformation != null)
		{
			_system.Data = new ObservableCollection<OrderedKeyValuePair>();
			if (IsKeyWithData(systemInformation, "ProductName"))
			{
				_system.Data.Add(GenerateLocalizedKeyValuePair(0, "SystemModel", systemInformation["ProductName"]));
			}
			if (IsKeyWithData(systemInformation, "Manufacturer"))
			{
				_system.Data.Add(GenerateLocalizedKeyValuePair(1, "SystemManufacturer", systemInformation["Manufacturer"]));
			}
			if (IsKeyWithData(systemInformation, "BiosVersion"))
			{
				_system.Data.Add(GenerateLocalizedKeyValuePair(2, "BiosVersion", systemInformation["BiosVersion"]));
			}
			if (IsKeyWithData(systemInformation, "SystemSKU"))
			{
				_system.Data.Add(GenerateLocalizedKeyValuePair(3, "SystemSku", systemInformation["SystemSKU"]));
			}
			if (IsKeyWithData(systemInformation, "ECFirmwareVersion"))
			{
				_system.Data.Add(GenerateLocalizedKeyValuePair(4, "EcVersion", systemInformation["ECFirmwareVersion"]));
			}
			if (IsKeyWithData(systemInformation, "SystemVersion"))
			{
				_system.Data.Add(GenerateLocalizedKeyValuePair(5, "SystemVersion", systemInformation["SystemVersion"]));
			}
			if (IsKeyWithData(systemInformation, "BaseBoardVersion"))
			{
				_system.Data.Add(GenerateLocalizedKeyValuePair(5, "BaseBoardVersion", systemInformation["BaseBoardVersion"]));
			}
		}
	}

	private void PopulateBatteryData(Dictionary<string, string> batteryInformation)
	{
		if (batteryInformation != null && batteryInformation.ContainsKey("EstimatedChargeRemaining"))
		{
			_events.GetEvent<BatteryStatusEvent>().Publish(int.Parse(batteryInformation["EstimatedChargeRemaining"]));
			if (batteryInformation != null && batteryInformation.ContainsKey("IsBatterySupported"))
			{
				_events.GetEvent<BatterySupportedEvent>().Publish(Convert.ToBoolean(batteryInformation["IsBatterySupported"]));
			}
		}
	}

	private void PopulateProcessorData(Dictionary<string, string> processorInformation)
	{
		if (processorInformation == null)
		{
			return;
		}
		_processor.Data = new ObservableCollection<OrderedKeyValuePair>();
		if (IsKeyWithData(processorInformation, "Name"))
		{
			_processor.Data.Add(GenerateLocalizedKeyValuePair(0, "Name", processorInformation["Name"]));
		}
		if (IsKeyWithData(processorInformation, "CurrentClockSpeed"))
		{
			string key = StringHelper.ConvToLangSpecDecStr("CurrentClockSpeed", processorInformation) + " " + Units.MEGAHERTZ;
			_processor.Data.Add(GenerateLocalizedKeyValuePair(1, "CurrentClock", key));
		}
		if (IsKeyWithData(processorInformation, "MaxClockSpeed"))
		{
			string key2 = StringHelper.ConvToLangSpecDecStr("MaxClockSpeed", processorInformation) + " " + Units.MEGAHERTZ;
			_processor.Data.Add(GenerateLocalizedKeyValuePair(2, "MaxClock", key2));
		}
		_processor.SummaryData = new ObservableCollection<OrderedKeyValuePair>();
		if (IsKeyWithData(processorInformation, "Name"))
		{
			_processor.SummaryData.Add(GenerateLocalizedKeyValuePair(10, "Cpu", processorInformation["Name"]));
			if (string.IsNullOrWhiteSpace(CpuName))
			{
				CpuName = processorInformation["Name"];
			}
		}
		if (_processor.Data.Count > 0 && processorInformation.ContainsKey("CurrentTemperature"))
		{
			UsageData usageData = GenerateUsageData(_processor.Type, processorInformation["CurrentTemperature"], processorInformation["CurrentUsage"]);
			if (usageData != null)
			{
				_events.GetEvent<UsageDataEvent>().Publish(usageData);
			}
		}
	}

	private void PopulateIntegratedGpuData(Dictionary<string, string> integratedGpuInformation)
	{
		if (integratedGpuInformation == null)
		{
			return;
		}
		_integratedGpu.Data = new ObservableCollection<OrderedKeyValuePair>();
		if (IsKeyWithData(integratedGpuInformation, "Name"))
		{
			_integratedGpu.Data.Add(GenerateLocalizedKeyValuePair(0, "Name", integratedGpuInformation["Name"]));
		}
		if (IsKeyWithData(integratedGpuInformation, "CoreClockSpeed"))
		{
			string key = integratedGpuInformation["CoreClockSpeed"] + " " + Units.MEGAHERTZ;
			_integratedGpu.Data.Add(GenerateLocalizedKeyValuePair(1, "CoreClockSpeed", key));
		}
		if (IsKeyWithData(integratedGpuInformation, "MemoryClockSpeed"))
		{
			string key2 = integratedGpuInformation["MemoryClockSpeed"] + " " + Units.MEGAHERTZ;
			_integratedGpu.Data.Add(GenerateLocalizedKeyValuePair(2, "MemoryClockSpeed", key2));
		}
		if (IsKeyWithData(integratedGpuInformation, "Memory"))
		{
			string key3 = integratedGpuInformation["Memory"] + " " + Units.GIGABYTE;
			_integratedGpu.Data.Add(GenerateLocalizedKeyValuePair(3, "MemoryKey", key3));
		}
		_integratedGpu.SummaryData = new ObservableCollection<OrderedKeyValuePair>();
		if (IsKeyWithData(integratedGpuInformation, "Name"))
		{
			_integratedGpu.SummaryData.Add(GenerateLocalizedKeyValuePair(11, "iGpu", integratedGpuInformation["Name"]));
		}
		if (_integratedGpu.Data.Count > 0)
		{
			UsageData usageData = GenerateUsageData(_integratedGpu.Type, integratedGpuInformation["Temperature"], integratedGpuInformation["Usage"]);
			if (usageData != null)
			{
				_events.GetEvent<UsageDataEvent>().Publish(usageData);
			}
		}
	}

	private void PopulateDiscreteGpuData(Dictionary<string, string> discreteGpuInformation)
	{
		if (discreteGpuInformation == null)
		{
			return;
		}
		_discreteGpu.Data = new ObservableCollection<OrderedKeyValuePair>();
		if (IsKeyWithData(discreteGpuInformation, "Name"))
		{
			_discreteGpu.Data.Add(GenerateLocalizedKeyValuePair(0, "Name", discreteGpuInformation["Name"]));
		}
		if (IsKeyWithData(discreteGpuInformation, "CoreClockSpeed"))
		{
			string key = discreteGpuInformation["CoreClockSpeed"] + " " + Units.MEGAHERTZ;
			_discreteGpu.Data.Add(GenerateLocalizedKeyValuePair(1, "CoreClockSpeed", key));
		}
		if (IsKeyWithData(discreteGpuInformation, "MemoryClockSpeed"))
		{
			string key2 = discreteGpuInformation["MemoryClockSpeed"] + " " + Units.MEGAHERTZ;
			_discreteGpu.Data.Add(GenerateLocalizedKeyValuePair(2, "MemoryClockSpeed", key2));
		}
		if (IsKeyWithData(discreteGpuInformation, "Memory"))
		{
			string key3 = discreteGpuInformation["Memory"] + " " + Units.GIGABYTE;
			_discreteGpu.Data.Add(GenerateLocalizedKeyValuePair(3, "MemoryKey", key3));
		}
		_discreteGpu.SummaryData = new ObservableCollection<OrderedKeyValuePair>();
		if (IsKeyWithData(discreteGpuInformation, "Name"))
		{
			_discreteGpu.SummaryData.Add(GenerateLocalizedKeyValuePair(12, "dGpu", discreteGpuInformation["Name"]));
			GpuName = discreteGpuInformation["Name"];
		}
		if (_discreteGpu.Data.Count > 0)
		{
			UsageData usageData = GenerateUsageData(_discreteGpu.Type, discreteGpuInformation["Temperature"], discreteGpuInformation["Usage"]);
			if (usageData != null)
			{
				_events.GetEvent<UsageDataEvent>().Publish(usageData);
			}
		}
	}

	private void PopulateMemoryData(Dictionary<string, string> memoryInformation)
	{
		if (memoryInformation == null)
		{
			return;
		}
		_memory.Data = new ObservableCollection<OrderedKeyValuePair>();
		if (IsKeyWithData(memoryInformation, "AvailableMemory"))
		{
			string key = StringHelper.ConvToLangSpecDecStr("AvailableMemory", memoryInformation) + " " + Units.GIGABYTE;
			_memory.Data.Add(GenerateLocalizedKeyValuePair(0, "AvailableMemory", key));
		}
		if (IsKeyWithData(memoryInformation, "TotalMemory"))
		{
			string key2 = StringHelper.ConvToLangSpecDecStr("TotalMemory", memoryInformation) + " " + Units.GIGABYTE;
			_memory.Data.Add(GenerateLocalizedKeyValuePair(1, "TotalMemory", key2));
		}
		if (_memory.Data.Count > 0)
		{
			UsageData usageData = GenerateUsageData(_memory.Type, null, memoryInformation["Usage"]);
			if (usageData != null)
			{
				_events.GetEvent<UsageDataEvent>().Publish(usageData);
			}
		}
	}

	private void PopulateStorageData(Dictionary<string, string>[] storageInformationList)
	{
		if (storageInformationList == null)
		{
			return;
		}
		for (int i = 0; i < storageInformationList.Length; i++)
		{
			switch (i)
			{
			case 0:
				PopulatedStorageEntry(storageInformationList[i], _storage0);
				break;
			case 1:
				PopulatedStorageEntry(storageInformationList[i], _storage1);
				break;
			case 2:
				PopulatedStorageEntry(storageInformationList[i], _storage2);
				break;
			}
		}
	}

	private void PopulatedStorageEntry(Dictionary<string, string> storageInformation, SystemMonitorData storageData)
	{
		storageData.Data = new ObservableCollection<OrderedKeyValuePair>();
		if (IsKeyWithData(storageInformation, "Name"))
		{
			storageData.Data.Add(GenerateLocalizedKeyValuePair(0, "Name", storageInformation["Name"]));
		}
		if (IsKeyWithData(storageInformation, "FreeSpace"))
		{
			string key = StringHelper.ConvToLangSpecDecStr("FreeSpace", storageInformation) + " " + Units.GIGABYTE;
			storageData.Data.Add(GenerateLocalizedKeyValuePair(1, "FreeSpace", key));
		}
		if (IsKeyWithData(storageInformation, "TotalSize"))
		{
			string key2 = StringHelper.ConvToLangSpecDecStr("TotalSize", storageInformation) + " " + Units.GIGABYTE;
			storageData.Data.Add(GenerateLocalizedKeyValuePair(2, "TotalSize", key2));
		}
		if (IsKeyWithData(storageInformation, "InterfaceType"))
		{
			storageData.Data.Add(GenerateLocalizedKeyValuePair(3, "Interface", storageInformation["InterfaceType"]));
		}
		if (storageData.Data.Count <= 0)
		{
			return;
		}
		UsageData usageData = GenerateUsageData(storageData.Type, null, storageInformation["Usage"]);
		if (usageData != null)
		{
			_events.GetEvent<UsageDataEvent>().Publish(usageData);
			if (IsKeyWithData(storageInformation, "Name") && !storageData.Gauge.Label.Contains(storageInformation["LogicalDiskName"]))
			{
				storageData.Gauge.Label = ResourceDictionaryConverter.GetString("Storage");
			}
		}
	}

	public static UsageData GenerateUsageData(SystemDataType type, string tempString, string usageString)
	{
		if (tempString == null && usageString == null)
		{
			return null;
		}
		float result = 0f;
		float result2 = 0f;
		if (tempString != null)
		{
			int num = tempString.IndexOf(" ");
			result = (float.TryParse((num > 0) ? tempString.Substring(0, num) : tempString, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ? result : (-1f));
		}
		if (usageString != null)
		{
			int num = usageString.IndexOf(" ");
			if (!float.TryParse((num > 0) ? usageString.Substring(0, num) : usageString, NumberStyles.Float, CultureInfo.InvariantCulture, out result2))
			{
				return null;
			}
		}
		if (tempString == null)
		{
			return new UsageData(type, null, (int)Math.Round(result2));
		}
		return new UsageData(type, (int)Math.Round(result), (int)Math.Round(result2));
	}

	private bool IsKeyWithData(Dictionary<string, string> dict, string key)
	{
		if (dict.ContainsKey(key))
		{
			return !string.IsNullOrEmpty(dict[key]);
		}
		return false;
	}

	private OrderedKeyValuePair GenerateLocalizedKeyValuePair(int Index, string Label, string Key)
	{
		return new OrderedKeyValuePair(Index, ResourceDictionaryConverter.GetString(Label), Key);
	}
}
