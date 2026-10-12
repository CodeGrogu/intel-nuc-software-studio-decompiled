namespace PerformanceTuningLibrary;

public struct FanProfileRN
{
	public string fanType;

	public string CurrentBiosFanMode;

	public byte[] profile;

	public FanSettingRN[] fanSettings;
}
