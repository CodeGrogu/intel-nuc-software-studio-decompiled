using System;
using System.Globalization;
using System.Windows.Data;
using MobileKeyboardLedMgr.Models.Profiles;

namespace MobileKeyboardLedMgr.Converters;

public class KeyboardProfileToName : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is KeyboardProfile keyboardProfile)
		{
			return keyboardProfile.Name;
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
