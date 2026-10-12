using System.Drawing;
using System.Globalization;

namespace NucLedMgr.Arduino.Converters;

public static class RgbToArduinoColorPaletteConverter
{
	public static byte RgbToHue(byte R, byte G, byte B)
	{
		byte.TryParse((Color.FromArgb(R, G, B).GetHue() / 360f * 255f).ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
		return result;
	}
}
