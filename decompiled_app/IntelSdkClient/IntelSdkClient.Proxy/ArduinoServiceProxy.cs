using System.Collections.Generic;
using System.ServiceModel;
using IntelSdkClient.ArduinoService;

namespace IntelSdkClient.Proxy;

public class ArduinoServiceProxy : ClientBase<IArduinoServiceContract>
{
	public void Arduino(string CommandName)
	{
		Channel.ArdCommand(CommandName);
	}

	public LedStatus GetCurrentPatterns()
	{
		return Channel.GetCurrentPatterns();
	}

	public void SetLedPattern(List<LedPattern> patternList)
	{
		Channel.SetLedPattern(patternList.ToArray());
	}
}
