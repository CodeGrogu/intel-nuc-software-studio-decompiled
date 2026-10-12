using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using PeripheralSettingsMgr.Events;
using Prism.Events;
using Prism.Logging;

namespace PeripheralSettingsMgr.Models.DisplayModes;

public class MonitorGroup
{
	private struct MonitorInfo
	{
		public int Size;

		public Rect Monitor;

		public Rect WorkArea;

		public uint Flag;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName;
	}

	private struct Rect
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	public delegate void DisplayStatusChangedHandler();

	private delegate bool EnumMonitorDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref System.Windows.Rect lprcMonitor, IntPtr dwData);

	private readonly ILoggerFacade _logger;

	private readonly IEventAggregator _events;

	public List<MonitorSetting> Monitors { get; private set; }

	public static event DisplayStatusChangedHandler DisplayStatusChanged;

	public MonitorGroup(ILoggerFacade logger, IEventAggregator events)
	{
		_logger = logger;
		_events = events;
		Task.Run(() =>
		{
			EnumAllMonitors();
			_events.GetEvent<MonitorStatusQueryCompleteEvent>().Publish(this);
		});
		SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanging;
	}

	public void MonitorGroupDispose()
	{
		Monitors.Clear();
		SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanging;
	}

	public List<MonitorSetting> EnumAllMonitors()
	{
		if (Monitors == null)
		{
			Monitors = new List<MonitorSetting>();
		}
		Monitors.Clear();
		EnumMonitorDelegate lpfnEnum = EnumMonitor;
		EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, lpfnEnum, IntPtr.Zero);
		return Monitors;
	}

	private void OnDisplaySettingsChanging(object sender, EventArgs e)
	{
		EnumAllMonitors();
		DisplayStatusChanged();
	}

	public static MonitorSetting.PHYSICAL_MONITOR[] GetPhysicalMonitors(IntPtr hMonitor)
	{
		GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out var pdwNumberOfPhysicalMonitors);
		MonitorSetting.PHYSICAL_MONITOR[] array = new MonitorSetting.PHYSICAL_MONITOR[pdwNumberOfPhysicalMonitors];
		GetPhysicalMonitorsFromHMONITOR(hMonitor, pdwNumberOfPhysicalMonitors, array);
		return array;
	}

	[DllImport("user32.dll")]
	private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorDelegate lpfnEnum, IntPtr dwData);

	private bool EnumMonitor(IntPtr hMonitor, IntPtr hdcMonitor, ref System.Windows.Rect lprcMonitor, IntPtr dwData)
	{
		MonitorSetting.PHYSICAL_MONITOR[] physicalMonitors = GetPhysicalMonitors(hMonitor);
		MonitorInfo monitorInfo = default;
		monitorInfo.Size = Marshal.SizeOf(monitorInfo);
		GetMonitorInfo(hMonitor, ref monitorInfo);
		IntPtr hdc = GetDCWrap();
		Screen[] allScreens = Screen.AllScreens;
		foreach (Screen screen in allScreens)
		{
			if (monitorInfo.DeviceName == screen.DeviceName)
			{
				hdc = CreateDC(null, screen.DeviceName, null, IntPtr.Zero);
				break;
			}
		}
		MonitorSetting item = new MonitorSetting(_logger, _events, monitorInfo.DeviceName, physicalMonitors[0], hdc);
		Monitors.Add(item);
		return true;
	}

	private static IntPtr GetDCWrap()
	{
		return GetDC(IntPtr.Zero);
	}

	[DllImport("dxva2.dll", SetLastError = true)]
	private static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] MonitorSetting.PHYSICAL_MONITOR[] pPhysicalMonitorArray);

	[DllImport("dxva2.dll", SetLastError = true)]
	private static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out uint pdwNumberOfPhysicalMonitors);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo monitorInfo);

	[DllImport("gdi32.dll")]
	public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

	[DllImport("gdi32.dll")]
	public static extern bool DelectDC([In] IntPtr hDC);

	[DllImport("user32.dll")]
	private static extern IntPtr GetDC(IntPtr hWnd);
}
