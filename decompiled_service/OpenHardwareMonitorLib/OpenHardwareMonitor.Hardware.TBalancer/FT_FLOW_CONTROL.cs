namespace OpenHardwareMonitor.Hardware.TBalancer;

internal enum FT_FLOW_CONTROL : ushort
{
	FT_FLOW_DTR_DSR = 512,
	FT_FLOW_NONE = 0,
	FT_FLOW_RTS_CTS = 256,
	FT_FLOW_XON_XOFF = 1024
}
