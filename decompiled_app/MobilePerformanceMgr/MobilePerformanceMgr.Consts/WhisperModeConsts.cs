using UniwillSdkClient.UniwillService;

namespace MobilePerformanceMgr.Consts;

public static class WhisperModeConsts
{
	public const string BALANCED_STRING_KEY = "WhisperModeBalancedString";

	public const string QUIET_STRING_KEY = "WhisperModeQuietString";

	public const string QUIETER_STRING_KEY = "WhisperModeQuieterString";

	public const bool DEFAULT_DISABLED_BOOL = false;

	public const WhisperModeType DEFAULT_ENABLED_TYPE = WhisperModeType.Balanced;

	public const int DEFAULT_MIN_FPS = 60;

	public const int SETTING_MIN = 1;

	public const int SETTING_MAX = 3;

	public const int MIN_FPS_MIN = 30;

	public const int MIN_FPS_MAX = 60;
}
