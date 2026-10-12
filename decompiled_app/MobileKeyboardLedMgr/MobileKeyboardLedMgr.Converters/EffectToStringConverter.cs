using System;
using System.Globalization;
using System.Windows.Data;
using MobileKeyboardLedMgr.Models.KingCounty;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Converters;

public class EffectToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is RGBKeyboardEffect)
		{
			switch ((RGBKeyboardEffect)value)
			{
			case RGBKeyboardEffect.Aurora:
				return DefaultKeyboardPatterns.PATTERN_AURORA;
			case RGBKeyboardEffect.Breathing:
				return DefaultKeyboardPatterns.PATTERN_BREATHING;
			case RGBKeyboardEffect.GamingMode:
				return DefaultKeyboardPatterns.PATTERN_GAMING;
			case RGBKeyboardEffect.Marquee:
				return DefaultKeyboardPatterns.PATTERN_MARQUEE;
			case RGBKeyboardEffect.Monocolor:
				return DefaultKeyboardPatterns.PATTERN_MONOCOLOR;
			case RGBKeyboardEffect.Music:
				return DefaultKeyboardPatterns.PATTERN_MUSIC;
			case RGBKeyboardEffect.Rainbow:
				return DefaultKeyboardPatterns.PATTERN_RAINBOW;
			case RGBKeyboardEffect.Raindrop:
				return DefaultKeyboardPatterns.PATTERN_RAINDROP;
			case RGBKeyboardEffect.Reactive:
				return DefaultKeyboardPatterns.PATTERN_REACTIVE;
			case RGBKeyboardEffect.Ripple:
				return DefaultKeyboardPatterns.PATTERN_RIPPLE;
			case RGBKeyboardEffect.Spark:
				return DefaultKeyboardPatterns.PATTERN_SPARK;
			case RGBKeyboardEffect.UserMode:
				return DefaultKeyboardPatterns.PATTERN_CUSTOM;
			case RGBKeyboardEffect.Wave:
				return DefaultKeyboardPatterns.PATTERN_WAVE;
			}
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
