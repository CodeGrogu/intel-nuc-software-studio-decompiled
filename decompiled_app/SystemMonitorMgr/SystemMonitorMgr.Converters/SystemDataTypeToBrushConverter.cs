using System;
using System.Globalization;
using System.Windows.Data;
using SystemMonitorMgr.Enums;
using Infrastructure.Converters;

namespace SystemMonitorMgr.Converters;

public class SystemDataTypeToBrushConverter : IValueConverter
{
	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		SystemDataType systemDataType = (SystemDataType)value;
		if ((uint)(systemDataType - 9) <= 5u)
		{
			return ResourceDictionaryConverter.GetSolidColorBrush("StorageGaugeColorBrush");
		}
		return ResourceDictionaryConverter.GetSolidColorBrush(systemDataType.ToString() + "GaugeColorBrush");
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return string.Empty;
	}
}
