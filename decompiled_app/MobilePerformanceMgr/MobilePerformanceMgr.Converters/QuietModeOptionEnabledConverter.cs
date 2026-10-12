using System;
using System.Globalization;
using System.Windows.Data;

namespace MobilePerformanceMgr.Converters;

public class QuietModeOptionEnabledConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int num && num == 1)
		{
			return false;
		}
		return true;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
