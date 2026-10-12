using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using NucLedMgr.Arduino.Enums;
using NucLedMgr.Arduino.Models;

namespace NucLedMgr.Arduino.Converters;

internal class ArduinoZoneToColorConverter : IValueConverter
{
	private enum Direction
	{
		TopToBottom,
		BottomToTop,
		LeftToRight,
		RightToLeft
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return new SolidColorBrush(Colors.Transparent);
		}
		if (value.GetType() == typeof(ArduinoLedData))
		{
			ArduinoLedData arduinoLedData = (ArduinoLedData)value;
			Color color = Colors.Transparent;
			switch (arduinoLedData.SelectedPattern)
			{
			case ArduinoPatternType.PulseTrain1:
			case ArduinoPatternType.PulseTrain3:
				return CreateGradient(Direction.LeftToRight, arduinoLedData.SelectedColors);
			case ArduinoPatternType.PulseTrain2:
				return CreateGradient(Direction.RightToLeft, arduinoLedData.SelectedColors);
			case ArduinoPatternType.Rainbow1:
			case ArduinoPatternType.Rainbow2:
				return CreateGradient(Direction.LeftToRight);
			default:
				if (arduinoLedData.RainbowSupported && arduinoLedData.Rainbow)
				{
					return CreateGradient(Direction.LeftToRight);
				}
				color = arduinoLedData.SelectedColor;
				break;
			case ArduinoPatternType.Off:
				break;
			}
			return new SolidColorBrush(color);
		}
		return new SolidColorBrush(Colors.Transparent);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private LinearGradientBrush CreateGradient(Direction direction, ObservableCollection<ArduinoColorPickerData> gradientColors = null)
	{
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
		switch (direction)
		{
		case Direction.BottomToTop:
			linearGradientBrush.StartPoint = new Point(1.0, 1.0);
			linearGradientBrush.EndPoint = new Point(1.0, 0.0);
			break;
		case Direction.TopToBottom:
			linearGradientBrush.StartPoint = new Point(1.0, 0.0);
			linearGradientBrush.EndPoint = new Point(1.0, 1.0);
			break;
		case Direction.LeftToRight:
			linearGradientBrush.StartPoint = new Point(0.0, 0.5);
			linearGradientBrush.EndPoint = new Point(1.0, 0.5);
			break;
		case Direction.RightToLeft:
			linearGradientBrush.StartPoint = new Point(1.0, 0.5);
			linearGradientBrush.EndPoint = new Point(0.0, 0.5);
			break;
		}
		List<Color> list = new List<Color>();
		if (gradientColors == null)
		{
			list.Add(Colors.Red);
			list.Add(Colors.Yellow);
			list.Add(Colors.Green);
			list.Add(Colors.Cyan);
			list.Add(Colors.Blue);
			list.Add(Colors.Purple);
		}
		else
		{
			if (direction == Direction.RightToLeft)
			{
				foreach (ArduinoColorPickerData item in gradientColors.Reverse())
				{
					list.Add(item.SelectedColor);
				}
			}
			else
			{
				foreach (ArduinoColorPickerData gradientColor in gradientColors)
				{
					list.Add(gradientColor.SelectedColor);
				}
			}
			list.Add(Colors.Black);
		}
		double num = 0.0;
		foreach (Color item2 in list)
		{
			GradientStop value = new GradientStop
			{
				Color = item2,
				Offset = num
			};
			linearGradientBrush.GradientStops.Add(value);
			num += 0.2;
		}
		return linearGradientBrush;
	}
}
