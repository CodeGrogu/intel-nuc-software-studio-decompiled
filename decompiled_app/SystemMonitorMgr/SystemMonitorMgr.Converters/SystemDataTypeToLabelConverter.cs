using System;
using System.Globalization;
using System.Windows.Data;
using SystemMonitorMgr.Enums;
using Infrastructure.Converters;

namespace SystemMonitorMgr.Converters;

public class SystemDataTypeToLabelConverter : IValueConverter
{
	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ResourceDictionaryConverter.GetString(((SystemDataType)value/*cast due to constrained. prefix*/).ToString() + "SystemDataType");
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return string.Empty;
	}
}
