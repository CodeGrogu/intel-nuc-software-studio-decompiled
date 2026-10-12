using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FeatureMgr.Enums;

namespace NucLedMgr.WMI.Converters;

public class MatchingObjectToCollapsedConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && parameter != null && value is NucType && parameter is NucType && (NucType)value == (NucType)parameter)
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
