using System.Collections.Generic;
using IntelSdkClient.EneService;

namespace IntelSdkClient.ENE.Interface;

public interface IEneClient
{
	void EneCommands(string CommandName);

	bool GetConnectionStatus();

	LedStatus GetCurrentPatterns();

	void SetLedPattern(List<LedPattern> patternList);

	string GetLEDFirmwareVersion();

	string CheckSeriportConnection();

	byte[] LedConnectionStatus();
}
