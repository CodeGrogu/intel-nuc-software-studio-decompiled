using Infrastructure.WMI;
using NucSoftwareStudioService.LedControl.Models;

namespace NucSoftwareStudioService.LedControl.WMI;

public static class AlexaWmiControl
{
	private static readonly byte[] OFF_PARAMETERS;

	private static string _alexaLedControlMethod;

	public static void SetAlexaLightBarState(byte State, byte Speed, byte Brightness, byte Direction)
	{
		if (string.IsNullOrEmpty(_alexaLedControlMethod))
		{
			GetAlexaLedControlMethod();
		}
		byte[] parameters = new byte[8] { 1, State, Speed, Brightness, Direction, 0, 0, 0 };
		byte[] array = WmiWrapper.WmiCall(_alexaLedControlMethod, parameters);
		if (array.Length >= 1)
		{
			_ = array[0];
		}
	}

	public static void SetAlexaLightBarOff()
	{
		if (string.IsNullOrEmpty(_alexaLedControlMethod))
		{
			GetAlexaLedControlMethod();
		}
		byte[] array = WmiWrapper.WmiCall(_alexaLedControlMethod, OFF_PARAMETERS);
		if (array.Length >= 1)
		{
			_ = array[0];
		}
	}

	private static void GetAlexaLedControlMethod()
	{
		if (WmiWrapper.DoesWmiMethodExist("AlexaLedControl"))
		{
			_alexaLedControlMethod = "AlexaLedControl";
		}
		else if (WmiWrapper.DoesWmiMethodExist("PlatformConfigurationAccessV2"))
		{
			_alexaLedControlMethod = "PlatformConfigurationAccessV2";
		}
		else
		{
			_alexaLedControlMethod = "AlexaLedControl";
		}
	}

	static AlexaWmiControl()
	{
		byte[] array = new byte[8] { 1, 0, 5, 100, 3, 0, 0, 0 };
		array[1] = AlexaConstants.BLANK;
		OFF_PARAMETERS = array;
		_alexaLedControlMethod = string.Empty;
	}
}
