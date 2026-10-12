using System;
using System.Globalization;
using System.Windows.Data;
using Infrastructure.Converters;
using Infrastructure.Models;

namespace SystemMonitorMgr.Converters;

public class ClientStateToMessageConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is ClientState clientState)
		{
			switch (clientState)
			{
			case ClientState.Connecting:
				return ResourceDictionaryConverter.GetString("LoadingMessage");
			case ClientState.Error:
				return ResourceDictionaryConverter.GetString("ErrorLoadingMessage");
			}
		}
		return "";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
