using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace OpenHardwareMonitor.Hardware.ATI;

internal class ADL
{
	private delegate ADLStatus ADL_Main_Control_CreateDelegate(ADL_Main_Memory_AllocDelegate callback, int enumConnectedAdapters);

	private delegate ADLStatus ADL_Adapter_AdapterInfo_GetDelegate(IntPtr info, int size);

	public delegate ADLStatus ADL_Main_Control_DestroyDelegate();

	public delegate ADLStatus ADL_Adapter_NumberOfAdapters_GetDelegate(ref int numAdapters);

	public delegate ADLStatus ADL_Adapter_ID_GetDelegate(int adapterIndex, out int adapterID);

	public delegate ADLStatus ADL_Display_AdapterID_GetDelegate(int adapterIndex, out int adapterID);

	public delegate int ADL_Adapter_Active_GetDelegate(int adapterIndex, out int status);

	public delegate ADLStatus ADL_Overdrive5_CurrentActivity_GetDelegate(int iAdapterIndex, ref ADLPMActivity activity);

	public delegate ADLStatus ADL_Overdrive5_Temperature_GetDelegate(int adapterIndex, int thermalControllerIndex, ref ADLTemperature temperature);

	public delegate ADLStatus ADL_Overdrive5_FanSpeed_GetDelegate(int adapterIndex, int thermalControllerIndex, ref ADLFanSpeedValue fanSpeedValue);

	public delegate ADLStatus ADL_Overdrive5_FanSpeedInfo_GetDelegate(int adapterIndex, int thermalControllerIndex, ref ADLFanSpeedInfo fanSpeedInfo);

	public delegate ADLStatus ADL_Overdrive5_FanSpeedToDefault_SetDelegate(int adapterIndex, int thermalControllerIndex);

	public delegate ADLStatus ADL_Overdrive5_FanSpeed_SetDelegate(int adapterIndex, int thermalControllerIndex, ref ADLFanSpeedValue fanSpeedValue);

	public delegate ADLStatus ADL_Overdrive_CapsDelegate(int adapterIndex, out int supported, out int enabled, out int version);

	private delegate ADLStatus ADL2_Main_Control_CreateDelegate(ADL_Main_Memory_AllocDelegate callback, int enumConnectedAdapters, out IntPtr context);

	public delegate ADLStatus ADL2_Main_Control_DestroyDelegate(IntPtr context);

	public delegate ADLStatus ADL2_OverdriveN_Temperature_GetDelegate(IntPtr context, int adapterIndex, ADLODNTemperatureType temperatureType, out int temperature);

	public delegate ADLStatus ADL2_Overdrive6_CurrentPower_GetDelegate(IntPtr context, int adapterIndex, ADLODNCurrentPowerType powerType, out int currentValue);

	public delegate ADLStatus ADL2_New_QueryPMLogData_GetDelegate(IntPtr context, int adapterIndex, out ADLPMLogDataOutput dataOutput);

	public delegate ADLStatus ADL_Overdrive5_ODParameters_GetDelegate(int adapterIndex, out ADLODParameters parameters);

	public delegate ADLStatus ADL2_OverdriveN_PerformanceStatus_GetDelegate(IntPtr context, int adapterIndex, out ADLODNPerformanceStatus performanceStatus);

	public delegate ADLStatus ADL_Graphics_Versions_GetDelegate(out ADLVersionsInfo versionInfo);

	private delegate IntPtr ADL_Main_Memory_AllocDelegate(int size);

	public const int ADL_MAX_PATH = 256;

	public const int ADL_MAX_ADAPTERS = 40;

	public const int ADL_MAX_DISPLAYS = 40;

	public const int ADL_MAX_DEVICENAME = 32;

	public const int ADL_DRIVER_OK = 0;

	public const int ADL_MAX_GLSYNC_PORTS = 8;

	public const int ADL_MAX_GLSYNC_PORT_LEDS = 8;

	public const int ADL_MAX_NUM_DISPLAYMODES = 1024;

	public const int ADL_PMLOG_MAX_SENSORS = 256;

	public const int ADL_DL_FANCTRL_SPEED_TYPE_PERCENT = 1;

	public const int ADL_DL_FANCTRL_SPEED_TYPE_RPM = 2;

	public const int ADL_DL_FANCTRL_SUPPORTS_PERCENT_READ = 1;

	public const int ADL_DL_FANCTRL_SUPPORTS_PERCENT_WRITE = 2;

	public const int ADL_DL_FANCTRL_SUPPORTS_RPM_READ = 4;

	public const int ADL_DL_FANCTRL_SUPPORTS_RPM_WRITE = 8;

	public const int ADL_DL_FANCTRL_FLAG_USER_DEFINED_SPEED = 1;

	public const int ATI_VENDOR_ID = 4098;

	private static ADL_Main_Control_CreateDelegate _ADL_Main_Control_Create;

	private static ADL_Adapter_AdapterInfo_GetDelegate _ADL_Adapter_AdapterInfo_Get;

	public static ADL_Main_Control_DestroyDelegate ADL_Main_Control_Destroy;

