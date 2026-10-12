using System.Runtime.Serialization;
using SerialDeviceAccessLibrary;

namespace NucSoftwareStudioService.LedControl.Models;

[DataContract]
public class LedPattern
{
	[DataMember]
	public byte[] Palette;

	[DataMember]
	public byte Channel { get; set; }

	[DataMember]
	public byte PatternIndex { get; set; }

	[DataMember]
	public byte MaxBrightness { get; set; } = LedPatternDefaultValueSettings.ParametersDefaultValue;

	[DataMember]
	public byte Speed { get; set; } = LedPatternDefaultValueSettings.ParametersDefaultValue;

	[DataMember]
	public byte RainBow { get; set; } = LedPatternDefaultValueSettings.ParametersDefaultValue;

	public LedPattern()
	{
		Palette = new byte[1];
	}

	public LedPattern(byte channel, byte patternIndex)
	{
		Palette = new byte[1];
		Channel = channel;
		PatternIndex = patternIndex;
	}

	public LedPattern(byte channel, byte patternIndex, byte palette, byte maxBrightness, byte rainBow, byte speed)
	{
		Palette = new byte[1];
		Channel = channel;
		PatternIndex = patternIndex;
		Palette[0] = palette;
		MaxBrightness = maxBrightness;
		RainBow = rainBow;
		Speed = speed;
	}

	public LedPattern(byte channel, byte patternIndex, byte palette5, byte palette6, byte palette7, byte maxBrightness, byte speed)
	{
		Palette = new byte[3];
		Channel = channel;
		PatternIndex = patternIndex;
		Palette[0] = palette5;
		Palette[1] = palette6;
		Palette[2] = palette7;
		MaxBrightness = maxBrightness;
		Speed = speed;
	}

	public LedPattern(byte channel, byte patternIndex, byte maxBrightness)
	{
		Palette = new byte[1];
		Channel = channel;
		PatternIndex = patternIndex;
		MaxBrightness = maxBrightness;
	}

	public LedPattern(byte channel, byte patternIndex, byte palette, byte maxBrightness, byte rainBow)
	{
		Palette = new byte[1];
		Channel = channel;
		PatternIndex = patternIndex;
		Palette[0] = palette;
		MaxBrightness = maxBrightness;
		RainBow = rainBow;
	}

	public static LedPattern DefaultPattern(byte Channel)
	{
		return new LedPattern(Channel, 1, 0, 5, 1, 5);
	}
}
