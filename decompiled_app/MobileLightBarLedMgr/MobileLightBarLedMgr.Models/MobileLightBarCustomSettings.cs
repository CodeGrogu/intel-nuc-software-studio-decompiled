using System;
using MobileLightBarLedMgr.Constants;
using Newtonsoft.Json;
using UniwillSdkClient.UniwillService;

namespace MobileLightBarLedMgr.Models;

[JsonObject(Title = "MobileLightBarSettings")]
public class MobileLightBarCustomSettings
{
	private static readonly LightbarProfile KING_COUNTY_LIGHT_BAR_PROFILE = new LightbarProfile
	{
		AcSetting = new LightbarSetting
		{
			Effect = LightBarEffect.Rainbow,
			R_Brightness = 100,
			G_Brightness = 100,
			B_Brightness = 100
		},
		DcSetting = new LightbarSetting
		{
			Effect = LightBarEffect.Monocolor,
			R_Brightness = 0,
			G_Brightness = 0,
			B_Brightness = 0
		},
		BreathingEnable = false
	};

	private static readonly Tuple<int, int> KING_COUNTY_DEFAULT_AC_DC_IDLE_TIMER_VALUES = new Tuple<int, int>(LightBarConstants.DEFAULT_IDLE_OFF, LightBarConstants.THIRTY_SECONDS);

	private static readonly int[] KING_COUNTY_DEFAULT_IDLE_TIMER_OPTIONS = new int[14]
	{
		LightBarConstants.DEFAULT_IDLE_OFF,
		LightBarConstants.FIVE_SECONDS,
		LightBarConstants.TEN_SECONDS,
		LightBarConstants.FIFTEEN_SECONDS,
		LightBarConstants.THIRTY_SECONDS,
		LightBarConstants.FOURTYFIVE_SECONDS,
		LightBarConstants.ONE_MINUTE,
		LightBarConstants.TWO_MINUTES,
		LightBarConstants.FIVE_MINUTES,
		LightBarConstants.TEN_MINUTES,
		LightBarConstants.THIRTY_MINUTES,
		LightBarConstants.ONE_HOUR,
		LightBarConstants.TWO_HOURS,
		LightBarConstants.THREE_HOURS
	};

	[JsonProperty]
	public LightbarProfile LightBarProfile { get; set; }

	[JsonProperty]
	public Tuple<int, int> AcDcIdleTimerValues { get; set; }

	[JsonProperty]
	public int[] IdleTimerValuesInSeconds { get; set; }

	[JsonConstructor]
	public MobileLightBarCustomSettings(LightbarProfile lightbarProfile, Tuple<int, int> acDcIdleTimerValues, int[] idleTimerValuesInSeconds)
	{
		LightBarProfile = lightbarProfile;
		AcDcIdleTimerValues = acDcIdleTimerValues;
		IdleTimerValuesInSeconds = idleTimerValuesInSeconds;
	}

	public static MobileLightBarCustomSettings GetDefaultSettingsKingCounty()
	{
		return new MobileLightBarCustomSettings(KING_COUNTY_LIGHT_BAR_PROFILE, KING_COUNTY_DEFAULT_AC_DC_IDLE_TIMER_VALUES, KING_COUNTY_DEFAULT_IDLE_TIMER_OPTIONS);
	}
}
