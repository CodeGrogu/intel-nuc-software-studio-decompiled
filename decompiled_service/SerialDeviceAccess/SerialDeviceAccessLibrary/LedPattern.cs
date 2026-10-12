using System.Collections.Generic;

namespace SerialDeviceAccessLibrary;

public class LedPattern
{
	public byte Channel { get; set; }

	public byte PatternIndex { get; set; }

	public byte MaxBrightness { get; set; } = LedPatternDefaultValueSettings.ParametersDefaultValue;

	public byte Speed { get; set; } = LedPatternDefaultValueSettings.ParametersDefaultValue;

	public byte RainBow { get; set; } = LedPatternDefaultValueSettings.ParametersDefaultValue;

	public List<byte> Palette { get; set; } = new List<byte>();

	public LedPattern()
	{
	}

	public LedPattern(byte channel, byte patternIndex)
	{
		Channel = channel;
		PatternIndex = patternIndex;
	}

	public LedPattern(byte channel, byte patternIndex, byte palette, byte maxBrightness, byte rainBow, byte speed)
	{
		Channel = channel;
		PatternIndex = patternIndex;
		Palette.Add(palette);
		MaxBrightness = maxBrightness;
		RainBow = rainBow;
		Speed = speed;
	}

	public LedPattern(byte channel, byte patternIndex, byte palette5, byte palette6, byte palette7, byte maxBrightness, byte speed)
	{
		Channel = channel;
		PatternIndex = patternIndex;
		Palette.Add(palette5);
		Palette.Add(palette6);
		Palette.Add(palette7);
		MaxBrightness = maxBrightness;
		Speed = speed;
	}

	public LedPattern(byte channel, byte patternIndex, byte maxBrightness)
	{
		Channel = channel;
		PatternIndex = patternIndex;
		MaxBrightness = maxBrightness;
	}

	public LedPattern(byte channel, byte patternIndex, byte palette, byte maxBrightness, byte rainBow)
	{
		Channel = channel;
		PatternIndex = patternIndex;
		Palette.Add(palette);
		MaxBrightness = maxBrightness;
		RainBow = rainBow;
	}

	public static LedPattern DefaultPattern(byte Channel)
	{
		return new LedPattern(Channel, 1, 0, 5, 1, 5);
	}
}
