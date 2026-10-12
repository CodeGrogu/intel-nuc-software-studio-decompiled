using System.ComponentModel;

namespace PeripheralSettingsMgr.Models.DisplayModes;

public enum ColorTemperatureType : uint
{
	[Description("Unknown")]
	MC_COLOR_TEMPERATURE_UNKNOWN,
	[Description("4000")]
	MC_COLOR_TEMPERATURE_4000K,
	[Description("5000")]
	MC_COLOR_TEMPERATURE_5000K,
	[Description("6500")]
	MC_COLOR_TEMPERATURE_6500K,
	[Description("7500")]
	MC_COLOR_TEMPERATURE_7500K,
	[Description("8200")]
	MC_COLOR_TEMPERATURE_8200K,
	[Description("9300")]
	MC_COLOR_TEMPERATURE_9300K,
	[Description("10000")]
	MC_COLOR_TEMPERATURE_10000K,
	[Description("11500")]
	MC_COLOR_TEMPERATURE_11500K
}
