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
using Infrastructure.ErrorHandling;
using Infrastructure.Events;
using Infrastructure.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace SystemMonitorMgr.ViewModels;

public class SystemMonitorSummaryViewModel : NavBaseSummaryViewModel
{
	private const string CARD_TITLE_STRING = "SystemMonitorTitleString";

	private const IconType CARD_ICON_TYPE = IconType.SystemMonitor;

	private readonly ISystemMonitor _monitor;

	private readonly Dictionary<SystemDataType, string> SystemInformation;

	private readonly IEventAggregator _events;

	private ObservableCollection<OrderedKeyValuePair> _dataSummary;

	private ObservableCollection<UsageGaugeDataContext> _gauges;

	private ClientState _isConnected;

	private DelegateCommand _openErrorMessageBox;

	public ObservableCollection<OrderedKeyValuePair> DataSummary
	{
		get
		{
			return _dataSummary;
		}
		set
		{
			SetProperty(ref _dataSummary, value, "DataSummary");
		}
	}

	public ObservableCollection<UsageGaugeDataContext> Gauges
	{
		get
		{
			return _gauges;
		}
		set
		{
			SetProperty(ref _gauges, value, "Gauges");
		}
	}

	public bool ShowLabel => true;

	public ClientState IsConnected
	{
		get
		{
			return _isConnected;
		}
		set
		{
			SetProperty(ref _isConnected, value, "IsConnected");
		}
	}

	public DelegateCommand OpenErrorMessageBox
	{
		get
		{
			return _openErrorMessageBox;
		}
		set
		{
			SetProperty(ref _openErrorMessageBox, value, "OpenErrorMessageBox");
		}
	}

	public SystemMonitorSummaryViewModel(IRegionManager regionMgr, IEventAggregator events, ISystemMonitor monitor, ICustomResourceManager resourceManager)
		: base(regionMgr, resourceManager, "SystemMonitorTitleString", "SystemMonitorCardView", IconType.SystemMonitor)
	{
		_monitor = monitor;
		_events = events;
		SystemInformation = new Dictionary<SystemDataType, string>();
		events.GetEvent<SystemMonitorDataChangeEvent>().Subscribe(UpdateData);
		events.GetEvent<SysMonConnectionEvent>().Subscribe((ClientState d) =>
		{
			IsConnected = d;
		});
		_isConnected = _monitor.Subscribed;
		_dataSummary = new ObservableCollection<OrderedKeyValuePair>();
		UsageGaugeDataContext item = new UsageGaugeDataContext(SystemDataType.Cpu, events.GetEvent<UsageDataEvent>());
		UsageGaugeDataContext item2 = new UsageGaugeDataContext(SystemDataType.iGpu, events.GetEvent<UsageDataEvent>());
		UsageGaugeDataContext item3 = new UsageGaugeDataContext(SystemDataType.Memory, events.GetEvent<UsageDataEvent>())
		{
			Temp = null
		};
		UsageGaugeDataContext item4 = new UsageGaugeDataContext(SystemDataType.SSD0, events.GetEvent<UsageDataEvent>())
		{
			Temp = null
		};
		_gauges = new ObservableCollection<UsageGaugeDataContext> { item, item2, item3, item4 };
		UpdateData();
		if (_isConnected == ClientState.Error)
		{
			_openErrorMessageBox = new DelegateCommand(OpenErrorMessage);
		}
	}

	private void OpenErrorMessage()
	{
		ErrorHandler.PublishError("ErrorLoadingMessage", ErrorType.SERVICE_OUTDATED, critical: true, msgIsKey: true);
		_events.GetEvent<ApplicationCloseEvent>().Publish(payload: false);
	}

	private void AddSummaryData(ObservableCollection<OrderedKeyValuePair> SummaryData)
	{
		if (SummaryData == null || SummaryData.Count <= 0)
		{
			return;
		}
		foreach (OrderedKeyValuePair e in SummaryData)
		{
			OrderedKeyValuePair orderedKeyValuePair = DataSummary.FirstOrDefault((OrderedKeyValuePair x) => x.Key == e.Key);
			if (orderedKeyValuePair != null)
			{
				orderedKeyValuePair.Value = e.Value;
				continue;
			}
			Application.Current.Dispatcher.Invoke(() =>
			{
				DataSummary.Add(e);
			});
		}
	}

	private void ShowGauge(UsageGaugeDataContext Gauge)
	{
		if (Gauge == null)
		{
			return;
		}
		UsageGaugeDataContext usageGaugeDataContext = Gauges.FirstOrDefault((UsageGaugeDataContext x) => x.Type.Equals(Gauge.Type));
		if (usageGaugeDataContext == null)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Gauges.Add(Gauge);
			});
		}
		else if (usageGaugeDataContext.Label != Gauge.Label)
		{
			int index = Gauges.IndexOf(usageGaugeDataContext);
			Application.Current.Dispatcher.Invoke(() =>
			{
				Gauges[index] = Gauge;
			});
		}
	}

	private void UpdateData()
	{
		foreach (SystemMonitorData d in _monitor.Data)
		{
			switch (d.Type)
			{
			case SystemDataType.dGpu:
			{
				if (d.Data.Count <= 0)
				{
					break;
				}
				AddSummaryData(d.SummaryData);
				UsageGaugeDataContext iGpu = Gauges.FirstOrDefault((UsageGaugeDataContext x) => x.Type == SystemDataType.iGpu);
				if (iGpu != null)
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						int index = Gauges.IndexOf(iGpu);
						Gauges[index] = d.Gauge;
					});
				}
				ShowGauge(d.Gauge);
				break;
			}
			case SystemDataType.iGpu:
				AddSummaryData(d.SummaryData);
				if (Gauges.FirstOrDefault((UsageGaugeDataContext x) => x.Type == SystemDataType.dGpu) == null)
				{
					ShowGauge(d.Gauge);
				}
				break;
			case SystemDataType.Cpu:
			case SystemDataType.Memory:
			case SystemDataType.SSD0:
				AddSummaryData(d.SummaryData);
				ShowGauge(d.Gauge);
				break;
			case SystemDataType.Battery:
			case SystemDataType.HwFuelGauge:
			case SystemDataType.Network:
			case SystemDataType.SSD1:
			case SystemDataType.SSD2:
			case SystemDataType.SSD3:
			case SystemDataType.HDD0:
				AddSummaryData(d.SummaryData);
				break;
			case SystemDataType.Hardware:
				if (d.Data.Count <= 0 || DataSummary.FirstOrDefault((OrderedKeyValuePair x) => x.Key == d.Data.First().Key) != null)
				{
					break;
				}
				if (DataSummary.Count > 0)
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						DataSummary.AddRange(d.Data);
					});
				}
				else
				{
					DataSummary = d.Data;
				}
				break;
			}
		}
	}
}
