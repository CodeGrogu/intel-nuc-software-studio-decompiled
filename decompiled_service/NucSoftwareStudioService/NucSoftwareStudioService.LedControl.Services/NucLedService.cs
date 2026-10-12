using System;
using System.ServiceModel;
using Infrastructure.WMI;
using NucSoftwareStudioService.Wcf;

namespace NucSoftwareStudioService.LedControl.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class NucLedService : INucLedServiceContract
{
	public byte[] LedCommand(string CommandName, byte[] Parameters)
	{
		try
		{
			return WmiWrapper.WmiCall(CommandName, Parameters);
		}
		catch (Exception)
		{
			return new byte[0];
		}
	}

	public bool CheckConnectionHealth()
	{
		return true;
	}
}
