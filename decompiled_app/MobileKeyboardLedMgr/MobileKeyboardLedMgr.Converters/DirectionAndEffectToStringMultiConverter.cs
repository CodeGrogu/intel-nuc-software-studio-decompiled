using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Converters;

public class DirectionAndEffectToStringMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values != null && values.Length == 2 && values[0] is RGBKeyboardDirection rGBKeyboardDirection && values[1] is RGBKeyboardEffect rGBKeyboardEffect)
		{
			if (rGBKeyboardEffect == RGBKeyboardEffect.Music)
			{
				switch (rGBKeyboardDirection)
				{
				case RGBKeyboardDirection.None:
					return ResourceDictionaryConverter.GetString("UpDirectionString");
				case RGBKeyboardDirection.Right:
					return ResourceDictionaryConverter.GetString("OutDirectionString");
				case RGBKeyboardDirection.Left:
					return ResourceDictionaryConverter.GetString("InDirectionString");
				case RGBKeyboardDirection.Up:
					return ResourceDictionaryConverter.GetString("DownDirectionString");
				}
			}
			else
			{
				switch (rGBKeyboardDirection)
				{
				case RGBKeyboardDirection.None:
					return ResourceDictionaryConverter.GetString("NoneDirectionString");
				case RGBKeyboardDirection.Left:
					return ResourceDictionaryConverter.GetString("LeftDirectionString");
				case RGBKeyboardDirection.Right:
					return ResourceDictionaryConverter.GetString("RightDirectionString");
				case RGBKeyboardDirection.Down:
					return ResourceDictionaryConverter.GetString("DownDirectionString");
				case RGBKeyboardDirection.Up:
					return ResourceDictionaryConverter.GetString("UpDirectionString");
				case RGBKeyboardDirection.Auto:
					return ResourceDictionaryConverter.GetString("AutoDirectionString");
				case RGBKeyboardDirection.PressAnyKey:
					return ResourceDictionaryConverter.GetString("PressAnyKeyDirectionString");
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
