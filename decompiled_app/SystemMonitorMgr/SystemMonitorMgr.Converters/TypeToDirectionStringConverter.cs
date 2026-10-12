using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using IntelSdkClient.SystemMonitorService;

namespace SystemMonitorMgr.Converters;

internal class TypeToDirectionStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is FanType)
		{
			switch ((FanType)value)
			{
			case FanType.CPUFan:
			case FanType.SystemFan1:
				return ResourceDictionaryConverter.GetString("LeftFan");
			case FanType.dGPUFan:
			case FanType.SystemFan2:
				return ResourceDictionaryConverter.GetString("RightFan");
			}
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
