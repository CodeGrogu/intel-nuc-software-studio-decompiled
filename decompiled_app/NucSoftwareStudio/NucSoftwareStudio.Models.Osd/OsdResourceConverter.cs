using System.Windows.Media;
using Infrastructure.Converters;
using UniwillSdkClient.UniwillService;

namespace NucSoftwareStudio.Models.Osd;

public static class OsdResourceConverter
{
	public static DrawingImage OsdTypeToIcon(OsdType Type, int State)
	{
		string key = "";
		switch (Type)
		{
		case OsdType.AirplaneMode:
			switch (State)
			{
			case 0:
				key = "OsdAirplaneOffIcon";
				break;
			case 1:
				key = "OsdAirplaneOnIcon";
				break;
			}
			break;
		case OsdType.CapsLock:
			switch (State)
			{
			case 0:
				key = "OsdCapsLockOffIcon";
				break;
			case 1:
				key = "OsdCapsLockOnIcon";
				break;
			}
			break;
		case OsdType.DisableTouchpadToggle:
			switch (State)
			{
			case 1:
				key = "OsdTouchpadToggleDisabledIcon";
				break;
			case 0:
				key = "OsdTouchpadToggleEnabledIcon";
				break;
			}
			break;
		case OsdType.FnLock:
			switch (State)
			{
			case 1:
				key = "OsdFnLockOffIcon";
				break;
			case 0:
				key = "OsdFnLockOnIcon";
				break;
			}
			break;
		case OsdType.MicrophoneMute:
			switch (State)
			{
			case 0:
				key = "OsdMicMuteIcon";
				break;
			case 1:
				key = "OsdMicUnmuteIcon";
				break;
			}
			break;
		case OsdType.NumPad:
			switch (State)
			{
			case 0:
				key = "OsdNumPadOffIcon";
				break;
			case 1:
				key = "OsdNumPadOnIcon";
				break;
			}
			break;
		case OsdType.PerformanceMode:
			key = "OsdPerformanceModeIcon";
			break;
		case OsdType.RGBKeyboardBrightness:
			switch (State)
			{
			case 0:
				key = "OsdKeyboardBrightness0Icon";
				break;
			case 1:
				key = "OsdKeyboardBrightness50Icon";
				break;
			case 2:
				key = "OsdKeyboardBrightness100Icon";
				break;
			}
			break;
		case OsdType.TouchpadOn:
			switch (State)
			{
			case 0:
				key = "OsdTouchPadOffIcon";
				break;
			case 1:
				key = "OsdTouchPadOnIcon";
				break;
			}
			break;
		case OsdType.WindowsKeyLock:
			switch (State)
			{
			case 0:
				key = "OsdWindowsKeyLockedIcon";
				break;
			case 1:
				key = "OsdWindowsKeyUnlockedIcon";
				break;
			}
			break;
		}
		return ResourceDictionaryConverter.GetIconCanvas(key);
	}

	public static string OsdTypeToMessage(OsdType Type, int State)
	{
		string key = "";
		switch (Type)
		{
		case OsdType.AirplaneMode:
			switch (State)
			{
			case 0:
				key = "OsdAirplaneOffString";
				break;
			case 1:
				key = "OsdAirplaneOnString";
				break;
			}
			break;
		case OsdType.CapsLock:
			switch (State)
			{
			case 0:
				key = "OsdCapsLockOffString";
				break;
			case 1:
				key = "OsdCapsLockOnString";
				break;
			}
			break;
		case OsdType.DisableTouchpadToggle:
			switch (State)
			{
			case 1:
				key = "OsdTouchpadToggleDisabledString";
				break;
			case 0:
				key = "OsdTouchpadToggleEnabledString";
				break;
			}
			break;
		case OsdType.FnLock:
			switch (State)
			{
			case 1:
				key = "OsdFnLockOffString";
				break;
			case 0:
				key = "OsdFnLockOnString";
				break;
			}
			break;
		case OsdType.MicrophoneMute:
			switch (State)
			{
			case 0:
				key = "OsdMicMuteString";
				break;
			case 1:
				key = "OsdMicUnmuteString";
				break;
			}
			break;
		case OsdType.NumPad:
			switch (State)
			{
			case 0:
				key = "OsdNumPadOffString";
				break;
			case 1:
				key = "OsdNumPadOnString";
				break;
			}
			break;
		case OsdType.PerformanceMode:
			switch (State)
			{
			case 1:
				key = "OsdPerformanceMode" + 1 + "String";
				break;
			case 2:
				key = "OsdPerformanceMode" + 2 + "String";
				break;
			case 3:
				key = "OsdPerformanceMode" + 3 + "String";
				break;
			}
			break;
		case OsdType.TouchpadOn:
			switch (State)
			{
			case 0:
				key = "OsdTouchPadOffString";
				break;
			case 1:
				key = "OsdTouchPadOnString";
				break;
			}
			break;
		case OsdType.WindowsKeyLock:
			switch (State)
			{
			case 0:
				key = "OsdWindowsKeyLockedString";
				break;
			case 1:
				key = "OsdWindowsKeyUnlockedString";
				break;
			}
			break;
		}
		return ResourceDictionaryConverter.GetString(key);
	}
}
