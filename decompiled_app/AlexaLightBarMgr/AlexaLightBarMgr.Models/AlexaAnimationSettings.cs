namespace AlexaLightBarMgr.Models;

public class AlexaAnimationSettings
{
	private const byte DEFAULT_SPEED = 50;

	private const byte DEFAULT_BRIGHTNESS = 50;

	private const byte DEFAULT_DIRECTION = 3;

	public byte Speed { get; set; }

	public byte Brightness { get; set; }

	public byte Direction { get; set; }

	public AlexaAnimationSettings()
	{
		Speed = 50;
		Brightness = 50;
		Direction = 3;
	}
}
