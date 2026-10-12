using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using SystemMonitorMgr.Enums;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Interfaces;
using Infrastructure.Converters;
using Infrastructure.Events;
using Infrastructure.Models;
using IntelSdkClient.SystemMonitorService;
using Prism.Events;
using Prism.Logging;
using UniwillSdkClient.Events;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace SystemMonitorMgr.Models;

public class UniwillSystemMonitor : ISystemMonitor
{
	private readonly UniwillServiceClient _client;

	private readonly ILoggerFacade _logger;

	private readonly IEventAggregator _events;

	private static readonly string PERCENT_KEY = ResourceDictionaryConverter.GetString("PercentKey");

	private const string KEY = "Key";

	private const string DECIMAL_SEPERATOR = ".";

	private const string USAGE_KEY = "Usage";

	private const string TEMPERATURE_KEY = "Temperature";

	private const string KBPS_KEY = "Kbps";

	private const string GB_KEY = "GB";

	private const string MHZ_KEY = "Mhz";

	private const string KB_KEY = "KB";

	private const string MWH_KEY = "mWh";

	private const string DESIGN_VOLT_KEY = "DesignVoltage";

	private const string ACTUAL_VOLT_KEY = "Voltage";

	private const string INDEX_KEY = "Index";

	private const string NAME_KEY = "Name";

	private const string TIME_REMAIN_KEY = "TimeRemaining";

	private const string ABNORMAL_KEY = "Abnormal";

	private const string CURRENT_KEY = "Current";

	private const string MINUTES_KEY = "Minutes";

	private const string HOURS_KEY = "Hours";

	private const string VOLTS_KEY = "Volts";

	private const string AMPS_KEY = "Amps";

	private const string MWH_UNIT_KEY = "MwhUnitKey";

	private const string PERC_KEY = "PercentKey";

	private const string KBS_UNIT_KEY = "KbpsUnitKey";

	private const string GB_UNIT_KEY = "GbUnitKey";

	private const string MGHZ_KEY = "Megahertz";

	private const string NORM_STR_KEY = "NormalString";

	private const string ABNORMAL_STR_KEY = "AbnormalString";

	private const string CHARGE_STR_KEY = "ChargingString";

	private const string CPU_FAN_SPEED_KEY = "CpuFanSpeed";

	private const string GPU_FAN_SPEED_KEY = "GpuFanSpeed";

	private const string IGPU_TYPE_KEY = "iGpuType";

	private const string DGPU_TYPE_KEY = "dGpuType";

	private const string CPU_TYPE_KEY = "ProcessorType";

	private const string L2_CACHE_SIZE_KEY = "ProcessorL2CacheSize";

	private const string DECIMAL_SEPERATOR_KEY = "DecimalSeperator";

	private OrderedKeyValuePair _igpuType;

	private OrderedKeyValuePair _dgpuType;

	private OrderedKeyValuePair _cpuType;

	private OrderedKeyValuePair _processorL2CacheSize;

	private readonly SystemMonitorData _battery;

	private readonly SystemMonitorData _hardwareFuelGauge;

	private readonly SystemMonitorData _cpu;

	private readonly SystemMonitorData _dgpu;

	private readonly SystemMonitorData _fan;

	private readonly SystemMonitorData _igpu;

	private readonly SystemMonitorData _memory;

	private readonly SystemMonitorData _network;

	private readonly SystemMonitorData _storageZero;

	private readonly SystemMonitorData _storageOne;

	private readonly SystemMonitorData _storageTwo;

	private readonly SystemMonitorData _storageThree;

	private bool _isSystemInfoInitialized;

	private readonly SystemMonitorData _systemInfo;

	private readonly Dictionary<SystemDataType, string> SystemInformation;

	public List<FanData> FanData { get; set; }

	public string CpuName { get; set; }

	public string GpuName { get; set; }

	public ObservableCollection<SystemMonitorData> Data { get; set; }

	public ClientState Subscribed { get; set; }

