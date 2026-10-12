using System.Diagnostics;
using System.Threading;
using ContextSensingClient;
using Prism.Events;
using Prism.Logging;
using UserPresenceMgr.Events;

namespace UserPresenceMgr.Models;

internal class HumanPresenceCallback : IFeatureCallback
{
	private bool error;

	private readonly ManualResetEvent responseEvent;

	private readonly Stopwatch stopWatch;

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	public HumanPresenceCallback(IEventAggregator events, ILoggerFacade logger)
	{
		_events = events;
		_logger = logger;
		responseEvent = new ManualResetEvent(initialState: false);
		Reset();
		stopWatch = new Stopwatch();
		stopWatch.Start();
	}

	public void Reset()
	{
		error = false;
		responseEvent.Reset();
	}

	public void OnSuccess(FeatureType featureType, ResponseType responseType)
	{
		_logger.Log("HumanPresenceCallback OnSuccess: " + featureType.ToString() + " " + responseType, Category.Debug, Priority.High);
		responseEvent.Set();
	}

	public void OnEvent(FeatureType featureType, EventType eventType, object eventPayload)
	{
		if (eventType == EventType.PROXIMITY)
		{
			float payload = (float)eventPayload;
			_events.GetEvent<UserPresenceProximityEvent>().Publish(payload);
		}
	}

	public void OnError(FeatureType featureType, Error state)
	{
		_logger.Log("HumanPresenceCallback OnError: " + featureType.ToString() + " " + state.Description.ToString(), Category.Debug, Priority.High);
		error = true;
		responseEvent.Set();
	}

	public int Wait(int timeoutMs)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (responseEvent.WaitOne(timeoutMs))
		{
			return (int)stopwatch.ElapsedMilliseconds;
		}
		return -1;
	}

	public void WaitLog(int timeoutMs)
	{
		if (Wait(timeoutMs) >= 0 && error)
		{
			_logger.Log("Context Sensing Service: Failed", Category.Debug, Priority.High);
		}
	}
}
