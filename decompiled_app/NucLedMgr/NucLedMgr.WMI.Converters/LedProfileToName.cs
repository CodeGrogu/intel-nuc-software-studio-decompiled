using System;
using System.Globalization;
using System.Windows.Data;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Converters;

public class LedProfileToName : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is LedProfile ledProfile)
		{
			return ledProfile.Name;
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
