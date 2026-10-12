using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Models.KeyMaps.ISO;
using MobileKeyboardLedMgr.Models.KeyMaps.JIS;
using MobileKeyboardLedMgr.Models.KeyMaps.US;
using MobileKeyboardLedMgr.Models.KingCounty;
using Prism.Events;
using Prism.Mvvm;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.Profiles;

public class KeyboardPattern : BindableBase
{
	private const int KEY_COLOR_BUFFER_SIZE = 126;

	private readonly IEventAggregator _events;

	private readonly string _patternId;

	private readonly PresetPatternsKC _presetPatterns;

	private readonly RGBKeyboardCapabilities _keyboardType;

	private string _name;

	private List<RGBKeyboardDirection> _directions;

	private RGBKeyboardDirection _selectedDirection;

	public ObservableCollection<string> DirectionStrings = new ObservableCollection<string>();

	private RGBKeyboardEffect _selectedEffect;

	private SortedList<int, Color> _availableColors;

	private bool _preset;

	private int _acBrightness;

	private int _dcBrightness;

	private int _speed;

	private bool _editingEnabled;

	private bool _speedEnabled;

	private ObservableCollection<ColorPickerColors> _selectedColors;

	private int _totalColorCount;

	private bool _keyGroupEnabled;

	public string Id => _patternId;

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

	public ObservableCollection<KeyColorData> Keys { get; set; }

	public List<RGBKeyboardDirection> Directions
	{
		get
		{
			return _directions;
		}
		set
		{
			SetProperty(ref _directions, value, "Directions");
		}
	}

	public RGBKeyboardDirection SelectedDirection
	{
		get
		{
			return _selectedDirection;
		}
		set
		{
			SetProperty(ref _selectedDirection, value, "SelectedDirection");
		}
	}

	public string SelectedDirectionString
	{
		get
		{
			return _selectedDirection.ToString();
		}
		set
		{
			foreach (RGBKeyboardDirection direction in _directions)
			{
				if (direction.ToString() == value)
				{
					_selectedDirection = direction;
				}
			}
		}
	}

	public RGBKeyboardEffect SelectedEffect
	{
		get
		{
			return _selectedEffect;
		}
		set
		{
			SetProperty(ref _selectedEffect, value, "SelectedEffect");
		}
	}

	public ObservableCollection<Color> StandardColors { get; set; }

	public ObservableCollection<Color> RecentColors { get; set; }

	public SortedList<int, Color> AvailableColors
	{
		get
		{
			return _availableColors;
		}
		set
		{
			SetProperty(ref _availableColors, value, "AvailableColors");
		}
	}

	public bool Preset
	{
		get
		{
			return _preset;
		}
		set
		{
			SetProperty(ref _preset, value, "Preset");
		}
	}

	public int ACBrightness
	{
		get
		{
			return _acBrightness;
		}
		set
		{
			SetProperty(ref _acBrightness, value, "ACBrightness");
		}
	}

	public int DCBrightness
	{
		get
		{
			return _dcBrightness;
		}
		set
		{
			SetProperty(ref _dcBrightness, value, "DCBrightness");
		}
	}

