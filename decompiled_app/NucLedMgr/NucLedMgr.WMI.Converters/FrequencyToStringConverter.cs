using System;
using System.Globalization;
using System.Windows.Data;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Converters;

public class FrequencyToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is ControlItem controlItem)
		{
			return ((int)controlItem.Value).ToString();
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
