using Alexa.Extensions;

namespace NucSoftwareStudioService.LedControl.Models;

public static class AlexaConstants
{
	public const string ALEXA_EXTENSION_REGISTRY_KEY = "Software\\WOW6432Node\\Amazon\\Alexa";

	public const string ALEXA_EXTENSION_KEY_NAME = "Extension";

	public const string ALEXA_STATE_HANDLER_LIBRARY = "NssAlexaStateHandler.dll";

	public const byte MIN_SPEED = 10;

	public const byte MAX_SPEED = 1;

	public const byte DEFAULT_SPEED = 5;

	public const byte MIN_BRIGHTNESS = 1;

	public const byte MAX_BRIGHTNESS = 100;

	public const byte DEFAULT_BRIGHTNESS = 100;

	public const byte LEFT = 1;

	public const byte RIGHT = 2;

	public const byte CENTER = 3;

	public const byte DEFAULT_DIRECTION = 3;

	public static readonly byte BLANK = 1;

	public static readonly byte ACTIVE_WAKING = 2;

	public static readonly byte ACTIVE_DIRECTION = 3;

	public static readonly byte ACTIVE_ENDING = 4;

	public static readonly byte ACTIVE_THINKING = 5;

	public static readonly byte ACTIVE_TALKING = 6;

	public static readonly byte MICS_OFF_START = 7;

	public static readonly byte MICS_OFF = 8;

	public static readonly byte MICS_OFF_END = 9;

	public static readonly byte ALERT = 10;

	public static readonly byte ALERT_SHORT = 11;

	public static readonly byte NOTIFICATION_INCOMING = 12;

	public static readonly byte NOTIFICATION_UNHEARD = 13;

	public static readonly byte DO_NOT_DISTURB = 14;

	public static readonly byte BLUETOOTH_CONNECT = 15;

	public static readonly byte BLUETOOTH_DISCONNECT = 16;

	public static readonly byte ERROR = 17;

	public static byte AlexaStateToSetting(AlexaState alexaState)
	{
		return alexaState switch
		{
			AlexaState.ActiveListening => ACTIVE_DIRECTION, 
			AlexaState.Listening => ACTIVE_WAKING, 
			AlexaState.DoNotDisturb => DO_NOT_DISTURB, 
			AlexaState.MicrophoneOff => MICS_OFF, 
			AlexaState.IncomingNotification => NOTIFICATION_INCOMING, 
			AlexaState.NotificationQueued => NOTIFICATION_UNHEARD, 
			AlexaState.Speaking => ACTIVE_TALKING, 
			AlexaState.SystemError => ERROR, 
			AlexaState.Thinking => ACTIVE_THINKING, 
			_ => BLANK, 
		};
	}
}
