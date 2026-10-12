using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NucSoftwareStudioService.Arduino;
using NucSoftwareStudioService.LedControl.Models;
using NucSoftwareStudioService.Wcf;
using SerialDeviceAccessLibrary;

namespace NucSoftwareStudioService.LedControl.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class ArduinoMockService : IArduinoServiceContract
{
	private SerialDeviceAccessMock serialDeviceAccess = new SerialDeviceAccessMock();

	public void ArdCommand(string CommandName)
	{
		if (serialDeviceAccess == null)
		{
			serialDeviceAccess = new SerialDeviceAccessMock();
		}
		switch (CommandName)
		{
		case "RESET_ALL_PATTERN":
			serialDeviceAccess.ResetAllPattern();
			break;
		case "ENABLE_ALL":
			serialDeviceAccess.EnableAll();
			break;
		case "DISABLE_ALL":
			serialDeviceAccess.DisableAll();
			break;
		case "COMMAND_DELAY":
			serialDeviceAccess.SendCommandDelayTime();
			break;
		case "DISCONNECT":
			serialDeviceAccess.DisconnectFromDevice();
			break;
		}
	}

	public bool CheckConnectionHealth()
	{
		return true;
	}

	public bool CheckSeriportConnection()
	{
		return true;
	}

	public string GetControllerIdentifier()
	{
		throw new NotImplementedException();
	}

	public NucSoftwareStudioService.LedControl.Models.LedStatus GetCurrentPatterns()
	{
		if (serialDeviceAccess == null)
		{
			serialDeviceAccess = new SerialDeviceAccessMock();
		}
		SerialDeviceAccessLibrary.LedStatus currentPatterns = serialDeviceAccess.GetCurrentPatterns();
		NucSoftwareStudioService.LedControl.Models.LedStatus ledStatus = new NucSoftwareStudioService.LedControl.Models.LedStatus();
		ledStatus.ButtonStatus = currentPatterns.ButtonStatus;
		if (currentPatterns.LedPatternList.Count > 0)
		{
			foreach (SerialDeviceAccessLibrary.LedPattern ledPattern2 in currentPatterns.LedPatternList)
			{
				NucSoftwareStudioService.LedControl.Models.LedPattern ledPattern = new NucSoftwareStudioService.LedControl.Models.LedPattern();
				ledPattern.Channel = ledPattern2.Channel;
				ledPattern.MaxBrightness = ledPattern2.MaxBrightness;
				ledPattern.PatternIndex = ledPattern2.PatternIndex;
				ledPattern.RainBow = ledPattern2.RainBow;
				ledPattern.Speed = ledPattern2.Speed;
				ledPattern.Palette = ledPattern2.Palette.ToArray();
				ledStatus.LedPatternList.Add(ledPattern);
			}
		}
		return ledStatus;
	}

	public void SetLedPattern(List<NucSoftwareStudioService.LedControl.Models.LedPattern> patternList)
	{
		if (serialDeviceAccess == null)
		{
			serialDeviceAccess = new SerialDeviceAccessMock();
		}
		List<SerialDeviceAccessLibrary.LedPattern> list = new List<SerialDeviceAccessLibrary.LedPattern>();
		foreach (NucSoftwareStudioService.LedControl.Models.LedPattern pattern in patternList)
		{
			SerialDeviceAccessLibrary.LedPattern ledPattern = new SerialDeviceAccessLibrary.LedPattern();
			ledPattern.Channel = pattern.Channel;
			ledPattern.MaxBrightness = pattern.MaxBrightness;
			ledPattern.PatternIndex = pattern.PatternIndex;
			ledPattern.RainBow = pattern.RainBow;
			ledPattern.Speed = pattern.Speed;
			ledPattern.Palette = ((pattern.Palette == null) ? new List<byte>() : pattern.Palette.ToList());
			list.Add(ledPattern);
		}
		serialDeviceAccess.SetLEDPattern(list);
	}

	string IArduinoServiceContract.CheckSeriportConnection()
	{
		throw new NotImplementedException();
	}
}
