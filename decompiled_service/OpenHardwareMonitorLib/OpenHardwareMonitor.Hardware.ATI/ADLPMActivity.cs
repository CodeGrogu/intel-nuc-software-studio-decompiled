namespace OpenHardwareMonitor.Hardware.ATI;

internal struct ADLPMActivity
{
	public int Size;

	public int EngineClock;

	public int MemoryClock;

	public int Vddc;

	public int ActivityPercent;

	public int CurrentPerformanceLevel;

	public int CurrentBusSpeed;

	public int CurrentBusLanes;

	public int MaximumBusLanes;

	public int Reserved;
}
