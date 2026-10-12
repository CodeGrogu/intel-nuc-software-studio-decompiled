using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;

namespace UserPresenceCstMgr.Converters;

public class MillisecondIntervalConverter : IValueConverter
{
	private const string MIN_ABBR_KEY = "MinuteAbbr";

	private const string SEC_ABBR_KEY = "SecondAbbr";

	private const string MILLISECOND_ABBR_KEY = "MillisecondAbbr";

	private const string HOUR_ABBR_KEY = "HourAbbr";

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is double num))
		{
			return "";
		}
		TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, (int)num);
		string text = "";
		if (timeSpan.Hours > 0)
		{
			text = timeSpan.Hours + " " + ResourceDictionaryConverter.GetString("HourAbbr");
		}
		if (timeSpan.Minutes > 0)
		{
			if (text.Length > 0)
			{
				text += " ";
			}
			text = text + timeSpan.Minutes + " " + ResourceDictionaryConverter.GetString("MinuteAbbr");
		}
		if (timeSpan.Hours > 0 && timeSpan.Minutes > 0)
		{
			return text;
		}
		if (timeSpan.Seconds > 0)
		{
			if (text.Length > 0)
			{
				text += " ";
			}
			return text + timeSpan.Seconds + " " + ResourceDictionaryConverter.GetString("SecondAbbr");
		}
		if (timeSpan.Milliseconds > 0 && text.Length == 0)
		{
			text = text + timeSpan.Milliseconds + " " + ResourceDictionaryConverter.GetString("MillisecondAbbr");
		}
		if (text.Length == 0)
		{
			return "0 " + ResourceDictionaryConverter.GetString("SecondAbbr");
		}
		return text;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