	public int Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			SetProperty(ref _speed, value, "Speed");
		}
	}

	public bool FrequencySupported { get; private set; }

	public bool EditingEnabled
	{
		get
		{
			return _editingEnabled;
		}
		set
		{
			SetProperty(ref _editingEnabled, value, "EditingEnabled");
		}
	}

	public bool SpeedEnabled
	{
		get
		{
			return _speedEnabled;
		}
		set
		{
			SetProperty(ref _speedEnabled, value, "SpeedEnabled");
		}
	}

	public ObservableCollection<ColorPickerColors> SelectedColors
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

	public int TotalColorCount
	{
		get
		{
			return _totalColorCount;
		}
		set
		{
			SetProperty(ref _totalColorCount, value, "TotalColorCount");
		}
	}

	public bool KeyGroupEnabled
	{
		get
		{
			return _keyGroupEnabled;
		}
		set
		{
			SetProperty(ref _keyGroupEnabled, value, "KeyGroupEnabled");
		}
	}

	public KeyboardPattern(IEventAggregator events, PresetPatternsKC presetPatterns, string id, string name, RGBKeyboardCapabilities keyboardType, ObservableCollection<KeyColorData> keys, List<RGBKeyboardDirection> directions, RGBKeyboardDirection selectedDirection, RGBKeyboardEffect selectedEffect, int acBrightness, int dcBrightness, int speed, bool preset, bool frequencySupported, SortedList<int, Color> colors, ObservableCollection<ColorPickerColors> selectedColors, ObservableCollection<Color> standardColors, ObservableCollection<Color> recentColors, bool editingEnabled, int totalColorCount, bool speedEnabled, bool keyGroupEnabled)
	{
		_events = events;
		_presetPatterns = presetPatterns;
		_patternId = id;
		_name = name;
		_keyboardType = keyboardType;
		Keys = keys;
		_directions = directions;
		_selectedDirection = selectedDirection;
		_selectedEffect = selectedEffect;
		Preset = preset;
		FrequencySupported = frequencySupported;
		if (colors == null)
		{
			throw new InvalidOperationException("Color list cannot be empty");
		}
		_availableColors = colors;
		_selectedColors = selectedColors;
		if (_selectedColors != null && _selectedColors.Count != 0 && _selectedColors[0].Effect != _selectedEffect)
		{
			foreach (ColorPickerColors selectedColor in _selectedColors)
			{
				selectedColor.Effect = _selectedEffect;
			}
		}
		StandardColors = standardColors;
		RecentColors = recentColors;
		_editingEnabled = editingEnabled;
		_totalColorCount = totalColorCount;
		_speedEnabled = speedEnabled;
		_keyGroupEnabled = keyGroupEnabled;
		_acBrightness = acBrightness;
		_dcBrightness = dcBrightness;
		_speed = speed;
		UpdateDirectionStrings(_directions);
	}

	public KeyboardPattern DeepCopy()
	{
		return new KeyboardPattern(_events, _presetPatterns, _patternId, _name, _keyboardType, new ObservableCollection<KeyColorData>(Keys.Select((KeyColorData k) => k.DeepCopy())), _directions, _selectedDirection, _selectedEffect, _acBrightness, _dcBrightness, _speed, _preset, FrequencySupported, new SortedList<int, Color>(_availableColors.ToDictionary((KeyValuePair<int, Color> c) => c.Key, (KeyValuePair<int, Color> c) => Color.FromArgb(c.Value.A, c.Value.R, c.Value.G, c.Value.B))), new ObservableCollection<ColorPickerColors>(_selectedColors.Select((ColorPickerColors c) => c.DeepCopy())), StandardColors, RecentColors, _editingEnabled, _totalColorCount, _speedEnabled, _keyGroupEnabled);
	}

	private void UpdateDirectionStrings(List<RGBKeyboardDirection> directions)
	{
		foreach (RGBKeyboardDirection direction in directions)
		{
			DirectionStrings.Add(direction.ToString());
		}
	}

	public void UpdateColorList(KeyValuePair<int, Color> item, Color newColor)
	{
		if (_availableColors != null && _availableColors.Count != 0 && _availableColors.Count <= TotalColorCount && SelectedEffect != RGBKeyboardEffect.Rainbow)
		{
			_availableColors[item.Key] = newColor;
			UpdateRecentColors(newColor);
			UpdateSelectedColors();
		}
	}

	public void UpdateSelectedColors()
	{
		if (_selectedColors.Count > 0)
		{
			foreach (ColorPickerColors selectedColor in _selectedColors)
			{
				selectedColor.Deselect();
			}
			foreach (KeyValuePair<int, Color> availableColor in _availableColors)
			{
				if (availableColor.Key < _selectedColors.Count)
				{
					_selectedColors[availableColor.Key] = new ColorPickerColors(_events, availableColor.Key, availableColor.Value, _selectedEffect, StandardColors, RecentColors, EditingEnabled, selected: true);
				}
			}
		}
		else
		{
			foreach (KeyValuePair<int, Color> availableColor2 in _availableColors)
			{
				_selectedColors.Add(new ColorPickerColors(_events, availableColor2.Key, availableColor2.Value, _selectedEffect, StandardColors, RecentColors, EditingEnabled, selected: true));
			}
		}
		UpdatePresetPatternKeysWithColors();
		RaisePropertyChanged("SelectedColors");
		RaisePropertyChanged("AvailableColors");
	}

	public void UpdateRecentColors(Color newColor)
	{
		if (!(newColor == DefaultColors.OFF_COLOR_UI) && !(newColor == DefaultColors.OFF_COLOR_LED))
		{
			if (!RecentColors.Contains(newColor))
			{
				RecentColors.RemoveAt(0);
				RecentColors.Add(newColor);
			}
			else
			{
				RecentColors.RemoveAt(RecentColors.IndexOf(newColor));
				RecentColors.Add(newColor);
			}
		}
	}

	public void UpdatePresetPatternKeysWithColors()
	{
		if (_name.Equals(DefaultKeyboardPatterns.PATTERN_AURORA))
		{
			Keys = AuroraPattern.Generate(_presetPatterns.KeyboardType, _availableColors);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_BREATHING))
		{
			Keys = BreathingPattern.Generate(_presetPatterns.KeyboardType, _availableColors);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_GAMING))
		{
			Keys = GamingPattern.Generate(_presetPatterns.KeyboardType, _availableColors);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_MARQUEE))
		{
			Keys = MarqueePattern.Generate(_presetPatterns.KeyboardType, _availableColors);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_MONOCOLOR))
		{
			Keys = MonocolorPattern.Generate(_presetPatterns.KeyboardType, _availableColors[0], enabled: true);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_MUSIC))
		{
			Keys = MusicPattern.Generate(_presetPatterns.KeyboardType, _availableColors, _selectedDirection);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_RAINDROP))
		{
			Keys = RaindropsPattern.Generate(_presetPatterns.KeyboardType, _availableColors);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_REACTIVE))
		{
			Keys = ReactivePattern.Generate(_presetPatterns.KeyboardType, _availableColors);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_RIPPLE))
		{
			Keys = RipplePattern.Generate(_presetPatterns.KeyboardType, _availableColors);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_SPARK))
		{
			Keys = SparkPattern.Generate(_presetPatterns.KeyboardType, _availableColors);
		}
		else if (_name.Equals(DefaultKeyboardPatterns.PATTERN_WAVE))
		{
			Keys = WavePattern.Generate(_presetPatterns.KeyboardType, _availableColors, _selectedDirection);
		}
	}

	public KeyboardLedProfile ToUniwillPatternData()
	{
		List<RGB> list = new List<RGB>();
		if (AvailableColors != null)
		{
			foreach (KeyValuePair<int, Color> availableColor in AvailableColors)
			{
				if (availableColor.Value == DefaultColors.OFF_COLOR_UI)
				{
					list.Add(new RGB
					{
						R = DefaultColors.OFF_COLOR_LED.R,
						G = DefaultColors.OFF_COLOR_LED.G,
						B = DefaultColors.OFF_COLOR_LED.B
					});
				}
				else
				{
					list.Add(new RGB
					{
						R = availableColor.Value.R,
						G = availableColor.Value.G,
						B = availableColor.Value.B
					});
				}
			}
		}
		else
		{
			list = new List<RGB>
			{
				new RGB
				{
					R = byte.MaxValue,
					G = byte.MaxValue,
					B = byte.MaxValue
				}
			};
		}
		RGB[] keyColor = ((SelectedEffect != RGBKeyboardEffect.UserMode) ? new List<RGB>().ToArray() : BuildKeyColorBufferForUserMode());
		int count = list.Count;
		return new KeyboardLedProfile
		{
			Brightness_AC = _acBrightness,
			Brightness_DC = _dcBrightness,
			Color = list.ToArray(),
			ColorLength = count,
			KeyColor = keyColor,
			Direction = _selectedDirection,
			Effect = _selectedEffect,
			Speed = _speed
		};
	}

	private RGB[] BuildKeyColorBufferForUserMode()
	{
		RGB[] array = new RGB[126];
		foreach (KeyColorData key in Keys)
		{
			List<int> list = new List<int>();
			switch (_keyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				list = KeyMapUS.GetIndicesForKey(key.Index);
				break;
			case RGBKeyboardCapabilities.KEY86:
				list = KeyMapIso.GetIndicesForKey(key.Index);
				break;
			case RGBKeyboardCapabilities.KEY87:
				list = KeyMapJis.GetIndicesForKey(key.Index);
				break;
			}
			foreach (int item in list)
			{
				if (key.SelectedColor == DefaultColors.OFF_COLOR_UI)
				{
					array[item].R = DefaultColors.OFF_COLOR_LED.R;
					array[item].G = DefaultColors.OFF_COLOR_LED.G;
					array[item].B = DefaultColors.OFF_COLOR_LED.B;
				}
				else
				{
					array[item].R = key.SelectedColor.R;
					array[item].G = key.SelectedColor.G;
					array[item].B = key.SelectedColor.B;
				}
			}
		}
		return array;
	}

	public SerializableKeyboardPattern GetSerializableKeyboardPattern()
	{
		return new SerializableKeyboardPattern(this);
	}
}
