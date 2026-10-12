using Infrastructure.Converters;

namespace MobileKeyboardLedMgr.Models.KingCounty;

public static class DefaultKeyboardPatterns
{
	public static readonly string PATTERN_MONOCOLOR = ResourceDictionaryConverter.GetString("MonocolorString");

	public static readonly string PATTERN_BREATHING = ResourceDictionaryConverter.GetString("BreathingString");

	public static readonly string PATTERN_WAVE = ResourceDictionaryConverter.GetString("WaveString");

	public static readonly string PATTERN_REACTIVE = ResourceDictionaryConverter.GetString("ReactiveString");

	public static readonly string PATTERN_RAINBOW = ResourceDictionaryConverter.GetString("RainbowString");

	public static readonly string PATTERN_RIPPLE = ResourceDictionaryConverter.GetString("RippleString");

	public static readonly string PATTERN_RAINDROP = ResourceDictionaryConverter.GetString("RaindropString");

	public static readonly string PATTERN_MARQUEE = ResourceDictionaryConverter.GetString("MarqueeString");

	public static readonly string PATTERN_AURORA = ResourceDictionaryConverter.GetString("AuroraString");

	public static readonly string PATTERN_SPARK = ResourceDictionaryConverter.GetString("SparkString");

	public static readonly string PATTERN_MUSIC = ResourceDictionaryConverter.GetString("MusicString");

	public static readonly string PATTERN_GAMING = ResourceDictionaryConverter.GetString("GamingModeString");

	public static readonly string PATTERN_CUSTOM = ResourceDictionaryConverter.GetString("CustomString");
}
