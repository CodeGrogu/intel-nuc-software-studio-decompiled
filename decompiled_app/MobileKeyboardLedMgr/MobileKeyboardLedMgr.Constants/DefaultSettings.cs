using MobileKeyboardLedMgr.Models;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Constants;

public static class DefaultSettings
{
	public const string EFFECT = "Monocolor";

	public const bool PROJECT_AURORA_ENABLED = false;

	public const int BRIGHTNESS_AC = 100;

	public const int BRIGHTNESS_DC = 50;

	public const int IDLE_TIMER_AC_SEC = 0;

	public static readonly int IDLE_TIMER_AC_VAL = KeyboardIdleTimer.ConvertSecondsToIdleTImer(0);

	public const int IDLE_TIMER_DC_SEC = 30;

	public static readonly int IDLE_TIMER_DC_VAL = KeyboardIdleTimer.ConvertSecondsToIdleTImer(30);

	public const int SPEED = 5;

	public const int SPEED_UNSUPPORTED = 0;

	public const string BEHAVIOR_DIRECTION = "Right";

	public const RGBKeyboardDirection BEHAVIOR_DIRECTION_WAVE = RGBKeyboardDirection.Right;

	public const string BEHAVIOR_EFFECT = "Auto";
}
