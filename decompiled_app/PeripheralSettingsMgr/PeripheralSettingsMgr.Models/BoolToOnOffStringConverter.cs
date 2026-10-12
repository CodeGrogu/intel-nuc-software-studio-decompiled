using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;

namespace PeripheralSettingsMgr.Models;

internal class BoolToOnOffStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool)
		{
			if ((bool)value)
			{
				return ResourceDictionaryConverter.GetString("ToggleOnString");
			}
			return ResourceDictionaryConverter.GetString("ToggleOffString");
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
