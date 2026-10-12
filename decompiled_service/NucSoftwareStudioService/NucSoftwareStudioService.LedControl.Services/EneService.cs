using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NucSoftwareStudioService.ENE;
using NucSoftwareStudioService.LedControl.Models;
using NucSoftwareStudioService.Wcf;
using SerialDeviceAccessLibrary;

namespace NucSoftwareStudioService.LedControl.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class EneService : IEneServiceContract
{
	public bool CheckConnectionHealth()
	{
		return true;
	}

	public string CheckSeriportConnection()
	{
		if (!EneWrapper.Connected)
		{
			EneWrapper.Connect();
		}
		return EneWrapper.SerialPortError;
	}

	public void EneCommand(string CommandName)
	{
		EneWrapper.EneCall(CommandName);
	}

	public string GetControllerIdentifier()
	{
		throw new NotImplementedException();
	}

	public NucSoftwareStudioService.LedControl.Models.LedStatus GetCurrentPatterns()
	{
		if (!EneWrapper.Connected)
		{
			EneWrapper.Connect();
		}
		if (EneWrapper.Connected)
		{
			SerialDeviceAccessLibrary.LedStatus currentPatternsV = SerialDeviceAccess.GetCurrentPatternsV2();
			NucSoftwareStudioService.LedControl.Models.LedStatus ledStatus = new NucSoftwareStudioService.LedControl.Models.LedStatus();
			ledStatus.ButtonStatus = currentPatternsV.ButtonStatus;
			if (currentPatternsV.LedPatternList.Count > 0)
			{
				foreach (SerialDeviceAccessLibrary.LedPattern ledPattern2 in currentPatternsV.LedPatternList)
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
		return new NucSoftwareStudioService.LedControl.Models.LedStatus();
	}

	public byte[] LedConnectionStatus()
	{
		return EneWrapper.LedConnectionStatus();
	}

	public void SetLedPattern(List<NucSoftwareStudioService.LedControl.Models.LedPattern> patternList)
	{
		if (!EneWrapper.Connected)
		{
			EneWrapper.Connect();
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
		SerialDeviceAccess.SetLEDPattern(list);
	}
}
