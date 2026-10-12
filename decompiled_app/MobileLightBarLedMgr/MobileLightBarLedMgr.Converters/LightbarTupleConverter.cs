using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MobileLightBarLedMgr.Converters;

public class LightbarTupleConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values != null && values.Length == 2 && values[0] is Color item && values[1] is int item2)
		{
			return new Tuple<Color, int>(item, item2);
		}
		return null;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
