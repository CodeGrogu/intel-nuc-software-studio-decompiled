using System.Collections.ObjectModel;
using SystemMonitorMgr.Enums;
using IntelSdkClient.SystemMonitorService;
using Prism.Events;
using Prism.Mvvm;

namespace SystemMonitorMgr.Models;

public class SystemMonitorData : BindableBase
{
	private readonly FanType _fanType;

	private UsageGaugeDataContext _gauge;

	private ObservableCollection<OrderedKeyValuePair> _data;

	private ObservableCollection<OrderedKeyValuePair> _summaryData;

	private readonly PubSubEvent<UsageData> _usageChangeEvent;

	private readonly SubscriptionToken _subscriptionToken;

	private FanData _fan;

	public SystemDataType Type { get; private set; }

	public UsageGaugeDataContext Gauge
	{
		get
		{
			return _gauge;
		}
		set
		{
			SetProperty(ref _gauge, value, "Gauge");
		}
	}

	public ObservableCollection<OrderedKeyValuePair> Data
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

	public ObservableCollection<OrderedKeyValuePair> SummaryData
	{
		get
		{
			return _summaryData;
		}
		set
		{
			SetProperty(ref _summaryData, value, "SummaryData");
		}
	}

	public FanData Fan
	{
		get
		{
			return _fan;
		}
		set
		{
			SetProperty(ref _fan, value, "Fan");
		}
	}

	public SystemMonitorData(SystemDataType type, PubSubEvent<UsageData> usageChangeEvent, PubSubEvent<FanData> fanEvent = null, ObservableCollection<OrderedKeyValuePair> data = null)
	{
		Type = type;
		Gauge = null;
		_usageChangeEvent = usageChangeEvent;
		_subscriptionToken = usageChangeEvent.Subscribe(UpdateUsage);
		if (fanEvent == null)
		{
			Fan = null;
		}
		else
		{
			switch (type)
			{
			case SystemDataType.Cpu:
				_fanType = FanType.CPUFan;
				break;
			case SystemDataType.dGpu:
				_fanType = FanType.dGPUFan;
				break;
			}
			Fan = new FanData(_fanType);
			fanEvent.Subscribe(FanChange);
		}
		Data = data ?? new ObservableCollection<OrderedKeyValuePair>();
		SummaryData = new ObservableCollection<OrderedKeyValuePair>();
	}

	private void UpdateUsage(UsageData data)
	{
		if (data.Type == Type)
		{
			Gauge = new UsageGaugeDataContext(Type, _usageChangeEvent);
			_usageChangeEvent.Unsubscribe(_subscriptionToken);
			Gauge.RenderArc(data.Usage);
			Gauge.Temp = data.Temp;
		}
	}

	private void FanChange(FanData data)
	{
		if (data.Type == _fanType)
		{
			Fan.Rpm = data.Rpm;
		}
	}
}
