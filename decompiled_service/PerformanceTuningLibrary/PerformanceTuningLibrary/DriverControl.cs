using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace PerformanceTuningLibrary;

internal class DriverControl
{
	private static class NativeMethods
	{
		[DllImport("advapi32.dll")]
		public static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, uint dwDesiredAccess);

		[DllImport("advapi32.dll")]
		public static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName, uint dwDesiredAccess, uint dwServiceType, uint dwStartType, uint dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, string lpdwTagId, string lpDependencies, string lpSerivceStartName, string lpPassword);

		[DllImport("advapi32.dll")]
		public static extern bool CloseServiceHandle(IntPtr hSCObject);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool StartService(IntPtr hService, uint dwNumServiceArgs, string[] lpServiceArgVectors);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool ControlService(IntPtr hService, uint dwControl, ref ServiceStatus lpServiceStatus);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool DeleteService(IntPtr hService);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll")]
		public static extern bool DeviceIoControl(SafeFileHandle device, DwIoControlCodeType ioControlCode, [In][MarshalAs(UnmanagedType.AsAny)] object inBuffer, uint inBufferSize, [Out][MarshalAs(UnmanagedType.AsAny)] object outBuffer, uint nOutBufferSize, out uint bytesReturned, IntPtr overlapped);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, uint flags);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, ref SpDeviceInterfaceData deviceInterfaceData);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SpDeviceInterfaceData deviceInterfaceData, ref SpDeviceInterfaceDetailData deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, ref SpDevInfoData deviceInfoData);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SpDeviceInterfaceData deviceInterfaceData, IntPtr deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, IntPtr deviceInfoData);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);
	}

	private enum ServiceAccessForSCMOpen : uint
	{
		ALL_ACCESS = 983103u
	}

	private enum ServiceAccessRights : uint
	{
		SERVICE_ALL_ACCESS = 983551u
	}

	private enum ServiceType : uint
	{
		SERVICE_KERNEL_DRIVER = 1u
	}

	private enum StartType : uint
	{
		SERVICE_BOOT_START,
		SERVICE_SYSTEM_START,
		SERVICE_AUTO_START,
		SERVICE_DEMAND_START,
		SERVICE_DISABLED
	}

	private enum ErrorControl : uint
	{
		SERVICE_ERROR_IGNORE,
		SERVICE_ERROR_NORMAL,
		SERVICE_ERROR_SEVERE,
		SERVICE_ERROR_CRITICAL
	}

	private enum ServiceControl : uint
	{
		SERVICE_CONTROL_STOP = 1u,
		SERVICE_CONTROL_PAUSE,
		SERVICE_CONTROL_CONTINUE,
		SERVICE_CONTROL_INTERROGATE,
		SERVICE_CONTROL_SHUTDOWN,
		SERVICE_CONTROL_PARAMCHANGE,
		SERVICE_CONTROL_NETBINDADD,
		SERVICE_CONTROL_NETBINDREMOVE,
		SERVICE_CONTROL_NETBINDENABLE,
		SERVICE_CONTROL_NETBINDDISABLE
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct ServiceStatus
	{
		public uint dwServiceType;

		public uint dwCurrentState;

		public uint dwControlsAccepted;

		public uint dwWin32ExitCode;

		public uint dwServiceSpecificExitCode;

		public uint dwCheckPoint;

		public uint dwWaitHint;
	}

	[Flags]
	private enum GeneralAccessRight : uint
	{
		None = 0u,
		GENERIC_READ = 0x80000000u,
		GENERIC_WRITE = 0x40000000u
	}

	[Flags]
	private enum FileShare : uint
	{
		None = 0u,
		FileShareRead = 1u,
		FileShareWrite = 2u
	}

	private struct DwIoControlCodeType
	{
		public uint IoControlCode;
	}

	private enum CreationDisposition : uint
	{
		CREATE_NEW = 1u,
		CREATE_ALWAYS,
		OPEN_EXISTING,
		OPEN_ALWAYS,
		TRUNCATE_EXISTING
	}

	private enum FileAttributes : uint
	{
		FILE_ATTRIBUTE_NORMAL = 0x80u
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct EcWriteByteType
	{
		public uint portNum;

		public byte ByteData;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MsrWriteLongType
	{
		public uint registerAddr;

		public ulong LongData;
	}

	public const uint FAN_PROFILE_EC = 94u;

	public const uint FAN_PROFILE_WMI_LENGTH = 90u;

	public const uint FAN_PROFILE_WMI_LENGTH_23 = 21u;

	public const uint FAN_SPEED_LENGTH = 18u;

	public const uint TEMPRATURE_PROFILE_EC = 15u;

	private const int DIGCF_DEFAULT = 1;

	private const int DIGCF_PRESENT = 2;

	private const int DIGCF_ALLCLASSES = 4;

	private const int DIGCF_PROFILE = 8;

	private const int DIGCF_DEVICEINTERFACE = 16;

	private const uint MSRReadPowerUnit = 2621464588u;

	private const uint MSRReadPowerLimit = 2621464592u;

	private const uint MSRWritePowerLimit = 2621480980u;

	private const uint WriteFanProfileNuc21Code = 2621480984u;

	private const uint WriteFanProfileNuc22Code = 2621480988u;

	private const uint WriteFanProfileNuc23Code = 2621481036u;

	private const uint ReadFanProfileCode = 2621464608u;

	private const uint ReadFanSpeedCode = 2621464612u;

	private const uint SetFanSpeedCode = 2621481024u;

	private const uint ReadDriverStatus = 2621464644u;

	private const uint ReadTempratureProfileCode = 2621464648u;

	private static readonly Guid GUID_DEVINTERFACE_NUCWDT = new Guid(449657408u, 50395, 17730, 156, 115, 152, 152, 176, 216, byte.MaxValue, 110);

	private const string DRIVER_ID = "ACPI\\INTC1036";

	private const string DRIVER_FILENAME = "PerformanceDriver.sys";

	private const string MUTEX_NAME = "Global\\Access_ISABUS.HTP.Method";

	private const int ERROR_SERVICE_EXISTS = -2147023823;

	private const int ERROR_SERVICE_ALREADY_RUNNING = -2147023840;

	private SafeFileHandle device;

	private Mutex isABusMutex;

	public byte ReadByteBuf { get; }

	public bool InitializeDriver()
	{
		OpenDriver();
		if (device == null)
		{
			DeleteDriver();
		}
		try
		{
			isABusMutex = new Mutex(initiallyOwned: false, "Global\\Access_ISABUS.HTP.Method");
		}
		catch (UnauthorizedAccessException)
		{
			try
			{
				isABusMutex = Mutex.OpenExisting("Global\\Access_ISABUS.HTP.Method", MutexRights.Synchronize);
			}
			catch (Exception)
			{
				isABusMutex.Close();
				isABusMutex.Dispose();
				isABusMutex = null;
				return false;
			}
		}
		return device != null;
	}

	public bool UninitializeDriver()
	{
		StopDriver();
		DeleteDriver();
		return true;
	}

	public bool ReadDriverStatusFun()
	{
		if (device == null)
		{
			return false;
		}
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621464644u
		};
		try
		{
			uint bytesReturned;
			return NativeMethods.DeviceIoControl(device, ioControlCode, IntPtr.Zero, 0u, IntPtr.Zero, 0u, out bytesReturned, IntPtr.Zero);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool WritePowerLimit(ulong writeContent)
	{
		if (device == null)
		{
			return false;
		}
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621480980u
		};
		try
		{
			uint bytesReturned;
			return NativeMethods.DeviceIoControl(device, ioControlCode, writeContent, (uint)Marshal.SizeOf(writeContent), IntPtr.Zero, 0u, out bytesReturned, IntPtr.Zero);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool ReadPowerLimit(ref ulong value)
	{
		if (device == null)
		{
			return false;
		}
		object obj = value;
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621464592u
		};
		try
		{
			bool result = NativeMethods.DeviceIoControl(device, ioControlCode, IntPtr.Zero, 0u, obj, (uint)Marshal.SizeOf(value), out var _, IntPtr.Zero);
			value = (ulong)obj;
			return result;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool ReadFanProfile(ref FanProfileEC value)
	{
		if (device == null)
		{
			return false;
		}
		object obj = value;
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621464608u
		};
		try
		{
			bool result = NativeMethods.DeviceIoControl(device, ioControlCode, IntPtr.Zero, 0u, obj, (uint)Marshal.SizeOf(value), out var _, IntPtr.Zero);
			value = (FanProfileEC)obj;
			return result;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool WriteFanProfileNUC21(FanProfileWmi value)
	{
		if (device == null)
		{
			return false;
		}
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621480984u
		};
		try
		{
			uint bytesReturned;
			return NativeMethods.DeviceIoControl(device, ioControlCode, value, (uint)Marshal.SizeOf(value), IntPtr.Zero, 0u, out bytesReturned, IntPtr.Zero);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool WriteFanProfileNUC22(FanProfileWmi value)
	{
		if (device == null)
		{
			return false;
		}
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621480988u
		};
		try
		{
			uint bytesReturned;
			return NativeMethods.DeviceIoControl(device, ioControlCode, value, (uint)Marshal.SizeOf(value), IntPtr.Zero, 0u, out bytesReturned, IntPtr.Zero);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool WriteFanProfileNUC23(FanProfileWmi23 value)
	{
		if (device == null)
		{
			return false;
		}
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621481036u
		};
		try
		{
			uint bytesReturned;
			return NativeMethods.DeviceIoControl(device, ioControlCode, value, (uint)Marshal.SizeOf(value), IntPtr.Zero, 0u, out bytesReturned, IntPtr.Zero);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool GetFanSpeed(ref FanSpeedStruct value, byte ecSection)
	{
		if (device == null)
		{
			return false;
		}
		object obj = value;
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621464612u
		};
		try
		{
			bool result = NativeMethods.DeviceIoControl(device, ioControlCode, ecSection, (uint)Marshal.SizeOf(ecSection), obj, (uint)Marshal.SizeOf(value), out var _, IntPtr.Zero);
			value = (FanSpeedStruct)obj;
			return result;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetFanSpeed(FanDuty value)
	{
		if (device == null)
		{
			return false;
		}
		DwIoControlCodeType ioControlCode = new DwIoControlCodeType
		{
			IoControlCode = 2621481024u
		};
		try
		{
			uint bytesReturned;
			return NativeMethods.DeviceIoControl(device, ioControlCode, value, (uint)Marshal.SizeOf(value), IntPtr.Zero, 0u, out bytesReturned, IntPtr.Zero);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool ReadTempratureProfile(ref TempratureProfileEC value)
	{
		uint bytesReturned = 0u;
		object obj = value;
		bool result = NativeMethods.DeviceIoControl(ioControlCode: new DwIoControlCodeType
		{
			IoControlCode = 2621464648u
		}, device: device, inBuffer: IntPtr.Zero, inBufferSize: 0u, outBuffer: obj, nOutBufferSize: (uint)Marshal.SizeOf(value), bytesReturned: out bytesReturned, overlapped: IntPtr.Zero);
		value = (TempratureProfileEC)obj;
		return result;
	}

	private string GetDriverPath(Guid guid)
	{
		string result = null;
		uint num = 0u;
		IntPtr intPtr = default;
		try
		{
			intPtr = NativeMethods.SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero, 18u);
			for (SpDeviceInterfaceData deviceInterfaceData = new SpDeviceInterfaceData
			{
				cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(SpDeviceInterfaceData)))
			}; NativeMethods.SetupDiEnumDeviceInterfaces(intPtr, IntPtr.Zero, ref guid, num, ref deviceInterfaceData); num++)
			{
				SpDevInfoData deviceInfoData = new SpDevInfoData
				{
					cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(SpDevInfoData)))
				};
				SpDeviceInterfaceDetailData deviceInterfaceDetailData = new SpDeviceInterfaceDetailData
				{
					cbSize = ((IntPtr.Size == 8) ? 8u : Convert.ToUInt32(4 + Marshal.SystemDefaultCharSize))
				};
				NativeMethods.SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, IntPtr.Zero, 0u, out var requiredSize, IntPtr.Zero);
				if (NativeMethods.SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, ref deviceInterfaceDetailData, requiredSize, out requiredSize, ref deviceInfoData))
				{
					result = deviceInterfaceDetailData.DevicePath;
				}
			}
			return result;
		}
		catch (Exception)
		{
			return null;
		}
		finally
		{
			if (intPtr != (IntPtr)0)
			{
				NativeMethods.SetupDiDestroyDeviceInfoList(intPtr);
			}
		}
	}

	private bool OpenDriver()
	{
		string driverPath = GetDriverPath(GUID_DEVINTERFACE_NUCWDT);
		if (string.IsNullOrEmpty(driverPath))
		{
			return false;
		}
		try
		{
			device = new SafeFileHandle(NativeMethods.CreateFile(driverPath, 3221225472u, 3u, IntPtr.Zero, 3u, 128u, IntPtr.Zero), ownsHandle: true);
			SafeFileHandle safeFileHandle = device;
			if (safeFileHandle != null && safeFileHandle.IsInvalid)
			{
				StopDriver();
				return false;
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	private bool StopDriver()
	{
		SafeFileHandle safeFileHandle = device;
		if (safeFileHandle != null && safeFileHandle.IsInvalid)
		{
			device.Close();
			device.Dispose();
			device = null;
		}
		return true;
	}

	private bool DeleteDriver()
	{
		try
		{
			IntPtr intPtr = NativeMethods.OpenSCManager(null, null, 983103u);
			if (intPtr == IntPtr.Zero)
			{
				return false;
			}
			IntPtr intPtr2 = NativeMethods.OpenService(intPtr, "ACPI\\INTC1036", 983551u);
			if (intPtr2 == IntPtr.Zero)
			{
				return true;
			}
			ServiceStatus lpServiceStatus = default;
			NativeMethods.ControlService(intPtr2, 1u, ref lpServiceStatus);
			bool result = NativeMethods.DeleteService(intPtr2);
			NativeMethods.CloseServiceHandle(intPtr2);
			return result;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
