using System;
using System.Globalization;
using System.Windows.Data;

namespace UserPresenceMgr.Converters;

public class AutoIfFalseConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is bool))
		{
			return "0";
		}
		if (!(bool)value)
		{
			return "Auto";
		}
		return "0";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
