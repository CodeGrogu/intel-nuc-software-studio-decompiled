using Intel.Cst.Client;
using Prism.Events;
using Prism.Logging;

namespace UserPresenceCstMgr.Models;

internal class HumanPresenceCstCallback : IFeatureCallback
{
	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	public HumanPresenceCstCallback(IEventAggregator events, ILoggerFacade logger)
	{
		_events = events;
		_logger = logger;
	}

	public void OnSuccess(FeatureType featureType, ResponseType responseType)
	{
		_logger.Log("HumanPresenceCallback OnSuccess: " + featureType.ToString() + " " + responseType, Category.Debug, Priority.High);
	}

	public void OnEvent(FeatureType featureType, EventType eventType, object eventPayload)
	{
	}

	public void OnError(FeatureType featureType, Error state)
	{
		_logger.Log("HumanPresenceCallback OnError: " + featureType.ToString() + " " + state.Description.ToString(), Category.Debug, Priority.High);
	}
}
