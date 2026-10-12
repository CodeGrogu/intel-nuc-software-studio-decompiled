using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using Alexa.Extensions;
using NssAlexaStateHandler.AlexaStateService;

namespace NssAlexaStateHandler;

public class AlexaStateManager
{
	private const string ALEXA_STATE_SERVICE_URI = "http://localhost:43563/NssService/service/AlexaState";

	private WSHttpBinding _alexaStateBinding;

	private EndpointAddress _alexaStateEndpoint;

	private ChannelFactory<IAlexaStateServiceContract> _channelFactory;

	private IAlexaStateServiceContract _client;

	private const int STATE_CHECK_INTERVAL = 5000;

	private Timer _timer;

	private bool isConnectionHealthy;

	public AlexaStateManager()
	{
		CreateChannel();
		SetTimer();
	}

	private void SetTimer()
	{
		_timer = new Timer
		{
			Interval = 5000.0
		};
		_timer.Elapsed += MonitorTimer_Tick;
		_timer.Start();
	}

	private void MonitorTimer_Tick(object sender, ElapsedEventArgs e)
	{
		Task.Run(() =>
		{
			CheckStatus();
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public void CheckStatus()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			isConnectionHealthy = _client.CheckConnectionHealth();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
		}
		catch (Exception)
		{
			isConnectionHealthy = false;
		}
	}

	public void SetAlexaState(AlexaState newState)
	{
		if (_client == null)
		{
			CreateChannel();
		}
		_client.SetAlexaState(newState);
		_client.CheckConnectionHealth();
	}

	public void CloseConnection()
	{
		if (_client != null)
		{
			((IClientChannel)_client).Close();
		}
	}

	private void CreateChannel()
	{
		_alexaStateBinding = new WSHttpBinding();
		_alexaStateEndpoint = new EndpointAddress("http://localhost:43563/NssService/service/AlexaState");
		_channelFactory = new ChannelFactory<IAlexaStateServiceContract>(_alexaStateBinding, _alexaStateEndpoint);
		_client = _channelFactory.CreateChannel();
	}
}
