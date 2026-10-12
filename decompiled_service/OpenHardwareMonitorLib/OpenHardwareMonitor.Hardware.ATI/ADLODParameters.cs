namespace OpenHardwareMonitor.Hardware.ATI;

internal struct ADLODParameters
{
	public int Size;

	public int NumberOfPerformanceLevels;

	public int ActivityReportingSupported;

	public int DiscretePerformanceLevels;

	public int Reserved;

	public ADLODParameterRange EngineClock;

	public ADLODParameterRange MemoryClock;

	public ADLODParameterRange Vddc;
}
