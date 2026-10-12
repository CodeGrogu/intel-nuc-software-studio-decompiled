using System;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.Nvidia;

internal class NVML
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate NvmlReturn nvmlInitDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate NvmlReturn nvmlInit_v2Delegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvmlReturn nvmlShutdownDelegate();

	public delegate NvmlReturn nvmlDeviceGetHandleByPciBusIdDelegate(string pciBusId, out NvmlDevice device);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvmlReturn nvmlDeviceGetPowerUsageDelegate(NvmlDevice device, out int power);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvmlReturn nvmlDeviceGetPcieThroughputDelegate(NvmlDevice devicee, NvmlPcieUtilCounter counter, out uint value);

	internal struct NvmlDevice
	{
		private readonly IntPtr ptr;
	}

	internal enum NvmlPcieUtilCounter
	{
		TxBytes,
		RxBytes
	}

	internal enum NvmlReturn
	{
		Success = 0,
		ErrorUninitialized = 1,
		ErrorInvalidArgument = 2,
		ErrorNotSupported = 3,
		ErrorNoPermission = 4,
		ErrorAlreadyInitialized = 5,
		ErrorNotFound = 6,
		ErrorInsufficientSize = 7,
		ErrorInsufficientPower = 8,
		ErrorDriverNotLoaded = 9,
		ErrorTimeout = 10,
		ErrorIrqIssue = 11,
		ErrorLibraryNotFound = 12,
		ErrorFunctionNotFound = 13,
		ErrorCorruptedInforom = 14,
		ErrorGpuIsLost = 15,
		ErrorResetRequired = 16,
		ErrorOperatingSystem = 17,
		ErrorLibRmVersionMismatch = 18,
		ErrorInUse = 19,
		ErrorMemory = 20,
		ErrorNoData = 21,
		ErrorvGpuEccNotSupported = 22,
		ErrorUnknown = 999
	}

	private static readonly nvmlInitDelegate nvmlInit;

	private static readonly nvmlInit_v2Delegate nvmlInit_v2;

	public static readonly nvmlShutdownDelegate NvmlShutdown;

	public static readonly nvmlDeviceGetHandleByPciBusIdDelegate NvmlDeviceGetHandleByPciBusId;

	public static readonly nvmlDeviceGetPowerUsageDelegate NvmlDeviceGetPowerUsage;

	public static readonly nvmlDeviceGetPcieThroughputDelegate NvmlDeviceGetPcieThroughput;

	private static bool initialized;

	public static bool IsInitialized => initialized;

	public static NvmlReturn NvmlInit()
	{
		try
		{
			NvmlReturn nvmlReturn = nvmlInit_v2();
			initialized = nvmlReturn == NvmlReturn.Success;
			return nvmlReturn;
		}
		catch
		{
			try
			{
				NvmlReturn nvmlReturn2 = nvmlInit();
				initialized = nvmlReturn2 == NvmlReturn.Success;
				return nvmlReturn2;
			}
			catch
			{
				return NvmlReturn.ErrorLibraryNotFound;
			}
		}
	}

	private static string GetDllName()
	{
		if (OperatingSystem.IsUnix)
		{
			return "libnvidia-ml.so";
		}
		return "nvml.dll";
	}

	private static T CreateDelegate<T>(string entryPoint) where T : Delegate
	{
		PInvokeDelegateFactory.CreateDelegate<T>(new DllImportAttribute(GetDllName())
		{
			CallingConvention = CallingConvention.Cdecl,
			PreserveSig = true,
			EntryPoint = entryPoint
		}, out var newDelegate);
		return newDelegate;
	}

	static NVML()
	{
		nvmlInit = CreateDelegate<nvmlInitDelegate>("nvmlInit");
		nvmlInit_v2 = CreateDelegate<nvmlInit_v2Delegate>("nvmlInit_v2");
		NvmlShutdown = CreateDelegate<nvmlShutdownDelegate>("nvmlShutdown");
		NvmlDeviceGetHandleByPciBusId = CreateDelegate<nvmlDeviceGetHandleByPciBusIdDelegate>("nvmlDeviceGetHandleByPciBusId_v2");
		NvmlDeviceGetPowerUsage = CreateDelegate<nvmlDeviceGetPowerUsageDelegate>("nvmlDeviceGetPowerUsage");
		NvmlDeviceGetPcieThroughput = CreateDelegate<nvmlDeviceGetPcieThroughputDelegate>("nvmlDeviceGetPcieThroughput");
	}
}
