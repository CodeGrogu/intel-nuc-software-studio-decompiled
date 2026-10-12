using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenHardwareMonitor.Hardware.Nvidia;

internal class NVAPI
{
	private delegate IntPtr nvapi_QueryInterfaceDelegate(uint id);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate NvStatus NvAPI_InitializeDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate NvStatus NvAPI_GPU_GetFullNameDelegate(NvPhysicalGpuHandle gpuHandle, StringBuilder name);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_GetThermalSettingsDelegate(NvPhysicalGpuHandle gpuHandle, int sensorIndex, ref NvGPUThermalSettings gpuThermalSettings);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_EnumNvidiaDisplayHandleDelegate(int thisEnum, ref NvDisplayHandle displayHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GetPhysicalGPUsFromDisplayDelegate(NvDisplayHandle displayHandle, [Out] NvPhysicalGpuHandle[] gpuHandles, out uint gpuCount);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_EnumPhysicalGPUsDelegate([Out] NvPhysicalGpuHandle[] gpuHandles, out int gpuCount);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_GetTachReadingDelegate(NvPhysicalGpuHandle gpuHandle, out int value);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_GetAllClocksDelegate(NvPhysicalGpuHandle gpuHandle, ref NvClocks clocks);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_GetDynamicPstatesInfoExDelegate(NvPhysicalGpuHandle gpuHandle, ref NvDynamicPstatesInfoEx dynamicPstatesInfoEx);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_GetDynamicPstatesInfoDelegate(NvPhysicalGpuHandle gpuHandle, ref NvDynamicPstatesInfo dynamicPstatesInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_GetCoolerSettingsDelegate(NvPhysicalGpuHandle gpuHandle, int coolerIndex, ref NvGPUCoolerSettings gpuCoolerSettings);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_SetCoolerLevelsDelegate(NvPhysicalGpuHandle gpuHandle, int coolerIndex, ref NvGPUCoolerLevels gpuCoolerLevels);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GetDisplayDriverMemoryInfoDelegate(NvDisplayHandle displayHandle, ref NvDisplayDriverMemoryInfo memoryInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GetDisplayDriverVersionDelegate(NvDisplayHandle displayHandle, [In][Out] ref NvDisplayDriverVersion displayDriverVersion);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GetInterfaceVersionStringDelegate(StringBuilder version);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_GetPCIIdentifiersDelegate(NvPhysicalGpuHandle gpuHandle, out uint deviceId, out uint subSystemId, out uint revisionId, out uint extDeviceId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_GetBusIdDelegate(NvPhysicalGpuHandle gpuHandle, out uint busId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NvStatus NvAPI_GPU_ClientFanCoolersGetStatusDelegate(NvPhysicalGpuHandle gpuHandle, ref NvFanCoolersStatus fanCoolersStatus);

	public const int MAX_PHYSICAL_GPUS = 64;

	public const int SHORT_STRING_MAX = 64;

	public const int MAX_THERMAL_SENSORS_PER_GPU = 3;

	public const int MAX_CLOCKS_PER_GPU = 288;

	public const int NVAPI_MAX_GPU_UTILIZATIONS = 8;

	public const int MAX_COOLER_PER_GPU = 20;

	public const int MAX_MEMORY_VALUES_PER_GPU = 5;

	public const int MAX_FAN_COOLERS_STATUS_ITEMS = 32;

	public static readonly uint GPU_THERMAL_SETTINGS_VER;

	public static readonly uint GPU_CLOCKS_VER;

	public static readonly uint GPU_DYNAMIC_PSTATES_INFO_EX_VER;

	public static readonly uint GPU_DYNAMIC_PSTATES_INFO_VER;

	public static readonly uint GPU_COOLER_SETTINGS_VER;

	public static readonly uint DISPLAY_DRIVER_MEMORY_INFO_VER;

	public static readonly uint DISPLAY_DRIVER_VERSION_VER;

	public static readonly uint GPU_COOLER_LEVELS_VER;

	public static readonly uint GPU_FAN_COOLERS_STATUS_VER;

	private static readonly bool available;

	private static readonly nvapi_QueryInterfaceDelegate nvapi_QueryInterface;

	private static readonly NvAPI_InitializeDelegate NvAPI_Initialize;

	private static readonly NvAPI_GPU_GetFullNameDelegate _NvAPI_GPU_GetFullName;

	private static readonly NvAPI_GetInterfaceVersionStringDelegate _NvAPI_GetInterfaceVersionString;

	public static readonly NvAPI_GPU_GetThermalSettingsDelegate NvAPI_GPU_GetThermalSettings;

	public static readonly NvAPI_EnumNvidiaDisplayHandleDelegate NvAPI_EnumNvidiaDisplayHandle;

	public static readonly NvAPI_GetPhysicalGPUsFromDisplayDelegate NvAPI_GetPhysicalGPUsFromDisplay;

	public static readonly NvAPI_EnumPhysicalGPUsDelegate NvAPI_EnumPhysicalGPUs;

	public static readonly NvAPI_GPU_GetTachReadingDelegate NvAPI_GPU_GetTachReading;

	public static readonly NvAPI_GPU_GetAllClocksDelegate NvAPI_GPU_GetAllClocks;

	public static readonly NvAPI_GPU_GetDynamicPstatesInfoExDelegate NvAPI_GPU_GetDynamicPstatesInfoEx;

	public static readonly NvAPI_GPU_GetDynamicPstatesInfoDelegate NvAPI_GPU_GetDynamicPstatesInfo;

	public static readonly NvAPI_GPU_GetCoolerSettingsDelegate NvAPI_GPU_GetCoolerSettings;

	public static readonly NvAPI_GPU_SetCoolerLevelsDelegate NvAPI_GPU_SetCoolerLevels;

	public static readonly NvAPI_GetDisplayDriverMemoryInfoDelegate NvAPI_GetDisplayDriverMemoryInfo;

	public static readonly NvAPI_GetDisplayDriverVersionDelegate NvAPI_GetDisplayDriverVersion;

	public static readonly NvAPI_GPU_GetPCIIdentifiersDelegate NvAPI_GPU_GetPCIIdentifiers;

	public static readonly NvAPI_GPU_GetBusIdDelegate NvAPI_GPU_GetBusId;

	public static readonly NvAPI_GPU_ClientFanCoolersGetStatusDelegate NvAPI_GPU_ClientFanCoolersGetStatus;

	public static bool IsAvailable => available;

	private NVAPI()
	{
	}

	public static NvStatus NvAPI_GPU_GetFullName(NvPhysicalGpuHandle gpuHandle, out string name)
	{
		StringBuilder stringBuilder = new StringBuilder(64);
		NvStatus result = ((_NvAPI_GPU_GetFullName == null) ? NvStatus.FUNCTION_NOT_FOUND : _NvAPI_GPU_GetFullName(gpuHandle, stringBuilder));
		name = stringBuilder.ToString();
		return result;
	}

	public static NvStatus NvAPI_GetInterfaceVersionString(out string version)
	{
		StringBuilder stringBuilder = new StringBuilder(64);
		NvStatus result = ((_NvAPI_GetInterfaceVersionString == null) ? NvStatus.FUNCTION_NOT_FOUND : _NvAPI_GetInterfaceVersionString(stringBuilder));
		version = stringBuilder.ToString();
		return result;
	}

	private static string GetDllName()
	{
		if (IntPtr.Size == 4)
		{
			return "nvapi.dll";
		}
		return "nvapi64.dll";
	}

	private static void GetDelegate<T>(uint id, out T newDelegate) where T : class
	{
		IntPtr intPtr = nvapi_QueryInterface(id);
		if (intPtr != IntPtr.Zero)
		{
			newDelegate = Marshal.GetDelegateForFunctionPointer(intPtr, typeof(T)) as T;
		}
		else
		{
			newDelegate = null;
		}
	}

	static NVAPI()
	{
		GPU_THERMAL_SETTINGS_VER = (uint)(Marshal.SizeOf(typeof(NvGPUThermalSettings)) | 0x10000);
		GPU_CLOCKS_VER = (uint)(Marshal.SizeOf(typeof(NvClocks)) | 0x20000);
		GPU_DYNAMIC_PSTATES_INFO_EX_VER = (uint)(Marshal.SizeOf(typeof(NvDynamicPstatesInfoEx)) | 0x10000);
		GPU_DYNAMIC_PSTATES_INFO_VER = (uint)(Marshal.SizeOf(typeof(NvDynamicPstatesInfo)) | 0x10000);
		GPU_COOLER_SETTINGS_VER = (uint)(Marshal.SizeOf(typeof(NvGPUCoolerSettings)) | 0x20000);
		DISPLAY_DRIVER_MEMORY_INFO_VER = (uint)(Marshal.SizeOf(typeof(NvDisplayDriverMemoryInfo)) | 0x20000);
		DISPLAY_DRIVER_VERSION_VER = (uint)(Marshal.SizeOf(typeof(NvDisplayDriverVersion)) | 0x10000);
		GPU_COOLER_LEVELS_VER = (uint)(Marshal.SizeOf(typeof(NvGPUCoolerLevels)) | 0x10000);
		GPU_FAN_COOLERS_STATUS_VER = (uint)(Marshal.SizeOf(typeof(NvFanCoolersStatus)) | 0x10000);
		PInvokeDelegateFactory.CreateDelegate<nvapi_QueryInterfaceDelegate>(new DllImportAttribute(GetDllName())
		{
			CallingConvention = CallingConvention.Cdecl,
			PreserveSig = true,
			EntryPoint = "nvapi_QueryInterface"
		}, out nvapi_QueryInterface);
		try
		{
			GetDelegate<NvAPI_InitializeDelegate>(22079528u, out NvAPI_Initialize);
		}
		catch (DllNotFoundException)
		{
			return;
		}
		catch (EntryPointNotFoundException)
		{
			return;
		}
		catch (ArgumentNullException)
		{
			return;
		}
		if (NvAPI_Initialize() == NvStatus.OK)
		{
			GetDelegate<NvAPI_GPU_GetThermalSettingsDelegate>(3814984278u, out NvAPI_GPU_GetThermalSettings);
			GetDelegate<NvAPI_GPU_GetFullNameDelegate>(3471740575u, out _NvAPI_GPU_GetFullName);
			GetDelegate<NvAPI_EnumNvidiaDisplayHandleDelegate>(2596131853u, out NvAPI_EnumNvidiaDisplayHandle);
			GetDelegate<NvAPI_GetPhysicalGPUsFromDisplayDelegate>(888116486u, out NvAPI_GetPhysicalGPUsFromDisplay);
			GetDelegate<NvAPI_EnumPhysicalGPUsDelegate>(3853292063u, out NvAPI_EnumPhysicalGPUs);
			GetDelegate<NvAPI_GPU_GetTachReadingDelegate>(1600160533u, out NvAPI_GPU_GetTachReading);
			GetDelegate<NvAPI_GPU_GetAllClocksDelegate>(467050313u, out NvAPI_GPU_GetAllClocks);
			GetDelegate<NvAPI_GPU_GetDynamicPstatesInfoExDelegate>(1625215725u, out NvAPI_GPU_GetDynamicPstatesInfoEx);
			GetDelegate<NvAPI_GPU_GetDynamicPstatesInfoDelegate>(412753887u, out NvAPI_GPU_GetDynamicPstatesInfo);
			GetDelegate<NvAPI_GPU_GetCoolerSettingsDelegate>(3658748736u, out NvAPI_GPU_GetCoolerSettings);
			GetDelegate<NvAPI_GPU_SetCoolerLevelsDelegate>(2300551342u, out NvAPI_GPU_SetCoolerLevels);
			GetDelegate<NvAPI_GetDisplayDriverMemoryInfoDelegate>(2001381762u, out NvAPI_GetDisplayDriverMemoryInfo);
			GetDelegate<NvAPI_GetDisplayDriverVersionDelegate>(4182877393u, out NvAPI_GetDisplayDriverVersion);
			GetDelegate<NvAPI_GetInterfaceVersionStringDelegate>(17121189u, out _NvAPI_GetInterfaceVersionString);
			GetDelegate<NvAPI_GPU_GetPCIIdentifiersDelegate>(769635950u, out NvAPI_GPU_GetPCIIdentifiers);
			GetDelegate<NvAPI_GPU_GetBusIdDelegate>(467712229u, out NvAPI_GPU_GetBusId);
			GetDelegate<NvAPI_GPU_ClientFanCoolersGetStatusDelegate>(900650472u, out NvAPI_GPU_ClientFanCoolersGetStatus);
			available = true;
		}
	}
}
