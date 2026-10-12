using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr.Converters;

public class ProfileIndexToNameConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is ProfileIndex)
		{
			switch ((ProfileIndex)value)
			{
			case ProfileIndex.Performance:
				return ResourceDictionaryConverter.GetString("PerformanceString");
			case ProfileIndex.Balanced:
				return ResourceDictionaryConverter.GetString("BalancedMode");
			case ProfileIndex.BatterySaver:
				return ResourceDictionaryConverter.GetString("BatterySaverString");
			}
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
