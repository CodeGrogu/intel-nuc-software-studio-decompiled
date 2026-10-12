namespace NucSoftwareStudioService.LedControl.Interfaces;

public interface ILedControl
{
	void SetEffect(byte State, byte Speed, byte Brightness, byte Direction);

	void TurnOff();

	void Shutdown();
}
