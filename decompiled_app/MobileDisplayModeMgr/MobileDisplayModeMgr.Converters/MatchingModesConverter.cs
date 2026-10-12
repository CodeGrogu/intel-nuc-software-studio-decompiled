using System;
using System.Globalization;
using System.Windows.Data;
using UniwillSdkClient.UniwillService;

namespace MobileDisplayModeMgr.Converters;

public class MatchingModesConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values != null && values.Length == 2 && values[0] is DisplayMode displayMode && values[1] is DisplayMode displayMode2 && displayMode.Type == displayMode2.Type)
		{
			return true;
		}
		return false;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
