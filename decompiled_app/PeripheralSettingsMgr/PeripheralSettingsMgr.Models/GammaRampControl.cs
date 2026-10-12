using System;
using System.Runtime.InteropServices;

namespace PeripheralSettingsMgr.Models;

public static class GammaRampControl
{
	public struct RAMP
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] Red;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] Green;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public ushort[] Blue;
	}

	public static readonly double MIN_CONTRAST = 0.0;

	public static readonly double MAX_CONTRAST = 100.0;

	public static readonly double MIN_BRIGHTNESS = 0.0;

	public static readonly double MAX_BRIGHTNESS = 100.0;

	public static readonly double MIN_TEMPERATURE = 1000.0;

	public static readonly double MAX_TEMPERATURE = 40000.0;

	[DllImport("gdi32.dll")]
	public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

	[DllImport("gdi32.dll")]
	public static extern bool GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

	[DllImport("user32.dll")]
	private static extern IntPtr GetDC(IntPtr hWnd);

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

	public static bool SetGammaSetting(double Red, double Green, double Blue, IntPtr hdc)
	{
		return CalculateRamp(Red, Green, Blue, hdc);
	}

	public static DisplaySetting.Rgb GetGammaSetting(IntPtr hdc)
	{
		RAMP deviceGammaRampWrap = GetDeviceGammaRampWrap(hdc);
		return new DisplaySetting.Rgb
		{
			Red = (int)deviceGammaRampWrap.Red[1],
			Blue = (int)deviceGammaRampWrap.Blue[1],
			Green = (int)deviceGammaRampWrap.Green[1]
		};
	}

	private static bool SetDeviceGammaRampWrap(ref RAMP lpRamp, IntPtr hdc)
	{
		return SetDeviceGammaRamp(hdc, ref lpRamp);
	}

	private static RAMP GetDeviceGammaRampWrap(IntPtr hdc)
	{
		RAMP lpRamp = default;
		lpRamp.Red = new ushort[256];
		lpRamp.Green = new ushort[256];
		lpRamp.Blue = new ushort[256];
		GetDeviceGammaRamp(hdc, ref lpRamp);
		return lpRamp;
	}

	public static bool CalculateRamp(double red, double green, double blue, IntPtr hdc)
	{
		RAMP lpRamp = default;
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
}
