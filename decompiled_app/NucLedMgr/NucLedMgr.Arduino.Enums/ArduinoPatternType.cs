using System.ComponentModel;

namespace NucLedMgr.Arduino.Enums;

public enum ArduinoPatternType
{
	[Description("Off")]
	Off,
	[Description("Solid")]
	Solid,
	[Description("Pulse")]
	Pulse,
	[Description("Breathing")]
	Breathing,
	[Description("Strobing")]
	Strobing,
	[Description("PulseTrain1")]
	PulseTrain1,
	[Description("PulseTrain2")]
	PulseTrain2,
	[Description("PulseTrain3")]
	PulseTrain3,
	[Description("Rainbow1")]
	Rainbow1,
	[Description("Rainbow2")]
	Rainbow2
}
