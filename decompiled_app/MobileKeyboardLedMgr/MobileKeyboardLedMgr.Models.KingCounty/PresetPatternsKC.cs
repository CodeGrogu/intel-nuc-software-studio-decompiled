using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Infrastructure.Converters;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Interfaces;
using MobileKeyboardLedMgr.Models.Profiles;
using MobileKeyboardLedMgr.Storage.Data;
using Prism.Events;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

public class PresetPatternsKC
{
	public const int KEYBOARD_ROW_COUNT_KC = 6;

	private readonly IKeyboardColorManager _colorMgr;

	private readonly IEventAggregator _events;

	private readonly ISettingsFileManager _settingsFileManager;

	private readonly ObservableCollection<Color> StandardColors;

	private readonly ObservableCollection<Color> RecentColors;

	public List<RGBKeyboardEffect> Effects;

	public RGBKeyboardCapabilities KeyboardType;

	public KeyboardPattern DefaultPattern { get; }

	public List<KeyboardPattern> Patterns { get; }

	public PresetPatternsKC(IKeyboardColorManager colorMgr, IEventAggregator events, ISettingsFileManager settingsFileManager, RGBKeyboardCapabilities keyboardType)
	{
		_colorMgr = colorMgr;
		_events = events;
		_settingsFileManager = settingsFileManager;
		KeyboardType = keyboardType;
		StandardColors = _colorMgr.StandardColors;
		RecentColors = _colorMgr.RecentColors;
		Effects = new List<RGBKeyboardEffect>
		{
			RGBKeyboardEffect.Monocolor,
			RGBKeyboardEffect.Rainbow,
			RGBKeyboardEffect.GamingMode,
			RGBKeyboardEffect.Breathing,
			RGBKeyboardEffect.Raindrop,
			RGBKeyboardEffect.Marquee,
			RGBKeyboardEffect.Music,
			RGBKeyboardEffect.Wave,
			RGBKeyboardEffect.Reactive,
			RGBKeyboardEffect.Ripple,
			RGBKeyboardEffect.Spark,
			RGBKeyboardEffect.Aurora,
			RGBKeyboardEffect.UserMode
		};
		Patterns = new List<KeyboardPattern>
		{
			Monocolor(),
			Rainbow(),
			Gaming(),
			Breathing(),
			Raindrop(),
			Marquee(),
			Music(),
			Wave(),
			Reactive(),
			Ripple(),
			Spark(),
			Aurora(),
			Custom()
		};
		if (_settingsFileManager.CustomDefaultSettings != null)
		{
			switch (_settingsFileManager.CustomDefaultSettings.DefaultEffect)
			{
			case "Monocolor":
				DefaultPattern = Monocolor();
				break;
			case "Rainbow":
				DefaultPattern = Rainbow();
				break;
			case "GamingMode":
				DefaultPattern = Gaming();
				break;
			case "Breathing":
				DefaultPattern = Breathing();
				break;
			case "Raindrop":
				DefaultPattern = Raindrop();
				break;
			case "Marquee":
				DefaultPattern = Marquee();
				break;
			case "Music":
				DefaultPattern = Music();
				break;
			case "Wave":
				DefaultPattern = Wave();
				break;
			case "Reactive":
				DefaultPattern = Reactive();
				break;
			case "Ripple":
				DefaultPattern = Ripple();
				break;
			case "Spark":
				DefaultPattern = Spark();
				break;
			case "Aurora":
				DefaultPattern = Aurora();
				break;
			default:
				DefaultPattern = Monocolor();
				break;
			}
		}
		else
		{
			DefaultPattern = Monocolor();
		}
	}

	public PresetPatternsKC DeepCopy()
	{
		return new PresetPatternsKC(_colorMgr, _events, _settingsFileManager, KeyboardType);
	}

