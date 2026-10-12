using System.Collections.Generic;

namespace SerialDeviceAccessLibrary;

public class LedStatus
{
	public List<LedPattern> LedPatternList { get; set; } = new List<LedPattern>();

	public BntStatus ButtonStatus { get; set; }
}
