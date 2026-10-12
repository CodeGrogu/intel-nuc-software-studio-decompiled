using System;
using System.Globalization;
using System.Windows.Data;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Converters;

public class FrequencyToBoolConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter != null)
		{
			if (values[0] is ControlItem)
			{
				ControlItem controlItem = (ControlItem)values[0];
				if (controlItem == null)
				{
					return false;
				}
				if ((bool)values[1])
				{
					return false;
				}
				ControlItem controlItem2 = (ControlItem)parameter;
				return controlItem.Index != controlItem2.Index || controlItem.Value != controlItem2.Value;
			}
			return false;
		}
		return !(bool)values[0];
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
