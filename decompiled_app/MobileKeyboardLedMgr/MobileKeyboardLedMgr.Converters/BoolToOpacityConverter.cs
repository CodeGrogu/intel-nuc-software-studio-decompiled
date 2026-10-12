using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MobileKeyboardLedMgr.Converters;

public class BoolToOpacityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool? flag = value as bool?;
		if (!flag.HasValue)
		{
			return 0;
		}
		return (flag == true) ? 0.6 : 0.0;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return DependencyProperty.UnsetValue;
	}
}
