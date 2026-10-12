using System.ServiceModel;
using IntelSdkClient.NucLedService;

namespace IntelSdkClient.Proxy;

public class NucLedServiceProxy : ClientBase<INucLedServiceContract>
{
	public byte[] Wmi(string CommandName, byte[] Parameters)
	{
		return Channel.LedCommand(CommandName, Parameters);
	}
}
