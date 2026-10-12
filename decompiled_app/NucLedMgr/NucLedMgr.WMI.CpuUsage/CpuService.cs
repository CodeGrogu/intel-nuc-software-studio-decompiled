using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using NucLedMgr.WMI.Models.Indicators;
using Prism.Events;
using Prism.Logging;

namespace NucLedMgr.WMI.CpuUsage;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
internal class CpuService : ICpuService
{
	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private System.Windows.Forms.Timer _monitorTimer;

	private List<CpuIndicator> CpuLeds { get; }

	public float CpuUsage { get; private set; }

	public CpuService(IEventAggregator events, ILoggerFacade logger)
	{
		_events = events;
		_logger = logger;
		CpuLeds = new List<CpuIndicator>();
		Task.Run(async () =>
		{
			await CalculateCurrentUsage().ConfigureAwait(continueOnCapturedContext: false);
		});
		SetTimer();
	}

	private void SetTimer()
	{
		_monitorTimer = new System.Windows.Forms.Timer
		{
			Interval = 5000
		};
		_monitorTimer.Tick += MonitorTimer_Tick;
		_monitorTimer.Start();
	}

	private void MonitorTimer_Tick(object sender, EventArgs e)
	{
		Task.Run(async () =>
		{
			await CalculateCurrentUsage();
		});
	}

	public void AddSetting(CpuIndicator setting)
	{
		CpuIndicator cpuIndicator = CpuLeds.SingleOrDefault((CpuIndicator x) => x.LedType == setting.LedType);
		if (cpuIndicator != null)
		{
			CpuLeds.Remove(cpuIndicator);
		}
		CpuLeds.Add(setting);
		updateColors();
	}

	public void RemoveSetting(CpuIndicator setting)
	{
		CpuIndicator cpuIndicator = CpuLeds.SingleOrDefault((CpuIndicator x) => x.LedType == setting.LedType);
		if (cpuIndicator != null)
		{
			CpuLeds.Remove(cpuIndicator);
		}
	}

	private async Task CalculateCurrentUsage()
	{
		using PerformanceCounter cpuCounter = new PerformanceCounter
		{
			CategoryName = "Processor",
			CounterName = "% Processor Time",
			InstanceName = "_Total"
		};
		try
		{
			cpuCounter.NextValue();
			Thread.Sleep(1000);
			CpuUsage = cpuCounter.NextValue();
		}
		catch
		{
		}
		if (CpuLeds.Count > 0)
		{
			updateColors();
		}
		await Task.Delay(TimeSpan.FromMilliseconds(1000.0));
	}

	private void updateColors()
	{
		foreach (CpuIndicator cpuLed in CpuLeds)
		{
			Color color;
			Color color2;
			if (cpuLed.GreenToRed is bool && cpuLed.GreenToRed.Value)
			{
				color = CpuServiceConstants.GREEN;
				color2 = CpuServiceConstants.RED;
			}
			else
			{
				color = CpuServiceConstants.BLACK;
				color2 = cpuLed.CustomColor;
			}
			double step;
			if (CpuUsage < 30f)
			{
				step = 0.0;
			}
			else if (CpuUsage >= 30f && CpuUsage < 50f)
			{
				step = 0.25;
			}
			else if (CpuUsage >= 50f && CpuUsage < 70f)
			{
				step = 0.5;
			}
			else
			{
				step = ((!(CpuUsage >= 70f) || !(CpuUsage < 90f)) ? 1.0 : 0.75);
			}
			Interpolate(color.R, color2.R, step);
			Interpolate(color.B, color2.B, step);
			Interpolate(color.G, color2.G, step);
		}
	}

	private byte Interpolate(byte start, byte end, double step)
	{
		return (byte)((double)(end - start) * step + (double)(int)start);
	}
}
