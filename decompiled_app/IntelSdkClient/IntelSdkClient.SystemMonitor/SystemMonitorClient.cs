using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using Infrastructure.Models;
using IntelSdkClient.SystemMonitorService;
using Prism.Events;

namespace IntelSdkClient.SystemMonitor;

public class SystemMonitorClient : WorkingClient
{
	private readonly SystemMonitorServiceCallbackHandler _callbackHandler;

	private InstanceContext _instanceContext;

	private SystemMonitorServiceContractClient _client;

	private Timer _timer;

	private const int STATE_CHECK_INTERVAL = 5000;

	public override string EndpointConfigName { get; } = WorkingClient.GetWorkingEndpointConfigName("ISystemMonitorServiceContract");

	public SystemMonitorServiceContractClient Client => _client;

	public SystemMonitorClient(IEventAggregator events)
	{
		_callbackHandler = new SystemMonitorServiceCallbackHandler(events);
		_instanceContext = new InstanceContext(_callbackHandler);
		_client = GetWorkingClient(_instanceContext);
		Task.Run(() =>
		{
			CheckStatus();
		}).ConfigureAwait(continueOnCapturedContext: false);
		SetTimer();
	}

	private SystemMonitorServiceContractClient GetWorkingClient(InstanceContext instContx)
	{
		if (!(EndpointConfigName == ""))
		{
			return new SystemMonitorServiceContractClient(instContx, EndpointConfigName);
		}
		return new SystemMonitorServiceContractClient(instContx);
	}

	private void SetTimer()
	{
		_timer = new Timer
		{
			Interval = 5000.0
		};
		_timer.Elapsed += Monitor_Timer_Tick;
		_timer.Start();
	}

	private void Monitor_Timer_Tick(object sender, ElapsedEventArgs e)
	{
		Task.Run(() =>
		{
			CheckStatus();
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void CheckStatus()
	{
		try
		{
			if (_client.InnerChannel.State == CommunicationState.Faulted || _client.InnerDuplexChannel.State == CommunicationState.Faulted || _client.InnerChannel.State == CommunicationState.Closed || _client.InnerDuplexChannel.State == CommunicationState.Closed)
			{
				_client.Abort();
				_instanceContext = new InstanceContext(_callbackHandler);
				_client = GetWorkingClient(_instanceContext);
				_client.Subscribe();
			}
		}
		catch (Exception)
		{
			_instanceContext = new InstanceContext(_callbackHandler);
			_client = GetWorkingClient(_instanceContext);
			_client.Subscribe();
		}
	}
}
