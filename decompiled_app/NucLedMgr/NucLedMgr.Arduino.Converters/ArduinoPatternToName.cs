using System;
using System.Globalization;
using System.Windows.Data;
using NucLedMgr.Arduino.Models;

namespace NucLedMgr.Arduino.Converters;

public class ArduinoPatternToName : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is LedProfile ledProfile)
		{
			return ledProfile.Name;
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
