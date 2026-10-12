namespace SystemMonitorLibrary.Hardware.Battery;

internal enum BatteryStatus : ushort
{
	Other = 1,
	Unknown,
	FullyCharged,
	Low,
	Critical,
	Charging,
	ChargingHigh,
	ChargingLow,
	ChargingCritical,
	Undefined,
	PartiallyCharged
}
