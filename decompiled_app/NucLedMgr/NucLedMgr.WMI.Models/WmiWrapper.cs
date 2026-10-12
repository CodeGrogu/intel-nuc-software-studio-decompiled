using System;
using System.Collections;
using System.Collections.Generic;
using FeatureMgr.Enums;
using NucLedMgr.WMI.Enums;

namespace NucLedMgr.WMI.Models;

public static class WmiWrapper
{
	public delegate byte[] WmiCallDelegate(string CommandName, byte[] Parameters);

	public static readonly byte NO_ERROR = 0;

	public static readonly byte FUNCTION_NOT_SUPPORTED = 225;

	public static readonly byte UNDEFINED_DEVICE = 226;

	public static readonly byte EC_NO_RESPONSE = 227;

	public static readonly byte INVALID_PARAMETER = 228;

	public static readonly byte UNEXPECTED_ERROR = 239;

	private const string QUERY_LED_FUNCTION_NAME = "QueryLed";

	private const string QUERY_LED_FUNCTION_NAME_SERPENT = "QueryLedSupportCapability";

	private const string GET_LED_STATUS_FUNCTION_NAME = "NewGetLedStatus";

	private const string SET_LED_INDICATOR_FUNCTION_NAME = "SetIndicatorOptionLedType";

	private const string SET_LED_INDICATOR_FUNCTION_NAME_SERPANT = "SetAnIndicatorOptionForTheLedType";

	private const string SET_LED_INDICATOR_CONTROL_ITEMS_FUNCTION_NAME = "SetValueIndicatorOptionLedType";

	private const string SAVE_BIOS_DEFAULTS_FUNCTION_NAME = "AppNotify";

	private const string SAVE_BIOS_DEFAULTS_FUNCTION_NAME_SERPENT = "NotificationLedApp";

	private const string ALEXA_LED_FUNCTION = "AlexaLEDControl";

	private const byte SET_ALEXA_FUNCTION = 3;

	private static bool SetAlexaLED(bool isEnable, WmiCallDelegate WMI)
	{
		byte[] array = null;
		array = ((!isEnable) ? new byte[8] { 3, 1, 0, 0, 0, 0, 0, 0 } : new byte[8] { 3, 2, 0, 0, 0, 0, 0, 0 });
		byte[] array2 = WMI("AlexaLEDControl", array);
		if (array2.Length >= 1 && array2[0] == NO_ERROR)
		{
			return true;
		}
		return false;
	}

