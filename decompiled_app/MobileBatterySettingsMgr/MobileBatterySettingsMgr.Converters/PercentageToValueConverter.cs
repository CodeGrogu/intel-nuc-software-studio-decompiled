using System;
using System.Globalization;
using System.Windows.Data;

namespace MobileBatterySettingsMgr.Converters;

public class PercentageToValueConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is int val) || !(parameter is int num))
		{
			return 0;
		}
		int num2 = 100;
		int num3 = num / num2;
		return Math.Max(1, val) * num3;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
