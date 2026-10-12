using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using NucLedMgr.WMI.Interfaces;

namespace NucLedMgr.WMI.Converters;

public class IndicatorToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = false;
		if (value is IIndicator { IndicatorType: var indicatorType })
		{
			flag = indicatorType.Equals(parameter);
		}
		if (flag)
		{
			return Visibility.Visible;
		}
		return Visibility.Hidden;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
