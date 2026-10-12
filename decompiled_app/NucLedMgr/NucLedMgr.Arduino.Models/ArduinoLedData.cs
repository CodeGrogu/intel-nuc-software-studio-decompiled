using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Infrastructure.Converters;
using IntelSdkClient.ArduinoService;
using NucLedMgr.Arduino.Enums;
using NucLedMgr.Arduino.Events;
using NucLedMgr.Interfaces;
using Prism.Events;
using Prism.Mvvm;

namespace NucLedMgr.Arduino.Models;

public class ArduinoLedData : BindableBase
{
	private const string ZONE_STRING_RESOURCE_KEY = "ArduinoLedZone";

	private const ArduinoPatternType DEFAULT_PATTERN = ArduinoPatternType.Off;

	private readonly IEventAggregator _events;

	private readonly IColorManager _colorMgr;

	private bool _firstTimeSetup = true;

	private int _brightness;

	private bool _brightnessSupported;

	private int _speed;

	private bool _speedSupported;

	private bool _rainbow;

	private bool _rainbowSupported;

	private ArduinoPatternType _selectedPattern;

	private readonly ArduinoColorPickerData _singleColor;

	private readonly ObservableCollection<ArduinoColorPickerData> _multiColors;

	private ObservableCollection<ArduinoColorPickerData> _selectedColors;

	public MonsterCoveZone Index { get; private set; }

	public string Name { get; set; }

	public LedPattern PreviousPattern { get; private set; }

	public Color SelectedColor
	{
		get
		{
			return _singleColor.SelectedColor;
		}
		set
		{
			_singleColor.SelectedColor = value;
		}
	}

