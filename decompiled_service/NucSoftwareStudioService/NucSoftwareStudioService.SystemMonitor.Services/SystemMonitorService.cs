using System.ServiceModel;
using NucSoftwareStudioService.Wcf;

namespace NucSoftwareStudioService.SystemMonitor.Services;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class SystemMonitorService : ISystemMonitorServiceContract
{
	public delegate void SubscribeToSystemMonitor(bool Subscribe);

	private SubscribeToSystemMonitor _subscriptionCallback;

	public ISystemMonitorCallbackContract Callback => OperationContext.Current.GetCallbackChannel<ISystemMonitorCallbackContract>();

	public SystemMonitorService(SubscribeToSystemMonitor subscriptionCallback)
	{
		_subscriptionCallback = subscriptionCallback;
	}

	public void Subscribe()
	{
		_subscriptionCallback(Subscribe: true);
	}
}
