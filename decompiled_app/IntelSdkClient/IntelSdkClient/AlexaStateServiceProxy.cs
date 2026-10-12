using System.ServiceModel;
using IntelSdkClient.AlexaStateService;

namespace IntelSdkClient;

public class AlexaStateServiceProxy : ClientBase<IAlexaStateServiceContract>
{
	public void SetAlexaState(AlexaState state)
	{
		Channel.SetAlexaState(state);
	}
}
