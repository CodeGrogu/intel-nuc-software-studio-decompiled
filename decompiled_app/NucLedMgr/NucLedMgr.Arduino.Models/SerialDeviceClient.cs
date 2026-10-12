using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Models;
using IntelSdkClient.Arduino.Interface;
using IntelSdkClient.ArduinoService;
using NucLedMgr.Arduino.Enums;
using NucLedMgr.Arduino.Events;
using Prism.Events;
using Prism.Logging;

namespace NucLedMgr.Arduino.Models;

public class SerialDeviceClient
{
	public const char MONSTER_COVE_SKULL_CHANNEL = 'A';

	public const char MONSTER_COVE_BOTTOM_LEFT_CHANNEL = 'B';

	public const char MONSTER_COVE_BOTTOM_RIGHT_CHANNEL = 'C';

	public const char MONSTER_COVE_FRONT_CHANNEL = 'D';

	private const string MONSTER_COVE_VID = "067B";

	private const string MONSTER_COVE_PID = "2323";

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private readonly IArduinoClient _arduinoClient;

	private List<LedPattern> previousLedPattern;

	public SerialDeviceClient(IEventAggregator events, ILoggerFacade logger, IArduinoClient arduinoClient)
	{
		_logger = logger;
		_events = events;
		_arduinoClient = arduinoClient;
		_events.GetEvent<PreviousLedSetEvent>().Subscribe(SetPreviousPattern);
	}

	private void SetPreviousPattern(List<LedPattern> pattern)
	{
		previousLedPattern = pattern;
	}

	public void Disconnect()
	{
		try
		{
			_arduinoClient.Arduino("DISCONNECT");
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void Reset()
	{
		try
		{
			if (_arduinoClient.GetConnectionStatus())
			{
				_events.GetEvent<LedAvailable>().Publish(payload: false);
				_arduinoClient.Arduino("RESET_ALL_PATTERN");
				_arduinoClient.Arduino("COMMAND_DELAY");
				Thread.Sleep(1000);
				LedStatus currentPatterns = _arduinoClient.GetCurrentPatterns();
				_events.GetEvent<CurrentArduinoPatternsEvent>().Publish(currentPatterns);
				_events.GetEvent<PreviousLedSetEvent>().Publish(currentPatterns.LedPatternList.ToList());
			}
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void DisableAll()
	{
		try
		{
			if (_arduinoClient.GetConnectionStatus())
			{
				_events.GetEvent<LedAvailable>().Publish(payload: false);
				_arduinoClient.Arduino("DISABLE_ALL");
				_arduinoClient.Arduino("COMMAND_DELAY");
				Thread.Sleep(1000);
				LedStatus currentPatterns = _arduinoClient.GetCurrentPatterns();
				_events.GetEvent<CurrentArduinoPatternsEvent>().Publish(currentPatterns);
			}
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void EnableAll()
	{
		try
		{
			if (_arduinoClient.GetConnectionStatus())
			{
				_events.GetEvent<LedAvailable>().Publish(payload: false);
				_arduinoClient.Arduino("ENABLE_ALL");
				_arduinoClient.Arduino("COMMAND_DELAY");
				if (previousLedPattern != null)
				{
					_arduinoClient.SetLedPattern(previousLedPattern);
				}
				Thread.Sleep(1000);
				LedStatus currentPatterns = _arduinoClient.GetCurrentPatterns();
				_events.GetEvent<CurrentArduinoPatternsEvent>().Publish(currentPatterns);
			}
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void SetLedPattern(List<LedPattern> patterns, bool SetOnly)
	{
		try
		{
			if (_arduinoClient.GetConnectionStatus())
			{
				_arduinoClient.SetLedPattern(patterns);
				if (!SetOnly)
				{
					_events.GetEvent<LedAvailable>().Publish(payload: false);
					_arduinoClient.Arduino("COMMAND_DELAY");
					Thread.Sleep(1000);
					LedStatus currentPatterns = _arduinoClient.GetCurrentPatterns();
					_events.GetEvent<CurrentArduinoPatternsEvent>().Publish(currentPatterns);
				}
			}
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void GetCurrentPattern()
	{
		try
		{
			if (_arduinoClient.GetConnectionStatus())
			{
				Task.Run(() =>
				{
					_events.GetEvent<LedAvailable>().Publish(payload: false);
					_arduinoClient.Arduino("COMMAND_DELAY");
					Thread.Sleep(1000);
					LedStatus currentPatterns = _arduinoClient.GetCurrentPatterns();
					_events.GetEvent<CurrentArduinoPatternsEvent>().Publish(currentPatterns);
				});
			}
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public static byte ConvertZoneToChannel(MonsterCoveZone Index)
	{
		return Index switch
		{
			MonsterCoveZone.Skull => (byte)65, 
			MonsterCoveZone.BottomLeft => (byte)66, 
			MonsterCoveZone.BottomRight => (byte)67, 
			MonsterCoveZone.BottomFront => (byte)68, 
			_ => (byte)0, 
		};
	}

	public static MonsterCoveZone ConvertChannelToZone(byte Channel)
	{
		return (char)Channel switch
		{
			'A' => MonsterCoveZone.Skull, 
			'B' => MonsterCoveZone.BottomLeft, 
			'C' => MonsterCoveZone.BottomRight, 
			'D' => MonsterCoveZone.BottomFront, 
			_ => MonsterCoveZone.Error, 
		};
	}
}
