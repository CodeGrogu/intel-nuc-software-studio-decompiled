using Newtonsoft.Json;
using PeripheralSettingsMgr.Models.DisplayModes;
using Prism.Mvvm;

namespace PeripheralSettingsMgr.Models;

public class DisplaySetting : BindableBase
{
	public struct Rgb
	{
		public double Red { get; set; }

		public double Green { get; set; }

		public double Blue { get; set; }
	}

	private bool _brightnessSupported;

	private double _brightness;

	private bool _contrastSupported;

	private double _contrast;

	private bool _colorTemperatureSupported;

	private ColorTemperatureType _colorTempType;

	private double _colorTemperature;

	private bool _gammaRampSupported;

	private double _red;

	private double _green;

	private double _blue;

	[JsonIgnore]
	public MonitorDisplayModeProfile.MonitorSetDisplayDelegate SetDisplay { get; private set; }

	public bool BrightnessSupported
	{
		get
		{
			return _brightnessSupported;
		}
		set
		{
			SetProperty(ref _brightnessSupported, value, "BrightnessSupported");
		}
	}

	public double Brightness
	{
		get
		{
			return _brightness;
		}
		set
		{
			SetProperty(ref _brightness, value, "Brightness");
			SetDisplay?.Invoke(this);
		}
	}

	[JsonIgnore]
	public double BrightnessMin => GammaRampControl.MIN_BRIGHTNESS;

	[JsonIgnore]
	public double BrightnessMax => GammaRampControl.MAX_BRIGHTNESS;

	public bool ContrastSupported
	{
		get
		{
			return _contrastSupported;
		}
		set
		{
			SetProperty(ref _contrastSupported, value, "ContrastSupported");
		}
	}

	public double Contrast
	{
		get
		{
			return _contrast;
		}
		set
		{
			SetProperty(ref _contrast, value, "Contrast");
			SetDisplay?.Invoke(this);
		}
	}

	[JsonIgnore]
	public double ContrastMin => GammaRampControl.MIN_CONTRAST;

	[JsonIgnore]
	public double ContrastMax => GammaRampControl.MAX_CONTRAST;

	public bool ColorTemperatureSupported
	{
		get
		{
			return _colorTemperatureSupported;
		}
		set
		{
			SetProperty(ref _colorTemperatureSupported, value, "ColorTemperatureSupported");
		}
	}

	public ColorTemperatureType ColorTempType
	{
		get
		{
			return _colorTempType;
		}
		set
		{
			SetProperty(ref _colorTempType, value, "ColorTempType");
			_colorTemperature = (int)_colorTempType;
			RaisePropertyChanged("ColorTemperature");
		}
	}

	public double ColorTemperature
	{
		get
		{
			return _colorTemperature;
		}
		set
		{
			SetProperty(ref _colorTemperature, value, "ColorTemperature");
			ColorTempType = (ColorTemperatureType)_colorTemperature;
			SetDisplay?.Invoke(this);
		}
	}

	[JsonIgnore]
	public double ColorTempMin => 1.0;

	[JsonIgnore]
	public double ColorTempMax => 8.0;

	[JsonIgnore]
	public double ColorMin => 0.0;

	[JsonIgnore]
	public double ColorMax => 255.0;

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

	public double Red
	{
		get
		{
			return _red;
		}
		set
		{
			SetProperty(ref _red, value, "Red");
			SetDisplay?.Invoke(this);
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
			SetDisplay?.Invoke(this);
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
			SetDisplay?.Invoke(this);
		}
	}

	[JsonConstructor]
	public DisplaySetting(MonitorCapabilities capabilities, double brightness, double contrast, ColorTemperatureType colorTemperatureType, double red, double green, double blue, MonitorDisplayModeProfile.MonitorSetDisplayDelegate setDisplay = null)
	{
		SetDisplay = setDisplay;
		_brightnessSupported = capabilities.BrightnessSupported;
		_brightness = brightness;
		_contrast = contrast;
		_contrastSupported = capabilities.ContrastSupported;
		_colorTempType = colorTemperatureType;
		_colorTemperature = (int)colorTemperatureType;
		_colorTemperatureSupported = capabilities.ColorTemperatureSupported;
		_red = red;
		_green = green;
		_blue = blue;
		_gammaRampSupported = capabilities.GammaRampSupported;
	}

	public DisplaySetting()
	{
		_brightness = -1.0;
		_red = -1.0;
		_green = -1.0;
		_blue = -1.0;
		_contrast = -1.0;
		_colorTempType = ColorTemperatureType.MC_COLOR_TEMPERATURE_UNKNOWN;
	}

	public void UpdateDisplayDelegate(MonitorDisplayModeProfile.MonitorSetDisplayDelegate setDisplay)
	{
		SetDisplay = setDisplay;
	}

	public void UpdateColor(double Red, double Green, double Blue)
	{
		_red = Red;
		_green = Green;
		_blue = Blue;
		RaisePropertyChanged("Red");
		RaisePropertyChanged("Green");
		RaisePropertyChanged("Blue");
	}

	public void UpdateColorTemperature(double Temp)
	{
		_colorTemperature = Temp;
		RaisePropertyChanged("ColorTemperature");
	}

	public void ApplySettings()
	{
		SetDisplay?.Invoke(this);
		RaisePropertyChanged("Red");
		RaisePropertyChanged("Green");
		RaisePropertyChanged("Blue");
		RaisePropertyChanged("ColorTemperature");
		RaisePropertyChanged("Brightness");
		RaisePropertyChanged("Contrast");
	}
}
