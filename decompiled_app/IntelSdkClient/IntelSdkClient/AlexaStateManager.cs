using System.ServiceModel;
using IntelSdkClient.AlexaStateService;

namespace IntelSdkClient;

public class AlexaStateManager
{
	private const string ALEXA_STATE_SERVICE_URI = "http://localhost:43563/NssService/service/AlexaState";

	private WSHttpBinding _alexaStateBinding;

	private EndpointAddress _alexaStateEndpoint;

	private ChannelFactory<IAlexaStateServiceContract> _channelFactory;

	private IAlexaStateServiceContract _client;

	public AlexaStateManager()
	{
		CreateChannel();
	}

	public void SetAlexaState(AlexaState newState)
	{
		if (_client == null)
		{
			CreateChannel();
		}
		_client.SetAlexaState(newState);
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
