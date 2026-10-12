using System;
using System.Windows.Media;

namespace NucLedMgr.WMI.Controls;

public class RGB
{
	public byte R { get; set; }

	public byte G { get; set; }

	public byte B { get; set; }

	public RGB()
	{
		R = byte.MaxValue;
		G = byte.MaxValue;
		B = byte.MaxValue;
	}

	public RGB(double r, double g, double b)
	{
		if (r > 255.0 || g > 255.0 || b > 255.0)
		{
			throw new Exception("RGB must be under 255 (1byte)");
		}
		R = (byte)r;
		G = (byte)g;
		B = (byte)b;
	}

	public string Hex()
	{
		return BitConverter.ToString(new byte[3] { R, G, B }).Replace("-", string.Empty);
	}

	public Color Color()
	{
		return new Color
		{
			R = R,
			G = G,
			B = B,
			A = byte.MaxValue
		};
	}
}
