using System;
using Prism.Mvvm;

namespace PeripheralSettingsMgr.Models.DisplayModes;

public class GammaRampSetting : BindableBase
{
	private readonly IntPtr _hdc;

	private bool _gammaRampSupported;

	private double _red;

	private double _green;

	private double _blue;

	public DisplaySetting.Rgb InitialGammaSetting { get; private set; }

	public bool GammaRampSupported
	{
		get
		{
			return _gammaRampSupported;
		}
		set
		{
			SetProperty(ref _gammaRampSupported, value, "GammaRampSupported");
		}
	}

	public double ColorMin => 0.0;

	public double ColorMax => 255.0;

	public double Red
	{
		get
		{
			return _red;
		}
		set
		{
			SetProperty(ref _red, value, "Red");
			GammaRampControl.SetGammaSetting(Red, Green, Blue, _hdc);
		}
	}

	public double Green
	{
		get
		{
			return _green;
		}
		set
		{
			SetProperty(ref _green, value, "Green");
			GammaRampControl.SetGammaSetting(Red, Green, Blue, _hdc);
		}
	}

	public double Blue
	{
		get
		{
			return _blue;
		}
		set
		{
			SetProperty(ref _blue, value, "Blue");
			GammaRampControl.SetGammaSetting(Red, Green, Blue, _hdc);
		}
	}

	public GammaRampSetting(IntPtr hdc, double red, double green, double blue, bool supported)
	{
		_hdc = hdc;
		_red = red;
		_green = green;
		_blue = blue;
		_gammaRampSupported = supported;
		InitialGammaSetting = GammaRampControl.GetGammaSetting(_hdc);
	}

	public GammaRampSetting(IntPtr hdc)
	{
		_hdc = hdc;
		InitialGammaSetting = GammaRampControl.GetGammaSetting(_hdc);
		_red = InitialGammaSetting.Red;
		_green = InitialGammaSetting.Green;
		_blue = InitialGammaSetting.Blue;
	}
}
