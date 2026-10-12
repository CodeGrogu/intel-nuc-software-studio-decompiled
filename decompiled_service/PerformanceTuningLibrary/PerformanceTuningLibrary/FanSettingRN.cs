namespace PerformanceTuningLibrary;

public struct FanSettingRN
{
	public string FanModeName;

	public byte[] FanModeBytes;

	public byte PrimaryTemperatureSourceAndTemperatureHystersis;

	public byte RampAccelerationTemperature;

	public byte PrimaryMinimumTemperature;

	public byte PrimaryMinimumDutyCycleAndRampAcceleration;

	public byte PrimaryDutyCycleIncrementAndSamplingPeriod;

	public byte PrimaryFanCutOffTemperatureOffsetAndRampDownRate;

	public byte SecondaryTemperatureSourceAndTemperatureHystersis;

	public byte SecondaryRampAccelerationTemperature;

	public byte SecondaryMinimumTemperature;

	public byte SecondaryMinimumDutyCycleAndRampAcceleration;

	public byte SecondaryDutyCycleIncrementAndSamplingPeriod;

	public byte SecondaryFanCutOffTemperatureOffsetAndRampDownRate;

	public byte FanControlModeAndSpeedRampRateLimit;

	public byte MaximumDutyCycle;
}
