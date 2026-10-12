using System;
using System.Globalization;
using System.Windows.Data;

namespace MobileLightBarLedMgr.Converters;

public class LightbarMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if ((bool)values[0] && !(bool)values[1])
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
