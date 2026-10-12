using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr.Converters;

public class SelectedProfileConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Count() != 2)
		{
			return false;
		}
		if (values[0] is PerformanceProfile performanceProfile && values[1] is PerformanceProfile performanceProfile2 && performanceProfile.Index == performanceProfile2.Index)
		{
			return true;
		}
		return false;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		return new object[1] { Binding.DoNothing };
	}
}
