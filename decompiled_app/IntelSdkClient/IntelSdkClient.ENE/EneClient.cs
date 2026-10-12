using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using IntelSdkClient.ENE.Interface;
using IntelSdkClient.EneService;

namespace IntelSdkClient.ENE;

public class EneClient : IEneClient
{
	private const string ENE_SERVICE_URI = "http://localhost:43569/NssService/service/Ene";

	private WSHttpBinding _eneBinding;

	private EndpointAddress _eneEndPoint;

	private ChannelFactory<IEneServiceContract> _channelFactory;

	private IEneServiceContract _client;

	private const int STATE_CHECK_INTERVAL = 5000;

	private Timer _timer;

	private bool isConnectionHealthy;

	public EneClient()
	{
		isConnectionHealthy = false;
		CreateChannel();
		SetTimer();
	}

	private void CreateChannel()
	{
		_eneBinding = new WSHttpBinding();
		_eneEndPoint = new EndpointAddress("http://localhost:43569/NssService/service/Ene");
		_channelFactory = new ChannelFactory<IEneServiceContract>(_eneBinding, _eneEndPoint);
		_client = _channelFactory.CreateChannel();
		isConnectionHealthy = true;
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

	public void EneCommands(string CommandName)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			_client.EneCommand(CommandName);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			_client.EneCommand(CommandName);
		}
		catch (Exception)
		{
			isConnectionHealthy = false;
		}
	}

	public string CheckSeriportConnection()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.CheckSeriportConnection();
		}
		catch (CommunicationException)
		{
			try
			{
				_channelFactory.Abort();
				CreateChannel();
				return _client.CheckSeriportConnection();
			}
			catch (Exception)
			{
				return "LEDServiceComaptibilityError";
			}
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public bool GetConnectionStatus()
	{
		if (!isConnectionHealthy)
		{
			return false;
		}
		return true;
	}

	public LedStatus GetCurrentPatterns()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.GetCurrentPatterns();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.GetCurrentPatterns();
		}
		catch (Exception)
		{
			isConnectionHealthy = false;
			return new LedStatus();
		}
	}

	public string GetLEDFirmwareVersion()
	{
		throw new NotImplementedException();
	}

	public void SetLedPattern(List<LedPattern> patternList)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			_client.SetLedPattern(patternList.ToArray());
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			_client.SetLedPattern(patternList.ToArray());
		}
		catch (Exception)
		{
			isConnectionHealthy = false;
		}
	}

	public byte[] LedConnectionStatus()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.LedConnectionStatus();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.LedConnectionStatus();
		}
		catch (Exception)
		{
			isConnectionHealthy = false;
			return new byte[4];
		}
	}
}
