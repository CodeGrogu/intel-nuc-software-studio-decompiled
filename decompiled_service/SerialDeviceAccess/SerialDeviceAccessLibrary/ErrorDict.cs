using System.Collections.Generic;

namespace SerialDeviceAccessLibrary;

public class ErrorDict
{
	public Dictionary<byte, string> MsgErrorDict { get; }

	public ErrorDict()
	{
		MsgErrorDict = new Dictionary<byte, string>
		{
			{ 0, "no error" },
			{ 1, "channel error" },
			{ 2, "pattern index error" },
			{ 3, "maxBrightness error" },
			{ 4, "pattle count error" },
			{ 5, "rainbow error" },
			{ 6, "speed error" }
		};
	}
}
