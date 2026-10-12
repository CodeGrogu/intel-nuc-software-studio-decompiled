using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Converters;

public class LedIsCheckedMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length < 2)
		{
			return Visibility.Hidden;
		}
		if (values[0] is Led && values[1] is Led)
		{
			Led led = (Led)values.First();
			Led led2 = (Led)values[1];
			LedType ledType = led.LedType;
			LedType ledType2 = (LedType)parameter;
			if (led == led2 && ledType2 == ledType)
			{
				return Visibility.Visible;
			}
		}
		if (values[0] is Led && values[1] is LedType)
		{
			Led led3 = (Led)values.First();
			LedType ledType3 = (LedType)values[1];
			LedType ledType4 = led3.LedType;
			LedType ledType5 = (LedType)parameter;
			if (led3.LedType == ledType3 && ledType4 == ledType5)
			{
				return Visibility.Visible;
			}
		}
		return Visibility.Hidden;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
