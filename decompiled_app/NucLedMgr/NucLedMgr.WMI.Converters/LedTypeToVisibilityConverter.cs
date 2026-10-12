using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using NucLedMgr.WMI.Enums;

namespace NucLedMgr.WMI.Converters;

internal class LedTypeToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && parameter != null && value is LedType && parameter is LedType && (LedType)value == (LedType)parameter)
		{
			return Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
