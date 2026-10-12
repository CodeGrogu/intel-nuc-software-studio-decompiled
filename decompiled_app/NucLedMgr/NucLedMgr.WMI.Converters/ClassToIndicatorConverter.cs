using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Interfaces;

namespace NucLedMgr.WMI.Converters;

public class ClassToIndicatorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is IIndicator indicator)
		{
			string key = indicator.IndicatorType switch
			{
				IndicatorType.Ethernet => "EthernetIndicatorString", 
				IndicatorType.HddActivity => "HDDIndicatorString", 
				IndicatorType.PowerLimit => "PowerLimitIndicatorString", 
				IndicatorType.Wifi => "WiFiIndicatorString", 
				IndicatorType.Software => "SoftwareIndicatorString", 
				IndicatorType.PowerState => "PowerStateIndicatorString", 
				IndicatorType.Disable => "DisabledIndicatorString", 
				IndicatorType.Alexa => "AlexaIndicatorString", 
				_ => "", 
			};
			return (string)Application.Current.Resources[key];
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
