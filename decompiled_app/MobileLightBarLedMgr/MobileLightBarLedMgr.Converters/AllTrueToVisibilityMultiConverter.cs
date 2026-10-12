using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MobileLightBarLedMgr.Converters;

public class AllTrueToVisibilityMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		foreach (object obj in values)
		{
			if (!(obj is bool) || (obj is bool && !(bool)obj))
			{
				return Visibility.Hidden;
			}
		}
		return Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
