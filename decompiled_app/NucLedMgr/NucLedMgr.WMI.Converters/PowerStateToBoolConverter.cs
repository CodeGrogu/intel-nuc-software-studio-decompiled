using System;
using System.Globalization;
using System.Windows.Data;
using NucLedMgr.WMI.Enums;

namespace NucLedMgr.WMI.Converters;

public class PowerStateToBoolConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is PowerStateOptions powerStateOptions)
		{
			PowerStateOptions powerStateOptions2 = (PowerStateOptions)parameter;
			return powerStateOptions == powerStateOptions2;
		}
		return false;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = default;
		int num;
		if (value is bool)
		{
			flag = (bool)value;
			num = 1;
		}
		else
		{
			num = 0;
		}
		if (((uint)num & (flag ? 1u : 0u)) != 0)
		{
			return (PowerStateOptions)parameter;
		}
		return Binding.DoNothing;
	}
}
