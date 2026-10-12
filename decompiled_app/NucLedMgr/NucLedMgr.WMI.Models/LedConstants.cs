using Infrastructure.Converters;

namespace NucLedMgr.WMI.Models;

public static class LedConstants
{
	public static readonly string POWER_BUTTON_NAME = ResourceDictionaryConverter.GetString("PowerButtonString");

	public static readonly string SKULL_NAME = ResourceDictionaryConverter.GetString("SkullString");

	public static readonly string EYES_NAME = ResourceDictionaryConverter.GetString("EyesString");

	public static readonly string FRONT1_NAME = ResourceDictionaryConverter.GetString("Front1String");

	public static readonly string FRONT2_NAME = ResourceDictionaryConverter.GetString("Front2String");

	public static readonly string FRONT3_NAME = ResourceDictionaryConverter.GetString("Front3String");

	public static readonly string HDD_NAME = ResourceDictionaryConverter.GetString("HddString");

	public static readonly string MULTIPURPOSELED_NAME = ResourceDictionaryConverter.GetString("MultipurposeLED");

	public static readonly string DEFAULT = ResourceDictionaryConverter.GetString("DefaultString");

	public const string POWERBUTTON = "PowerButton";

	public const string SKULL = "Skull";

	public const string EYES = "Eyes";

	public const string FRONT1 = "Front1";

	public const string FRONT2 = "Front2";

	public const string FRONT3 = "Front3";

	public const string HDD = "HDD";

	public const string LEDS_AVAILABLE = "LedsAvailable";

	public const string SELECTED_LED = "SelectedLed";

	public const string LEDS = "Leds";

	public const string PROFILES = "Profiles";

	public const string SELECTED_PROFILE = "SelectedProfile";

	public const string CURRENT_PROFILE = "CurrentProfile";

	public const string SELECTED_COLOR = "SelectedColor";

	public const string SELECTED_COLORS = "SelectedColors";

	public const string ON = "On";

	public const string PROFILE_NAME = "ProfileName";

	public const string LEDS_LOADED = "LedsLoaded";

	public const string NUCTYPE = "NucType";

	public const string OPEN_DETAIL_VIEW = "OpenDetailView";

	public const string CHASSIS_LED_CARD_TITLE_STRING = "ChassisLedCardTitleString";

	public const string NUC_LED_VIEW = "NucLedView";

	public const string SELECTED_FREQUENCY = "SelectedFrequency";

	public const string SELECTED_BEHAVIOR = "SelectedBehavior";

	public const string SELECTED_INDICATOR = "SelectedIndicator";

	public const string LIGHTUP = "Lightup";

	public const string INDICATOR_NAME = "IndicatorName";

	public const string CURRENT_POWER_STATE = "CurrentPowerOption";

	public const string SUPPORTED_INDICATORS = "SupportedIndicators";

	public const string MICROSOFT_LINK = "https://www.microsoft.com/en-us/p/alexa/9n12z3cctcnz?cid=intellightring";

	public const string ALEXA_FOLDER_NAME = "57540AMZNMobileLLC.AmazonAlexa_";

	public const string ALEXA_ENABLE = "AlexaEnable";

	public const string PROCESS_COMPLETE = "ProcessComplete";

	public const string DUPLICATE_PROFILE_ERROR = "DuplicateProfileError";
}
