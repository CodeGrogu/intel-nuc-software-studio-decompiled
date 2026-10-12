using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Converters;

public class IndicatorIsCheckedMultiConverter : IMultiValueConverter
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
			IndicatorType indicatorType = led.SelectedIndicator.IndicatorType;
			IndicatorType indicatorType2 = (IndicatorType)parameter;
			if (led == led2 && indicatorType2 == indicatorType)
			{
				return Visibility.Visible;
			}
		}
		if (values[0] is Led && values[1] is IIndicator)
		{
			Led led3 = (Led)values.First();
			IIndicator indicator = (IIndicator)values[1];
			IndicatorType indicatorType3 = led3.SelectedIndicator.IndicatorType;
			IndicatorType indicatorType4 = (IndicatorType)parameter;
			if (led3.SelectedIndicator == indicator && indicatorType3 == indicatorType4)
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
