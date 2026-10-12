using System;
using System.Globalization;
using System.Windows.Data;
using MobilePerformanceMgr.Models;
using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr.Converters;

public class PerformanceModeToCpuStringMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Length != 2)
		{
			return string.Empty;
		}
		if (values[0] is PerformanceMode performanceMode)
		{
			object obj = values[1];
			if (obj is bool)
			{
				if ((bool)obj)
				{
					return MobilePerformanceTuningConstants.BENCHMARK_STRING;
				}
				switch (performanceMode.CpuMode)
				{
				case 1:
					return MobilePerformanceTuningConstants.PERFORMANCE_STRING;
				case 2:
					return MobilePerformanceTuningConstants.BALANCED_STRING;
				case 3:
					return MobilePerformanceTuningConstants.BATTERY_SAVER_STRING;
				}
			}
		}
		return string.Empty;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
