namespace OpenHardwareMonitor.Hardware.ATI;

internal struct ADLODNPerformanceStatus
{
	public int CoreClock;

	public int MemoryClock;

	public int DCEFClock;

	public int GFXClock;

	public int UVDClock;

	public int VCEClock;

	public int GPUActivityPercent;

	public int CurrentCorePerformanceLevel;

	public int CurrentMemoryPerformanceLevel;

	public int CurrentDCEFPerformanceLevel;

	public int CurrentGFXPerformanceLevel;

	public int UVDPerformanceLevel;

	public int VCEPerformanceLevel;

	public int CurrentBusSpeed;

	public int CurrentBusLanes;

	public int MaximumBusLanes;

	public int VDDC;

	public int VDDCI;
}
