using System;

namespace MobileLightBarLedMgr.Constants;

public static class LightBarConstants
{
	public const string LIGHTBAR_DETAIL_VIEW_KEY = "LightbarDetailView";

	public const string LIGHTBAR_SUMMARY_VIEW_KEY = "LightbarSummaryView";

	public static readonly string LIGHTBAR_CARD_TITLE_STRING = "LightBarCardTitleString";

	public const int BRIGHTNESS_MAX = 100;

	public const int BRIGHTNESS_MIN = 0;

	public static readonly int DEFAULT_IDLE_OFF = (int)TimeSpan.FromSeconds(0.0).TotalSeconds;

	public static readonly int FIVE_SECONDS = (int)TimeSpan.FromSeconds(5.0).TotalSeconds;

	public static readonly int TEN_SECONDS = (int)TimeSpan.FromSeconds(10.0).TotalSeconds;

	public static readonly int FIFTEEN_SECONDS = (int)TimeSpan.FromSeconds(15.0).TotalSeconds;

	public static readonly int THIRTY_SECONDS = (int)TimeSpan.FromSeconds(30.0).TotalSeconds;

	public static readonly int FOURTYFIVE_SECONDS = (int)TimeSpan.FromSeconds(45.0).TotalSeconds;

	public static readonly int ONE_MINUTE = (int)TimeSpan.FromMinutes(1.0).TotalSeconds;

	public static readonly int TWO_MINUTES = (int)TimeSpan.FromMinutes(2.0).TotalSeconds;

	public static readonly int FIVE_MINUTES = (int)TimeSpan.FromMinutes(5.0).TotalSeconds;

	public static readonly int TEN_MINUTES = (int)TimeSpan.FromMinutes(10.0).TotalSeconds;

	public static readonly int THIRTY_MINUTES = (int)TimeSpan.FromMinutes(30.0).TotalSeconds;

	public static readonly int ONE_HOUR = (int)TimeSpan.FromHours(1.0).TotalSeconds;

	public static readonly int TWO_HOURS = (int)TimeSpan.FromHours(2.0).TotalSeconds;

	public static readonly int THREE_HOURS = (int)TimeSpan.FromHours(3.0).TotalSeconds;
}
