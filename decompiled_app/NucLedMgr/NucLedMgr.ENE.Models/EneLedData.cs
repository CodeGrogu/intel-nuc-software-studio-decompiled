using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Infrastructure.Converters;
using IntelSdkClient.EneService;
using NucLedMgr.ENE.Enums;
using NucLedMgr.ENE.Events;
using NucLedMgr.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NucLedMgr.ENE.Models;

public class EneLedData : BindableBase
{
	private const EnePatternType DEFAULT_PATTERN = EnePatternType.Off;

	private readonly IEventAggregator _events;

	private readonly IColorManager _colorMgr;

	private bool _firstTimeSetup = true;

	private bool _rainbow;

	private bool _buttonStatus;

	private bool _rainbowSupported;

	private bool _brightnessSupported;

	private int _brightness;

	private int _speed;

	private bool _speedSupported;

	private ObservableCollection<EneColorPickerData> _colorPickerDataSingle;

	private ObservableCollection<EneColorPickerData> _colorPickerDataMulti;

	private ObservableCollection<EneColorPickerData> _colorPickerData;

	private EnePatternType _selectedPattern;

	private string _name;

	private bool _IsEditable;

	private DelegateCommand _updateName;

	public LedPattern PreviousPattern { get; private set; }

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
				foreach (EneColorPickerData colorPickerDatum in ColorPickerData)
				{
					colorPickerDatum.ShowRainbow(value);
				}
				EnableColorPallet();
				PreviousPattern = ToArduinoLedPattern();
				_events.GetEvent<PreviousEneLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
				_events.GetEvent<EneZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), item2: false));
				RefreshUi();
			});
		}
	}

	public bool ButtonStatus
	{
		get
		{
			return _buttonStatus;
		}
		set
		{
			_buttonStatus = value;
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

	public int BrightnessMin => EnePatternManager.BRIGHTNESS_MIN;

	public int BrightnessMax => EnePatternManager.BRIGHTNESS_MAX;

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

	public int SpeedMin => EnePatternManager.SPEED_MIN;

	public int SpeedMax => EnePatternManager.SPEED_MAX;

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

	public ObservableCollection<EneColorPickerData> ColorPickerDataSingle
	{
		get
		{
			return _colorPickerDataSingle;
		}
		set
		{
			SetProperty(ref _colorPickerDataSingle, value, "ColorPickerDataSingle");
		}
	}

	public ObservableCollection<EneColorPickerData> ColorPickerDataMulti
	{
		get
		{
			return _colorPickerDataMulti;
		}
		set
		{
			SetProperty(ref _colorPickerDataMulti, value, "ColorPickerDataMulti");
		}
	}

	public ObservableCollection<EneColorPickerData> ColorPickerData
	{
		get
		{
			return _colorPickerData;
		}
		set
		{
			SetProperty(ref _colorPickerData, value, "ColorPickerData");
		}
	}

	public EnePatternType SelectedPattern
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
				UpdateColorSelection();
				UpdateSupportedFeatures();
				RefreshUi();
				PreviousPattern = ToArduinoLedPattern();
				RefreshUi();
				_events.GetEvent<PreviousEneLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
				_events.GetEvent<EneZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), item2: false));
			});
		}
	}

	public ObservableCollection<EnePatternType> Patterns { get; } = new ObservableCollection<EnePatternType>
	{
		EnePatternType.Off,
		EnePatternType.Solid,
		EnePatternType.Pulse,
		EnePatternType.Breathing,
		EnePatternType.Strobing,
		EnePatternType.PulseTrain1,
		EnePatternType.PulseTrain2,
		EnePatternType.PulseTrain3,
		EnePatternType.Rainbow1
	};

	public EneLedZone Index { get; private set; }

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			SetProperty(ref _name, value, "Name");
		}
	}

	public string InitialName { get; set; }

	public bool IsEditable
	{
		get
		{
			return _IsEditable;
		}
		set
		{
			SetProperty(ref _IsEditable, value, "IsEditable");
		}
	}

	public DelegateCommand UpdateName
	{
		get
		{
			return _updateName;
		}
		set
		{
			SetProperty(ref _updateName, value, "UpdateName");
		}
	}

	public bool IsLedActive { get; set; }

	public EneLedData(IEventAggregator events, IColorManager colorMgr, EneLedZone index, int ledCount)
	{
		_events = events;
		_colorMgr = colorMgr;
		Index = index;
		Name = ResourceDictionaryConverter.GetString("EneLedZone" + Index);
		InitialName = Name;
		_updateName = new DelegateCommand(UpdateZoneName);
		_rainbow = true;
		RainbowSupported = true;
		_selectedPattern = EnePatternType.Solid;
		_colorPickerDataSingle = new ObservableCollection<EneColorPickerData>
		{
			new EneColorPickerData(0, _rainbow, _colorMgr, colorMgr.First, enable: true, 1, ColorChangeOccured),
			new EneColorPickerData(1, _rainbow, _colorMgr, colorMgr.Second, enable: false, 1, ColorChangeOccured),
			new EneColorPickerData(2, _rainbow, _colorMgr, colorMgr.Third, enable: false, 1, ColorChangeOccured)
		};
		_colorPickerDataMulti = new ObservableCollection<EneColorPickerData>
		{
			new EneColorPickerData(0, _rainbow, _colorMgr, colorMgr.First, enable: true, 3, ColorChangeOccured),
			new EneColorPickerData(1, _rainbow, _colorMgr, colorMgr.Second, enable: true, 3, ColorChangeOccured),
			new EneColorPickerData(2, _rainbow, _colorMgr, colorMgr.Third, enable: true, 3, ColorChangeOccured)
		};
		UpdateColorSelection();
		if (ledCount > 0)
		{
			IsLedActive = true;
		}
	}

	public void UpdateFromEneLedPattern(LedPattern pattern)
	{
		if (_firstTimeSetup)
		{
			_firstTimeSetup = false;
		}
		else
		{
			PreviousPattern = ToArduinoLedPattern();
		}
		_selectedPattern = (EnePatternType)Enum.Parse(typeof(EnePatternType), pattern.PatternIndex.ToString());
		int num = NumberColorsSupported();
		if (num == 1 && pattern.Palette.Length >= 1)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				ColorPickerDataSingle[0].UpdateUiColorHsv(pattern.Palette[0]);
			});
		}
		else if (num == 3 && pattern.Palette.Length >= 3)
		{
			_colorMgr.First = EneColorPickerData.ColorFromFastLedHsv(pattern.Palette[0]);
			_colorMgr.Second = EneColorPickerData.ColorFromFastLedHsv(pattern.Palette[1]);
			_colorMgr.Third = EneColorPickerData.ColorFromFastLedHsv(pattern.Palette[2]);
			Application.Current.Dispatcher.Invoke(() =>
			{
				ColorPickerDataMulti[0].UpdateUiColorHsv(pattern.Palette[0]);
				ColorPickerDataMulti[1].UpdateUiColorHsv(pattern.Palette[1]);
				ColorPickerDataMulti[2].UpdateUiColorHsv(pattern.Palette[2]);
			});
		}
		_rainbow = _selectedPattern != EnePatternType.PulseTrain3 && _selectedPattern != EnePatternType.PulseTrain2 && _selectedPattern != EnePatternType.PulseTrain1 && Convert.ToBoolean(pattern.RainBow);
		foreach (EneColorPickerData item in _colorPickerDataMulti)
		{
			item.ShowRainbow(_rainbow);
		}
		_colorPickerDataSingle[0].ShowRainbow(_rainbow);
		UpdateColorSelection();
		_brightness = Convert.ToInt32(pattern.MaxBrightness);
		_speed = Convert.ToInt32(pattern.Speed);
		UpdateSupportedFeatures();
		RefreshUi();
	}

	private LedPattern ToArduinoLedPattern()
	{
		byte channel = EneLedHelper.ConvertZoneToChannel(Index);
		byte patternIndex = Convert.ToByte(_selectedPattern);
		LedPattern ledPattern = new LedPattern();
		switch (_selectedPattern)
		{
		case EnePatternType.Off:
			ledPattern.Palette = new byte[1];
			ledPattern.Channel = channel;
			ledPattern.PatternIndex = patternIndex;
			return ledPattern;
		case EnePatternType.Solid:
		case EnePatternType.Pulse:
		case EnePatternType.Breathing:
		case EnePatternType.Strobing:
			ledPattern.Palette = new byte[1];
			ledPattern.Channel = channel;
			ledPattern.PatternIndex = patternIndex;
			ledPattern.Palette[0] = FirstColorToPalette();
			if (Brightness == 255)
			{
				ledPattern.MaxBrightness = Convert.ToByte(EnePatternManager.BRIGHTNESS_MAX);
			}
			else
			{
				ledPattern.MaxBrightness = Convert.ToByte(Brightness);
			}
			ledPattern.RainBow = Convert.ToByte(Rainbow);
			if (Speed == 255)
			{
				ledPattern.Speed = Convert.ToByte(EnePatternManager.DEFAULT_SPEED);
			}
			else
			{
				ledPattern.Speed = Convert.ToByte(Speed);
			}
			return ledPattern;
		case EnePatternType.PulseTrain1:
		case EnePatternType.PulseTrain2:
		case EnePatternType.PulseTrain3:
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
				ledPattern.MaxBrightness = Convert.ToByte(EnePatternManager.BRIGHTNESS_MAX);
			}
			else
			{
				ledPattern.MaxBrightness = Convert.ToByte(Brightness);
			}
			if (Speed == 255)
			{
				ledPattern.Speed = Convert.ToByte(EnePatternManager.DEFAULT_SPEED);
			}
			else
			{
				ledPattern.Speed = Convert.ToByte(Speed);
			}
			return ledPattern;
		}
		case EnePatternType.Rainbow1:
			ledPattern.Channel = channel;
			ledPattern.PatternIndex = patternIndex;
			if (Brightness == 255)
			{
				ledPattern.MaxBrightness = Convert.ToByte(EnePatternManager.BRIGHTNESS_MAX);
			}
			else
			{
				ledPattern.MaxBrightness = Convert.ToByte(Brightness);
			}
			return ledPattern;
		default:
			return null;
		}
	}

	private byte FirstColorToPalette()
	{
		return (byte)_colorPickerData.First().SelectedColorValue;
	}

	private byte[] ThreeColorToPalette()
	{
		return new byte[3]
		{
			(byte)_colorPickerData[0].SelectedColorValue,
			(byte)_colorPickerData[1].SelectedColorValue,
			(byte)_colorPickerData[2].SelectedColorValue
		};
	}

	private void ColorChangeOccured(bool SetOnly)
	{
		PreviousPattern = ToArduinoLedPattern();
		_events.GetEvent<PreviousEneLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
		_events.GetEvent<EneZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), SetOnly));
	}

	private int NumberColorsSupported()
	{
		switch (_selectedPattern)
		{
		case EnePatternType.Off:
		case EnePatternType.Rainbow1:
			return 0;
		case EnePatternType.Solid:
		case EnePatternType.Pulse:
		case EnePatternType.Breathing:
		case EnePatternType.Strobing:
			return 1;
		case EnePatternType.PulseTrain1:
		case EnePatternType.PulseTrain2:
		case EnePatternType.PulseTrain3:
			return 3;
		default:
			return 0;
		}
	}

	private void UpdateColorSelection()
	{
		switch (_selectedPattern)
		{
		case EnePatternType.Off:
			_colorPickerData[0].EnableColor = false;
			_colorPickerData[1].EnableColor = false;
			_colorPickerData[2].EnableColor = false;
			break;
		case EnePatternType.Solid:
		case EnePatternType.Pulse:
		case EnePatternType.Breathing:
		case EnePatternType.Strobing:
			_colorPickerData = _colorPickerDataSingle;
			EnableColorPallet();
			UpdateRainbow();
			break;
		case EnePatternType.PulseTrain1:
		case EnePatternType.PulseTrain2:
		case EnePatternType.PulseTrain3:
			_colorPickerData = _colorPickerDataMulti;
			_colorPickerData[0].EnableColor = true;
			_colorPickerData[1].EnableColor = true;
			_colorPickerData[2].EnableColor = true;
			UpdateRainbow();
			break;
		case EnePatternType.Rainbow1:
			_colorPickerData[0].EnableColor = false;
			_colorPickerData[1].EnableColor = false;
			_colorPickerData[2].EnableColor = false;
			break;
		}
	}

	private void UpdateRainbow()
	{
		foreach (EneColorPickerData colorPickerDatum in ColorPickerData)
		{
			colorPickerDatum.Rainbow = Rainbow;
		}
	}

	private void UpdateSupportedFeatures()
	{
		switch (_selectedPattern)
		{
		case EnePatternType.Off:
			_brightnessSupported = false;
			_speedSupported = false;
			_rainbowSupported = false;
			_rainbow = false;
			break;
		case EnePatternType.Solid:
			_brightnessSupported = true;
			_speedSupported = false;
			_rainbowSupported = true;
			break;
		case EnePatternType.Pulse:
		case EnePatternType.Breathing:
		case EnePatternType.Strobing:
			_brightnessSupported = true;
			_speedSupported = true;
			_rainbowSupported = true;
			break;
		case EnePatternType.PulseTrain1:
		case EnePatternType.PulseTrain2:
		case EnePatternType.PulseTrain3:
			_brightnessSupported = true;
			_speedSupported = true;
			_rainbowSupported = false;
			_rainbow = false;
			break;
		case EnePatternType.Rainbow1:
			_brightnessSupported = true;
			_speedSupported = false;
			_rainbowSupported = false;
			_rainbow = true;
			break;
		}
	}

	public void BrightnessSelectionChanged(object sender, EventArgs args)
	{
		double value = ((Slider)((MouseButtonEventArgs)args).Source).Value;
		_brightness = (int)value;
		Task.Run(() =>
		{
			PreviousPattern = ToArduinoLedPattern();
			_events.GetEvent<PreviousEneLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
			_events.GetEvent<EneZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), item2: false));
		});
	}

	public void SpeedSelectionChanged(object sender, EventArgs args)
	{
		double value = ((Slider)((MouseButtonEventArgs)args).Source).Value;
		_speed = (int)value;
		Task.Run(() =>
		{
			PreviousPattern = ToArduinoLedPattern();
			_events.GetEvent<PreviousEneLedSetEvent>().Publish(new List<LedPattern> { PreviousPattern });
			_events.GetEvent<EneZoneSettingChangeEvent>().Publish(new Tuple<LedPattern, bool>(ToArduinoLedPattern(), item2: false));
		});
	}

	public void EnableColorPallet()
	{
		if (_rainbow)
		{
			ColorPickerData[0].EnableColor = false;
		}
		else
		{
			ColorPickerData[0].EnableColor = true;
		}
	}

	private void UpdateMultiColor()
	{
		_colorPickerDataMulti[0].UpdateUiColor(_colorMgr.First);
		_colorPickerDataMulti[1].UpdateUiColor(_colorMgr.Second);
		_colorPickerDataMulti[2].UpdateUiColor(_colorMgr.Third);
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
		RaisePropertyChanged("ColorPickerData");
		RaisePropertyChanged("ButtonStatus");
	}

	private void UpdateZoneName()
	{
		IsEditable = true;
	}

	public void SetPreviousName(object sender, EventArgs args)
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			Name = InitialName;
			IsEditable = false;
		}
	}
}
