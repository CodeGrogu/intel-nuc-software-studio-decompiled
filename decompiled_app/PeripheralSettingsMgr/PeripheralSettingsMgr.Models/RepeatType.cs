using System.ComponentModel;

namespace PeripheralSettingsMgr.Models;

public enum RepeatType
{
	[Description("None")]
	None,
	[Description("Daily")]
	Daily,
	[Description("Weekly")]
	Weekly,
	[Description("Monthly")]
	Monthly,
	[Description("Yearly")]
	Yearly
}
