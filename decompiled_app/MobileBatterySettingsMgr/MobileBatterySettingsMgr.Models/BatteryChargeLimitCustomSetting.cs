using Newtonsoft.Json;

namespace MobileBatterySettingsMgr.Models;

[JsonObject(Title = "BatteryChargeLimit")]
public class BatteryChargeLimitCustomSetting
{
	private const int DEFAULT_LIMIT_KING_COUNTY = 100;

	[JsonProperty]
	public int DefaultLimit { get; set; }

	[JsonConstructor]
	public BatteryChargeLimitCustomSetting(int defaultLimit)
	{
		DefaultLimit = defaultLimit;
	}

	public static BatteryChargeLimitCustomSetting GetDefaultSettingKingCounty()
	{
		return new BatteryChargeLimitCustomSetting(100);
	}
}
