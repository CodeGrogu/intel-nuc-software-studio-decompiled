using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Infrastructure.Models;

public class FanSpeedRatioConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int? num = value as int?;
		if (!num.HasValue)
		{
			return 0;
		}
		int? num2 = num % 100;
		if (num2 <= 0)
		{
			return 0.1;
		}
		return num2;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return DependencyProperty.UnsetValue;
	}
}
