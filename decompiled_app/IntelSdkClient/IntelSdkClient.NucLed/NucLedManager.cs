using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using IntelSdkClient.NucLed.Interface;
using IntelSdkClient.NucLedService;

namespace IntelSdkClient.NucLed;

public class NucLedManager : INucLedManager
{
	private const string NUC_LED_SERVICE_URI = "http://localhost:43565/NssService/service/NucLed";

	private WSHttpBinding _nucLedBinding;

	private EndpointAddress _nucLedEndpoint;

	private ChannelFactory<INucLedServiceContract> _channelFactory;

	private INucLedServiceContract _client;

	private const int STATE_CHECK_INTERVAL = 5000;

	private Timer _timer;

	private bool isConnectionHealthy;

	public NucLedManager()
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

	private void MonitorTimer_Tick(object sender, EventArgs e)
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
			isConnectionHealthy = false;
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

	private void CreateChannel()
	{
		_nucLedBinding = new WSHttpBinding();
		_nucLedEndpoint = new EndpointAddress("http://localhost:43565/NssService/service/NucLed");
		_channelFactory = new ChannelFactory<INucLedServiceContract>(_nucLedBinding, _nucLedEndpoint);
		_client = _channelFactory.CreateChannel();
	}

	public byte[] Wmi(string CommandName, byte[] Parameters)
	{
		try
		{
			return _client.LedCommand(CommandName, Parameters);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			_nucLedBinding = new WSHttpBinding();
			_nucLedEndpoint = new EndpointAddress("http://localhost:43565/NssService/service/NucLed");
			_channelFactory = new ChannelFactory<INucLedServiceContract>(_nucLedBinding, _nucLedEndpoint);
			_client = _channelFactory.CreateChannel();
			return _client.LedCommand(CommandName, Parameters);
		}
		catch (Exception)
		{
			return new byte[0];
		}
	}
}
