using System;
using System.Globalization;
using System.Windows.Data;
using MobilePerformanceMgr.Models;

namespace MobilePerformanceMgr.Converters;

public class IntToFanStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int)
		{
			return (int)value switch
			{
				1 => (object)MobilePerformanceTuningConstants.PERFORMANCE_STRING, 
				2 => MobilePerformanceTuningConstants.STANDARD_STRING, 
				3 => MobilePerformanceTuningConstants.QUIET_STRING, 
				_ => "", 
			};
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
