using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr.Converters;

public class WhisperModeIntToTypeStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int)
		{
			return (WhisperModeType)(int)value switch
			{
				WhisperModeType.Balanced => (object)ResourceDictionaryConverter.GetString("WhisperModeBalancedString"), 
				WhisperModeType.Quiet => ResourceDictionaryConverter.GetString("WhisperModeQuietString"), 
				WhisperModeType.Quieter => ResourceDictionaryConverter.GetString("WhisperModeQuieterString"), 
				_ => string.Empty, 
			};
		}
		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