	private KeyboardPattern Monocolor()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Monocolor");
		SortedList<int, Color> sortedList = ((pattern?.Colors == null) ? new SortedList<int, Color> { 
		{
			0,
			DefaultColors.MONOCOLOR
		} } : new SortedList<int, Color> { 
		{
			0,
			pattern.Colors[0]
		} });
		return new KeyboardPattern(_events, this, "Monocolor", ResourceDictionaryConverter.GetString("MonocolorString"), KeyboardType, MonocolorPattern.Generate(KeyboardType, sortedList[0], enabled: true), new List<RGBKeyboardDirection> { RGBKeyboardDirection.None }, RGBKeyboardDirection.None, RGBKeyboardEffect.Monocolor, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 0, preset: true, frequencySupported: false, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Monocolor), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: false, keyGroupEnabled: false);
	}

	private KeyboardPattern Rainbow()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Rainbow");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		return new KeyboardPattern(_events, this, "Rainbow", ResourceDictionaryConverter.GetString("RainbowString"), KeyboardType, RainbowPattern.Generate(KeyboardType, sortedList.Values.ToList()), new List<RGBKeyboardDirection> { RGBKeyboardDirection.None }, RGBKeyboardDirection.None, RGBKeyboardEffect.Rainbow, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 0, preset: true, frequencySupported: false, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Rainbow), StandardColors, RecentColors, editingEnabled: false, sortedList.Count, speedEnabled: false, keyGroupEnabled: false);
	}

	private KeyboardPattern Gaming()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "GamingMode");
		SortedList<int, Color> sortedList = ((pattern?.Colors == null) ? new SortedList<int, Color> { 
		{
			0,
			DefaultColors.GAMING
		} } : new SortedList<int, Color> { 
		{
			0,
			pattern.Colors[0]
		} });
		return new KeyboardPattern(_events, this, "GamingMode", ResourceDictionaryConverter.GetString("GamingModeString"), KeyboardType, GamingPattern.Generate(KeyboardType, sortedList), new List<RGBKeyboardDirection> { RGBKeyboardDirection.None }, RGBKeyboardDirection.None, RGBKeyboardEffect.GamingMode, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 0, preset: true, frequencySupported: false, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.GamingMode), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: false, keyGroupEnabled: false);
	}

	private KeyboardPattern Breathing()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Breathing");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		return new KeyboardPattern(_events, this, "Breathing", ResourceDictionaryConverter.GetString("BreathingString"), KeyboardType, BreathingPattern.Generate(KeyboardType, sortedList), new List<RGBKeyboardDirection> { RGBKeyboardDirection.None }, RGBKeyboardDirection.None, RGBKeyboardEffect.Breathing, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 5, preset: true, frequencySupported: true, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Breathing), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: true, keyGroupEnabled: false);
	}

	private KeyboardPattern Raindrop()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Raindrop");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		return new KeyboardPattern(_events, this, "Raindrop", ResourceDictionaryConverter.GetString("RaindropString"), KeyboardType, RaindropsPattern.Generate(KeyboardType, sortedList), new List<RGBKeyboardDirection> { RGBKeyboardDirection.None }, RGBKeyboardDirection.None, RGBKeyboardEffect.Raindrop, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 5, preset: true, frequencySupported: true, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Raindrop), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: true, keyGroupEnabled: false);
	}

	private KeyboardPattern Marquee()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Marquee");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		return new KeyboardPattern(_events, this, "Marquee", ResourceDictionaryConverter.GetString("MarqueeString"), KeyboardType, MarqueePattern.Generate(KeyboardType, sortedList), new List<RGBKeyboardDirection> { RGBKeyboardDirection.None }, RGBKeyboardDirection.None, RGBKeyboardEffect.Marquee, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 5, preset: true, frequencySupported: true, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Marquee), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: true, keyGroupEnabled: false);
	}

	private KeyboardPattern Music()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Music");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		return new KeyboardPattern(_events, this, "Music", ResourceDictionaryConverter.GetString("MusicString"), KeyboardType, MusicPattern.Generate(KeyboardType, sortedList, RGBKeyboardDirection.None), new List<RGBKeyboardDirection>
		{
			RGBKeyboardDirection.None,
			RGBKeyboardDirection.Right,
			RGBKeyboardDirection.Left,
			RGBKeyboardDirection.Up
		}, RGBKeyboardDirection.None, RGBKeyboardEffect.Music, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 0, preset: true, frequencySupported: false, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Music), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: false, keyGroupEnabled: false);
	}

	private KeyboardPattern Wave()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Wave");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		RGBKeyboardDirection rGBKeyboardDirection = ((pattern == null || pattern.Behavior == null) ? RGBKeyboardDirection.Right : (pattern.Behavior switch
		{
			"Left" => RGBKeyboardDirection.Left, 
			"Right" => RGBKeyboardDirection.Right, 
			"Up" => RGBKeyboardDirection.Up, 
			"Down" => RGBKeyboardDirection.Down, 
			_ => RGBKeyboardDirection.Right, 
		}));
		return new KeyboardPattern(_events, this, "Wave", ResourceDictionaryConverter.GetString("WaveString"), KeyboardType, WavePattern.Generate(KeyboardType, sortedList, rGBKeyboardDirection), new List<RGBKeyboardDirection>
		{
			RGBKeyboardDirection.Right,
			RGBKeyboardDirection.Left,
			RGBKeyboardDirection.Up,
			RGBKeyboardDirection.Down
		}, rGBKeyboardDirection, RGBKeyboardEffect.Wave, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 5, preset: true, frequencySupported: true, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Wave), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: true, keyGroupEnabled: false);
	}

	private KeyboardPattern Reactive()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Reactive");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		RGBKeyboardDirection selectedDirection;
		if (pattern != null && pattern.Behavior != null)
		{
			string behavior = pattern.Behavior;
			if (behavior == "Auto")
			{
				selectedDirection = RGBKeyboardDirection.Auto;
			}
			else
			{
				selectedDirection = ((!(behavior == "PressAnyKey")) ? RGBKeyboardDirection.Auto : RGBKeyboardDirection.PressAnyKey);
			}
		}
		else
		{
			selectedDirection = RGBKeyboardDirection.Auto;
		}
		return new KeyboardPattern(_events, this, "Reactive", ResourceDictionaryConverter.GetString("ReactiveString"), KeyboardType, ReactivePattern.Generate(KeyboardType, sortedList), new List<RGBKeyboardDirection>
		{
			RGBKeyboardDirection.Auto,
			RGBKeyboardDirection.PressAnyKey
		}, selectedDirection, RGBKeyboardEffect.Reactive, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 5, preset: true, frequencySupported: true, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Reactive), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: true, keyGroupEnabled: false);
	}

	private KeyboardPattern Ripple()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Ripple");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		RGBKeyboardDirection selectedDirection;
		if (pattern != null && pattern.Behavior != null)
		{
			string behavior = pattern.Behavior;
			if (behavior == "Auto")
			{
				selectedDirection = RGBKeyboardDirection.Auto;
			}
			else
			{
				selectedDirection = ((!(behavior == "PressAnyKey")) ? RGBKeyboardDirection.Auto : RGBKeyboardDirection.PressAnyKey);
			}
		}
		else
		{
			selectedDirection = RGBKeyboardDirection.Auto;
		}
		return new KeyboardPattern(_events, this, "Ripple", ResourceDictionaryConverter.GetString("RippleString"), KeyboardType, RipplePattern.Generate(KeyboardType, sortedList), new List<RGBKeyboardDirection>
		{
			RGBKeyboardDirection.Auto,
			RGBKeyboardDirection.PressAnyKey
		}, selectedDirection, RGBKeyboardEffect.Ripple, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 5, preset: true, frequencySupported: true, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Ripple), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: true, keyGroupEnabled: false);
	}

	private KeyboardPattern Spark()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Spark");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		RGBKeyboardDirection selectedDirection;
		if (pattern != null && pattern.Behavior != null)
		{
			string behavior = pattern.Behavior;
			if (behavior == "Auto")
			{
				selectedDirection = RGBKeyboardDirection.Auto;
			}
			else
			{
				selectedDirection = ((!(behavior == "PressAnyKey")) ? RGBKeyboardDirection.Auto : RGBKeyboardDirection.PressAnyKey);
			}
		}
		else
		{
			selectedDirection = RGBKeyboardDirection.Auto;
		}
		return new KeyboardPattern(_events, this, "Spark", ResourceDictionaryConverter.GetString("SparkString"), KeyboardType, SparkPattern.Generate(KeyboardType, sortedList), new List<RGBKeyboardDirection>
		{
			RGBKeyboardDirection.Auto,
			RGBKeyboardDirection.PressAnyKey
		}, selectedDirection, RGBKeyboardEffect.Spark, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 5, preset: true, frequencySupported: true, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Spark), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: true, keyGroupEnabled: false);
	}

	private KeyboardPattern Aurora()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "Aurora");
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		if (pattern?.Colors != null)
		{
			for (int num = 0; num < pattern.Colors.Count; num++)
			{
				sortedList.Add(num, pattern.Colors[num]);
			}
		}
		else
		{
			sortedList = GenerateSortedColorList7();
		}
		RGBKeyboardDirection selectedDirection;
		if (pattern != null && pattern.Behavior != null)
		{
			string behavior = pattern.Behavior;
			if (behavior == "Auto")
			{
				selectedDirection = RGBKeyboardDirection.Auto;
			}
			else
			{
				selectedDirection = ((!(behavior == "PressAnyKey")) ? RGBKeyboardDirection.Auto : RGBKeyboardDirection.PressAnyKey);
			}
		}
		else
		{
			selectedDirection = RGBKeyboardDirection.Auto;
		}
		return new KeyboardPattern(_events, this, "Aurora", ResourceDictionaryConverter.GetString("AuroraString"), KeyboardType, AuroraPattern.Generate(KeyboardType, sortedList), new List<RGBKeyboardDirection>
		{
			RGBKeyboardDirection.Auto,
			RGBKeyboardDirection.PressAnyKey
		}, selectedDirection, RGBKeyboardEffect.Aurora, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 5, preset: true, frequencySupported: true, sortedList, BuildColorPickerColors(sortedList, RGBKeyboardEffect.Aurora), StandardColors, RecentColors, editingEnabled: true, sortedList.Count, speedEnabled: true, keyGroupEnabled: false);
	}

	private KeyboardPattern Custom()
	{
		Pattern pattern = _settingsFileManager.CustomDefaultSettings?.DefaultPatterns?.Find((Pattern x) => x.Effect == "UserMode");
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		if (pattern?.Keys != null)
		{
			foreach (Key key in pattern.Keys)
			{
				if (key.Color != DefaultColors.OFF_COLOR_LED)
				{
					observableCollection.Add(new KeyColorData(key.Id, key.Color, enabled: true));
				}
				else
				{
					observableCollection.Add(new KeyColorData(key.Id, DefaultColors.OFF_COLOR_UI, enabled: true));
				}
			}
		}
		else
		{
			observableCollection = MonocolorPattern.Generate(KeyboardType, DefaultColors.OFF_COLOR_UI, enabled: false);
		}
		SortedList<int, Color> colors = new SortedList<int, Color> { 
		{
			0,
			DefaultColors.OFF_COLOR_UI
		} };
		return new KeyboardPattern(_events, this, "UserMode", DefaultKeyboardPatterns.PATTERN_CUSTOM, KeyboardType, observableCollection, new List<RGBKeyboardDirection> { RGBKeyboardDirection.None }, RGBKeyboardDirection.None, RGBKeyboardEffect.UserMode, pattern?.BrightnessAc ?? 100, pattern?.BrightnessDc ?? 50, pattern?.Speed ?? 0, preset: true, frequencySupported: false, colors, BuildColorPickerColors(colors, RGBKeyboardEffect.UserMode), StandardColors, RecentColors, editingEnabled: true, 1, speedEnabled: false, keyGroupEnabled: true);
	}

	private SortedList<int, Color> GenerateSortedColorList7()
	{
		SortedList<int, Color> sortedList = new SortedList<int, Color>();
		for (int i = 0; i < DefaultColors.COLORS.Count; i++)
		{
			sortedList.Add(i, DefaultColors.COLORS[i]);
		}
		return sortedList;
	}

	private ObservableCollection<ColorPickerColors> BuildColorPickerColors(SortedList<int, Color> colors, RGBKeyboardEffect effect)
	{
		ObservableCollection<ColorPickerColors> observableCollection = new ObservableCollection<ColorPickerColors>();
		foreach (KeyValuePair<int, Color> color in colors)
		{
			observableCollection.Add(new ColorPickerColors(_events, color.Key, color.Value, effect, StandardColors, RecentColors, editingEnabled: true, selected: true));
		}
		return observableCollection;
	}

	public static ObservableCollection<KeyColorData> UpdateMultiRowKeys(RGBKeyboardCapabilities keyboardType, ObservableCollection<KeyColorData> keys)
	{
		foreach (KeyValuePair<int, List<int>> k in KeyboardCapabilities.GetMultiRowKeys(keyboardType))
		{
			KeyColorData keyColorData = keys.FirstOrDefault((KeyColorData x) => x.Index == k.Key);
			if (keyColorData == null)
			{
				continue;
			}
			foreach (int additionalKey in k.Value)
			{
				keys.FirstOrDefault((KeyColorData x) => x.Index == additionalKey).SelectedColor = keyColorData.SelectedColor;
			}
		}
		return keys;
	}

	internal static ObservableCollection<KeyColorData> GenerateRandomKeysFromColors(RGBKeyboardCapabilities keyboardType, List<Color> colors, int keysPerColor = 1)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		observableCollection.AddRange(MonocolorPattern.Generate(keyboardType, DefaultColors.OFF_COLOR_UI, enabled: true));
		Random random = new Random();
		List<int> list = new List<int>();
		foreach (Color color in colors)
		{
			int randIndex = 0;
			for (int i = 0; i < keysPerColor; i++)
			{
				do
				{
					randIndex = random.Next(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5));
				}
				while (list.Contains(randIndex));
				list.Add(randIndex);
				KeyColorData keyColorData = observableCollection.FirstOrDefault((KeyColorData k) => k.Index == randIndex);
				if (keyColorData != null)
				{
					keyColorData.SelectedColor = color;
				}
			}
		}
		return UpdateMultiRowKeys(keyboardType, observableCollection);
	}
}
