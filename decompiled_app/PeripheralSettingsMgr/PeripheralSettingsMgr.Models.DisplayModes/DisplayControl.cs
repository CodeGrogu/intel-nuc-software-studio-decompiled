using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;

namespace PeripheralSettingsMgr.Models.DisplayModes;

internal static class DisplayControl
{
	public struct Ramp
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] Red;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] Green;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] Blue;
	}

	public struct Point
	{
		private readonly int x;

		private readonly int y;
	}

	public struct BrightnessCapabilities
	{
		public bool WmiSupported;

		public bool DDCCISupported;
	}

	private const int MC_CAPS_BRIGHTNESS = 2;

	private const int MC_CAPS_CONTRAST = 4;

	private const int MONITOR_DEFAULTTONEAREST = 1;

	public static bool GetContrastCapabilities(MonitorSetting.PHYSICAL_MONITOR physicalMonitor)
	{
		if (!GetMonitorContrast(physicalMonitor.hPhysicalMonitor, out var pdwMinimumContrast, out var pdwCurrentContrast, out var pdwMaximumContrast))
		{
			return GetMonitorContrast(physicalMonitor.hPhysicalMonitor, out pdwMinimumContrast, out pdwCurrentContrast, out pdwMaximumContrast);
		}
		return true;
	}

	public static double GetContrast(MonitorSetting.PHYSICAL_MONITOR physicalMonitor)
	{
		if (!GetMonitorContrast(physicalMonitor.hPhysicalMonitor, out var _, out var pdwCurrentContrast, out var _))
		{
			return -1.0;
		}
		return pdwCurrentContrast;
	}

	public static bool SetContrast(MonitorSetting.PHYSICAL_MONITOR physicalMonitor, double contrast)
	{
		return SetMonitorContrast(physicalMonitor.hPhysicalMonitor, (uint)contrast);
	}

	public static bool GetColorTemperatureCapabilities(MonitorSetting.PHYSICAL_MONITOR physicalMonitor)
	{
		if (!GetMonitorCapabilities(physicalMonitor.hPhysicalMonitor, out var pdwMonitorCapabilities, out var pdwSupportedColorTemperatures))
		{
			if (!GetMonitorCapabilities(physicalMonitor.hPhysicalMonitor, out pdwMonitorCapabilities, out pdwSupportedColorTemperatures))
			{
				return false;
			}
			return true;
		}
		return true;
	}

	public static ColorTemperatureType GetColorTemperature(MonitorSetting.PHYSICAL_MONITOR physicalMonitor)
	{
		if (!GetMonitorColorTemperature(physicalMonitor.hPhysicalMonitor, out var colorTemperature))
		{
			return ColorTemperatureType.MC_COLOR_TEMPERATURE_UNKNOWN;
		}
		return colorTemperature;
	}

	public static bool SetColorTemperature(MonitorSetting.PHYSICAL_MONITOR physicalMonitor, ColorTemperatureType colorTemperature)
	{
		if (!SetMonitorColorTemperature(physicalMonitor.hPhysicalMonitor, colorTemperature))
		{
			return false;
		}
		return true;
	}

	public static BrightnessCapabilities GetBrightnessCapabilities(MonitorSetting.PHYSICAL_MONITOR physicalMonitor)
	{
		BrightnessCapabilities result = new BrightnessCapabilities
		{
			DDCCISupported = GetBrightnessDDCCICapabilities(physicalMonitor)
		};
		if (!result.DDCCISupported)
		{
			result.WmiSupported = GetBrightnessWmiCapabilities();
		}
		else
		{
			result.WmiSupported = false;
		}
		return result;
	}

	public static double GetBrightness(MonitorSetting.PHYSICAL_MONITOR physicalMonitor)
	{
		if (GetBrightnessWmiCapabilities())
		{
			return GetBrightnessWmi();
		}
		if (GetBrightnessDDCCICapabilities(physicalMonitor))
		{
			return GetBrightnessDDCCI(physicalMonitor);
		}
		return -1.0;
	}

	public static bool SetBrightness(MonitorSetting.PHYSICAL_MONITOR physicalMonitor, double brightness, BrightnessCapabilities brightnessSupport)
	{
		if (brightnessSupport.WmiSupported)
		{
			return SetBrightnessWmi(brightness);
		}
		if (brightnessSupport.DDCCISupported)
		{
			return SetBrightnessDDCCI(physicalMonitor, brightness);
		}
		return false;
	}

	private static bool GetBrightnessWmiCapabilities()
	{
		ManagementScope scope = new ManagementScope("root\\WMI");
		SelectQuery query = new SelectQuery("WmiMonitorBrightness");
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query))
		{
			try
			{
				using ManagementObjectCollection source = managementObjectSearcher.Get();
				using IEnumerator<ManagementObject> enumerator = source.OfType<ManagementObject>().GetEnumerator();
				if (enumerator.MoveNext())
				{
					_ = enumerator.Current;
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		return true;
	}

	private static bool GetBrightnessDDCCICapabilities(MonitorSetting.PHYSICAL_MONITOR physicalMonitor)
	{
		if (!GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, out var pdwMinimumBrightness, out var pdwCurrentBrightness, out var pdwMaximumBrightness))
		{
			return GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, out pdwMinimumBrightness, out pdwCurrentBrightness, out pdwMaximumBrightness);
		}
		return true;
	}

	private static double GetBrightnessWmi()
	{
		ManagementScope scope = new ManagementScope("root\\WMI");
		SelectQuery query = new SelectQuery("WmiMonitorBrightness");
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query))
		{
			try
			{
				using ManagementObjectCollection source = managementObjectSearcher.Get();
				using IEnumerator<ManagementObject> enumerator = source.OfType<ManagementObject>().GetEnumerator();
				if (enumerator.MoveNext())
				{
					return uint.Parse(enumerator.Current["CurrentBrightness"].ToString());
				}
			}
			catch (Exception)
			{
				return -1.0;
			}
		}
		return -1.0;
	}

	private static bool SetBrightnessWmi(double brightness)
	{
		ManagementScope scope = new ManagementScope("root\\WMI");
		SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query))
		{
			using ManagementObjectCollection source = managementObjectSearcher.Get();
			using IEnumerator<ManagementObject> enumerator = source.OfType<ManagementObject>().GetEnumerator();
			if (enumerator.MoveNext())
			{
				enumerator.Current.InvokeMethod("WmiSetBrightness", new object[2]
				{
					uint.MaxValue,
					(uint)brightness
				});
			}
		}
		return true;
	}

	private static double GetBrightnessDDCCI(MonitorSetting.PHYSICAL_MONITOR physicalMonitor)
	{
		if (!GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, out var _, out var pdwCurrentBrightness, out var _))
		{
			return -1.0;
		}
		return pdwCurrentBrightness;
	}

	public static bool SetBrightnessDDCCI(MonitorSetting.PHYSICAL_MONITOR physicalMonitor, double brightness)
	{
		if (!SetMonitorBrightness(physicalMonitor.hPhysicalMonitor, (uint)brightness))
		{
			return false;
		}
		return true;
	}

	public static void SaveGammaSettingOnLaunch()
	{
	}

	public static bool ResetToDefaultGammaSetting()
	{
		return true;
	}

	public static bool SetGammaSetting(DisplaySetting.Rgb rgb, IntPtr hdc)
	{
		return CalculateRamp(rgb.Red, rgb.Green, rgb.Blue, hdc);
	}

	public static DisplaySetting.Rgb GetGammaSetting(IntPtr hdc)
	{
		Ramp deviceGammaRampWrap = GetDeviceGammaRampWrap(hdc);
		return new DisplaySetting.Rgb
		{
			Red = (int)deviceGammaRampWrap.Red[1],
			Blue = (int)deviceGammaRampWrap.Blue[1],
			Green = (int)deviceGammaRampWrap.Green[1]
		};
	}

	private static bool SetDeviceGammaRampWrap(ref Ramp lpRamp, IntPtr hdc)
	{
		return SetDeviceGammaRamp(hdc, ref lpRamp);
	}

	private static Ramp GetDeviceGammaRampWrap(IntPtr hdc)
	{
		Ramp lpRamp = default;
		lpRamp.Red = new ushort[256];
		lpRamp.Green = new ushort[256];
		lpRamp.Blue = new ushort[256];
		GetDeviceGammaRamp(hdc, ref lpRamp);
		return lpRamp;
	}

	public static bool CalculateRamp(double red, double green, double blue, IntPtr hdc)
	{
		Ramp lpRamp = default;
		lpRamp.Red = new ushort[256];
		lpRamp.Green = new ushort[256];
		lpRamp.Blue = new ushort[256];
		for (int i = 0; i < 256; i++)
		{
			double val = (double)i * red;
			double val2 = (double)i * green;
			double val3 = (double)i * blue;
			lpRamp.Red[i] = (ushort)Math.Min(65535.0, Math.Max(0.0, val));
			lpRamp.Green[i] = (ushort)Math.Min(65535.0, Math.Max(0.0, val2));
			lpRamp.Blue[i] = (ushort)Math.Min(65535.0, Math.Max(0.0, val3));
		}
		return SetDeviceGammaRampWrap(ref lpRamp, hdc);
	}

	[DllImport("gdi32.dll")]
	public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref Ramp lpRamp);

	[DllImport("gdi32.dll")]
	public static extern bool GetDeviceGammaRamp(IntPtr hDC, ref Ramp lpRamp);

	[DllImport("dxva2.dll", SetLastError = true)]
	private static extern bool GetMonitorContrast(IntPtr hMonitor, out uint pdwMinimumContrast, out uint pdwCurrentContrast, out uint pdwMaximumContrast);

	[DllImport("dxva2.dll", SetLastError = true)]
	private static extern bool GetMonitorBrightness(IntPtr hMonitor, out uint pdwMinimumBrightness, out uint pdwCurrentBrightness, out uint pdwMaximumBrightness);

	[DllImport("dxva2.dll", SetLastError = true)]
	private static extern bool SetMonitorBrightness(IntPtr hMonitor, uint dwNewBrightness);

	[DllImport("dxva2.dll", SetLastError = true)]
	private static extern bool SetMonitorContrast(IntPtr hMonitor, uint dwNewContrast);

	[DllImport("dxva2.dll", SetLastError = true)]
	private static extern bool GetMonitorColorTemperature(IntPtr hMonitor, out ColorTemperatureType colorTemperature);

	[DllImport("dxva2.dll", SetLastError = true)]
	private static extern bool SetMonitorColorTemperature(IntPtr hMonitor, ColorTemperatureType colorTemperature);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool GetCursorPos(out Point lpPoint);

	[DllImport("user32.dll")]
	private static extern IntPtr MonitorFromPoint(Point pt, uint dwFlags);

	[DllImport("Dxva2.dll", SetLastError = true)]
	private static extern bool GetMonitorCapabilities(IntPtr hMonitor, out uint pdwMonitorCapabilities, out uint pdwSupportedColorTemperatures);

	[DllImport("user32.dll")]
	private static extern IntPtr MonitorFromWindow(IntPtr dtw, uint dwFlags);

	[DllImport("user32.dll")]
	private static extern IntPtr GetDesktopWindow();
}