	public UniwillSystemMonitor(ILoggerFacade logger, IEventAggregator events, IUniwillClient client)
	{
		Data = new ObservableCollection<SystemMonitorData>();
		_logger = logger;
		_events = events;
		events.GetEvent<MobileSystemDataChangeEvent>().Subscribe(UpdateData);
		try
		{
			_client = client.Client;
			if (_client == null)
			{
				throw new ArgumentNullException("Unable to connect to SDK");
			}
			_battery = new SystemMonitorData(SystemDataType.Battery, events.GetEvent<UsageDataEvent>());
			_hardwareFuelGauge = new SystemMonitorData(SystemDataType.Battery, events.GetEvent<UsageDataEvent>());
			_cpu = new SystemMonitorData(SystemDataType.Cpu, events.GetEvent<UsageDataEvent>());
			_dgpu = new SystemMonitorData(SystemDataType.dGpu, events.GetEvent<UsageDataEvent>());
			_fan = new SystemMonitorData(SystemDataType.Fan, events.GetEvent<UsageDataEvent>());
			_igpu = new SystemMonitorData(SystemDataType.iGpu, events.GetEvent<UsageDataEvent>());
			_memory = new SystemMonitorData(SystemDataType.Memory, events.GetEvent<UsageDataEvent>());
			_network = new SystemMonitorData(SystemDataType.Network, events.GetEvent<UsageDataEvent>());
			_storageZero = new SystemMonitorData(SystemDataType.SSD0, events.GetEvent<UsageDataEvent>());
			_storageOne = new SystemMonitorData(SystemDataType.SSD1, events.GetEvent<UsageDataEvent>());
			_storageTwo = new SystemMonitorData(SystemDataType.SSD2, events.GetEvent<UsageDataEvent>());
			_storageThree = new SystemMonitorData(SystemDataType.SSD3, events.GetEvent<UsageDataEvent>());
			_systemInfo = new SystemMonitorData(SystemDataType.Hardware, events.GetEvent<UsageDataEvent>());
			SystemInformation = new Dictionary<SystemDataType, string>();
			Data = new ObservableCollection<SystemMonitorData>
			{
				_systemInfo, _hardwareFuelGauge, _fan, _cpu, _memory, _igpu, _dgpu, _network, _storageZero, _storageOne,
				_storageTwo, _storageThree, _battery
			};
			_events.GetEvent<SubscribeEvent>().Publish(payload: true);
			Subscribed = ClientState.Success;
		}
		catch (Exception exception)
		{
			Subscribed = ClientState.Error;
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
		events.GetEvent<SysMonConnectionEvent>().Publish(Subscribed);
	}

	private ObservableCollection<OrderedKeyValuePair> UpdateBatteryData(UniwillSdkClient.UniwillService.SystemMonitorData BatteryData)
	{
		ObservableCollection<OrderedKeyValuePair> observableCollection = new ObservableCollection<OrderedKeyValuePair>();
		foreach (KeyValuePair<string, string> datum in BatteryData.Data)
		{
			string text = datum.Value;
			if (text.Contains("."))
			{
				text = text.Replace(".", ResourceDictionaryConverter.GetString("DecimalSeperator"));
			}
			if (datum.Key == "TimeRemaining")
			{
				if (text == "-1")
				{
					observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), ResourceDictionaryConverter.GetString("ChargingString")));
				}
				else
				{
					observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), GetTimeString(text)));
				}
			}
			else if (datum.Key == "Abnormal")
			{
				if (text == "0")
				{
					observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), ResourceDictionaryConverter.GetString("NormalString")));
				}
				else
				{
					observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), ResourceDictionaryConverter.GetString("AbnormalString")));
				}
			}
			else if (datum.Key == "DesignVoltage")
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text + " " + ResourceDictionaryConverter.GetString("Volts")));
			}
			else if (datum.Key == "Voltage")
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text + " " + ResourceDictionaryConverter.GetString("Volts")));
			}
			else if (datum.Key == "Current")
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text + " " + ResourceDictionaryConverter.GetString("Amps")));
			}
			else if (text.Contains("mWh"))
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text.Replace("mWh", ResourceDictionaryConverter.GetString("MwhUnitKey"))));
			}
			else
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text));
			}
		}
		if (BatteryData.DataType == SysMonType.Battery)
		{
			OrderedKeyValuePair orderedKeyValuePair = observableCollection.FirstOrDefault((OrderedKeyValuePair x) => x.Key.Equals(PERCENT_KEY) || x.Key.Equals(ResourceDictionaryConverter.GetString(PERCENT_KEY)));
			if (orderedKeyValuePair != null)
			{
				_events.GetEvent<BatteryStatusEvent>().Publish(int.Parse(orderedKeyValuePair.Value));
				UsageData usageData = GenericSystemMonitor.GenerateUsageData(SystemDataType.Battery, null, orderedKeyValuePair.Value);
				if (usageData != null)
				{
					_events.GetEvent<UsageDataEvent>().Publish(usageData);
					_events.GetEvent<BatterySupportedEvent>().Publish(payload: true);
				}
			}
			observableCollection.Remove(orderedKeyValuePair);
		}
		return observableCollection;
	}

	private string GetTimeString(string minutesString)
	{
		int num = int.Parse(minutesString);
		if (num >= 60)
		{
			return num / 60 + " " + ResourceDictionaryConverter.GetString("Hours") + " " + num % 60 + " " + ResourceDictionaryConverter.GetString("Minutes");
		}
		return num + " " + ResourceDictionaryConverter.GetString("Minutes");
	}

	private ObservableCollection<OrderedKeyValuePair> UpdateDataWithGauge(SystemDataType Type, UniwillSdkClient.UniwillService.SystemMonitorData Data, ObservableCollection<OrderedKeyValuePair> AdditionalData = null)
	{
		if (Data.Data.TryGetValue("Usage", out var value))
		{
			Data.Data.Remove("Usage");
		}
		if (Data.Data.TryGetValue("Temperature", out var value2))
		{
			Data.Data.Remove("Temperature");
		}
		if (Data.Data.TryGetValue("Index", out var _))
		{
			Data.Data.Remove("Index");
		}
		UsageData usageData = GenericSystemMonitor.GenerateUsageData(Type, value2, value);
		if (usageData != null)
		{
			_events.GetEvent<UsageDataEvent>().Publish(usageData);
		}
		ObservableCollection<OrderedKeyValuePair> observableCollection = new ObservableCollection<OrderedKeyValuePair>();
		foreach (KeyValuePair<string, string> datum in Data.Data)
		{
			string text = datum.Value;
			if (text.Contains("."))
			{
				text = text.Replace(".", ResourceDictionaryConverter.GetString("DecimalSeperator"));
			}
			if (text.Contains("Kbps"))
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text.Replace("Kbps", ResourceDictionaryConverter.GetString("KbpsUnitKey"))));
			}
			else if (text.Contains("GB"))
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text.Replace("GB", ResourceDictionaryConverter.GetString("GbUnitKey"))));
			}
			else if (text.Contains("Mhz"))
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text.Replace("Mhz", ResourceDictionaryConverter.GetString("Megahertz"))));
			}
			else if (text.Contains("MHz"))
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text.Replace("MHz", ResourceDictionaryConverter.GetString("Megahertz"))));
			}
			else
			{
				observableCollection.Add(new OrderedKeyValuePair(ResourceDictionaryConverter.GetString(datum.Key + "Key"), text));
			}
		}
		if (AdditionalData != null)
		{
			observableCollection = (ObservableCollection<OrderedKeyValuePair>)observableCollection.AddRange(AdditionalData);
		}
		return observableCollection;
	}

	private void UpdateData(UniwillSdkClient.UniwillService.SystemMonitorData data)
	{
		if (data.Data == null || data.Data.Count == 0)
		{
			return;
		}
		switch (data.DataType)
		{
		case SysMonType.HwFuelGauge:
			_hardwareFuelGauge.Data = UpdateBatteryData(data);
			break;
		case SysMonType.Battery:
			_battery.Data = UpdateBatteryData(data);
			break;
		case SysMonType.Cpu:
			_cpu.Data = UpdateDataWithGauge(SystemDataType.Cpu, data, new ObservableCollection<OrderedKeyValuePair> { _cpuType, _processorL2CacheSize });
			_cpu.Data.Move(_cpu.Data.IndexOf(_cpuType), 0);
			_cpu.Data[_cpu.Data.IndexOf(_cpuType)].Key = ResourceDictionaryConverter.GetString("Name");
			break;
		case SysMonType.dGpu:
			_dgpu.Data = UpdateDataWithGauge(SystemDataType.dGpu, data, new ObservableCollection<OrderedKeyValuePair> { _dgpuType });
			_dgpu.Data.Move(_dgpu.Data.IndexOf(_dgpuType), 0);
			_dgpu.Data[_dgpu.Data.IndexOf(_dgpuType)].Key = ResourceDictionaryConverter.GetString("Name");
			break;
		case SysMonType.dGpu_basic:
			_dgpu.Data = UpdateDataWithGauge(SystemDataType.dGpu, data, new ObservableCollection<OrderedKeyValuePair> { _dgpuType });
			_dgpu.Data.Move(_dgpu.Data.IndexOf(_dgpuType), 0);
			_dgpu.Data[_dgpu.Data.IndexOf(_dgpuType)].Key = ResourceDictionaryConverter.GetString("Name");
			break;
		case SysMonType.Fan:
			PublishFanData(data.Data);
			return;
		case SysMonType.iGpu:
			_igpu.Data = UpdateDataWithGauge(SystemDataType.iGpu, data, new ObservableCollection<OrderedKeyValuePair> { _igpuType });
			_igpu.Data.Move(_igpu.Data.IndexOf(_igpuType), 0);
			_igpu.Data[_igpu.Data.IndexOf(_igpuType)].Key = ResourceDictionaryConverter.GetString("Name");
			break;
		case SysMonType.Memory:
			_memory.Data = UpdateDataWithGauge(SystemDataType.Memory, data);
			break;
		case SysMonType.Network:
			_network.Data = UpdateDataWithGauge(SystemDataType.Network, data);
			break;
		case SysMonType.Storage:
		{
			if (data.Data.TryGetValue("Index", out var value))
			{
				if (value.Equals("0"))
				{
					_storageZero.Data = UpdateDataWithGauge(SystemDataType.SSD0, data);
				}
				else if (value.Equals("1"))
				{
					_storageOne.Data = UpdateDataWithGauge(SystemDataType.SSD1, data);
				}
				else if (value.Equals("2"))
				{
					_storageTwo.Data = UpdateDataWithGauge(SystemDataType.SSD2, data);
				}
				else if (value.Equals("3"))
				{
					_storageThree.Data = UpdateDataWithGauge(SystemDataType.SSD3, data);
				}
			}
			break;
		}
		case SysMonType.SystemInfo:
			if (!_isSystemInfoInitialized)
			{
				ParseSystemInfo(data);
				if (_igpuType.Key == "iGpuType")
				{
					LocalizeSystemInfo();
				}
				_isSystemInfoInitialized = true;
			}
			return;
		}
		_events.GetEvent<SystemMonitorDataChangeEvent>().Publish();
		_events.GetEvent<SystemComponentNamesEvent>().Publish(SystemInformation);
	}

	private void LocalizeSystemInfo()
	{
		string key = ResourceDictionaryConverter.GetString("iGpuTypeKey");
		string key2 = ResourceDictionaryConverter.GetString("dGpuTypeKey");
		string key3 = ResourceDictionaryConverter.GetString("ProcessorTypeKey");
		string key4 = ResourceDictionaryConverter.GetString("ProcessorL2CacheSizeKey");
		foreach (SystemDataType key5 in SystemInformation.Keys)
		{
			switch (key5)
			{
			case SystemDataType.iGpu:
				_igpuType = new OrderedKeyValuePair(key, _igpuType.Value);
				break;
			case SystemDataType.dGpu:
				_dgpuType = new OrderedKeyValuePair(key2, _dgpuType.Value);
				if (_dgpu.Data.Count == 0)
				{
					_dgpu.Data.Add(_dgpuType);
				}
				break;
			case SystemDataType.Cpu:
				_cpuType = new OrderedKeyValuePair(key3, _cpuType.Value);
				_processorL2CacheSize = new OrderedKeyValuePair(key4, _processorL2CacheSize.Value);
				break;
			}
		}
		foreach (OrderedKeyValuePair datum in _systemInfo.Data)
		{
			datum.Key = ResourceDictionaryConverter.GetString(datum.Key + "Key");
			if (datum.Key.EndsWith(":") || datum.Key.EndsWith(ResourceDictionaryConverter.GetString("TextSeparatorString")))
			{
				datum.Key = datum.Key.Remove(datum.Key.Length - 1, 1);
			}
		}
	}

	private void ParseSystemInfo(UniwillSdkClient.UniwillService.SystemMonitorData data)
	{
		if (data.Data.TryGetValue("iGpuType", out var value))
		{
			data.Data.Remove("iGpuType");
			SystemInformation[SystemDataType.iGpu] = value;
		}
		_igpuType = new OrderedKeyValuePair("iGpuType", value);
		if (data.Data.TryGetValue("dGpuType", out var value2))
		{
			data.Data.Remove("dGpuType");
			SystemInformation[SystemDataType.dGpu] = value2;
		}
		_dgpuType = new OrderedKeyValuePair("dGpuType", value2);
		if (data.Data.TryGetValue("ProcessorType", out var value3))
		{
			data.Data.Remove("ProcessorType");
			SystemInformation[SystemDataType.Cpu] = value3;
		}
		_cpuType = new OrderedKeyValuePair("ProcessorType", value3);
		if (data.Data.TryGetValue("ProcessorL2CacheSize", out var value4))
		{
			data.Data.Remove("ProcessorL2CacheSize");
		}
		value4 = value4.Replace("KB", ResourceDictionaryConverter.GetString("KbpsUnitKey"));
		_processorL2CacheSize = new OrderedKeyValuePair("ProcessorL2CacheSize", value4);
		ObservableCollection<OrderedKeyValuePair> observableCollection = new ObservableCollection<OrderedKeyValuePair>();
		foreach (KeyValuePair<string, string> datum in data.Data)
		{
			observableCollection.Add(new OrderedKeyValuePair(datum.Key, datum.Value));
		}
		_systemInfo.Data = observableCollection;
		_events.GetEvent<SystemMonitorDataChangeEvent>().Publish();
		_events.GetEvent<SystemComponentNamesEvent>().Publish(SystemInformation);
	}

	private void PublishFanData(Dictionary<string, string> fanData)
	{
		FanData = new List<FanData>();
		foreach (KeyValuePair<string, string> fanDatum in fanData)
		{
			IntelSdkClient.SystemMonitorService.FanType fanType = IntelSdkClient.SystemMonitorService.FanType.NotConnected;
			if (int.TryParse(fanDatum.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				string key = fanDatum.Key;
				if (!(key == "CpuFanSpeed"))
				{
					if (key == "GpuFanSpeed")
					{
						fanType = IntelSdkClient.SystemMonitorService.FanType.SystemFan2;
					}
				}
				else
				{
					fanType = IntelSdkClient.SystemMonitorService.FanType.SystemFan1;
				}
			}
			if (fanType != IntelSdkClient.SystemMonitorService.FanType.NotConnected)
			{
				FanData.Add(new FanData(fanType, result));
			}
		}
		_events.GetEvent<FanStatusEvent>().Publish(FanData);
	}
}
