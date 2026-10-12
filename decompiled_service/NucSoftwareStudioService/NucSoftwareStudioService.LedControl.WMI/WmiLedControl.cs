using NucSoftwareStudioService.LedControl.Interfaces;

namespace NucSoftwareStudioService.LedControl.WMI;

public class WmiLedControl : ILedControl
{
	public void SetEffect(byte State, byte Speed, byte Brightness, byte Direction)
	{
		AlexaWmiControl.SetAlexaLightBarState(State, Speed, Brightness, Direction);
	}

	public void Shutdown()
	{
		AlexaWmiControl.SetAlexaLightBarOff();
	}

	public void TurnOff()
	{
		AlexaWmiControl.SetAlexaLightBarOff();
	}
}
