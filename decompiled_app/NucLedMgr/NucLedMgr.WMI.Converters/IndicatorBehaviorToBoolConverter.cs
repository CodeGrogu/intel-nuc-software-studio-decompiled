using System;
using System.Globalization;
using System.Windows.Data;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Converters;

public class IndicatorBehaviorToBoolConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is ControlItem controlItem)
		{
			ControlItem controlItem2 = controlItem;
			ControlItem controlItem3 = (ControlItem)parameter;
			return controlItem2.Index == controlItem3.Index && controlItem2.Value == controlItem3.Value;
		}
		return false;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool)
		{
			if (!(bool)value)
			{
				return Binding.DoNothing;
			}
			return (ControlItem)parameter;
		}
		return Binding.DoNothing;
	}
}
