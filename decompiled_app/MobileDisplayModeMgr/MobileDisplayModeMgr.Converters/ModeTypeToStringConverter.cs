using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using UniwillSdkClient.UniwillService;

namespace MobileDisplayModeMgr.Converters;

public class ModeTypeToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is DisplayModeType)
		{
			switch ((DisplayModeType)value)
			{
			case DisplayModeType.Standard:
				return ResourceDictionaryConverter.GetString("StandardDisplayString");
			case DisplayModeType.Gaming:
				return ResourceDictionaryConverter.GetString("GamingDisplayString");
			case DisplayModeType.Video:
				return ResourceDictionaryConverter.GetString("VideoDisplayString");
			case DisplayModeType.Reading:
				return ResourceDictionaryConverter.GetString("ReadingDisplayString");
			case DisplayModeType.Custom:
				return ResourceDictionaryConverter.GetString("CustomDisplayString");
			}
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
