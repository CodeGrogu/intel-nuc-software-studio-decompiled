using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using IntelSdkClient.Alexa.Interface;
using IntelSdkClient.AlexaSettingsService;

namespace IntelSdkClient.Alexa;

public class AlexaSettingsManager : IAlexaSettingsManager
{
	public const string ALEXA_SETTINGS_SERVICE_URI = "http://localhost:43562/NssService/service/AlexaSettings";

	private WSHttpBinding _alexaSettingsServiceBinding;

	private EndpointAddress _alexaSettingsServiceEndpoint;

	private ChannelFactory<IAlexaSettingsServiceContract> _channelFactory;

	private IAlexaSettingsServiceContract _client;

	private const int STATE_CHECK_INTERVAL = 10000;

	private Timer _timer;

	private bool isConnectionHealthy;

	public AlexaSettingsManager()
	{
		CreateChannel();
		SetTimer();
	}

	private void SetTimer()
	{
		_timer = new Timer
		{
			Interval = 10000.0
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

	public void SetLightBarState(bool enable)
	{
		try
		{
			_client.SetLightBarState(enable);
		}
		catch (Exception)
		{
		}
	}

	public void CreateChannel()
	{
		_alexaSettingsServiceBinding = new WSHttpBinding();
		_alexaSettingsServiceEndpoint = new EndpointAddress("http://localhost:43562/NssService/service/AlexaSettings");
		_channelFactory = new ChannelFactory<IAlexaSettingsServiceContract>(_alexaSettingsServiceBinding, _alexaSettingsServiceEndpoint);
		_client = _channelFactory.CreateChannel();
	}
}
