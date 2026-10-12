using System;
using System.Globalization;
using System.Windows.Data;

namespace MobilePerformanceMgr.Converters;

public class ModeToggleToEnabledBoolMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Length != 2)
		{
			return true;
		}
		if (values[0] is bool flag && values[1] is bool flag2)
		{
			return !(flag | flag2);
		}
		return true;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
