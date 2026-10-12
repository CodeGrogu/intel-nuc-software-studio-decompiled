namespace SerialDeviceAccessLibrary;

public static class DeviceControlCommand
{
	public const string TurnOffChannelA = "AP0\n";

	public const string TurnOffChannelB = "BP0\n";

	public const string TurnOffChannelC = "CP0\n";

	public const string TurnOffChannelD = "DP0\n";

	public const string GetControllerIDInfoCMD = "ID?\n";

	public const string ResetAllPatternCMD = "RST\n";

	public const string GetCurrentLEDPatternInfoCMD = "STA?\n";

	public const string SelectPresetPatternCMD = "P";

	public const string SetPaletteOneCMD = "C1:";

	public const string SetPaletteTwoCMD = "C2:";

	public const string SetPaletteThreeCMD = "C3:";

	public const string SetPaletteFourCMD = "C4:";

	public const string SetPaletteFiveCMD = "C5:";

	public const string SetPaletteSixCMD = "C6:";

	public const string SetPaletteSevenCMD = "C7:";

	public const string SetMaxBrightCMD = "V:";

	public const string SetRainBowCMD = "R:";

	public const string ModifiesPatternSpeedCMD = "D:";

	public const string EndCMDSymbol = "\n";

	public const string DisableAll = "BTN:0\n";

	public const string EnableAll = "BTN:1\n";

	public const string AutoNum = "AUTONUM?\n";
}
