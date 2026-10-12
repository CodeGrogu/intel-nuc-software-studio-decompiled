using System.Runtime.InteropServices;

namespace PerformanceTuningLibrary;

public struct FanSetting
{
	[MarshalAs(UnmanagedType.U1)]
	public PrimaryInputSourceConfiguration primaryInputSourceConfiguration;

	public byte PrimaryMaximumTemperature;

	public byte PrimaryMinimumTemperature;

	public byte PrimaryMinimumDuty;

	public byte PrimaryDutyIncrement;

	public byte PrimaryFanOffTemperature;

	[MarshalAs(UnmanagedType.U1)]
	public SecondaryInputSourceConfiguration secondaryInputSourceConfiguration;

	public byte SecondaryMaximumTemperature;

	public byte SecondaryMinimumTemperature;

	public byte SecondaryMinimumDuty;

	public byte SecondaryDutyIncrement;

	public byte SecondaryFanOffTemperature;

	public byte Configuration1;

	public byte Configuration2;
}
