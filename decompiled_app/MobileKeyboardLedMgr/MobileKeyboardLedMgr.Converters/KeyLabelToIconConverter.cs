using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;

namespace MobileKeyboardLedMgr.Converters;

public class KeyLabelToIconConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is string)
		{
			switch (value as string)
			{
			case "WND":
				return ResourceDictionaryConverter.GetIconViewbox("WindowsIcon");
			case "[=]":
				return ResourceDictionaryConverter.GetIconViewbox("MenuIcon");
			case "ENTER":
				return ResourceDictionaryConverter.GetIconViewbox("EnterIcon");
			case "BACKSPACE":
				return ResourceDictionaryConverter.GetIconViewbox("BackspaceIcon");
			case "BACK":
				return ResourceDictionaryConverter.GetIconViewbox("BackspaceNoTextIcon");
			case "LEFT":
				return ResourceDictionaryConverter.GetIconViewbox("LeftIcon");
			case "UP":
				return ResourceDictionaryConverter.GetIconViewbox("UpIcon");
			case "DOWN":
				return ResourceDictionaryConverter.GetIconViewbox("DownIcon");
			case "RIGHT":
				return ResourceDictionaryConverter.GetIconViewbox("RightIcon");
			}
		}
		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
