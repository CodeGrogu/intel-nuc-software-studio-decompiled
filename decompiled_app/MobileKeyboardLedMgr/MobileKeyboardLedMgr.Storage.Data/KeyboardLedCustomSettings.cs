using System;
using System.Collections.Generic;
using System.Windows.Media;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Models.KeyMaps.US;

namespace MobileKeyboardLedMgr.Storage.Data;

public class KeyboardLedCustomSettings
{
	public string DefaultEffect { get; set; }

	public List<Color> DefaultStandardColors { get; set; }

	public bool ProjectAuroraEnabled { get; set; }

	public int DefaultIdleTimerAc { get; set; }

	public int DefaultIdleTimerDc { get; set; }

	public List<Pattern> DefaultPatterns { get; set; }

	public static KeyboardLedCustomSettings GetDefaultSettings()
	{
		return new KeyboardLedCustomSettings
		{
			DefaultEffect = "Monocolor",
			DefaultStandardColors = DefaultColors.COLORS,
			ProjectAuroraEnabled = false,
			DefaultIdleTimerAc = 0,
			DefaultIdleTimerDc = 30,
			DefaultPatterns = BuildDefaultPatterns()
		};
	}

	private static List<Pattern> BuildDefaultPatterns()
	{
		return new List<Pattern>
		{
			new Pattern
			{
				Effect = "Monocolor",
				Colors = new List<Color> { DefaultColors.MONOCOLOR },
				BrightnessAc = 100,
				BrightnessDc = 50
			},
			new Pattern
			{
				Effect = "Rainbow",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50
			},
			new Pattern
			{
				Effect = "GamingMode",
				Colors = new List<Color> { DefaultColors.GAMING },
				BrightnessAc = 100,
				BrightnessDc = 50
			},
			new Pattern
			{
				Effect = "Breathing",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50,
				Speed = 5
			},
			new Pattern
			{
				Effect = "Raindrop",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50,
				Speed = 5
			},
			new Pattern
			{
				Effect = "Marquee",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50,
				Speed = 5
			},
			new Pattern
			{
				Effect = "Music",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50
			},
			new Pattern
			{
				Effect = "Wave",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50,
				Speed = 5,
				Behavior = "Right"
			},
			new Pattern
			{
				Effect = "Reactive",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50,
				Speed = 5,
				Behavior = "Auto"
			},
			new Pattern
			{
				Effect = "Ripple",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50,
				Speed = 5,
				Behavior = "Auto"
			},
			new Pattern
			{
				Effect = "Spark",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50,
				Speed = 5,
				Behavior = "Auto"
			},
			new Pattern
			{
				Effect = "Aurora",
				Colors = DefaultColors.COLORS,
				BrightnessAc = 100,
				BrightnessDc = 50,
				Speed = 5,
				Behavior = "Auto"
			},
			BuildDefaultCustomPattern()
		};
	}

	private static Pattern BuildDefaultCustomPattern()
	{
		List<Key> list = new List<Key>();
		foreach (int value in Enum.GetValues(typeof(PhysicalKeyUS)))
		{
			list.Add(new Key
			{
				Id = value,
				Color = DefaultColors.OFF_COLOR_LED
			});
		}
		return new Pattern
		{
			Effect = "UserMode",
			Keys = list,
			BrightnessAc = 100,
			BrightnessDc = 50
		};
	}
}
