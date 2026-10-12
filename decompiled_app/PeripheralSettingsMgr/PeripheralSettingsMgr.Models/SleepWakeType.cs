using System.ComponentModel;

namespace PeripheralSettingsMgr.Models;

public enum SleepWakeType
{
	[Description("None")]
	None,
	[Description("Sleep")]
	Sleep,
	[Description("Wake")]
	Wake
}
