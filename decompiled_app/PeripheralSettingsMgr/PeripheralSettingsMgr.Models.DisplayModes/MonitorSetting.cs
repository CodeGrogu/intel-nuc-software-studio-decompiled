using System;
using System.Runtime.InteropServices;
using System.Threading;
using Infrastructure.Models;
using Prism.Events;
using Prism.Logging;

namespace PeripheralSettingsMgr.Models.DisplayModes;

public class MonitorSetting
{
	public struct PHYSICAL_MONITOR
	{
		public IntPtr hPhysicalMonitor;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U2)]
		public char[] szPhysicalMonitorDescription;
	}

	private readonly ILoggerFacade _logger;

	private const int PHYSICAL_MONITOR_DESCRIPTION_SIZE = 64;

	private Thread _writeThread;

	private PHYSICAL_MONITOR physicalMonitor;

	private readonly IntPtr hdc;

	private DisplayControl.BrightnessCapabilities brightnessCapabilities;

	private DisplaySetting settingSendingBuffer;

	private DisplaySetting settingPendingBuffer;

	private readonly object _bufferLock = new object();

	public IntPtr Hdc => hdc;

	public string Name { get; private set; }

	public MonitorCapabilities Capabilities { get; private set; }

	public MonitorSetting(ILoggerFacade logger, IEventAggregator events, string name, PHYSICAL_MONITOR physicalMonitor, IntPtr hdc)
	{
		_logger = logger;
		if (name == null)
		{
			Name = "UNKNOWN";
		}
		else
		{
			string text = name.Replace("\\", "");
			text = text.Replace(".", "");
			Name = text;
		}
		this.physicalMonitor = physicalMonitor;
		this.hdc = hdc;
		UpdateDisplaySettingCapabilities();
	}

	public void UpdateDisplaySettingCapabilities()
	{
		brightnessCapabilities = DisplayControl.GetBrightnessCapabilities(physicalMonitor);
		bool brightnessSupported = brightnessCapabilities.DDCCISupported || brightnessCapabilities.WmiSupported;
		bool contrastCapabilities = DisplayControl.GetContrastCapabilities(physicalMonitor);
		bool colorTemperatureCapabilities = DisplayControl.GetColorTemperatureCapabilities(physicalMonitor);
		Capabilities = new MonitorCapabilities
		{
			BrightnessSupported = brightnessSupported,
			ContrastSupported = contrastCapabilities,
			ColorTemperatureSupported = colorTemperatureCapabilities,
			GammaRampSupported = true
		};
	}

	public bool SetDisplaySetting(DisplaySetting setting)
	{
		if (settingSendingBuffer != null)
		{
			settingPendingBuffer = setting;
		}
		else
		{
			settingSendingBuffer = setting;
		}
		if (_writeThread == null)
		{
			_writeThread = new Thread(SetDisplay);
		}
		if (_writeThread.ThreadState == ThreadState.Stopped)
		{
			_writeThread.Abort();
			_writeThread = new Thread(SetDisplay);
		}
		if (_writeThread.ThreadState == ThreadState.Unstarted)
		{
			_writeThread.Start();
		}
		return true;
	}

	public void SetDisplay()
	{
		while (settingSendingBuffer != null)
		{
			if (Capabilities.BrightnessSupported && settingSendingBuffer.Brightness <= 100.0 && settingSendingBuffer.Brightness >= 0.0)
			{
				try
				{
					DisplayControl.SetBrightness(physicalMonitor, settingSendingBuffer.Brightness, brightnessCapabilities);
				}
				catch (Exception exception)
				{
					_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
				}
			}
			if (Capabilities.ContrastSupported && settingSendingBuffer.Contrast <= 100.0 && settingSendingBuffer.Contrast >= 0.0)
			{
				try
				{
					DisplayControl.SetContrast(physicalMonitor, settingSendingBuffer.Contrast);
				}
				catch (Exception exception2)
				{
					_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception2), Category.Exception, Priority.High);
				}
			}
			if (Capabilities.ColorTemperatureSupported)
			{
				try
				{
					DisplayControl.SetColorTemperature(physicalMonitor, settingSendingBuffer.ColorTempType);
				}
				catch (Exception exception3)
				{
					_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception3), Category.Exception, Priority.High);
				}
			}
			if (Capabilities.GammaRampSupported)
			{
				try
				{
					DisplayControl.SetGammaSetting(new DisplaySetting.Rgb
					{
						Red = settingSendingBuffer.Red,
						Blue = settingSendingBuffer.Blue,
						Green = settingSendingBuffer.Green
					}, hdc);
				}
				catch (Exception exception4)
				{
					_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception4), Category.Exception, Priority.High);
				}
			}
			lock (_bufferLock)
			{
				if (settingPendingBuffer != null)
				{
					settingSendingBuffer = settingPendingBuffer;
					settingPendingBuffer = null;
				}
				else
				{
					settingSendingBuffer = null;
				}
			}
		}
	}

	public DisplaySetting GetDisplaySetting()
	{
		DisplaySetting displaySetting = new DisplaySetting();
		if (Capabilities.BrightnessSupported)
		{
			displaySetting.Brightness = DisplayControl.GetBrightness(physicalMonitor);
		}
		if (Capabilities.ContrastSupported)
		{
			displaySetting.Contrast = DisplayControl.GetContrast(physicalMonitor);
		}
		if (Capabilities.ColorTemperatureSupported)
		{
			displaySetting.ColorTempType = DisplayControl.GetColorTemperature(physicalMonitor);
		}
		if (Capabilities.GammaRampSupported)
		{
			DisplaySetting.Rgb gammaSetting = DisplayControl.GetGammaSetting(hdc);
			displaySetting.Red = gammaSetting.Red;
			displaySetting.Green = gammaSetting.Green;
			displaySetting.Blue = gammaSetting.Blue;
		}
		return displaySetting;
	}
}
