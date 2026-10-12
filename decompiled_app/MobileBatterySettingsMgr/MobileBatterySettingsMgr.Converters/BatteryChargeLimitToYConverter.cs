using System;
using System.Globalization;
using System.Windows.Data;

namespace MobileBatterySettingsMgr.Converters;

public class BatteryChargeLimitToYConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int num)
		{
			if (num <= 1)
			{
				return 425;
			}
			return -3.3 * (double)num + 425.0;
		}
		return 420;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
