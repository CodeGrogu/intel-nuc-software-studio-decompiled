using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Models;
using NucLedMgr.WMI.Models.Indicators;

namespace NucLedMgr.WMI.Converters;

public class LedToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is Led { SelectedIndicator: var selectedIndicator } led)
		{
			if (selectedIndicator.IndicatorType == IndicatorType.Alexa)
			{
				return new SolidColorBrush(selectedIndicator.ColorSetting.CurrentColor);
			}
			if (parameter == null && led.SelectedColors != null)
			{
				return new SolidColorBrush(led.SelectedColors[0].SelectedColor);
			}
			switch (selectedIndicator.IndicatorType)
			{
			case IndicatorType.PowerState:
			{
				int num = 0;
				if (led.S0Supported)
				{
					num++;
				}
				if (led.S3Supported)
				{
					num++;
				}
				if (led.ReadyModeSupported)
				{
					num++;
				}
				if (led.S5Supported)
				{
					num++;
				}
				return PowerStateGradient(led.SelectedColors, num);
			}
			case IndicatorType.PowerLimit:
				if (selectedIndicator is PowerLimitIndicator_Multi powerLimitIndicator_Multi && powerLimitIndicator_Multi.GetControlItems().FirstOrDefault((ControlItem i) => i.Index == 0).Value == 0)
				{
					return OtherGradient(Colors.Green, Colors.Red);
				}
				return OtherGradient(led.SelectedColors[0].SelectedColor, Colors.Black);
			case IndicatorType.Software:
				return OtherGradient(led.SelectedColors[0].SelectedColor, Colors.Black);
			default:
				return new SolidColorBrush(led.SelectedColor);
			}
		}
		return new SolidColorBrush(Colors.Transparent);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private LinearGradientBrush PowerStateGradient(ObservableCollection<WmiColorPickerData> colors, int numOfStates)
	{
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush
		{
			StartPoint = new Point(0.0, 0.0),
			EndPoint = new Point(0.0, 1.0)
		};
		switch (numOfStates)
		{
		case 4:
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0].SelectedColor, 0.0));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0].SelectedColor, 0.25));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[1].SelectedColor, 0.25));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[1].SelectedColor, 0.5));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[2].SelectedColor, 0.5));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[2].SelectedColor, 0.75));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[3].SelectedColor, 0.75));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[3].SelectedColor, 1.0));
			break;
		case 3:
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0].SelectedColor, 0.0));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0].SelectedColor, 0.33));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[1].SelectedColor, 0.33));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[1].SelectedColor, 0.66));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[2].SelectedColor, 0.66));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[2].SelectedColor, 1.0));
			break;
		case 2:
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0].SelectedColor, 0.0));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0].SelectedColor, 0.5));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[1].SelectedColor, 0.5));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[1].SelectedColor, 1.0));
			break;
		case 1:
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0].SelectedColor, 0.0));
			linearGradientBrush.GradientStops.Add(new GradientStop(colors[0].SelectedColor, 1.0));
			break;
		}
		return linearGradientBrush;
	}

	private LinearGradientBrush OtherGradient(Color color1, Color color2)
	{
		return new LinearGradientBrush
		{
			StartPoint = new Point(0.0, 0.5),
			EndPoint = new Point(1.0, 0.5),
			GradientStops = 
			{
				new GradientStop(color1, 0.0),
				new GradientStop(color2, 1.0)
			}
		};
	}
}
