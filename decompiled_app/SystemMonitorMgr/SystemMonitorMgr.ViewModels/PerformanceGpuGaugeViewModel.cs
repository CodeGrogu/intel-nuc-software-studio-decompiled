using System.Windows;
using SystemMonitorMgr.Enums;
using SystemMonitorMgr.Events;
using SystemMonitorMgr.Interfaces;
using SystemMonitorMgr.Models;
using Prism.Events;

namespace SystemMonitorMgr.ViewModels;

internal class PerformanceGpuGaugeViewModel : CpuUsageGaugeViewModel
{
	private readonly IEventAggregator _events;

	private ISystemMonitor _systemMonitor;

	private string _gpuString;

	public Visibility isGpuVisible { get; set; }

	public string GpuString
	{
		get
		{
			return _gpuString;
		}
		set
		{
			SetProperty(ref _gpuString, value, "GpuString");
		}
	}

	public PerformanceGpuGaugeViewModel(IEventAggregator events, ISystemMonitor monitor)
		: base(events)
	{
		_events = events;
		_systemMonitor = monitor;
		Gauge = new UsageGaugeDataContext(SystemDataType.dGpu, events.GetEvent<UsageDataEvent>());
		if (!string.IsNullOrWhiteSpace(monitor.GpuName))
		{
			GpuString = monitor.GpuName;
			isGpuVisible = Visibility.Visible;
		}
		else
		{
			isGpuVisible = Visibility.Collapsed;
		}
	}
}
