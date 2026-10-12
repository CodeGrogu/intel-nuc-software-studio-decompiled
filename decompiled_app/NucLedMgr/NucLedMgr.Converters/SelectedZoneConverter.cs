using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NucLedMgr.Converters;

public class SelectedZoneConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Count() != 2)
		{
			return false;
		}
		if (values[0] != values[1])
		{
			return false;
		}
		return true;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		return new object[1] { Binding.DoNothing };
	}
}
