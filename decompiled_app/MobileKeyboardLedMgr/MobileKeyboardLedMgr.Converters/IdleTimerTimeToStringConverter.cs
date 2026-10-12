using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using MobileKeyboardLedMgr.Models;

namespace MobileKeyboardLedMgr.Converters;

public class IdleTimerTimeToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is double)
		{
			int num = KeyboardIdleTimer.ConvertIdleTimerToSeconds((int)(double)value);
			if (num == 0)
			{
				return ResourceDictionaryConverter.GetString("OffString");
			}
			TimeSpan timeSpan = new TimeSpan(0, 0, num);
			string text = "";
			if (timeSpan.Hours > 0)
			{
				text = timeSpan.Hours + " " + ResourceDictionaryConverter.GetString("ShortHourString");
			}
			if (timeSpan.Minutes > 0)
			{
				if (text.Length > 0)
				{
					text += " ";
				}
				text = text + timeSpan.Minutes + " " + ResourceDictionaryConverter.GetString("ShortMinuteString");
			}
			if (timeSpan.Seconds > 0)
			{
				if (text.Length > 0)
				{
					text += " ";
				}
				text = text + timeSpan.Seconds + " " + ResourceDictionaryConverter.GetString("ShortSecondString");
			}
			return text;
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
