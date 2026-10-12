using System.Collections.Generic;
using IntelSdkClient.ArduinoService;

namespace IntelSdkClient.Arduino.Interface;

public interface IArduinoClient
{
	void Arduino(string CommandName);

	bool GetConnectionStatus();

	LedStatus GetCurrentPatterns();

	void SetLedPattern(List<LedPattern> patternList);

	string GetLEDFirmwareVersion();

	string CheckSeriportConnection();
}
