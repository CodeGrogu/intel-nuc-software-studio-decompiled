using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Converters;

public class ColorPickerColorsToBrushConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is ObservableCollection<WmiColorPickerData> observableCollection)
		{
			ObservableCollection<Color> observableCollection2 = new ObservableCollection<Color>();
			foreach (WmiColorPickerData item in observableCollection)
			{
				observableCollection2.Add(item.SelectedColor);
			}
			for (int num = observableCollection2.Count - 1; num >= 0; num--)
			{
				if (observableCollection2[num] == Colors.Transparent)
				{
					observableCollection2.Remove(observableCollection2[num]);
				}
			}
			if (observableCollection[0].Rainbow)
			{
				return HorzGradient(observableCollection2);
			}
			return VertGradient(observableCollection2);
		}
		return new SolidColorBrush(Colors.Black);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private LinearGradientBrush VertGradient(ObservableCollection<Color> colors)
	{
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush
		{
			StartPoint = new Point(0.0, 0.0),
			EndPoint = new Point(0.0, 1.0)
		};
		double num = 1.0 / (double)colors.Count;
		double num2 = 0.0;
		for (int i = 0; i < colors.Count; i++)
		{
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[i], num2));
			num2 += num;
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[i], num2));
		}
		return linearGradientBrush;
	}

	private LinearGradientBrush HorzGradient(ObservableCollection<Color> colors)
	{
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush
		{
			StartPoint = new Point(1.0, 0.0),
			EndPoint = new Point(0.0, 0.0)
		};
		if (colors.Count >= 2)
		{
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[1], 0.0));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0], 0.7));
		}
		else if (colors.Count == 1)
		{
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0], 0.0));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0], 0.7));
		}
		return linearGradientBrush;
	}
}
