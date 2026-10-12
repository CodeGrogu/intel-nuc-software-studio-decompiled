using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using NucLedMgr.WMI.Enums;

namespace NucLedMgr.WMI.Converters;

public class IsMouseOverMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || parameter == null)
		{
			throw new InvalidOperationException("Null value");
		}
		if (values.Last() is LedType ledType && ledType == (LedType)parameter)
		{
			return true;
		}
		return values.Any((object value) =>
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
			return (byte)((uint)num & (flag ? 1u : 0u)) != 0;
		});
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
