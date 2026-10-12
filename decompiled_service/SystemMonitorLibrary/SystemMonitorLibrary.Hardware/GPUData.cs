using System.Runtime.InteropServices;

namespace SystemMonitorLibrary.Hardware;

public class GPUData
{
	[DllImport("IGCLLibrary.dll")]
	public static extern long TotalMemoryUsed();

	[DllImport("IGCLLibrary.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	public static extern string GetiGPUName();

	[DllImport("IGCLLibrary.dll")]
	public static extern double CurrentFrequency();

	[DllImport("IGCLLibrary.dll")]
	public static extern double MemoryFrequency();

	[DllImport("IGCLLibrary.dll")]
	public static extern double GPUUsage();

	[DllImport("IGCLLibrary.dll")]
	public static extern double IGPUUsage();
}
