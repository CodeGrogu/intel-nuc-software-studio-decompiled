using System;
using System.Globalization;
using System.Windows.Data;

namespace UserPresenceCstMgr.Converters;

public class MinuteIntervalConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is double num))
		{
			return "";
		}
		TimeSpan timeSpan = new TimeSpan(0, (int)num, 0);
		string text = "";
		if (timeSpan.Hours > 0)
		{
			text = timeSpan.Hours + " h";
		}
		if (timeSpan.Minutes > 0)
		{
			if (text.Length > 0)
			{
				text += " ";
			}
			text = text + timeSpan.Minutes + " min";
		}
		if (timeSpan.Seconds > 0)
		{
			if (text.Length > 0)
			{
				text += " ";
			}
			text = text + timeSpan.Seconds + " sec";
		}
		if (text.Length == 0)
		{
			return "0 min";
		}
		return text;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
