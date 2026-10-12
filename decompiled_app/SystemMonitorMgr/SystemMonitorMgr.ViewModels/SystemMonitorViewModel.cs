using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SystemMonitorMgr.Enums;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Interfaces;
using SystemMonitorMgr.Models;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Infrastructure.Converters;
using Infrastructure.Events;
using Infrastructure.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace SystemMonitorMgr.ViewModels;

public class SystemMonitorViewModel : BaseDetailViewModel
{
	private const IconType ICON_TYPE = IconType.SystemMonitor;

	private const string MAIN_CONTENT_REGION = "MainContentRegion";

	private const string PROFILES_VIEW = "ProfilesView";

	private const string PERCENT_SYMBOL_KEY = "PercentSymbol";

	private const string BATTERY_PERCENT_STRING_PROPERTY_NAME = "BatteryPercentString";

	private readonly IRegionManager _regionMgr;

	private readonly ISystemMonitor _monitor;

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private ObservableCollection<SystemMonitorData> _data;

	private List<FanData> _fans;

	private SystemMonitorData _dataHw;

	private int _batteryPercent = -1;

	private readonly string _localizedPercentString;

	private bool _isBatterySupported;

	private DelegateCommand _returnToMainView;

	public bool ShowLabel => false;

	public ObservableCollection<SystemMonitorData> Data
	{
		get
		{
			return _data;
		}
		set
		{
			SetProperty(ref _data, value, "Data");
		}
	}

	public List<FanData> Fans
	{
		get
		{
			return _fans;
		}
		set
		{
			SetProperty(ref _fans, value, "Fans");
		}
	}

	public SystemMonitorData DataHw
	{
		get
		{
			return _dataHw;
		}
		set
		{
			SetProperty(ref _dataHw, value, "DataHw");
		}
	}

	public int BatteryPercent
	{
		get
		{
			return _batteryPercent;
		}
		set
		{
			SetProperty(ref _batteryPercent, value, "BatteryPercent");
		}
	}

	public string BatteryPercentString
	{
		get
		{
			if (_batteryPercent != -1)
			{
				return _batteryPercent + _localizedPercentString;
			}
			return "";
		}
	}

	public bool IsBatterySupported
	{
		get
		{
			return _isBatterySupported;
		}
		set
		{
			SetProperty(ref _isBatterySupported, value, "IsBatterySupported");
		}
	}

	public bool IsUniwill => _monitor.GetType() == typeof(UniwillSystemMonitor);

	public DelegateCommand ReturnToMainView
	{
		get
		{
			return _returnToMainView;
		}
		set
		{
			SetProperty(ref _returnToMainView, value, "ReturnToMainView");
		}
	}

	public SystemMonitorViewModel(IRegionManager regionMgr, IEventAggregator events, ILoggerFacade logger, ISystemMonitor monitor, ICustomResourceManager resourceManager)
		: base(resourceManager, IconType.SystemMonitor)
	{
		_regionMgr = regionMgr;
		_monitor = monitor;
		_events = events;
		_logger = logger;
		_returnToMainView = new DelegateCommand(NavBack);
		_data = new ObservableCollection<SystemMonitorData>();
		_dataHw = null;
		_fans = monitor.FanData;
		_localizedPercentString = ResourceDictionaryConverter.GetString("PercentSymbol");
		UpdateData();
		events.GetEvent<BatteryStatusEvent>().Subscribe(UpdateBattery);
		events.GetEvent<BatterySupportedEvent>().Subscribe(BatterySupported);
		events.GetEvent<SystemMonitorDataChangeEvent>().Subscribe(UpdateData);
		events.GetEvent<FanStatusEvent>().Subscribe(UpdateFans);
	}

	private void UpdateFans(List<FanData> fanUpdate)
	{
		Fans = fanUpdate;
		RaisePropertyChanged("Fans");
	}

	private void UpdateData()
	{
		foreach (SystemMonitorData d in _monitor.Data)
		{
			switch (d.Type)
			{
			case SystemDataType.Battery:
			case SystemDataType.Cpu:
			case SystemDataType.dGpu:
			case SystemDataType.iGpu:
			case SystemDataType.HwFuelGauge:
			case SystemDataType.Memory:
			case SystemDataType.Network:
			case SystemDataType.SSD0:
			case SystemDataType.SSD1:
			case SystemDataType.SSD2:
			case SystemDataType.SSD3:
			case SystemDataType.HDD0:
				try
				{
					if (d.Data.Count <= 0)
					{
						break;
					}
					if (!Data.Any((SystemMonitorData x) => x.Type == d.Type))
					{
						Application.Current.Dispatcher.Invoke(() =>
						{
							Data.Add(d);
						});
						break;
					}
					Application.Current.Dispatcher.Invoke(() =>
					{
						Data[Data.IndexOf(Data.FirstOrDefault((SystemMonitorData x) => x.Type == d.Type))] = d;
					});
				}
				catch (Exception exception2)
				{
					_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception2), Category.Exception, Priority.High);
				}
				break;
			case SystemDataType.Hardware:
				try
				{
					if (d.Data.Count > 0)
					{
						DataHw = d;
					}
				}
				catch (Exception exception)
				{
					_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
				}
				break;
			}
		}
		RaisePropertyChanged("Data");
		RaisePropertyChanged("DataHw");
		RaisePropertyChanged("Fans");
	}

	private void UpdateBattery(int batteryPercent)
	{
		BatteryPercent = batteryPercent;
		RaisePropertyChanged("BatteryPercentString");
	}

	private void BatterySupported(bool isBatterySupported)
	{
		IsBatterySupported = isBatterySupported;
	}

	private void NavBack()
	{
		_regionMgr.Regions["MainContentRegion"].RequestNavigate("ProfilesView");
		_events.GetEvent<SysMonViewOpenedEvent>().Publish(payload: false);
	}
}