	public static ADL_Adapter_NumberOfAdapters_GetDelegate ADL_Adapter_NumberOfAdapters_Get;

	public static ADL_Adapter_ID_GetDelegate _ADL_Adapter_ID_Get;

	public static ADL_Display_AdapterID_GetDelegate _ADL_Display_AdapterID_Get;

	public static ADL_Adapter_Active_GetDelegate ADL_Adapter_Active_Get;

	public static ADL_Overdrive5_CurrentActivity_GetDelegate ADL_Overdrive5_CurrentActivity_Get;

	public static ADL_Overdrive5_Temperature_GetDelegate ADL_Overdrive5_Temperature_Get;

	public static ADL_Overdrive5_FanSpeed_GetDelegate ADL_Overdrive5_FanSpeed_Get;

	public static ADL_Overdrive5_FanSpeedInfo_GetDelegate ADL_Overdrive5_FanSpeedInfo_Get;

	public static ADL_Overdrive5_FanSpeedToDefault_SetDelegate ADL_Overdrive5_FanSpeedToDefault_Set;

	public static ADL_Overdrive5_FanSpeed_SetDelegate ADL_Overdrive5_FanSpeed_Set;

	public static ADL_Overdrive_CapsDelegate ADL_Overdrive_Caps;

	private static ADL2_Main_Control_CreateDelegate _ADL2_Main_Control_Create;

	public static ADL2_Main_Control_DestroyDelegate ADL2_Main_Control_Destroy;

	public static ADL2_OverdriveN_Temperature_GetDelegate ADL2_OverdriveN_Temperature_Get;

	public static ADL2_Overdrive6_CurrentPower_GetDelegate ADL2_Overdrive6_CurrentPower_Get;

	public static ADL2_New_QueryPMLogData_GetDelegate ADL2_New_QueryPMLogData_Get;

	public static ADL_Overdrive5_ODParameters_GetDelegate ADL_Overdrive5_ODParameters_Get;

	public static ADL2_OverdriveN_PerformanceStatus_GetDelegate ADL2_OverdriveN_PerformanceStatus_Get;

	public static ADL_Graphics_Versions_GetDelegate ADL_Graphics_Versions_Get;

	private static string dllName;

	private static ADL_Main_Memory_AllocDelegate Main_Memory_Alloc;

	private static void GetDelegate<T>(string entryPoint, out T newDelegate) where T : class
	{
		PInvokeDelegateFactory.CreateDelegate<T>(new DllImportAttribute(dllName)
		{
			CallingConvention = CallingConvention.Cdecl,
			PreserveSig = true,
			EntryPoint = entryPoint
		}, out newDelegate);
	}

	private static void CreateDelegates(string name)
	{
		if (OperatingSystem.IsUnix)
		{
			dllName = name + ".so";
		}
		else
		{
			dllName = name + ".dll";
		}
		GetDelegate<ADL_Main_Control_CreateDelegate>("ADL_Main_Control_Create", out _ADL_Main_Control_Create);
		GetDelegate<ADL_Adapter_AdapterInfo_GetDelegate>("ADL_Adapter_AdapterInfo_Get", out _ADL_Adapter_AdapterInfo_Get);
		GetDelegate<ADL_Main_Control_DestroyDelegate>("ADL_Main_Control_Destroy", out ADL_Main_Control_Destroy);
		GetDelegate<ADL_Adapter_NumberOfAdapters_GetDelegate>("ADL_Adapter_NumberOfAdapters_Get", out ADL_Adapter_NumberOfAdapters_Get);
		GetDelegate<ADL_Adapter_ID_GetDelegate>("ADL_Adapter_ID_Get", out _ADL_Adapter_ID_Get);
		GetDelegate<ADL_Display_AdapterID_GetDelegate>("ADL_Display_AdapterID_Get", out _ADL_Display_AdapterID_Get);
		GetDelegate<ADL_Adapter_Active_GetDelegate>("ADL_Adapter_Active_Get", out ADL_Adapter_Active_Get);
		GetDelegate<ADL_Overdrive5_CurrentActivity_GetDelegate>("ADL_Overdrive5_CurrentActivity_Get", out ADL_Overdrive5_CurrentActivity_Get);
		GetDelegate<ADL_Overdrive5_Temperature_GetDelegate>("ADL_Overdrive5_Temperature_Get", out ADL_Overdrive5_Temperature_Get);
		GetDelegate<ADL_Overdrive5_FanSpeed_GetDelegate>("ADL_Overdrive5_FanSpeed_Get", out ADL_Overdrive5_FanSpeed_Get);
		GetDelegate<ADL_Overdrive5_FanSpeedInfo_GetDelegate>("ADL_Overdrive5_FanSpeedInfo_Get", out ADL_Overdrive5_FanSpeedInfo_Get);
		GetDelegate<ADL_Overdrive5_FanSpeedToDefault_SetDelegate>("ADL_Overdrive5_FanSpeedToDefault_Set", out ADL_Overdrive5_FanSpeedToDefault_Set);
		GetDelegate<ADL_Overdrive5_FanSpeed_SetDelegate>("ADL_Overdrive5_FanSpeed_Set", out ADL_Overdrive5_FanSpeed_Set);
		GetDelegate<ADL_Overdrive_CapsDelegate>("ADL_Overdrive_Caps", out ADL_Overdrive_Caps);
		GetDelegate<ADL2_Main_Control_CreateDelegate>("ADL2_Main_Control_Create", out _ADL2_Main_Control_Create);
		GetDelegate<ADL2_Main_Control_DestroyDelegate>("ADL2_Main_Control_Destroy", out ADL2_Main_Control_Destroy);
		GetDelegate<ADL2_OverdriveN_Temperature_GetDelegate>("ADL2_OverdriveN_Temperature_Get", out ADL2_OverdriveN_Temperature_Get);
		GetDelegate<ADL2_Overdrive6_CurrentPower_GetDelegate>("ADL2_Overdrive6_CurrentPower_Get", out ADL2_Overdrive6_CurrentPower_Get);
		GetDelegate<ADL2_New_QueryPMLogData_GetDelegate>("ADL2_New_QueryPMLogData_Get", out ADL2_New_QueryPMLogData_Get);
		GetDelegate<ADL_Overdrive5_ODParameters_GetDelegate>("ADL_Overdrive5_ODParameters_Get", out ADL_Overdrive5_ODParameters_Get);
		GetDelegate<ADL2_OverdriveN_PerformanceStatus_GetDelegate>("ADL2_OverdriveN_PerformanceStatus_Get", out ADL2_OverdriveN_PerformanceStatus_Get);
		GetDelegate<ADL_Graphics_Versions_GetDelegate>("ADL_Graphics_Versions_Get", out ADL_Graphics_Versions_Get);
	}