	public ArduinoPatternType SelectedPattern
	{
		get
		{
			return _selectedPattern;
		}
		set
		{
			Task.Run(() =>
			{
				SetProperty(ref _selectedPattern, value, "SelectedPattern");
				UpdateMultiColor();
				UpdateColorSelection();
				UpdateSupportedFeatures();
				if (value != ArduinoPatternType.Off)
				{
					PreviousPattern = ToArduinoLedPattern();
				}
				RefreshUi();
				_events.GetEvent<PreviousLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
				_events.GetEvent<ArduinoZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), item2: false));
				_events.GetEvent<ArduinoZoneColorPatternChangeEvent>().Publish(Index);
			});
		}
	}

	public ObservableCollection<ArduinoPatternType> Patterns { get; } = new ObservableCollection<ArduinoPatternType>
	{
		ArduinoPatternType.Off,
		ArduinoPatternType.Solid,
		ArduinoPatternType.Pulse,
		ArduinoPatternType.Breathing,
		ArduinoPatternType.Strobing,
		ArduinoPatternType.PulseTrain1,
		ArduinoPatternType.PulseTrain2,
		ArduinoPatternType.PulseTrain3,
		ArduinoPatternType.Rainbow1
	};

	public int BrightnessMin => ArduinoPatternManager.BRIGHTNESS_MIN;

	public int BrightnessMax => ArduinoPatternManager.BRIGHTNESS_MAX;

	public int Brightness
	{
		get
		{
			return _brightness;
		}
		set
		{
			Task.Run(() =>
			{
				SetProperty(ref _brightness, value, "Brightness");
			});
		}
	}

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

	public int SpeedMin => ArduinoPatternManager.SPEED_MIN;

	public int SpeedMax => ArduinoPatternManager.SPEED_MAX;

	public int Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			Task.Run(() =>
			{
				SetProperty(ref _speed, value, "Speed");
			});
		}
	}

	public bool SpeedSupported
	{
		get
		{
			return _speedSupported;
		}
		set
		{
			SetProperty(ref _speedSupported, value, "SpeedSupported");
		}
	}

	public bool Rainbow
	{
		get
		{
			return _rainbow;
		}
		set
		{
			Task.Run(() =>
			{
				SetProperty(ref _rainbow, value, "Rainbow");
				foreach (ArduinoColorPickerData selectedColor in SelectedColors)
				{
					selectedColor.ShowRainbow(value);
				}
				PreviousPattern = ToArduinoLedPattern();
				_events.GetEvent<PreviousLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
				_events.GetEvent<ArduinoZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), item2: false));
				_events.GetEvent<ArduinoZoneColorPatternChangeEvent>().Publish(Index);
			});
		}
	}

	public bool RainbowSupported
	{
		get
		{
			return _rainbowSupported;
		}
		set
		{
			SetProperty(ref _rainbowSupported, value, "RainbowSupported");
		}
	}

	public ObservableCollection<ArduinoColorPickerData> SelectedColors
	{
		get
		{
			return _selectedColors;
		}
		set
		{
			SetProperty(ref _selectedColors, value, "SelectedColors");
		}
	}

	public ArduinoLedData(IEventAggregator events, IColorManager colorMgr, MonsterCoveZone index, LedPattern previous = null)
	{
		_events = events;
		_colorMgr = colorMgr;
		_rainbow = false;
		_speed = ArduinoPatternManager.SPEED_MIN;
		_brightness = ArduinoPatternManager.BRIGHTNESS_MAX;
		Index = index;
		Name = ResourceDictionaryConverter.GetString("ArduinoLedZone" + Index);
		_selectedPattern = ArduinoPatternType.Off;
		_singleColor = new ArduinoColorPickerData(0, _rainbow, _colorMgr, colorMgr.First, ColorChangeOccured);
		_multiColors = new ObservableCollection<ArduinoColorPickerData>
		{
			new ArduinoColorPickerData(0, _rainbow, _colorMgr, colorMgr.First, ColorChangeOccured),
			new ArduinoColorPickerData(1, _rainbow, _colorMgr, colorMgr.Second, ColorChangeOccured),
			new ArduinoColorPickerData(2, _rainbow, _colorMgr, colorMgr.Third, ColorChangeOccured)
		};
		UpdateColorSelection();
		UpdateSupportedFeatures();
		if (previous == null)
		{
			PreviousPattern = ToArduinoLedPattern();
		}
		else
		{
			PreviousPattern = previous;
		}
	}

	public void SpeedSelectionChanged(object sender, EventArgs args)
	{
		double value = ((Slider)((MouseButtonEventArgs)args).Source).Value;
		_speed = (int)value;
		Task.Run(() =>
		{
			PreviousPattern = ToArduinoLedPattern();
			_events.GetEvent<PreviousLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
			_events.GetEvent<ArduinoZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), item2: false));
		});
	}

	public void BrightnessSelectionChanged(object sender, EventArgs args)
	{
		double value = ((Slider)((MouseButtonEventArgs)args).Source).Value;
		_brightness = (int)value;
		Task.Run(() =>
		{
			PreviousPattern = ToArduinoLedPattern();
			_events.GetEvent<PreviousLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
			_events.GetEvent<ArduinoZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), item2: false));
		});
	}

	private void ColorChangeOccured(bool SetOnly)
	{
		PreviousPattern = ToArduinoLedPattern();
		_events.GetEvent<PreviousLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
		_events.GetEvent<ArduinoZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), SetOnly));
	}

	public void UpdateFromArduinoLedPattern(LedPattern pattern)
	{
		if (_firstTimeSetup)
		{
			_firstTimeSetup = false;
		}
		else
		{
			PreviousPattern = ToArduinoLedPattern();
		}
		_selectedPattern = (ArduinoPatternType)Enum.Parse(typeof(ArduinoPatternType), pattern.PatternIndex.ToString());
		int num = NumberColorsSupported();
		if (num == 1 && pattern.Palette.Length >= 1)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				_singleColor.UpdateUiColorHsv(pattern.Palette[0]);
			});
		}
		else if (num == 3 && pattern.Palette.Length >= 3)
		{
			_colorMgr.First = ArduinoColorPickerData.ColorFromFastLedHsv(pattern.Palette[0]);
			_colorMgr.Second = ArduinoColorPickerData.ColorFromFastLedHsv(pattern.Palette[1]);
			_colorMgr.Third = ArduinoColorPickerData.ColorFromFastLedHsv(pattern.Palette[2]);
			Application.Current.Dispatcher.Invoke(() =>
			{
				_multiColors[0].UpdateUiColorHsv(pattern.Palette[0]);
				_multiColors[1].UpdateUiColorHsv(pattern.Palette[1]);
				_multiColors[2].UpdateUiColorHsv(pattern.Palette[2]);
			});
		}
		_rainbow = _selectedPattern != ArduinoPatternType.PulseTrain3 && _selectedPattern != ArduinoPatternType.PulseTrain2 && _selectedPattern != ArduinoPatternType.PulseTrain1 && Convert.ToBoolean(pattern.RainBow);
		foreach (ArduinoColorPickerData multiColor in _multiColors)
		{
			multiColor.ShowRainbow(_rainbow);
		}
		_singleColor.ShowRainbow(_rainbow);
		UpdateColorSelection();
		_brightness = Convert.ToInt32(pattern.MaxBrightness);
		_speed = Convert.ToInt32(pattern.Speed);
		UpdateSupportedFeatures();
		RefreshUi();
	}

	private void RefreshUi()
	{
		RaisePropertyChanged("SelectedPattern");
		RaisePropertyChanged("Rainbow");
		RaisePropertyChanged("Brightness");
		RaisePropertyChanged("Speed");
		RaisePropertyChanged("BrightnessSupported");
		RaisePropertyChanged("SpeedSupported");
		RaisePropertyChanged("RainbowSupported");
		RaisePropertyChanged("SelectedColors");
	}

	private void UpdateMultiColor()
	{
		_multiColors[0].UpdateUiColor(_colorMgr.First);
		_multiColors[1].UpdateUiColor(_colorMgr.Second);
		_multiColors[2].UpdateUiColor(_colorMgr.Third);
	}

	public LedPattern ToArduinoLedPattern()
	{
		byte channel = SerialDeviceClient.ConvertZoneToChannel(Index);
		byte patternIndex = Convert.ToByte(_selectedPattern);
		LedPattern ledPattern = new LedPattern();
		switch (_selectedPattern)
		{
		case ArduinoPatternType.Off:
			ledPattern.Palette = new byte[1];
			ledPattern.Channel = channel;
			ledPattern.PatternIndex = patternIndex;
			return ledPattern;
		case ArduinoPatternType.Solid:
		case ArduinoPatternType.Pulse:
		case ArduinoPatternType.Breathing:
		case ArduinoPatternType.Strobing:
			ledPattern.Palette = new byte[1];
			ledPattern.Channel = channel;
			ledPattern.PatternIndex = patternIndex;
			ledPattern.Palette[0] = FirstColorToPalette();
			if (Brightness == 255)
			{
				ledPattern.MaxBrightness = Convert.ToByte(ArduinoPatternManager.BRIGHTNESS_MAX);
			}
			else
			{
				ledPattern.MaxBrightness = Convert.ToByte(Brightness);
			}
			ledPattern.RainBow = Convert.ToByte(Rainbow);
			if (Speed == 255)
			{
				ledPattern.Speed = Convert.ToByte(ArduinoPatternManager.DEFAULT_SPEED);
			}
			else
			{
				ledPattern.Speed = Convert.ToByte(Speed);
			}
			return ledPattern;
		case ArduinoPatternType.PulseTrain1:
		case ArduinoPatternType.PulseTrain2:
		case ArduinoPatternType.PulseTrain3:
		{
			byte[] array = ThreeColorToPalette();
			ledPattern.Palette = new byte[3];
			ledPattern.Channel = channel;
			ledPattern.PatternIndex = patternIndex;
			ledPattern.Palette[0] = array[0];
			ledPattern.Palette[1] = array[1];
			ledPattern.Palette[2] = array[2];
			if (Brightness == 255)
			{
				ledPattern.MaxBrightness = Convert.ToByte(ArduinoPatternManager.BRIGHTNESS_MAX);
			}
			else
			{
				ledPattern.MaxBrightness = Convert.ToByte(Brightness);
			}
			if (Speed == 255)
			{
				ledPattern.Speed = Convert.ToByte(ArduinoPatternManager.DEFAULT_SPEED);
			}
			else
			{
				ledPattern.Speed = Convert.ToByte(Speed);
			}
			return ledPattern;
		}
		case ArduinoPatternType.Rainbow1:
			ledPattern.Channel = channel;
			ledPattern.PatternIndex = patternIndex;
			if (Brightness == 255)
			{
				ledPattern.MaxBrightness = Convert.ToByte(ArduinoPatternManager.BRIGHTNESS_MAX);
			}
			else
			{
				ledPattern.MaxBrightness = Convert.ToByte(Brightness);
			}
			return ledPattern;
		case ArduinoPatternType.Rainbow2:
			ledPattern.Palette = new byte[1];
			ledPattern.Channel = channel;
			ledPattern.PatternIndex = patternIndex;
			ledPattern.Palette[0] = FirstColorToPalette();
			if (Brightness == 255)
			{
				ledPattern.MaxBrightness = Convert.ToByte(ArduinoPatternManager.BRIGHTNESS_MAX);
			}
			else
			{
				ledPattern.MaxBrightness = Convert.ToByte(Brightness);
			}
			ledPattern.RainBow = 1;
			return ledPattern;
		default:
			return null;
		}
	}

	private byte FirstColorToPalette()
	{
		return (byte)_selectedColors.First().SelectedColorValue;
	}

	private byte[] ThreeColorToPalette()
	{
		return new byte[3]
		{
			(byte)_selectedColors[0].SelectedColorValue,
			(byte)_selectedColors[1].SelectedColorValue,
			(byte)_selectedColors[2].SelectedColorValue
		};
	}

	private void UpdateColorSelection()
	{
		switch (_selectedPattern)
		{
		case ArduinoPatternType.Off:
			_selectedColors = new ObservableCollection<ArduinoColorPickerData>();
			break;
		case ArduinoPatternType.Solid:
		case ArduinoPatternType.Pulse:
		case ArduinoPatternType.Breathing:
		case ArduinoPatternType.Strobing:
			_selectedColors = new ObservableCollection<ArduinoColorPickerData> { _singleColor };
			UpdateRainbow();
			break;
		case ArduinoPatternType.PulseTrain1:
		case ArduinoPatternType.PulseTrain2:
		case ArduinoPatternType.PulseTrain3:
			_selectedColors = _multiColors;
			UpdateRainbow();
			break;
		case ArduinoPatternType.Rainbow1:
			_selectedColors = new ObservableCollection<ArduinoColorPickerData> { _singleColor };
			break;
		case ArduinoPatternType.Rainbow2:
			_selectedColors = new ObservableCollection<ArduinoColorPickerData> { _singleColor };
			UpdateRainbow();
			break;
		}
		_events.GetEvent<ArduinoZoneColorPatternChangeEvent>().Publish(Index);
	}

	private int NumberColorsSupported()
	{
		switch (_selectedPattern)
		{
		case ArduinoPatternType.Off:
		case ArduinoPatternType.Rainbow1:
			return 0;
		case ArduinoPatternType.Solid:
		case ArduinoPatternType.Pulse:
		case ArduinoPatternType.Breathing:
		case ArduinoPatternType.Strobing:
		case ArduinoPatternType.Rainbow2:
			return 1;
		case ArduinoPatternType.PulseTrain1:
		case ArduinoPatternType.PulseTrain2:
		case ArduinoPatternType.PulseTrain3:
			return 3;
		default:
			return 0;
		}
	}

	private void UpdateRainbow()
	{
		foreach (ArduinoColorPickerData selectedColor in SelectedColors)
		{
			selectedColor.Rainbow = Rainbow;
		}
	}

	private void UpdateSupportedFeatures()
	{
		switch (_selectedPattern)
		{
		case ArduinoPatternType.Off:
			_brightnessSupported = false;
			_speedSupported = false;
			_rainbowSupported = false;
			_rainbow = false;
			break;
		case ArduinoPatternType.Solid:
			_brightnessSupported = true;
			_speedSupported = false;
			_rainbowSupported = true;
			break;
		case ArduinoPatternType.Pulse:
		case ArduinoPatternType.Breathing:
		case ArduinoPatternType.Strobing:
			_brightnessSupported = true;
			_speedSupported = true;
			_rainbowSupported = true;
			break;
		case ArduinoPatternType.PulseTrain1:
		case ArduinoPatternType.PulseTrain2:
		case ArduinoPatternType.PulseTrain3:
			_brightnessSupported = true;
			_speedSupported = true;
			_rainbowSupported = false;
			_rainbow = false;
			break;
		case ArduinoPatternType.Rainbow1:
			_brightnessSupported = true;
			_speedSupported = false;
			_rainbowSupported = false;
			_rainbow = true;
			break;
		case ArduinoPatternType.Rainbow2:
			_brightnessSupported = true;
			_speedSupported = false;
			_rainbowSupported = false;
			if (Index == MonsterCoveZone.Skull)
			{
				_rainbow = false;
			}
			else
			{
				_rainbow = true;
			}
			break;
		}
	}
}
