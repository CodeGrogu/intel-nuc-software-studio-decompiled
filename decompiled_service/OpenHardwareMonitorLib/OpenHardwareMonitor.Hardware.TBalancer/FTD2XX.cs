using System;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.TBalancer;

internal class FTD2XX
{
	public delegate FT_STATUS FT_CreateDeviceInfoListDelegate(out uint numDevices);

	public delegate FT_STATUS FT_GetDeviceInfoListDelegate([Out] FT_DEVICE_INFO_NODE[] deviceInfoNodes, ref uint length);

	public delegate FT_STATUS FT_OpenDelegate(int device, out FT_HANDLE handle);

	public delegate FT_STATUS FT_CloseDelegate(FT_HANDLE handle);

	public delegate FT_STATUS FT_SetBaudRateDelegate(FT_HANDLE handle, uint baudRate);

	public delegate FT_STATUS FT_SetDataCharacteristicsDelegate(FT_HANDLE handle, byte wordLength, byte stopBits, byte parity);

	public delegate FT_STATUS FT_SetFlowControlDelegate(FT_HANDLE handle, FT_FLOW_CONTROL flowControl, byte xon, byte xoff);

	public delegate FT_STATUS FT_SetTimeoutsDelegate(FT_HANDLE handle, uint readTimeout, uint writeTimeout);

	public delegate FT_STATUS FT_WriteDelegate(FT_HANDLE handle, byte[] buffer, uint bytesToWrite, out uint bytesWritten);

	public delegate FT_STATUS FT_PurgeDelegate(FT_HANDLE handle, FT_PURGE mask);

	public delegate FT_STATUS FT_GetStatusDelegate(FT_HANDLE handle, out uint amountInRxQueue, out uint amountInTxQueue, out uint eventStatus);

	public delegate FT_STATUS FT_ReadDelegate(FT_HANDLE handle, [Out] byte[] buffer, uint bytesToRead, out uint bytesReturned);

	public delegate FT_STATUS FT_ReadByteDelegate(FT_HANDLE handle, out byte buffer, uint bytesToRead, out uint bytesReturned);

	public static readonly FT_CreateDeviceInfoListDelegate FT_CreateDeviceInfoList = CreateDelegate<FT_CreateDeviceInfoListDelegate>("FT_CreateDeviceInfoList");

	public static readonly FT_GetDeviceInfoListDelegate FT_GetDeviceInfoList = CreateDelegate<FT_GetDeviceInfoListDelegate>("FT_GetDeviceInfoList");

	public static readonly FT_OpenDelegate FT_Open = CreateDelegate<FT_OpenDelegate>("FT_Open");

	public static readonly FT_CloseDelegate FT_Close = CreateDelegate<FT_CloseDelegate>("FT_Close");

	public static readonly FT_SetBaudRateDelegate FT_SetBaudRate = CreateDelegate<FT_SetBaudRateDelegate>("FT_SetBaudRate");

	public static readonly FT_SetDataCharacteristicsDelegate FT_SetDataCharacteristics = CreateDelegate<FT_SetDataCharacteristicsDelegate>("FT_SetDataCharacteristics");

	public static readonly FT_SetFlowControlDelegate FT_SetFlowControl = CreateDelegate<FT_SetFlowControlDelegate>("FT_SetFlowControl");

	public static readonly FT_SetTimeoutsDelegate FT_SetTimeouts = CreateDelegate<FT_SetTimeoutsDelegate>("FT_SetTimeouts");

	public static readonly FT_WriteDelegate FT_Write = CreateDelegate<FT_WriteDelegate>("FT_Write");

	public static readonly FT_PurgeDelegate FT_Purge = CreateDelegate<FT_PurgeDelegate>("FT_Purge");

	public static readonly FT_GetStatusDelegate FT_GetStatus = CreateDelegate<FT_GetStatusDelegate>("FT_GetStatus");

	public static readonly FT_ReadDelegate FT_Read = CreateDelegate<FT_ReadDelegate>("FT_Read");

	public static readonly FT_ReadByteDelegate FT_ReadByte = CreateDelegate<FT_ReadByteDelegate>("FT_Read");

	private FTD2XX()
	{
	}

	public static FT_STATUS Write(FT_HANDLE handle, byte[] buffer)
	{
		FT_STATUS result = FT_Write(handle, buffer, (uint)buffer.Length, out var bytesWritten);
		if (bytesWritten != buffer.Length)
		{
			return FT_STATUS.FT_FAILED_TO_WRITE_DEVICE;
		}
		return result;
	}

	public static int BytesToRead(FT_HANDLE handle)
	{
		if (FT_GetStatus(handle, out var amountInRxQueue, out var _, out var _) == FT_STATUS.FT_OK)
		{
			return (int)amountInRxQueue;
		}
		return 0;
	}

	public static byte ReadByte(FT_HANDLE handle)
	{
		if (FT_ReadByte(handle, out var buffer, 1u, out var bytesReturned) != FT_STATUS.FT_OK || bytesReturned != 1)
		{
			throw new InvalidOperationException();
		}
		return buffer;
	}

	public static void Read(FT_HANDLE handle, byte[] buffer)
	{
		if (FT_Read(handle, buffer, (uint)buffer.Length, out var bytesReturned) != FT_STATUS.FT_OK || bytesReturned != buffer.Length)
		{
			throw new InvalidOperationException();
		}
	}

	private static string GetDllName()
	{
		if (OperatingSystem.IsUnix)
		{
			return "libftd2xx.so";
		}
		return "ftd2xx.dll";
	}

	private static T CreateDelegate<T>(string entryPoint) where T : class
	{
		PInvokeDelegateFactory.CreateDelegate<T>(new DllImportAttribute(GetDllName())
		{
			CallingConvention = CallingConvention.StdCall,
			PreserveSig = true,
			EntryPoint = entryPoint
		}, out var newDelegate);
		return newDelegate;
	}
}