	static ADL()
	{
		Main_Memory_Alloc = (int size) => Marshal.AllocHGlobal(size);
		CreateDelegates("atiadlxx");
	}

	private ADL()
	{
	}

	public static ADLStatus ADL_Main_Control_Create(int enumConnectedAdapters)
	{
		try
		{
			try
			{
				return _ADL_Main_Control_Create(Main_Memory_Alloc, enumConnectedAdapters);
			}
			catch
			{
				CreateDelegates("atiadlxy");
				return _ADL_Main_Control_Create(Main_Memory_Alloc, enumConnectedAdapters);
			}
		}
		catch
		{
			return ADLStatus.ERR;
		}
	}

	public static ADLStatus ADL2_Main_Control_Create(int enumConnectedAdapters, out IntPtr context)
	{
		try
		{
			ADLStatus aDLStatus = _ADL2_Main_Control_Create(Main_Memory_Alloc, enumConnectedAdapters, out context);
			if (aDLStatus != ADLStatus.OK)
			{
				context = IntPtr.Zero;
			}
			return aDLStatus;
		}
		catch
		{
			context = IntPtr.Zero;
			return ADLStatus.ERR;
		}
	}

	public static ADLStatus ADL_Adapter_AdapterInfo_Get(ADLAdapterInfo[] info)
	{
		int num = Marshal.SizeOf(typeof(ADLAdapterInfo));
		int num2 = info.Length * num;
		IntPtr intPtr = Marshal.AllocHGlobal(num2);
		ADLStatus result = _ADL_Adapter_AdapterInfo_Get(intPtr, num2);
		for (int i = 0; i < info.Length; i++)
		{
			info[i] = (ADLAdapterInfo)Marshal.PtrToStructure((IntPtr)((long)intPtr + i * num), typeof(ADLAdapterInfo));
		}
		Marshal.FreeHGlobal(intPtr);
		for (int j = 0; j < info.Length; j++)
		{
			Match match = Regex.Match(info[j].UDID, "PCI_VEN_([A-Fa-f0-9]{1,4})&.*");
			if (match.Success && match.Groups.Count == 2)
			{
				info[j].VendorID = Convert.ToInt32(match.Groups[1].Value, 16);
				continue;
			}
			match = Regex.Match(info[j].UDID, "[0-9]+:[0-9]+:([0-9]+):[0-9]+:[0-9]+");
			if (match.Success && match.Groups.Count == 2)
			{
				info[j].VendorID = Convert.ToInt32(match.Groups[1].Value, 10);
			}
		}
		return result;
	}

	public static ADLStatus ADL_Adapter_ID_Get(int adapterIndex, out int adapterID)
	{
		try
		{
			return _ADL_Adapter_ID_Get(adapterIndex, out adapterID);
		}
		catch (EntryPointNotFoundException)
		{
			try
			{
				return _ADL_Display_AdapterID_Get(adapterIndex, out adapterID);
			}
			catch (EntryPointNotFoundException)
			{
				adapterID = 1;
				return ADLStatus.OK;
			}
		}
	}

	private static void Main_Memory_Free(IntPtr buffer)
	{
		if (IntPtr.Zero != buffer)
		{
			Marshal.FreeHGlobal(buffer);
		}
	}
}
