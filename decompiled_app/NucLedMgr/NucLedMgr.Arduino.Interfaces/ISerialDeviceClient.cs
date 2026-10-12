using System.Collections.Generic;
using IntelSdkClient.ArduinoService;

namespace NucLedMgr.Arduino.Interfaces;

public interface ISerialDeviceClient
{
	void Connect();

	void Disconnect();

	void Reset();

	void DisableAll();

	void EnableAll();

	void SetLedPattern(List<LedPattern> patterns, bool SetOnly);

	void GetCurrentPattern();
}
