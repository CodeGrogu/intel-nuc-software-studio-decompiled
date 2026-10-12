namespace PeripheralSettingsMgr.Models.DisplayModes;

public struct MonitorCapabilities
{
	public bool BrightnessSupported { get; set; }

	public bool ContrastSupported { get; set; }

	public bool ColorTemperatureSupported { get; set; }

	public bool GammaRampSupported { get; set; }
}
