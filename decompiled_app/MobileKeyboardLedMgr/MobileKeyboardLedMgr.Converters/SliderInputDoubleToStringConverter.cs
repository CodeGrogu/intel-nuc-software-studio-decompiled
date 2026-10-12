using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MobileKeyboardLedMgr.Converters;

public class SliderInputDoubleToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is double num)
		{
			return new string((from c in num.ToString()
				where char.IsDigit(c)
				select c).ToArray());
		}
		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is string source)
		{
			string text = new string(source.Where((char c) => char.IsDigit(c)).ToArray());
			if (string.IsNullOrEmpty(text))
			{
				return 0;
			}
			return double.Parse(text);
		}
		return 0;
	}
}
