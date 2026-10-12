using System;
using System.Collections.Generic;
using System.IO;
using CustomizationMgr.Enums;
using Infrastructure.Converters;
using Infrastructure.Models;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models;

public static class KeyboardLedConstants
{
	public const string KEYBOARD_LED_CARD_TITLE = "KeyboardLedSummaryCardTitleString";

	public const string DETAIL_CARD_NAME = "KeyboardView";

	public const IconType CARD_ICON_TYPE = IconType.Keyboard;

	public static readonly string DEFAULT_NAME = ResourceDictionaryConverter.GetString("DefaultProfileString");

	public static readonly string CUSTOM_NAME = ResourceDictionaryConverter.GetString("CustomProfileString");

	public const string JSON_FILE_EXTENSION = ".json";

	public static readonly string PROFILES_DIRECTORY = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "KeyboardLedProfiles\\");

	public static readonly string ENABLE_LEDS = ResourceDictionaryConverter.GetString("EnableLedsString");

	public static readonly string DISABLE_LEDS = ResourceDictionaryConverter.GetString("DisableLedsString");

	public static readonly string USE_PROJECT_AURORA = ResourceDictionaryConverter.GetString("UseProjectAuroraString");

	public const int BRIGHTNESS_MIN = 0;

	public const int BRIGHTNESS_MAX = 100;

	public const int DEFAULT_BRIGHTNESS = 100;

	public const int SPEED_MIN = 0;

	public const int SPEED_MAX = 9;

	public const int IDLE_TIMER_MIN = 0;

	public const int IDLE_TIMER_MAX = 13;

	public const int IDLE_TIMER_INTERVAL = 1;

	public static readonly List<Type> ACCEPTED_JSON_TYPES = new List<Type>
	{
		typeof(SerializableKeyboardProfile),
		typeof(SerializableKeyboardPattern),
		typeof(KeyColorData),
		typeof(SerializableColorPickerColors),
		typeof(RGBKeyboardDirection),
		typeof(RGBKeyboardEffect)
	};

	public const string NSS_APP_LAUNCHER_DIRECTORY = "NssOnStartupLauncher\\";

	public const string NSS_APP_LAUNCHER_EXE = "NssOnStartupLauncher.exe";

	public const string NSS_APP_LAUNCHER_AURORA_PARAM = "aurora";
}
