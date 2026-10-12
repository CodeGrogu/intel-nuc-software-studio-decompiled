using System.Threading.Tasks;

namespace Alexa.Extensions;

public interface IAlexaStateHandler
{
	Task OnStateChangedAsync(AlexaState newState);
}
