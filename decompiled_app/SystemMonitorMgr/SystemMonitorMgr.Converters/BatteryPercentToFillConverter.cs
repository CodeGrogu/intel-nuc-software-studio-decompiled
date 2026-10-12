using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Infrastructure.Converters;

namespace SystemMonitorMgr.Converters;

public class BatteryPercentToFillConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int? num = value as int?;
		int? num2 = parameter as int?;
		if (num == -1)
		{
			return new SolidColorBrush(Colors.Transparent);
		}
		if (num2 != 0)
		{
			if (num <= num2)
			{
				return new SolidColorBrush(Colors.Transparent);
			}
			return ResourceDictionaryConverter.GetSolidColorBrush("BatteryIconBrush");
		}
		if (num >= 10)
		{
			return ResourceDictionaryConverter.GetSolidColorBrush("BatteryIconBrush");
		}
		return ResourceDictionaryConverter.GetSolidColorBrush("BatteryWarningIconBrush");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return DependencyProperty.UnsetValue;
	}
}
