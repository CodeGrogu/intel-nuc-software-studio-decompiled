namespace NucSoftwareStudioService.LedControl.Models;

public class AlexaAnimationSettings
{
	private const byte DEFAULT_SPEED = 6;

	private const byte DEFAULT_BRIGHTNESS = 100;

	private const byte DEFAULT_DIRECTION = 3;

	public byte Speed { get; set; }

	public byte Brightness { get; set; }

	public byte Direction { get; set; }

	public AlexaAnimationSettings()
	{
		Speed = 6;
		Brightness = 100;
		Direction = 3;
	}
}
