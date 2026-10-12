using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FeatureMgr.Enums;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Converters;

public class StandardColorsArrayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value.GetType() == typeof(Led) && ((Led)value).LedType == LedType.PowerButton && ((Led)value).NucType == NucType.Panther)
		{
			return Visibility.Hidden;
		}
		return Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
