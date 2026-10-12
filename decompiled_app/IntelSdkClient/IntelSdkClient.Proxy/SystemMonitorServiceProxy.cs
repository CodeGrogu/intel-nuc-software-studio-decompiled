using System.ServiceModel;
using IntelSdkClient.SystemMonitorService;

namespace IntelSdkClient.Proxy;

public class SystemMonitorServiceProxy : ClientBase<ISystemMonitorServiceContract>
{
	public void Subscribe()
	{
		Channel.Subscribe();
	}
}
