using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SystemMonitorMgr.Converters;

internal class BatteryPercentageToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (string.IsNullOrWhiteSpace(value.ToString()) || value.ToString() == "0%")
		{
			return Visibility.Collapsed;
		}
		return Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
