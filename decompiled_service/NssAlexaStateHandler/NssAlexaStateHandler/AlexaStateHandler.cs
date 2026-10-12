using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Alexa.Extensions;

namespace NssAlexaStateHandler;

[Export(typeof(IAlexaStateHandler))]
public class AlexaStateHandler : IAlexaStateHandler
{
	private AlexaStateManager _stateManager;

	public AlexaStateHandler()
	{
		Task.Run(() =>
		{
			_stateManager = new AlexaStateManager();
		});
	}

	~AlexaStateHandler()
	{
		try
		{
			_stateManager.CloseConnection();
		}
		catch
		{
		}
	}

	public async Task OnStateChangedAsync(AlexaState newState)
	{
		Task task = Task.Run(() =>
		{
			_stateManager.SetAlexaState(newState);
		});
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
		}
	}
}
