using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Infrastructure.Models;

namespace SystemMonitorMgr.Converters;

public class ClientStateToConnectedConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value is ClientState clientState && clientState == ClientState.Success) ? Visibility.Hidden : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
