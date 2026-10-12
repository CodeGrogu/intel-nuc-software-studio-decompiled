using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Events;
using Infrastructure.Models;
using Prism.Events;
using Prism.Logging;
using UniwillSdkClient.Events;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace UniwillSdkClient.Models;

[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
public class UniwillClient : IUniwillClient
{
	private readonly ILoggerFacade _logger;

	private readonly IEventAggregator _events;

	private Timer _timer;

	private const int STATE_CHECK_INTERVAL = 5000;

	private UniwillCallbackHandler _callbackHandler;

	private InstanceContext _instanceContext;

	private UniwillServiceClient _client;

	private bool _monitorAllGpu;

	private RGBKeyboardCapabilities? _keyboardType;

	public UniwillServiceClient Client => _client;

	public UniwillClient(ILoggerFacade logger, IEventAggregator events)
	{
		_logger = logger;
		_events = events;
	}

	public void Initialize()
	{
		_callbackHandler = new UniwillCallbackHandler(_events);
		_instanceContext = new InstanceContext(_callbackHandler);
		_events.GetEvent<SubscribeEvent>().Subscribe(SysMonitorSubscribe);
		_events.GetEvent<SysMonViewOpenedEvent>().Subscribe(SysMonViewOpened);
		_client = new UniwillServiceClient(_instanceContext);
		Task.Run(() =>
		{
			CheckStatus();
		}).ConfigureAwait(continueOnCapturedContext: false);
		SetTimer();
		try
		{
			_keyboardType = _client.GetSupportedCapabilities();
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void SetTimer()
	{
		_timer = new Timer
		{
			Interval = 5000
		};
		_timer.Tick += MonitorTimer_Tick;
		_timer.Start();
	}

	private void MonitorTimer_Tick(object sender, EventArgs e)
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
				_client = new UniwillServiceClient(_instanceContext);
				SysMonitorSubscribe(subscribe: true);
			}
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			_instanceContext = new InstanceContext(_callbackHandler);
			_client = new UniwillServiceClient(_instanceContext);
			SysMonitorSubscribe(subscribe: true);
		}
	}

	private void SysMonitorSubscribe(bool subscribe)
	{
		try
		{
			if (subscribe && _monitorAllGpu)
			{
				_client.SysMonitorSubscribeAsync(new SysMonType[10]
				{
					SysMonType.Battery,
					SysMonType.Cpu,
					SysMonType.dGpu,
					SysMonType.Fan,
					SysMonType.HwFuelGauge,
					SysMonType.iGpu,
					SysMonType.Memory,
					SysMonType.Network,
					SysMonType.Storage,
					SysMonType.SystemInfo
				});
			}
			else if (subscribe && !_monitorAllGpu)
			{
				_client.SysMonitorSubscribeAsync(new SysMonType[10]
				{
					SysMonType.Battery,
					SysMonType.Cpu,
					SysMonType.dGpu_basic,
					SysMonType.Fan,
					SysMonType.HwFuelGauge,
					SysMonType.iGpu,
					SysMonType.Memory,
					SysMonType.Network,
					SysMonType.Storage,
					SysMonType.SystemInfo
				});
			}
			else
			{
				_client.StopMonitoringAsync();
			}
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void SysMonViewOpened(bool loaded)
	{
		_monitorAllGpu = loaded;
		SysMonitorSubscribe(subscribe: true);
	}

	public RGBKeyboardCapabilities GetKeyboardType()
	{
		if (!_keyboardType.HasValue)
		{
			_keyboardType = _client.GetSupportedCapabilities();
		}
		return _keyboardType.Value;
	}
}
