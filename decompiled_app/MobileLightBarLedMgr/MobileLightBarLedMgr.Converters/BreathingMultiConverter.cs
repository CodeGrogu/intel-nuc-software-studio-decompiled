using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MobileLightBarLedMgr.Converters;

public class BreathingMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if ((bool)values[0] && (Color)values[1] != Color.FromRgb(0, 0, 0))
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
