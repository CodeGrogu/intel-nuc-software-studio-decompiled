using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using NucLedMgr.ENE.Models;

namespace NucLedMgr.Converters;

internal class ColorCountToWidthConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value.GetType() == typeof(ObservableCollection<EneColorPickerData>))
		{
			if (!(value is ObservableCollection<EneColorPickerData> observableCollection))
			{
				return 0;
			}
			if (string.Equals(parameter.ToString(), "EneLedView"))
			{
				return 125 / observableCollection[0].ColorCount + 1;
			}
			return 250 / observableCollection[0].ColorCount;
		}
		return 0;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
