using System;

namespace NucLedMgr.WMI.Controls;

public static class HSV
{
	public static RGB[] GetSpectrum()
	{
		RGB[] array = new RGB[360];
		for (int i = 0; i < 360; i++)
		{
			array[i] = RGBFromHSV(i, 1.0, 1.0);
		}
		return array;
	}

	public static RGB[] GradientSpectrum()
	{
		RGB[] array = new RGB[7];
		for (int i = 0; i < 7; i++)
		{
			array[i] = RGBFromHSV(i * 60, 1.0, 1.0);
		}
		return array;
	}

	public static RGB RGBFromHSV(double h, double s, double v)
	{
		if (h > 360.0 || h < 0.0 || s > 1.0 || s < 0.0 || v > 1.0 || v < 0.0)
		{
			return null;
		}
		double num = v * s;
		double num2 = num * (1.0 - Math.Abs(h / 60.0 % 2.0 - 1.0));
		double num3 = v - num;
		double num4 = 0.0;
		double num5 = 0.0;
		double num6 = 0.0;
		if (h < 60.0)
		{
			num4 = num;
			num5 = num2;
		}
		else if (h < 120.0)
		{
			num4 = num2;
			num5 = num;
		}
		else if (h < 180.0)
		{
			num5 = num;
			num6 = num2;
		}
		else if (h < 240.0)
		{
			num5 = num2;
			num6 = num;
		}
		else if (h < 300.0)
		{
			num4 = num2;
			num6 = num;
		}
		else if (h <= 360.0)
		{
			num4 = num;
			num6 = num2;
		}
		return new RGB((num4 + num3) * 255.0, (num5 + num3) * 255.0, (num6 + num3) * 255.0);
	}
}
