using NucSoftwareStudioService.FeatureSupport.Models;

namespace NucSoftwareStudioService.LedControl.Models;

public class AlexaLedSettings
{
	public bool Enabled { get; set; }

	public FeatureInterfaceType LedInterface { get; set; }

	public AlexaAnimationSettings AlexaAnimationSettings { get; set; }

	public AlexaLedSettings()
	{
		Enabled = false;
		LedInterface = FeatureInterfaceType.Error;
		AlexaAnimationSettings = new AlexaAnimationSettings();
	}
}
