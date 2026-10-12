using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;

namespace SystemPerformanceMgr.Converters;

internal class StringUppercaseConvertor : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			return ResourceDictionaryConverter.GetString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToString().ToLower()));
		}
		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return string.Empty;
	}
}
