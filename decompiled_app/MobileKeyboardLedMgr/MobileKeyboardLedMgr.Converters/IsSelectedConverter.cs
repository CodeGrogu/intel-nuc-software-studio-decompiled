using System;
using System.Globalization;
using System.Windows.Data;
using MobileKeyboardLedMgr.Models.Profiles;

namespace MobileKeyboardLedMgr.Converters;

internal class IsSelectedConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Length != 2)
		{
			return false;
		}
		if (values[0] is ColorPickerColors colorPickerColors && values[1] is ColorPickerColors colorPickerColors2 && colorPickerColors == colorPickerColors2)
		{
			return true;
		}
		return false;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
