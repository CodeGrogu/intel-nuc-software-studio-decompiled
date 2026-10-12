using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using NucLedMgr.Arduino.Models;
using NucLedMgr.ENE.Models;
using NucLedMgr.Models;

namespace NucLedMgr.Converters;

public class ColorCountToHeightConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value.GetType() == typeof(ObservableCollection<ColorPickerData>))
		{
			if (!(value is ObservableCollection<ColorPickerData> { Count: not 0 } observableCollection))
			{
				return 0;
			}
			return 60 / observableCollection.Count;
		}
		if (value.GetType() == typeof(ObservableCollection<ArduinoColorPickerData>))
		{
			if (!(value is ObservableCollection<ArduinoColorPickerData> { Count: not 0 } observableCollection2))
			{
				return 0;
			}
			return 60 / observableCollection2.Count;
		}
		if (value.GetType() == typeof(ObservableCollection<EneColorPickerData>))
		{
			if (!(value is ObservableCollection<EneColorPickerData> { Count: not 0 } observableCollection3))
			{
				return 0;
			}
			return 60 / observableCollection3.Count;
		}
		return 0;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
