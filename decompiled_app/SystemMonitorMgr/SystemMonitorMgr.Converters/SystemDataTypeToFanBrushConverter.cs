using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using IntelSdkClient.SystemMonitorService;

namespace SystemMonitorMgr.Converters;

public class SystemDataTypeToFanBrushConverter : IValueConverter
{
	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ResourceDictionaryConverter.GetSolidColorBrush(((FanType)value/*cast due to constrained. prefix*/).ToString() + "FanBrush");
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return string.Empty;
	}
}
