using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using IntelSdkClient.SystemMonitorService;

namespace SystemMonitorMgr.Converters;

internal class TypeToFanNameConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is FanType)
		{
			switch ((FanType)value)
			{
			case FanType.CPUFan:
				return ResourceDictionaryConverter.GetString("CPUFan1");
			case FanType.CPUFan2:
				return ResourceDictionaryConverter.GetString("CPUFan2");
			case FanType.SystemFan1:
				return ResourceDictionaryConverter.GetString("SystemFan1");
			case FanType.SystemFan2:
				return ResourceDictionaryConverter.GetString("SystemFan2");
			}
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
