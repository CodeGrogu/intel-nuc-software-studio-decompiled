using System;
using System.Globalization;
using System.Windows.Data;

namespace NucLedMgr.WMI.Converters;

public class IndicatorToIsSelectedConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			return value.Equals(parameter);
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			if (!value.Equals(true))
			{
				return Binding.DoNothing;
			}
			return parameter;
		}
		return Binding.DoNothing;
	}
}