	private static bool QueryAlexaLED(WmiCallDelegate WMI)
	{
		byte[] parameters = new byte[8] { 2, 0, 0, 0, 0, 0, 0, 0 };
		byte[] array = WMI("AlexaLEDControl", parameters);
		if (array.Length >= 1 && array[0] == NO_ERROR)
		{
			if (array[3] == 2)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static bool SetIndicatorOption(byte Led, byte Indicator, WmiCallDelegate WMI, NucType nucType)
	{
		byte[] parameters = new byte[4] { Led, Indicator, 0, 0 };
		byte[] array = new byte[0];
		array = ((nucType != NucType.Serpent) ? WMI("SetIndicatorOptionLedType", parameters) : WMI("SetAnIndicatorOptionForTheLedType", parameters));
		if (array.Length >= 1 && array[0] == NO_ERROR)
		{
			SaveBIOSDefaults(WMI, nucType);
			return true;
		}
		return false;
	}

	public static bool SetIndicatorControlOption(byte Led, byte Indicator, byte Index, byte Value, WmiCallDelegate WMI, NucType nucType)
	{
		byte[] parameters = new byte[4] { Led, Indicator, Index, Value };
		byte[] array = WMI("SetValueIndicatorOptionLedType", parameters);
		if (array.Length >= 1 && array[0] == NO_ERROR)
		{
			SaveBIOSDefaults(WMI, nucType);
			return true;
		}
		return false;
	}

	public static byte[] GetLEDTypes(WmiCallDelegate WMI, NucType nucType)
	{
		byte b = 0;
		byte[] parameters = new byte[4] { b, 0, 0, 0 };
		byte[] array = new byte[0];
		array = ((nucType != NucType.Serpent) ? WMI("QueryLed", parameters) : WMI("QueryLedSupportCapability", parameters));
		if (array != null && array.Length >= 4 && array[0] == NO_ERROR)
		{
			byte[] array2 = new byte[3];
			Array.Copy(array, 1, array2, 0, 3);
			return array2;
		}
		return new byte[0];
	}

	public static byte? QueryLEDColorType(byte Led, WmiCallDelegate WMI, NucType nucType)
	{
		byte b = 1;
		byte[] parameters = new byte[4] { b, Led, 0, 0 };
		byte[] array = new byte[0];
		array = ((nucType != NucType.Serpent) ? WMI("QueryLed", parameters) : WMI("QueryLedSupportCapability", parameters));
		if (array.Length >= 2 && array[0] == NO_ERROR)
		{
			return array[1];
		}
		return null;
	}

	public static byte[] QuerySupportedIndicators(byte Led, WmiCallDelegate WMI, NucType nucType)
	{
		byte b = 2;
		byte[] parameters = new byte[4] { b, Led, 0, 0 };
		byte[] array = new byte[0];
		array = ((nucType != NucType.Serpent) ? WMI("QueryLed", parameters) : WMI("QueryLedSupportCapability", parameters));
		if (array != null && array.Length >= 4 && array[0] == NO_ERROR)
		{
			byte[] array2 = new byte[3];
			Array.Copy(array, 1, array2, 0, 3);
			return array2;
		}
		return new byte[0];
	}

	public static byte[] QuerySupportedIndicatorControlItems(byte Led, byte Indicator, WmiCallDelegate WMI, NucType nucType)
	{
		byte b = 3;
		byte[] parameters = new byte[4] { b, Led, Indicator, 0 };
		byte[] array = new byte[0];
		array = ((nucType != NucType.Serpent) ? WMI("QueryLed", parameters) : WMI("QueryLedSupportCapability", parameters));
		if (array != null && array.Length >= 4 && array[0] == NO_ERROR)
		{
			byte[] array2 = new byte[3];
			Array.Copy(array, 1, array2, 0, 3);
			return array2;
		}
		return new byte[0];
	}

	public static byte? GetIndicatorOption(byte Led, WmiCallDelegate WMI)
	{
		byte b = 0;
		byte[] parameters = new byte[4] { b, Led, 0, 0 };
		byte[] array = WMI("NewGetLedStatus", parameters);
		if (array.Length >= 2 && array[0] == NO_ERROR)
		{
			return array[1];
		}
		return null;
	}

	public static byte? GetIndicatorControlItem(byte Led, byte Indicator, byte Index, WmiCallDelegate WMI)
	{
		byte b = 1;
		byte[] parameters = new byte[4] { b, Led, Indicator, Index };
		byte[] array = WMI("NewGetLedStatus", parameters);
		if (array.Length >= 2 && array[0] == NO_ERROR)
		{
			return array[1];
		}
		return null;
	}

	public static byte? SaveBIOSDefaults(WmiCallDelegate WMI, NucType nucType)
	{
		byte b = 1;
		byte[] parameters = new byte[4] { b, 0, 0, 0 };
		byte[] array = new byte[0];
		array = ((nucType == NucType.Serpent) ? WMI("NotificationLedApp", parameters) : WMI("AppNotify", parameters));
		if (array.Length >= 2 && array[0] == NO_ERROR)
		{
			return array[1];
		}
		return null;
	}

	public static List<LedType> GetSupportedLeds(WmiCallDelegate WMI, NucType nucType)
	{
		List<LedType> list = new List<LedType>();
		BitArray bitArray = new BitArray(GetLEDTypes(WMI, nucType));
		for (int i = 0; i < bitArray.Count; i++)
		{
			if (bitArray[i])
			{
				list.Add((LedType)i);
			}
		}
		return list;
	}

	public static List<IndicatorType> GetSupportedIndicators(LedType led, WmiCallDelegate WMI, NucType nucType)
	{
		List<IndicatorType> list = new List<IndicatorType>();
		BitArray bitArray = new BitArray(QuerySupportedIndicators((byte)led, WMI, nucType));
		for (int i = 0; i < bitArray.Count; i++)
		{
			if (bitArray[i])
			{
				list.Add((IndicatorType)i);
			}
		}
		return list;
	}

	public static BitArray GetSupportedControlItems(LedType led, IndicatorType indicator, WmiCallDelegate WMI, NucType nucType)
	{
		return new BitArray(QuerySupportedIndicatorControlItems((byte)led, (byte)indicator, WMI, nucType));
	}

	public static ColorType GetColorType(LedType led, WmiCallDelegate WMI, NucType nucType)
	{
		byte? b = QueryLEDColorType((byte)led, WMI, nucType);
		if (b.HasValue)
		{
			return (ColorType)b.Value;
		}
		return ColorType.Error;
	}

	public static IndicatorType GetIndicator(LedType Led, WmiCallDelegate WMI)
	{
		byte? indicatorOption = GetIndicatorOption((byte)Led, WMI);
		if (indicatorOption.HasValue)
		{
			return (IndicatorType)indicatorOption.Value;
		}
		return IndicatorType.Error;
	}

	public static byte? GetControlItemValue(LedType Led, IndicatorType Indicator, byte Index, WmiCallDelegate WMI)
	{
		return GetIndicatorControlItem((byte)Led, (byte)Indicator, Index, WMI);
	}

	public static bool SetIndicator(LedType Led, IndicatorType Indicator, WmiCallDelegate WMI, NucType nucType)
	{
		return SetIndicatorOption((byte)Led, (byte)Indicator, WMI, nucType);
	}

	public static bool SetControlItem(LedType Led, IndicatorType Indicator, byte Index, byte Value, WmiCallDelegate WMI, NucType nucType)
	{
		return SetIndicatorControlOption((byte)Led, (byte)Indicator, Index, Value, WMI, nucType);
	}

	public static bool SetAlexa(bool Enable, WmiCallDelegate WMI)
	{
		return SetAlexaLED(Enable, WMI);
	}

	public static bool QueryAlexa(WmiCallDelegate WMI)
	{
		return QueryAlexaLED(WMI);
	}
}
