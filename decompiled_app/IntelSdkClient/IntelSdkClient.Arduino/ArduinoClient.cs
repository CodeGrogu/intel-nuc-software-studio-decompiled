using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using IntelSdkClient.Arduino.Interface;
using IntelSdkClient.ArduinoService;

namespace IntelSdkClient.Arduino;

public class ArduinoClient : IArduinoClient
{
	private const string ARDUINO_SERVICE_URI = "http://localhost:43567/NssService/service/Arduino";

	private WSHttpBinding _arduinoBinding;

	private EndpointAddress _arduinoEndpoint;

	private ChannelFactory<IArduinoServiceContract> _channelFactory;

	private IArduinoServiceContract _client;

	private const int STATE_CHECK_INTERVAL = 5000;

	private Timer _timer;

	private bool isConnectionHealthy;

	public ArduinoClient()
	{
		isConnectionHealthy = false;
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

	public bool GetConnectionStatus()
	{
		if (isConnectionHealthy)
		{
			return true;
		}
		return false;
	}

	public void Arduino(string CommandName)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			_client.ArdCommand(CommandName);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			_client.ArdCommand(CommandName);
		}
		catch (Exception)
		{
			isConnectionHealthy = false;
		}
	}

	private void CreateChannel()
	{
		_arduinoBinding = new WSHttpBinding();
		_arduinoEndpoint = new EndpointAddress("http://localhost:43567/NssService/service/Arduino");
		_channelFactory = new ChannelFactory<IArduinoServiceContract>(_arduinoBinding, _arduinoEndpoint);
		_client = _channelFactory.CreateChannel();
		isConnectionHealthy = true;
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

	public string GetLEDFirmwareVersion()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return GetFirmwareVersion();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return GetFirmwareVersion();
		}
		catch (Exception)
		{
			isConnectionHealthy = false;
			return string.Empty;
		}
	}

	private string GetFirmwareVersion()
	{
		try
		{
			return _client.GetControllerIdentifier();
		}
		catch (Exception)
		{
			return string.Empty;
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
}
