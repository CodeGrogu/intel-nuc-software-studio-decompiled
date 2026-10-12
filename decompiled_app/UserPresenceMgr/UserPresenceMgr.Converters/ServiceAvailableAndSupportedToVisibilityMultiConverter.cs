using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UserPresenceMgr.Converters;

public class ServiceAvailableAndSupportedToVisibilityMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Length != 2)
		{
			return Visibility.Hidden;
		}
		if (values[0] is bool flag)
		{
			object obj = values[1];
			if (obj is bool)
			{
				if (!(bool)obj)
				{
					return Visibility.Hidden;
				}
				if (flag)
				{
					return Visibility.Hidden;
				}
				if (!flag)
				{
					return Visibility.Visible;
				}
			}
		}
		return Visibility.Hidden;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
